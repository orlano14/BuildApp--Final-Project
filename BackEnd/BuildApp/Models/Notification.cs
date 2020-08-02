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
        bool isTop;

        public UserToShow Uts { get => uts; set => uts = value; }
        public Request Req { get => req; set => req = value; }
        public int Seen { get => seen; set => seen = value; }
        public bool IsTop { get => isTop; set => isTop = value; }

       

        public Notification()
        {

        }

        public Notification(UserToShow uts, Request req, int seen, bool isTop)
        {
            Uts = uts;
            Req = req;
            Seen = seen;
            IsTop = isTop;
        }

        public List<Notification> GetNotificationsByUserName(string un)
        {
            DBservices db = new DBservices();
            List<Notification> allNotifications = new List<Notification>();
            List<Notification> notifications = new List<Notification>();
            Rank r = new Rank();
            List<Rank> ranks = r.GetRanks(un);
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
            for (int i = 0; i < notifications.Count; i++)
            {
                string fullName = notifications.ElementAt(i).Uts.FirstName + " " + notifications.ElementAt(i).Uts.LastName;
                if (fullName==ranks.ElementAt(0).FullName)
                {
                    notifications.ElementAt(i).IsTop = true;
                    Notification n = new Notification();
                    n = notifications.ElementAt(i);
                    notifications.RemoveAt(i);
                    notifications.Insert(0, n);
                }
            }
            return notifications;
        }
    }
}