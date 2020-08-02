using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class Data
    {
        int requestSerialNum;
        string type;

        public Data(int requestSerialNum, string type)
        {
            RequestSerialNum = requestSerialNum;
            Type = type;
        }

        public int RequestSerialNum { get => requestSerialNum; set => requestSerialNum = value; }
        public string Type { get => type; set => type = value; }
    }
}