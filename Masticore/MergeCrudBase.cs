using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masticore
{
    /// <summary>
    /// Base class for a CRUD async object that uses the MergeModel automatic mapping strategy 
    /// Also applies semantic changes versus the interfaces in Masticore's Design Patterns (EG, IUniversal, IAuditable)
    /// </summary>
    /// <typeparam name="ModelType">Any type with an Id field</typeparam>
    /// <typeparam name="KeyType"></typeparam>
    public abstract class MergeCrudBase<ModelType, KeyType>
        : ICrud<ModelType, KeyType>
        where ModelType : class, IIdentifiable<KeyType>, new()
    {
        /// <summary>
        /// Creates a new instance, merges its properties, and returns it
        /// Overriding methods must do something to persist the newly created object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<ModelType> CreateAsync(ModelType model)
        {
            var newModel = Create(model);

            return Task.FromResult(newModel);
        }

        /// <summary>
        /// Creates an IEnumerable of new instances. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<IEnumerable<ModelType>> CreateAsync(IEnumerable<ModelType> models)
        {

            var createdModels = models
                                .Where(m => m != null)
                                .Select(m => Create(m));

            return Task.FromResult(createdModels);

        }

        /// <summary>
        /// Executes create logic, including automatic values for IAuditable, IUniversal, and ISoftDeletable
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ModelType Create(ModelType model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var newModel = new ModelType();
            newModel.MergeProperties(model, MergeMode.Create);

            SetCreateProperties(newModel);

            return newModel;
        }

        /// <summary>
        /// Dynamically sets properties if the given model adheres to key design pattern interfaces
        /// </summary>
        /// <param name="newModel"></param>
        public static void SetCreateProperties(ModelType newModel)
        {
            // Optionally apply created info
            if (newModel is IAuditable)
            {
                var auditable = newModel as IAuditable;
                auditable.SetCreatedUtc();
            }

            // Optionally generate external ID
            if (newModel is IUniversal)
            {
                var universal = newModel as IUniversal;
                if (string.IsNullOrEmpty(universal.UniversalId))
                    universal.GenerateUniversalId();
            }

            // Optionally ensure not soft deleted
            if (newModel is ISoftDeletable)
            {
                var deletable = newModel as ISoftDeletable;
                deletable.SoftRestore();
            }
        }

        /// <summary>
        /// Deletes the object with the given key - bring your own implementation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract Task DeleteAsync(KeyType id);


        /// <summary>
        /// Deletes the objects with the given keys - bring your own implementation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract Task DeleteAsync(IEnumerable<KeyType> id);

        /// <summary>
        /// Reads all objects in this CRUD
        /// </summary>
        /// <returns></returns>
        public abstract Task<IEnumerable<ModelType>> ReadAllAsync();

        /// <summary>
        /// Reads a single object with the given ID from thid CRUD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract Task<ModelType> ReadAsync(KeyType id);

        /// <summary>
        /// Updates the given model using the merge strategy
        /// Overriding methods must do something to persist the change in the return model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ModelType> UpdateAsync(ModelType model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var existingModel = await this.ReadAsync(model.Id);

            Update(existingModel, model);

            return existingModel;
        }

        /// <summary>
        /// Executes the update logic for the existingModel, using the MergeProperties method to include values in the new model
        /// </summary>
        /// <param name="existingModel"></param>
        /// <param name="newValuesModel"></param>
        public static void Update(ModelType existingModel, ModelType newValuesModel)
        {
            existingModel.MergeProperties(newValuesModel, MergeMode.Update);

            if (existingModel is IAuditable)
            {
                var auditable = existingModel as IAuditable;
                auditable.SetModifiedUtc();
            }
        }
    }
}
