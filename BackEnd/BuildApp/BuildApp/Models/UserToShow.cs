using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class UserToShow
    {
        string userName;
        string firstName;
        string lastName;
        DateTime birthday;
        string picUrl;
        string gender;

        public string UserName { get => userName; set => userName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public DateTime Birthday { get => birthday; set => birthday = value; }
        public string PicUrl { get => picUrl; set => picUrl = value; }
        public string Gender { get => gender; set => gender = value; }

        public UserToShow(string userName, string firstName, string lastName, DateTime birthday, string picUrl,string gender)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            PicUrl = picUrl;
            Gender = gender;
        }
        public UserToShow()
        {

        }
        public virtual UserToShow GetUserToShow(string userName)
        {
            DBservices db = new DBservices();
            return db.GetUserToShow(userName);
        }
    }
}