using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace MoG
{
    public static class MogConstants
    {
        public const string TEMPDATA_ERRORMESSAGE = "errorMessage";
        public static string MESSAGE_INBOX = "inbox";
        public static string MESSAGE_OUTBOX = "outbox";


        public static string MESSAGE_ARCHIVE = "archive";

        public static string PARAM_ADMINMAIL = "ADMIN_MAIL";


        public static string DROPBOX_KEY
        {
            get
            { return ConfigurationManager.AppSettings["DROPBOX_KEY"]; }
        }

        public static string DROPBOX_SECRET
        {
            get
            { return ConfigurationManager.AppSettings["DROPBOX_SECRET"]; }
        }

        public static string SKYDRIVE_REDIRECTURL
        {
            get
            {
                return ConfigurationManager.AppSettings["SKYDRIVE_REDIRECTURL"];
            }
        }

    }
}