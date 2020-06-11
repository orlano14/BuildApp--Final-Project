using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class Request
    {
        int serialNum;
        int addressId;
        string fromUserName;
        string type;
        DateTime dueDate;
        bool isItPaid;
        string note;
        string executingUser;
        bool isActive;
        double requestLong;
        int skillNum;

        public int SerialNum { get => serialNum; set => serialNum = value; }
        public int AddressId { get => addressId; set => addressId = value; }
        public string FromUserName { get => fromUserName; set => fromUserName = value; }
        public string Type { get => type; set => type = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public bool IsItPaid { get => isItPaid; set => isItPaid = value; }
        public string Note { get => note; set => note = value; }
        public string ExecutingUser { get => executingUser; set => executingUser = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public double RequestLong { get => requestLong; set => requestLong = value; }
        public int SkillNum { get => skillNum; set => skillNum = value; }

        public Request(int addressId, string fromUserName, string type, DateTime dueDate, bool isItPaid, string note, string executingUser, bool isActive, double requestLong, int skillNum)
        {

            AddressId = addressId;
            FromUserName = fromUserName;
            Type = type;
            DueDate = dueDate;
            IsItPaid = isItPaid;
            Note = note;
            ExecutingUser = executingUser;
            IsActive = isActive;
            RequestLong = requestLong;
            SkillNum = skillNum;


        }
        public Request()
        {

        }


        public List<Request> GetActiveRequestsByUserName(string userName)
        {
            DBservices db = new DBservices();
            List<Request> activeRequests = new List<Request>();
            for (int i = 0; i < db.GetActiveRequestsByUserName(userName).Count; i++)
            {
                if (db.GetActiveRequestsByUserName(userName).ElementAt(i).DueDate > DateTime.Now)//אפשר למחוק בדיקה זו לאחר שנטפל בסגירת בקשות לפי התאריך שלהן
                {
                    if (db.GetActiveRequestsByUserName(userName).ElementAt(i).IsActive == true)
                    {
                        activeRequests.Add(db.GetActiveRequestsByUserName(userName).ElementAt(i));
                    }

                }

            }

            return activeRequests;
        }
        public string UpdateExecutingUser(int serialNum, string userName)
        {
            DBservices db = new DBservices();
            return db.UpdateExecutingUser(serialNum, userName);
        }



    }
}