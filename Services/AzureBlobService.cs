using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;

namespace cldv_ice_task_04.Services
{
    public class AzureBlobService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public AzureBlobService(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}