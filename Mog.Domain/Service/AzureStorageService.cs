using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class AzureStorageService : ILocalStorageService
    {
        private string connectionString = null;
        public AzureStorageService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
        }
        public string UploadFile(Stream data, string filenameandPath, string container)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container.ToLower());

            // Create the container if it doesn't already exist.
            blobContainer.CreateIfNotExists();

            blobContainer.SetPermissions(
    new BlobContainerPermissions
    {
        PublicAccess =
            BlobContainerPublicAccessType.Blob
    });
            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(filenameandPath.ToLower());

            // Create or overwrite the "myblob" blob with contents from a local file.
            data.Position = 0;
            blockBlob.UploadFromStream(data);

            return blockBlob.Uri.ToString();
        }


        public string UploadFile(byte[] file, string path, string p)
        {
            Stream stream = new MemoryStream(file);
            return this.UploadFile(stream, path, p);
        }

        public bool DownloadFile(ref Stream outStream, string path, string container)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer blobcontainer = blobClient.GetContainerReference(container.ToLower());

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = blobcontainer.GetBlockBlobReference(path.ToLower());


            blockBlob.DownloadToStream(outStream);
            return true;
        }

        public async Task<string> RenameFile(string oldname, string newname, string container)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container.ToLower());


            // Create the container if it doesn't already exist.
            blobContainer.CreateIfNotExists();

            //        blobContainer.SetPermissions(
            //new BlobContainerPermissions
            //{
            //    PublicAccess =
            //        BlobContainerPublicAccessType.Blob
            //});

            //grab the blob
            var existBlob = blobContainer.GetBlobReferenceFromServer(oldname);
            var newBlob = blobContainer.GetBlockBlobReference(newname);
            //create a new blob

            Task<string> getStringTask = newBlob.StartCopyFromBlobAsync(existBlob.Uri);

            string urlContents = await getStringTask;
            //delete the old
            existBlob.Delete();

            return newBlob.Uri.ToString();
        }



    }

    public interface ILocalStorageService
    {
        bool DownloadFile(ref Stream stream, string path, string container);

        string UploadFile(Stream data, string filenameandPath, string container);
        string UploadFile(byte[] data, string filenameandPath, string container);


        Task<string> RenameFile(string oldname, string newname, string p);


    }
}
