using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace ConsoleAppBlobAzure
{
    public class DataManager
    {
        private CloudStorageAccount storageAccount = null;
        private CloudBlobClient blobClient = null;
        private CloudBlobContainer container = null;
        private CloudBlockBlob blockBlob = null;
        private CloudAppendBlob appendBlob = null;   

        

        public DataManager()
        {
            this.storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            this.blobClient = this.storageAccount.CreateCloudBlobClient();
        }

        public void CreateContainer(string containerName)
        {            
            if (!this.IsContainerExists(containerName))
                   this.container.Create();               
        }

        public bool IsContainerExists(string containerName)
        {
            this.container = this.blobClient.GetContainerReference(containerName);
            if (this.container.Exists())
                return true;
            else
                return false;
        }
                

        public void DeleteContainer(string containerName)
        {
            this.container = this.blobClient.GetContainerReference(containerName);
            this.container.DeleteIfExists();            
        }

        public bool GetFileReference(string containerName, string fileName)
        {
            if (this.IsContainerExists(containerName))
            {
                this.blockBlob = this.container.GetBlockBlobReference(fileName);
                if (this.blockBlob.Exists())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
                return false;
        }

        public void SetContainerPublicAccess()
        {
            this.container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        }

        public void AddBlobObject(string blobName, string path, string fileName)
        {
            this.blockBlob = container.GetBlockBlobReference(blobName);            
            using (var fileStream = File.OpenRead(path + fileName))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }

        public void ShowContainer(string containerName)
        {
            if(this.IsContainerExists(containerName))
            {
                foreach (IListBlobItem item in container.ListBlobs(null, false))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;
                        Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);
                    }
                    else if (item.GetType() == typeof(CloudPageBlob))
                    {
                        CloudPageBlob pageBlob = (CloudPageBlob)item;
                        Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);
                    }
                    else if (item.GetType() == typeof(CloudBlobDirectory))
                    {
                        CloudBlobDirectory directory = (CloudBlobDirectory)item;
                        Console.WriteLine("Directory: {0}", directory.Uri);
                    }
                }
                Console.WriteLine("\n\n");
            }
            else
            {
                Console.WriteLine("Container not found");
            }
        }

        public void DownloadBlobObject(string containerName, string blobName, string pathToSave)
        {
            this.container = blobClient.GetContainerReference(containerName);
            this.blockBlob = this.container.GetBlockBlobReference(blobName);
            if (this.blockBlob.Exists())
            {
                using (var fileStream = File.OpenWrite(pathToSave+blobName))
                {
                    this.blockBlob.DownloadToStream(fileStream);
                }
            }
            else
                Console.WriteLine("Файл не найден");
        }

        public void GetTextBlobObject(string containerName, string blobName)
        {
            this.container = blobClient.GetContainerReference(containerName);
            this.blockBlob = this.container.GetBlockBlobReference(blobName);
            string text = string.Empty;
            if (this.blockBlob.Exists())
            {                
                using (var memoryStream = new MemoryStream())
                {
                    this.blockBlob.DownloadToStream(memoryStream);
                    text = Encoding.UTF8.GetString(memoryStream.ToArray());
                    Console.WriteLine(text);
                }
            }
             
        }

        public void DeleteBlobObject(string containerName, string blobName)
        {
            this.container = blobClient.GetContainerReference(containerName);
            this.blockBlob = this.container.GetBlockBlobReference(blobName);
            if (blockBlob.Exists())
            {
                blockBlob.Delete();               
            }
            else
                Console.WriteLine("Error!!! Object not found");
        }

        async public static Task ListBlobsSegmentedInFlatListing(string containerName)
        {
            CloudStorageAccount storageAccount1 = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount1.CreateCloudBlobClient();
            CloudBlobContainer cont = blobClient.GetContainerReference(containerName);
            Console.WriteLine("List blobs in pages:");

            int i = 0;
            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;
            
            do
            {               
                resultSegment = await cont.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null);
                if (resultSegment.Results.Count<IListBlobItem>() > 0) { Console.WriteLine("Page {0}:", ++i); }
                foreach (var blobItem in resultSegment.Results)
                {
                    Console.WriteLine("\t{0}", blobItem.StorageUri.PrimaryUri);
                }
                Console.WriteLine();               
                continuationToken = resultSegment.ContinuationToken;
            }
            while (continuationToken != null);
        }

        public void SimulateAppendBlob(string containerName, string appendBlobName)
        {
            this.appendBlob = container.GetAppendBlobReference(appendBlobName);
            if (!appendBlob.Exists())
            {
                appendBlob.CreateOrReplace();                
            }
            this.appendBlob = container.GetAppendBlobReference(appendBlobName);

            int numBlocks = 10;            
            Random rnd = new Random();
            byte[] bytes = new byte[numBlocks];
            rnd.NextBytes(bytes);
            
            for (int i = 0; i < numBlocks; i++)
            {
                appendBlob.AppendText(String.Format("Timestamp: {0:u} \tLog Entry: {1}{2}",
                    DateTime.UtcNow, bytes[i], Environment.NewLine));
            }            
            Console.WriteLine(appendBlob.DownloadText());
        }


    }
}
