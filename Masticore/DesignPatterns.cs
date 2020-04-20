using System;
using System.Threading.Tasks;

namespace Masticore
{
    /// <summary>
    /// An interface for objects that have a unique ID field
    /// </summary>
    /// <typeparam name="KeyType"></typeparam>
    public interface IIdentifiable<KeyType>
    {
        /// <summary>
        /// Gets or sets the ID for this model
        /// NOTE: This ID must be enforced as unique by the system persisting it (EG, primary key in Entity Framework)
        /// </summary>
        KeyType Id { get; set; }
    }

    /// <summary>
    /// An interface for an object that has a unique ID across system (EG, a GUID)
    /// </summary>
    public interface IUniversal
    {
        /// <summary>
        /// Gets or sets an immutable and universal identifier for this item
        /// Traditionally, this would be a GUID in ToString("N") format
        /// In Entity Framework, you can limit to 32 non-unicode characters by annoating the property as follows:
        /// [Column(TypeName = "char")]
        /// [StringLength(32)]
        /// </summary>
        string UniversalId { get; set; }
    }

    /// <summary>
    /// Interfaces for an ICrud that wishes to extend into reading items via externalId of an IUniversal
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface IReadByUniversalId<ModelType>
        where ModelType : IUniversal
    {
        /// <summary>
        /// Asynchronoously retrieves via the ExternalId field of a 
        /// </summary>
        /// <param name="universalId"></param>
        /// <returns></returns>
        Task<ModelType> ReadByUniversalIdAsync(string universalId);
    }

    /// <summary>
    /// Interface for an object that tracks an auditable life-cycle over time
    /// </summary>
    public interface IAuditable
    {
        /// <summary>
        /// Gets or sets the created date UTC for this object
        /// </summary>
        DateTime? CreatedUtc { get; set; }

        /// <summary>
        /// Gets or sets the modified date UTC for this object
        /// </summary>
        DateTime? UpdatedUtc { get; set; }
    }

    /// <summary>
    /// Interface for an object that can be soft-deleted in the system with a flag
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// Gets or sets a DateTime indicating when the item was soft deleted
        /// NOTE: This property being null indicates it's not soft deleted
        /// </summary>
        DateTime? DeletedUtc { get; set; }
    }

    /// <summary>
    /// Interface for a model providing a name for itself
    /// </summary>
    public interface INamed
    {
        string Name { get; }
    }

    /// <summary>
    /// Extensions on the INamedModel interface
    /// </summary>
    public static class NamedModelExtensions
    {
        /// <summary>
        /// Creates a description string based on the type and name of the given INamedModel instance, appending the given description
        /// </summary>
        /// <param name="model"></param>
        /// <param name="changeDescription"></param>
        /// <returns></returns>
        public static string CreateDescriptionForModel(this INamed model, string changeDescription)
        {
            var modelTypeName = model.GetModelTypeName();
            return string.Format("{0} '{1}' {2}", modelTypeName, model.Name, changeDescription);
        }
    }

    // Creating a new object by creating a single instance of a builder, setting properties on it
    // Then using it to "build" a new instance of another object
    // This is often used to overcome limitatons of dependency injection systems

    /// <summary>
    /// Generic interface for an object that builds another object
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    public interface IBuilder<Type>
    {
        /// <summary>
        /// Creates a new instance of class Type
        /// </summary>
        /// <returns></returns>
        Type Build();
    }

    /// <summary>
    /// Generic interface for an object that builds an object asynchronously
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    public interface IBuilderAsync<Type>
    {
        /// <summary>
        /// Asynchronously create a new instance of class Type
        /// </summary>
        /// <returns></returns>
        Task<Type> BuildAsync();
    }
    /// <summary>
    /// Generic interface for an model that has optimistic concurrency. 
    /// </summary>
    public interface IConcurrent
    {
        /// <summary>
        /// The database generated concurrency token
        /// </summary>
        byte[] ConcurrencyToken { get; set; }
    }
}
