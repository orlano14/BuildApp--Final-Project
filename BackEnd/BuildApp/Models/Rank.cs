using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class Rank
    {
        string fullName;
        double avg;
        int helps;

        public string FullName { get => fullName; set => fullName = value; }
        public double Avg { get => avg; set => avg = value; }
        public int Helps { get => helps; set => helps = value; }

        public Rank(string fullName, double avg, int helps)
        {
            FullName = fullName;
            Avg = avg;
            Helps = helps;
        }
        public Rank()
        {

        }

        public List<Rank> GetRanks(string userName)
        {
            DBservices db = new DBservices();
            int buildingId = db.GetAddressIdByUN(userName);
            List<string> fullNames = db.GetFullNamesByAddressId(buildingId);
            List<string> userNames = db.GetUserNamesByAddressId(buildingId);
            List<Rank> ranks = new List<Rank>();
            MyList myList = new MyList();

            for (int i = 0; i < userNames.Count; i++)
            {
                List<ListItem> history = new List<ListItem>();
                List<ListItem> historyRequests = new List<ListItem>();
                List<int> ratesByUser = new List<int>();
                Rank rank = new Rank();
                rank.FullName = fullNames.ElementAt(i);          
                List<Request> tasks = db.GetTasksByUN(userNames.ElementAt(i));
                foreach (var item in tasks)//שמירת הדירוגים של כל הבקשות שביצעתי
                {
                    int rate = db.GetRate(item.SerialNum);
                    if (rate != -1)
                    {
                        ratesByUser.Add(rate);
                    }

                }
                double sum = 0;
                double avg = 0;
                for (int j = 0; j < ratesByUser.Count; j++)//חישוב ממוצע הדירוגים שלי
                {
                    sum += ratesByUser.ElementAt(j);
                }
                rank.Helps = ratesByUser.Count;
                if (ratesByUser.Count == 0)
                    avg = 0;

                else
                    avg = sum / ratesByUser.Count;


                rank.Avg = Math.Round(avg,2);

                ranks.Add(rank);
               
            }
            List<Rank> SortedRanks = ranks.OrderByDescending(o => o.Helps * o.Avg).ToList();//מיון לפי ממוצע כפול מספר עזרות

            return SortedRanks;
        }

        public string PostRank(int requestSerialNum, int rate)
        {
            DBservices db = new DBservices();
            return db.PostRank(requestSerialNum, rate);
        }
    }
}