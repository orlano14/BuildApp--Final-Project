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
        UserDetails ud;

        public Request Req { get => req; set => req = value; }
        public bool IsRequest { get => isRequest; set => isRequest = value; }
        public UserDetails Ud { get => ud; set => ud = value; }

        public ListItem(Request req, bool isRequest, UserDetails ud)
        {
            Req = req;
            IsRequest = isRequest;
            Ud = ud;
        }

        public ListItem()
        {

        }
    }
}