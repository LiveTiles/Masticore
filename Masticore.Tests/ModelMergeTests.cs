using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Masticore.Tests
{
    /// <summary>
    /// Test model for the merge unit tests, with multiple types and IsMerged settings
    /// </summary>
    public class TestModel
    {
        [Merge]
        public int Integer { get; set; }
        [Merge]
        public decimal Decimal { get; set; }
        [Merge(AllowCreate = false)]
        public float Float { get; set; }
        [Merge(AllowUpdate = false)]
        public string String { get; set; }
        [Merge]
        public object Object { get; set; }

        public int NotMergedProperty
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }

    /// <summary>
    /// Test model for only writing to a string once and checking for skipper properties
    /// </summary>
    public class OnceModel
    {
        [Merge(AllowOnce = true)]
        public string String { get; set; }

        public int AlsoNotMergedProperty
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }

    /// <summary>
    /// Test class for ModelMerge class
    /// </summary>
    [TestClass]
    public class ModelMergeTests
    {
        /// <summary>
        /// Initialized a new test model instance
        /// </summary>
        /// <param name="newValuesModel"></param>
        /// <param name="model"></param>
        static void InitializeModelsForMerge(out TestModel newValuesModel, out TestModel model)
        {
            newValuesModel = CreateLoadedTestModel();
            model = new TestModel();
        }

        private static TestModel CreateLoadedTestModel()
        {
            return new TestModel() { Integer = 42, Decimal = 4.88m, Float = 9.9f, String = "Hello World", Object = new { id = "Hi!" } };
        }

        /// <summary>
        /// Tests model merging all fields works
        /// </summary>
        [TestMethod]
        public void TestModelMergeAll()
        {
            // Arrange
            TestModel newValuesModel, model;
            InitializeModelsForMerge(out newValuesModel, out model);

            // Act
            ModelMerge.MergeProperties(model, newValuesModel);

            // Assert
            Assert.IsTrue(newValuesModel.Integer.Equals(model.Integer));
            Assert.IsTrue(newValuesModel.Decimal.Equals(model.Decimal));
            Assert.IsTrue(newValuesModel.Float.Equals(model.Float));
            Assert.IsTrue(newValuesModel.String.Equals(model.String));
            Assert.IsTrue(newValuesModel.Object.Equals(model.Object));
        }

        /// <summary>
        /// Tests model merge in the create mode
        /// </summary>
        [TestMethod]
        public void TestModelMergeCreate()
        {
            // Arrange
            TestModel newValuesModel, model;
            InitializeModelsForMerge(out newValuesModel, out model);

            // Act
            ModelMerge.MergeProperties(model, newValuesModel, MergeMode.Create);

            // Assert
            Assert.IsTrue(newValuesModel.Integer.Equals(model.Integer));
            Assert.IsTrue(newValuesModel.Decimal.Equals(model.Decimal));
            Assert.IsFalse(newValuesModel.Float.Equals(model.Float));       // Float is OnCreate false
            Assert.IsTrue(newValuesModel.String.Equals(model.String));
            Assert.IsTrue(newValuesModel.Object.Equals(model.Object));
        }

        /// <summary>
        /// Tests model merge in the edit mode
        /// </summary>
        [TestMethod]
        public void TestModelMergeEdit()
        {
            // Arrange
            TestModel newValuesModel, model;
            InitializeModelsForMerge(out newValuesModel, out model);

            // Act
            ModelMerge.MergeProperties(model, newValuesModel, MergeMode.Update);

            // Assert
            Assert.IsTrue(newValuesModel.Integer.Equals(model.Integer));
            Assert.IsTrue(newValuesModel.Decimal.Equals(model.Decimal));
            Assert.IsTrue(newValuesModel.Float.Equals(model.Float));
            Assert.IsFalse(newValuesModel.String.Equals(model.String));  // String is OnEdit false
            Assert.IsTrue(newValuesModel.Object.Equals(model.Object));

        }

        /// <summary>
        /// Tests model merge in the edit mode
        /// </summary>
        [TestMethod]
        public void TestModelMergeOnce()
        {
            // Arrange
            var firstValue = "Hello";
            var secondValue = "World";

            var model = new OnceModel { String = null };
            var newValues1 = new OnceModel { String = firstValue };
            var newValues2 = new OnceModel { String = secondValue };

            // Act
            ModelMerge.MergeProperties(model, newValues1, MergeMode.Update);
            ModelMerge.MergeProperties(model, newValues2, MergeMode.Update);

            // Assert
            Assert.IsTrue(model.String.Equals(newValues1.String));
        }

        /// <summary>
        /// Tests model merge between objects of two types
        /// </summary>
        [TestMethod]
        public void TestTestModelToOnce()
        {
            // Arrange
            var testModel = CreateLoadedTestModel();
            var onceModel = new OnceModel { String = "Came From Behind" };

            // Act
            ModelMerge.MergeProperties(testModel, onceModel, MergeMode.All);

            // Assert
            Assert.IsTrue(testModel.String.Equals(onceModel.String));
        }
    }
}
