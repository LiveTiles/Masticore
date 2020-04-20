using System.IO;
using System.Threading.Tasks;

namespace Masticore
{
    /// <summary>
    /// Interface for a file hosted in the cloud
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Gets  the container name of the cloud file
        /// Azure Contrainer names are between 3 and 63 characters long, thus you may want to annotate implementers of this property with
        /// [Required]
        /// [MinLength(3)]
        /// [MaxLength(63)]
        /// </summary>
        string ContainerName { get; }

        /// <summary>
        /// Gets the blob name
        /// [Required]
        /// [MinLength(1)]
        /// [MaxLength(1024)]
        /// </summary>
        string FileName { get; }
    }

    /// <summary>
    /// Base interface for a File Service that provides the connection string
    /// </summary>
    public interface IFileService : INeedStorageConnectionString, IService
    {
    }

    /// <summary>
    /// Interface for a service that implements cloud file upload, download, and delete
    /// </summary>
    public interface IFileService<FileType> : IFileService
        where FileType : IFile
    {
        /// <summary>
        /// Calculates a URL for the given cloud file, useful for situations like setting the src attribute on an img tag
        /// </summary>
        /// <param name="cloudFile"></param>
        /// <returns></returns>
        string GetFileUrl(FileType cloudFile);


        Task<string> GetFileUrlWithToken(FileType cloudFile);

        string GetFileUrlWithToken(string fileUrl);

        /// <summary>
        /// Deletes the given file from storage
        /// NOTE: This should NOT change the cloudFile properties at all
        /// </summary>
        /// <param name="cloudFile"></param>
        /// <returns></returns>
        Task DeleteAsync(FileType cloudFile);

        /// <summary>
        /// Async returns the stream for the given file
        /// NOTE: This should NOT change the cloudFile properties at all
        /// </summary>
        /// <param name="cloudFile"></param>
        /// <returns></returns>
        Task<Stream> DownloadAsync(FileType cloudFile);

        /// <summary>
        /// Async overwrites the file for the given cloudFile with the given stream
        /// NOTE: This should NOT change the cloudFile properties at all
        /// </summary>
        /// <param name="cloudFile"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        Task UploadAsync(FileType cloudFile, Stream fileStream);

        Task UploadAsync(FileType cloudFile, string fileContent);

        Task UploadAsync(FileType cloudFile, Stream fileStream, bool isSnapshot = false);

        Task UploadAsync(FileType cloudFile, string fileContent, bool isSnapshot = false);
    }
}
