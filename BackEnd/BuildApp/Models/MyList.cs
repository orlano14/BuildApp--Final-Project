using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class MyList
    {
        List<ListItem> requests;
        List<ListItem> tasks;
        List<ListItem> history;

        public List<ListItem> Requests { get => requests; set => requests = value; }
        public List<ListItem> Tasks { get => tasks; set => tasks = value; }
        public List<ListItem> History { get => history; set => history = value; }

        public MyList(List<ListItem> requests, List<ListItem> tasks, List<ListItem> history)
        {
            Requests = requests;
            Tasks = tasks;
            History = history;
        }

        public MyList()
        {
            Requests = new List<ListItem>();
            Tasks = new List<ListItem>();
            History = new List<ListItem>();
        }

        public MyList GetMyList(string un)
        {

            DBservices db = new DBservices();
            List<Request> allRequests = db.GetAllRequests();
            for (int i = 0; i < allRequests.Count; i++)
            {
                if (allRequests.ElementAt(i).DueDate < DateTime.Now)
                {
                    db.UpdateIsActive(allRequests.ElementAt(i));
                }
            }
            List<Request> requests = db.GetAllRequestsByUN(un);
            List<Request> tasks = db.GetTasksByUN(un);
            MyList myList = new MyList();
            UserDetails userDetails = new UserDetails();

            for (int i = 0; i < requests.Count; i++)
            {
                ListItem li = new ListItem();
                if (requests.ElementAt(i).DueDate > DateTime.Now)
                {
                    li.Req = requests.ElementAt(i);
                    li.IsRequest = true;
                    if (requests.ElementAt(i).ExecutingUser == "")
                    {
                        li.Ud = userDetails;
                    }
                    else
                    {
                        li.Ud = db.GetUserDetails(li.Req.ExecutingUser);
                    }
                    myList.Requests.Add(li);
                }
                else
                {
                    li.Req = requests.ElementAt(i);
                    li.IsRequest = true;
                    if (requests.ElementAt(i).ExecutingUser == "")
                    {
                        li.Ud = userDetails;
                    }
                    else
                    {
                        li.Ud = db.GetUserDetails(li.Req.ExecutingUser);
                    }
                    myList.History.Add(li);
                }
            }
            for (int j = 0; j < tasks.Count; j++)
            {
                ListItem listItem = new ListItem();
                if (tasks.ElementAt(j).DueDate > DateTime.Now)
                {
                    listItem.Req = tasks.ElementAt(j);
                    listItem.IsRequest = false;
                    listItem.Ud = db.GetUserDetails(listItem.Req.FromUserName);

                    myList.Tasks.Add(listItem);
                }
                else
                {
                    listItem.Req = tasks.ElementAt(j);
                    listItem.IsRequest = false;
                    listItem.Ud = db.GetUserDetails(listItem.Req.FromUserName);

                    myList.History.Add(listItem);
                }
            }
            List<ListItem> SortedList = myList.History.OrderBy(o => o.Req.DueDate).ToList();
            myList.History = SortedList;
            for (int i = 0; i < myList.History.Count; i++)
            {
                if (db.IsRankExist(myList.History[i].Req.SerialNum))
                    myList.History[i].IsRated = true;
                else
                    myList.History[i].IsRated = false;
            }
            return myList;
        }
    }
}