using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class ListItem
    {
        Request req;
        bool isRequest;
        bool isRated;
        UserDetails ud;

        public Request Req { get => req; set => req = value; }
        public bool IsRequest { get => isRequest; set => isRequest = value; }
        public UserDetails Ud { get => ud; set => ud = value; }
        public bool IsRated { get => isRated; set => isRated = value; }

        public ListItem(Request req, bool isRequest, UserDetails ud,bool isRated)
        {
            Req = req;
            IsRequest = isRequest;
            Ud = ud;
            IsRated = isRated;
        }

        public ListItem()
        {

        }
    }
}