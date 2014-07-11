using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class FakeAzureStorageService : ILocalStorageService
    {
        public bool DownloadFile(ref System.IO.Stream stream, string path, string container)
        {
            //stream = new System.IO.FileStream(@"C:\Data\TempAudio\DemoSound16.mp3",);
            return true;
        }

        public string UploadFile(System.IO.Stream data, string filenameandPath, string container)
        {
            return filenameandPath;
        }

        public string UploadFile(byte[] data, string filenameandPath, string container)
        {
            return filenameandPath;
        }

        public Task<string> RenameFile(string oldname, string newname, string p)
        {
            Task<string> task1 = Task<string>.Factory.StartNew(() => "renamed");
            return task1;
        }
    }
}
