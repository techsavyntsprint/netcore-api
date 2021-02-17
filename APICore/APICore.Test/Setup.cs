using APICore.Data.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;

namespace APICore.Test
{
    class Setup
    {
        //BLOB Setting are needed and for now they are hardcodded.
        public static CloudBlobClient GetBlobHardCodedSettings()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1/apicore;");
            return storageAccount.CreateCloudBlobClient();
        }
        public static List<User> FakeUsersData()
        {
            return new List<User>()
            {
                new User()
                {
                Email = "carlos@itguy.com",
                FullName = "Carlos Delgado",
                Gender = 0,
                Phone = "+53 12345678",
                Password = "S3cretP@$$",
                }
            };
        }
    }
}
