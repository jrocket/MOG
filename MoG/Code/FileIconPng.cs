using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Code
{
    public static class FileIconPng
    {
        private static string _rootPath = "/Content/Images/fileIcons/{0}.png";
        #region supported icons
        private static string[] supportedIcons = {"aac"
,"ai"
,"aiff"
,"avi"
,"bmp"
,"c"
,"cpp"
,"css"
,"dat"
,"dmg"
,"doc"
,"dotx"
,"dwg"
,"dxf"
,"eps"
,"exe"
,"flv"
,"gif"
,"h"
,"hpp"
,"html"
,"ics"
,"iso"
,"java"
,"jpg"
,"js"
,"key"
,"less"
,"mid"
,"mp3"
,"mp4"
,"mpg"
,"odf"
,"ods"
,"odt"
,"otp"
,"ots"
,"ott"
,"pdf"
,"php"
,"png"
,"ppt"
,"psd"
,"py"
,"qt"
,"rar"
,"rb"
,"rtf"
,"sass"
,"scss"
,"sql"
,"tga"
,"tgz"
,"tiff"
,"txt"
,"wav"
,"xls"
,"xlsx"
,"xml"
,"yml"
,"zip"
};
        #endregion 

        public static string GetIconPath(string filename)
        {
            string extension = System.IO.Path.GetExtension(filename);
            if (!String.IsNullOrEmpty(extension))
            {
                extension = extension.Trim('.');
            }
            if (supportedIcons.Contains(extension))
                return string.Format(_rootPath, extension);
            else
                return string.Format(_rootPath, "_blank");
        }
    }
}