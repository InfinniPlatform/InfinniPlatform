using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Storage for BLOB (Binary Large OBject).
    /// </summary>
    public interface IBlobStorage
    {
        /// <summary>
        /// Returns BLOB information.
        /// </summary>
        /// <param name="blobId">BLOB identifier.</param>
        BlobInfo GetBlobInfo(string blobId);

        /// <summary>
        /// Returns BLOB data by identifier.
        /// </summary>
        /// <param name="blobId">BLOB identifier.</param>
        BlobData GetBlobData(string blobId);

        /// <summary>
        /// Creates new BLOB from stream.
        /// </summary>
        /// <param name="blobName">BLOB name.</param>
        /// <param name="blobType">BLOB data type.</param>
        /// <param name="blobData">BLOB data.</param>
        BlobInfo CreateBlob(string blobName, string blobType, Stream blobData);

        /// <summary>
        /// Creates new BLOB from stream.
        /// </summary>
        /// <param name="blobName">BLOB name.</param>
        /// <param name="blobType">BLOB data type.</param>
        /// <param name="blobData">BLOB data.</param>
        Task<BlobInfo> CreateBlobAsync(string blobName, string blobType, Stream blobData);

        /// <summary>
        /// Creates new BLOB from form file.
        /// </summary>
        /// <param name="blobName">BLOB name.</param>
        /// <param name="blobType">BLOB data type.</param>
        /// <param name="formFile">File from HTTP request.</param>
        Task<BlobInfo> CreateBlobAsync(string blobName, string blobType, IFormFile formFile);

        /// <summary>
        /// Updates BLOB from stream.
        /// </summary>
        /// <param name="blobId">BLOB identifier.</param>
        /// <param name="blobName">BLOB name.</param>
        /// <param name="blobType">BLOB data type.</param>
        /// <param name="blobData">BLOB data.</param>
        BlobInfo UpdateBlob(string blobId, string blobName, string blobType, Stream blobData);

        /// <summary>
        /// Updates BLOB from stream.
        /// </summary>
        /// <param name="blobId">BLOB identifier.</param>
        /// <param name="blobName">BLOB name.</param>
        /// <param name="blobType">BLOB data type.</param>
        /// <param name="blobData">BLOB data.</param>
        Task<BlobInfo> UpdateBlobAsync(string blobId, string blobName, string blobType, Stream blobData);

        /// <summary>
        /// Updates BLOB from form file.
        /// </summary>
        /// <param name="blobId">BLOB identifier.</param>
        /// <param name="blobName">BLOB name.</param>
        /// <param name="blobType">BLOB data type.</param>
        /// <param name="formFile">File from htpp request.</param>
        Task<BlobInfo> UpdateBlobAsync(string blobId, string blobName, string blobType, IFormFile formFile);

        /// <summary>
        /// Delete BLOB by identifier.
        /// </summary>
        /// <param name="blobId">BLOB identifier.</param>
        void DeleteBlob(string blobId);
    }
}