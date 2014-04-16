using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public enum CloudStorageServices 
    {
        
        Dropbox = 1,
        GoogleDrive = 2,
        Skydrive = 3
    }

    public class UserStorageVM
    {
        public List<AuthCredential> CloudStorages { get; set; }

        public Dictionary<CloudStorageServices, string> CloudLogos { get; set; }

        public UserStorageVM()
        {
            CloudStorages = new List<AuthCredential>();
            CloudLogos = new Dictionary<CloudStorageServices, string>();
            CloudLogos[CloudStorageServices.Dropbox] = "~/Content/Images/Dropbox_logo.svg";
            CloudLogos[CloudStorageServices.GoogleDrive] = "~/Content/Images/GoogleDriveLogo.png";
            CloudLogos[CloudStorageServices.Skydrive] = "~/Content/Images/SkyDriveLogo.png";
        }

    }


}
