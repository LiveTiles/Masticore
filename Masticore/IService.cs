namespace Masticore
{
    /// <summary>
    /// Interface marking a service, which is generally an object injected into the system
    /// </summary>
    public interface IService
    {
    }

    /// <summary>
    /// Interface defining CRUD operations over a repository
    /// </summary>
    public interface ICrudService<ModelType, KeyType> : ICrud<ModelType, KeyType>, IService
    {
    }
}
