using System.Collections.Generic;

namespace Masticore
{
    /// <summary>
    /// Interface marking a service, which is generally an object injected into the system
    /// </summary>
    public interface IService
    {
    }

    public interface ITrackMessages
    {
        List<string> TrackedItems { get; set; }
    }
    /// <summary>
    /// Interface defining CRUD operations over a repository
    /// </summary>
    public interface ICrudService<ModelType, KeyType> : ICrud<ModelType, KeyType>, IService
    {
    }
}
