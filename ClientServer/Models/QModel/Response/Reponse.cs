using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientServer.Models.QModel.Response
{
    public class Reponse
    {
        public string Message { get; set; }
        public bool Success { get; set; } = true;
        public object Data { get; set; }
        public int ErrorCode { get; set; }
    }
}