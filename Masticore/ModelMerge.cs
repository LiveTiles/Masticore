using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Masticore
{
    /// <summary>
    /// Attribute for declaring a property on a model as merged with the ModelMerge system
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MergeAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deltaType"></param>
        public MergeAttribute(object deltaType = null)
        {
            DeltaType = deltaType;
            AllowUpdate = true;
            AllowCreate = true;
            AllowOnce = false;
        }

        /// <summary>
        /// Creates a new historical attribute, capturing the given delta type for this property
        /// </summary>
        public MergeAttribute()
        {
            DeltaType = null;
            AllowUpdate = true;
            AllowCreate = true;
            AllowOnce = false;
        }

        /// <summary>
        /// Gets or sets the Delta type for this attribute, which should be some category indicator for changes made to the attributed property
        /// </summary>
        public object DeltaType { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if this property is automatically merged on edits
        /// This affects the behavior of the ModelMerge class
        /// Default is TRUE
        /// </summary>
        public bool AllowUpdate { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if this property is automatically merged on create
        /// This affects the behavior of the ModelMerge class
        /// Default is TRUE
        /// </summary>
        public bool AllowCreate { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating the property will be written only once (EG, if the property already has a value, then ignore when merging)
        /// This is lower precedence than the AllowUpdate/AllowCreate setting, which can prevent updates completely
        /// Default is FALSE
        /// </summary>
        public bool AllowOnce { get; set; }
    }

    /// <summary>
    /// A class for capturing changes in a model
    /// One instance should be generated for each detected change in a model during the merge
    /// </summary>
    public class ModelDelta
    {
        /// <summary>
        /// Gets the name of the object associated with this delta
        /// </summary>
        public string ObjectTypeName { get; protected set; }

        /// <summary>
        /// Gets the name of the property associated with this delta
        /// This will use the Display or DisplayName attribute if present
        /// </summary>
        public string PropertyName { get; protected set; }

        /// <summary>
        /// The stringified old value for the property 
        /// </summary>
        public string OldValue { get; protected set; }

        /// <summary>
        /// The stringified new value for the property 
        /// </summary>
        public string NewValue { get; protected set; }

        /// <summary>
        /// The type integer (re-cast enum) indicating the category of change for the property 
        /// Normally this is read from the Historical attribute
        /// </summary>
        public object DeltaType { get; protected set; }

        /// <summary>
        /// Constructor for making a new model delta object
        /// </summary>
        /// <param name="objectTypeName"></param>
        /// <param name="propertyName"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="type"></param>
        public ModelDelta(string objectTypeName, string propertyName, string oldValue, string newValue, object type)
        {
            ObjectTypeName = objectTypeName;
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
            DeltaType = type;
        }

        /// <summary>
        /// Generates a default description for this model delta
        /// </summary>
        public string Description
        {
            get
            {
                return string.Format("{0} field '{1}' was changed from '{2}' to '{3}'", ObjectTypeName, PropertyName, OldValue, NewValue);
            }
        }
    }

    /// <summary>
    /// The various modes for the model merging system, determine how items are mapped
    /// </summary>
    public enum MergeMode
    {
        /// <summary>
        /// Merge where ALL properties marked with MergeAttribute
        /// </summary>
        All,

        /// <summary>
        /// Merge where properties with a MergeAttribute and AllowCreate == true
        /// </summary>
        Create,

        /// <summary>
        /// Merge where properties with a MergeAttribute and AllowEdit == true
        /// </summary>
        Update
    }

    /// <summary>
    /// Static class for working with merging between objects, usually annotated with the IsModelMerged attribute
    /// </summary>
    public static class ModelMerge
    {
        /// <summary>
        /// Merges changes in the source model into the target model, returning a list of the fields changed in the model
        /// This treats any property with and [Editable(true)] attribute as on a white-list for editing and merging
        /// </summary>
        /// <typeparam name="TargetType">The type of the models being handle, source model may be a sub-class</typeparam>
        /// <param name="targetModel">The model that will have its property values changed by this merge</param>
        /// <param name="newValuesModel">The model contributing new values to the target model</param>
        /// <param name="mode">The various merge policies for this method - All (every property), Edit (only properties with Editable(true) OR Historical and flag, Create (only properties with flagged Historical attribute)</param>
        /// <returns></returns>
        public static IEnumerable<ModelDelta> MergeProperties<TargetType, NewValueType>(this TargetType targetModel, NewValueType newValuesModel, MergeMode mode = MergeMode.All)
            where TargetType : class
            where NewValueType : class
        {
            if (newValuesModel == null)
                throw new ArgumentNullException(nameof(newValuesModel));

            var deltas = new List<ModelDelta>();

            var targetModelType = targetModel.GetType();
            var newValuesModelType = newValuesModel.GetType();
            var targetModelProperties = targetModelType.GetProperties();
            var newValuesModelProperties = newValuesModelType.GetProperties().ToDictionary(p => p.Name);

            // Iterate through the properties in the type
            foreach (var targetModelProperty in targetModelProperties)
            {
                // If it's not in the new values model, then skip it
                if (!newValuesModelProperties.ContainsKey(targetModelProperty.Name))
                    continue;

                // Find if they have an editable or historical attribute on them
                var isModelMergedAttr = targetModelProperty.GetCustomAttribute<MergeAttribute>();

                var isAllowed = IsMergeAllowed(mode, isModelMergedAttr);

                if (isAllowed)
                {
                    object deltaType = null;

                    // Optionally use the historical attribute's values
                    if (isModelMergedAttr != null)
                        deltaType = isModelMergedAttr.DeltaType;

                    // Pull out the newValues property so we can pass it along
                    var newValuesProperty = newValuesModelProperties[targetModelProperty.Name];

                    SetNewValueOnModel(targetModel, newValuesModel, deltas, targetModelProperty, newValuesProperty, deltaType, isModelMergedAttr.AllowOnce);
                }
            }

            return deltas;
        }

        /// <summary>
        /// Returns a boolean calculating whether or not the current action is 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="isModelMerge"></param>
        /// <returns></returns>
        private static bool IsMergeAllowed(MergeMode mode, MergeAttribute isModelMerge)
        {
            // If there is Merged property on it, then abort
            if (isModelMerge == null)
                return false;

            // If the mode is all, then map no matter what
            if (mode == MergeMode.All)
                return true;

            // If we're editing and that's enabled
            if (mode == MergeMode.Update && (isModelMerge != null && isModelMerge.AllowUpdate))
                return true;

            // If we're creating and that's enabled
            if (mode == MergeMode.Create && (isModelMerge != null && isModelMerge.AllowCreate))
                return true;

            // Otherwise, we're not allowed
            return false;
        }

        /// <summary>
        /// Sets a new value on the target model, based on the new values model
        /// </summary>
        /// <typeparam name="TargetModelType"></typeparam>
        /// <param name="targetModel"></param>
        /// <param name="newValuesModel"></param>
        /// <param name="deltas"></param>
        /// <param name="targetModelProperty"></param>
        /// <param name="deltaType"></param>
        private static void SetNewValueOnModel<TargetModelType, NewValueType>(TargetModelType targetModel, NewValueType newValuesModel,
            List<ModelDelta> deltas,
            PropertyInfo targetModelProperty, PropertyInfo newValueProperty,
            object deltaType, bool writeOnce)
        {
            var newValue = newValueProperty.GetValue(newValuesModel);
            var oldValue = targetModelProperty.GetValue(targetModel);

            // If it's staying null, then skip
            if (newValue == null && oldValue == null)
                return;

            // If we're in write once mode and the source value is already not null, then skip this
            if (writeOnce && oldValue != null)
                return;

            // If the value changed between source and target models
            if ((newValue == null && oldValue != null) || (newValue != null && oldValue == null) || !newValue.Equals(oldValue))
            {
                // Save that change into the delta list
                var propertyDisplayName = targetModelProperty.GetDisplayName();
                var oldValueString = oldValue != null ? oldValue.ToString() : null;
                var newValueString = newValue != null ? newValue.ToString() : null;

                var objectTypeName = targetModel.GetModelTypeName();

                // Add this delta to the return list
                var delta = new ModelDelta(objectTypeName, propertyDisplayName, oldValueString, newValueString, deltaType);
                deltas.Add(delta);

                // Set the value in the target object to the value in the source 
                targetModelProperty.SetValue(targetModel, newValue);
            }
        }
    }
}
