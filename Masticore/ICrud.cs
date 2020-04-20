using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masticore
{
    /// <summary>
    /// Interface for an object that enables asynchronous creation of a new model based on a template model
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface ICreate<ModelType>
    {
        /// <summary>
        /// Asynchronously creates a new instance of the given entity (based on the given object as a template)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ModelType> CreateAsync(ModelType model);
    }

    /// <summary>
    /// Interface for an object that enables asynchronous deletion of a model with the given ID
    /// </summary>
    /// <typeparam name="KeyType"></typeparam>
    public interface IDelete<KeyType>
    {
        /// <summary>
        /// Asynchronously deletes the model with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(KeyType id);
    }

    /// <summary>
    /// Interface for an object that enables asynchronous read of a model based on a given ID
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    /// <typeparam name="KeyType"></typeparam>
    public interface IRead<ModelType, KeyType>
    {
        /// <summary>
        /// Asynchronoously retrieves the single model with the given ID
        /// Returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ModelType> ReadAsync(KeyType id);
    }

    /// <summary>
    /// Interface for an object that enables asynchronous reading of all models of a type
    /// Generally, this is based on context set by properties on the object
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface IReadAll<ModelType>
    {
        /// <summary>
        /// Asynchronously returns all models of the given type for this async object
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ModelType>> ReadAllAsync();
    }

    /// <summary>
    /// Interface for an object that enables asynchronous reading of a range of models of a type
    /// Generally, this is based on context set by properties on the object
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface IReadRange<ModelType>
    {
        /// <summary>
        /// Asynchronously returns all models of the given type for this async object
        /// </summary>
        /// <returns></returns>
        Task<RangeResult<ModelType>> ReadRangeAsync(int startAt, int endAt, string filterBy = null);
    }

    /// <summary>
    /// Interface for an object that enables asynchronous update of a new model based on a template model
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface IUpdate<ModelType>
    {
        /// <summary>
        /// Asynchronously updates the given model
        /// </summary>
        /// <param name="model">Template for the model</param>
        /// <returns>The updated instance of the model (which is likely read fresh from the data source before update occurs)</returns>
        Task<ModelType> UpdateAsync(ModelType model);
    }
    /// <summary>
    /// Inteface defining asynchronous CRUD operations on a collection of ModelType using the key KeyType
    /// </summary>
    public interface ICrud<ModelType, KeyType> : ICreate<ModelType>, IRead<ModelType, KeyType>, IReadAll<ModelType>, IUpdate<ModelType>, IDelete<KeyType>
    {
    }

    /// <summary>
    /// Interface defining asynchronous CRUD operations on a singleton object
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface ISingletonCrud<ModelType> : IService
    {
        Task<ModelType> CreateOrUpdateAsync(ModelType model);
        Task<ModelType> ReadAsync();
        Task DeleteAsync();
    }

    /// <summary>
    /// Interface for reading objects by name
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface IReadByName<ModelType>
    {
        /// <summary>
        /// Asynchronously reads a single instance of the model from the CRUD by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ModelType> ReadByNameAsync(string name);
    }

    /// <summary>
    /// Common pattern for an ICrud that also checks by name
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    /// <typeparam name="KeyType"></typeparam>
    public interface ICrudWithByName<ModelType, KeyType> : ICrudService<ModelType, KeyType>, IReadByName<ModelType>
    {
    }
}
