using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class UserDetails : UserToShow
    {
        string phoneNum;

        public string PhoneNum { get => phoneNum; set => phoneNum = value; }

        public UserDetails(string userName, string firstName, string lastName, DateTime birthday, string picUrl,string gender, string phoneNum) : base(userName, firstName, lastName, birthday, picUrl, gender)
        {
            PhoneNum = phoneNum;
        }
        public UserDetails()
        {

        }
        public UserToShow GetUserDetails(string userName)
        {
            DBservices db = new DBservices();
            return db.GetUserDetails(userName);
        }


    }
}