using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Masticore
{
    public static class JsonHelper
    {
        private static string RegexReplace(this string src, string regex, string replace = "")
        {
            return Regex.Replace(src, regex, replace, RegexOptions.Singleline);
        }

        public static string RemoveEntireTag(this string src, string tagName)
        {
            const string patternTemplate = @"(<--placeholder-->.*?</--placeholder-->)";
            var pattern = patternTemplate.Replace("--placeholder--", tagName);
            var result = src.RegexReplace(pattern);

            return result;
        }

        public static string RemoveEntireTag(this string src, IEnumerable<string> tagNames)
        {
            return tagNames.Aggregate(src, (current, tagName) => current.RemoveEntireTag(tagName));
        }

        public static string RemoveMarkupTags(this string src)
        {
            return src.RegexReplace(@"((<.*?>)|(&nbsp;)|(\s+))", " ");
        }

        public static IEnumerable<string> WithoutEntireTags(this IEnumerable<string> strs, params string[] tagNames)
        {
            return strs.Select(s => s.RemoveEntireTag(tagNames));
        }

        public static IEnumerable<string> WithoutMarkupTags(this IEnumerable<string> strs)
        {
            return strs.Select(RemoveMarkupTags);
        }

        public static JObject RemoveProperties(this JObject container, string[] propertyNames)
        {
            foreach (var property in propertyNames)
            {
                container.Property(property)?.Remove();
            }

            return container;

        }
        public static IEnumerable<string> FindValues(this JToken container, string keyName)
        {
            return container.FindTokens(keyName).Select(t => t.ToString());
        }
        public static IEnumerable<string> FindValues(this JToken container)
        {
            return container.Select(t => t.ToString());
        }

        public static IEnumerable<JToken> FindTokens(this JToken containerToken, string keyName)
        {
            var matches = new List<JToken>();
            FindTokens(containerToken, keyName, matches);
            return matches;
        }

        private static void FindTokens(JToken containerToken, string keyName, ICollection<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (var child in containerToken.Children<JProperty>())
                {
                    if (child.Name == keyName)
                    {
                        matches.Add(child.Value);
                    }
                    FindTokens(child.Value, keyName, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (var child in containerToken.Children())
                {
                    FindTokens(child, keyName, matches);
                }
            }
        }
    }
}
