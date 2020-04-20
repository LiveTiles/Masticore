using System;

namespace Masticore
{
    /// <summary>
    /// Interface for an object that represents anything with a connection string
    /// </summary>
    public interface INeedConnectionString
    {
        /// <summary>
        /// Gets or sets the connection string for this database
        /// </summary>
        string ConnectionString { get; set; }
    }

    /// <summary>
    /// Interface for an object that represents anything with a storage connection string
    /// </summary>
    public interface INeedStorageConnectionString
    {
        /// <summary>
        /// Gets or sets the connection string for this storage table
        /// </summary>
        Func<string> GetStorageConnectionString { get; set; }
    }

    /// <summary>
    /// Interface for an object that depends on a NoSQL table concept
    /// </summary>
    public interface IStorageTable : INeedStorageConnectionString
    {
        /// <summary>
        /// Gets or sets the table name this this storage table
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// Gets or sets the partition name for this storage table
        /// </summary>
        string PartitionName { get; set; }
    }

    /// <summary>
    /// ICrud restricted to what Storage can map, since RowKey must be a string
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface IStorageTableCrud<ModelType> : ICrud<ModelType, string>, IStorageTable { }
}
