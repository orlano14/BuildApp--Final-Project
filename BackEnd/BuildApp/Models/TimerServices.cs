using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public static class TimerServices
    {
       
        public static void DoSomethingWithtimer(string path)
        {
            // File.AppendAllText(path + "fileFromGATimer.txt", "hey there time:" + DateTime.Now.ToString() + "\r\n");
            DBservices db = new DBservices();
            db.CheckRequestDueDate();
        }
    }
}