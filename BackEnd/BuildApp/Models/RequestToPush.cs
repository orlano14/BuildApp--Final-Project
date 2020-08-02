using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class RequestToPush
    {
        string fromUserName;
        string type;
        DateTime dueDate;
        bool isItPaid;
        string note;
        double requestLong;
        string skill;

        public string FromUserName { get => fromUserName; set => fromUserName = value; }
        public string Type { get => type; set => type = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public bool IsItPaid { get => isItPaid; set => isItPaid = value; }
        public string Note { get => note; set => note = value; }
        public double RequestLong { get => requestLong; set => requestLong = value; }
        public string Skill { get => skill; set => skill = value; }

        public RequestToPush(string fromUserName, string type, DateTime dueDate, bool isItPaid, string note, double requestLong, string skill)
        {
            FromUserName = fromUserName;
            Type = type;
            DueDate = dueDate;
            IsItPaid = isItPaid;
            Note = note;
            RequestLong = requestLong;
            Skill = skill;
        }

        public string AddRequst()
        {
            DBservices db = new DBservices();
            List<PushNotData> pndList = new List<PushNotData>();
            db.AddRequest(this);
            int lastRequest = db.GetTheLastRequest().SerialNum;
            List<string> userNamesToPush = db.GetUserNamesByRequest(lastRequest);
            List<string> tokens = db.GetTokens(userNamesToPush);
            foreach (string token in tokens)
            {
                pndList.Add(new PushNotData(
                    to: token,
                    title: Type,
                    body: DueDate.ToString().Substring(0, DueDate.ToString().Length - 3),
                    badge:0,
                    data:new Data(lastRequest,"newRequest")
                    ));
            }
            PushNotData.SendListPushNotification(pndList);
            return "ok";
        }
    }
}