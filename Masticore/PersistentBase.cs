using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Masticore
{
    /// <summary>
    /// Interface for an object persisted to a datastore
    /// </summary>
    public interface IPersistent<KeyType> : IIdentifiable<KeyType>, IAuditable, ISoftDeletable
    {
    }

    /// <summary>
    /// Base class for objects persisted across systems and time
    /// Designed largely for use with EntityFramework, but could be used in other systems
    /// Has DataContract applied to it to give opt-in JSON serialization in MVC applications
    /// </summary>
    [DataContract]
    public abstract class PersistentBase<KeyType> : IPersistent<KeyType>
    {
        /// <summary>
        /// Gets or sets the ID value for the model
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public KeyType Id { get; set; }

        /// <summary>
        /// Gets or sets the Created time for this record
        /// </summary>
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        [Editable(false)]
        [DataMember]
        public DateTime? CreatedUtc { get; set; }

        /// <summary>
        /// Gets or sets the time this record was last modified
        /// </summary>
        [Display(Name = "Modified Date")]
        [DataType(DataType.DateTime)]
        [Editable(false)]
        [DataMember]
        public DateTime? UpdatedUtc { get; set; }

        /// <summary>
        /// Gets or sets the time this record was soft-deleted
        /// </summary>
        [Display(Name = "Deleted Date")]
        [DataType(DataType.DateTime)]
        [Editable(false)]
        [DataMember]
        public virtual DateTime? DeletedUtc { get; set; }
    }

    /// <summary>
    /// Base class for an object that is persistent and universal
    /// </summary>
    /// <typeparam name="KeyType"></typeparam>
    [DataContract]
    public abstract class PersistentUniversalBase<KeyType> : PersistentBase<KeyType>, IUniversal
    {
        /// <summary>
        /// Gets or sets the universal ID for this object
        /// This assumes the object is a alphanumeric and 32 character slong (EG, a GUID)
        /// </summary>
        [Column(TypeName = "char")]
        [StringLength(32)]
        [Editable(false)]
        [DataMember]
        [Merge(AllowUpdate = false)]
        [Display(Name = "Universal ID")]
        public string UniversalId { get; set; }
    }

    /// <summary>
    /// Base class for an object that is persistent, concurrent, and universal
    /// </summary>
    ///  <typeparam name="KeyType">The <see cref="Type"/> of key for the model (e.g., int)</typeparam>
    [DataContract]
    public abstract class PersistentConcurrentUniversalBase<KeyType> : PersistentUniversalBase<KeyType>, IConcurrent
    {
        /// <summary>
        /// This will be an automatically tracked, generated field for optimistic concurrency. Will trigger <see cref="DbUpdateConcurrencyException"/> if changed.
        /// </summary>
        [Timestamp]
        [DataMember]
        public byte[] ConcurrencyToken { get; set; }
    }
}
