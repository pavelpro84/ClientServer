using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientServer.Models.QModel
{
    public class PagingData
    {
        //public int TotalPage { get; set; }
        public int TotalRecord { get; set; }
        public object Data { get; set; }
    }
}