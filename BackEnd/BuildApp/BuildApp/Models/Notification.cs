using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class Notification
    {
        UserToShow uts;
        Request req;
        int seen;

        public UserToShow Uts { get => uts; set => uts = value; }
        public Request Req { get => req; set => req = value; }
        public int Seen { get => seen; set => seen = value; }

        public Notification(UserToShow uts, Request req, int seen)
        {
            Uts = uts;
            Req = req;
            Seen = seen;
        }

        public Notification()
        {

        }

        public List<Notification> GetNotificationsByUserName(string un)
        {
            DBservices db = new DBservices();
            List<Notification> allNotifications = new List<Notification>();
            List<Notification> notifications = new List<Notification>();
            allNotifications = db.GetNotificationsByUN(un);
            for (int i = 0; i < allNotifications.Count; i++)
            {
                if (db.CheckIfNotificationIsRelevantForUN(allNotifications.ElementAt(i),un))//בדיקה אם המשתמש מוכן לבצע בקשה מסוג זה
                {
                    int seen = db.GetStatusForSeen(allNotifications.ElementAt(i),un);
                    if (seen!=-1)
                    {
                        allNotifications.ElementAt(i).Seen = seen;
                        notifications.Add(allNotifications.ElementAt(i));
                    }
                }  
            }
            
            return notifications;
        }
    }
}