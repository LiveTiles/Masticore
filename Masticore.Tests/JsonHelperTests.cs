using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Masticore
{
    [TestClass]
    public class JsonHelperTests
    {

        private static JObject GetObject()
        {
            return JObject.Parse(@"
            {
                'main': 'super',
                'chill': {
                    'text': 'some text goes here'
                },
                'text': 'outer text',
                'complex': {
                    'inner': {
                        'text': 'very inner'
                    }
                }
            }
            ");
        }

        [TestMethod]
        public void TestFindTokens()
        {
            var tokens = GetObject().FindTokens("text").ToList();
            Assert.AreEqual(tokens.Count, 3);
        }

        [TestMethod]
        public void TestFindValues()
        {
            var values = GetObject().FindValues("text").ToList();
            Assert.AreEqual(values.Count, 3);
            Assert.IsTrue(values.Any(v => v == "some text goes here"));
            Assert.IsTrue(values.Any(v => v == "outer text"));
            Assert.IsTrue(values.Any(v => v == "very inner"));
        }

        [TestMethod]
        public void TestRemoveEntireTag()
        {
            var str =
                "blah blah blah <style> a { some-prop: xyz; }</style> <div>blah blah blah</div> <script> var a = b; </script> <!-- comment >";

            var result = str.RemoveEntireTag("style");

            Assert.AreEqual(result, "blah blah blah  <div>blah blah blah</div> <script> var a = b; </script> <!-- comment >");

            var result2 = str.RemoveEntireTag("script");

            Assert.AreEqual(result2, "blah blah blah <style> a { some-prop: xyz; }</style> <div>blah blah blah</div>  <!-- comment >");
            
            var result3 = str.RemoveEntireTag(new []{ "script", "style" });

            Assert.AreEqual(result3, "blah blah blah  <div>blah blah blah</div>  <!-- comment >");



        }

        [TestMethod]
        public void TestRemoveTagAgain()
        {
            var result1 = JsonHelperTestConsts.Original.RemoveEntireTag("style");

            Assert.AreEqual(result1, JsonHelperTestConsts.WithoutStyle);

            var result2 = JsonHelperTestConsts.Original.RemoveEntireTag("script");

            Assert.AreEqual(result2, JsonHelperTestConsts.WithoutScript);

            var result3 = JsonHelperTestConsts.Original.RemoveEntireTag(new[] {"script", "style"});

            Assert.AreEqual(result3, JsonHelperTestConsts.WithoutScriptOrStyle);
        }

        [TestMethod]
        public void TestRemoveMarkup()
        {
            var result1 = JsonHelperTestConsts.RemoveMarkUpConsts.A.RemoveMarkupTags();

            Assert.AreEqual(result1, JsonHelperTestConsts.RemoveMarkUpConsts.AResult);
        }
    }
}
