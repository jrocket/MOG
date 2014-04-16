using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG
{
    public class MogJsonResult
    {
        public bool Result { get; set; }
        public object Data { get; set; }

        public string Message { get; set; }

        public string MessageCode { get; set; }
    }
    public class JsonHelper
    {
        public static JsonResult ResultOk(object data,JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            JsonResult result = new JsonResult();
            result.Data = new MogJsonResult() {Result = true, Data = data };
            result.JsonRequestBehavior = behavior;
            return result;

        }

        public static JsonResult ResultError(object data,Exception exc,String errorCode = null, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            JsonResult result = new JsonResult();
            MogJsonResult Data = new MogJsonResult() { Result = false, Data = data };
            if (exc!=null)
            {
                Data.Message = String.Format("Message : {0} | StackTrace : {1}", exc.Message, exc.StackTrace);
            }
            Data.MessageCode = errorCode;
            result.Data = Data;
            result.JsonRequestBehavior = behavior;
            return result;

        }

    
    }
}