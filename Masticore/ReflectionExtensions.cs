using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Masticore
{
    /// <summary>
    /// Extension and utility methods for System.Reflection that make it easier to work with attributes on members
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the property information for a member, based on a Member or Unary LINQ Expression
        /// </summary>
        /// <param name="propertyExpression">A MemberExpression or UnaryExpression indicating a property</param>
        /// <returns></returns>
        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            var memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                var unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }

        /// <summary>
        /// Gets the display name for the given member
        /// Checks for DisplayAttribute and DisplayNameAttribute, then falls back to the property name as coded
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static string GetDisplayName(this MemberInfo memberInfo)
        {
            // Check for the DisplayName attribute
            var displayNameAttribute = memberInfo.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                return displayNameAttribute.DisplayName;
            }

            // Check for the Display attribute
            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                return displayAttribute.Name;
            }

            // If nothing was found, then fall back to the property name
            return memberInfo.Name;
        }

        /// <summary>
        /// For a given property LINQ expression, returns either the display name as indicated by the Display attribute OR the property name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static string GetDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    nameof(propertyExpression));
            }

            return GetDisplayName(memberInfo);
        }

        /// <summary>
        /// Returns either the display friendly name or the stringified value of the given enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            if (!type.IsEnum)
                return value.ToString();

            var members = type.GetMember(value.ToString());
            if (members.Length == 0)
                return value.ToString();

            var memberInfo = members[0];
            return GetDisplayName(memberInfo);
        }

        /// <summary>
        /// Shallow copies all properties from the source object to the destination
        /// NOTE: If you need more than this, just try AutoMapper: http://automapper.org/
        /// </summary>
        /// <param name="objSource"></param>
        /// <param name="objDestination"></param>
        public static void CopyProperties(this object objSource, object objDestination)
        {
            //get the list of all properties in the destination object
            var destProps = objDestination.GetType().GetProperties();
            var sourceProps = objSource.GetType().GetProperties();
            var destPropsDict = destProps.ToDictionary(sp => sp.Name);

            //get the list of all properties in the source object
            foreach (var sourceProp in sourceProps)
            {
                if (destPropsDict.ContainsKey(sourceProp.Name))
                {
                    var destProp = destPropsDict[sourceProp.Name];
                    if (destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                    {
                        destProp.SetValue(objDestination, sourceProp.GetValue(objSource));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the type name for a Base model, handling the common case where it is a dynamic proxy of a DB object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetModelTypeName(this Object model)
        {
            var type = model.GetType();

            // If the type is a dynamic proxy of a type (EG, pulled from a DbContext), then take the name of the base class
            if (type.Namespace == "System.Data.Entity.DynamicProxies")
                return type.BaseType.Name;
            else
                return type.Name;
        }
    }
}
