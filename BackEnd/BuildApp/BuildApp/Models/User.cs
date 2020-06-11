using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuildApp.Models.DAL;

namespace BuildApp.Models
{
    public class User
    {
        string userName;
        string firstName;
        string lastName;
        DateTime birthday;
        string gender;
        string password;
        string picUrl;

        public User(string userName, string firstName, string lastName, DateTime birthday, string gender, string password, string picUrl)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            Gender = gender;
            Password = password;
            PicUrl = picUrl;
        }


        public string UserName { get => userName; set => userName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public DateTime Birthday { get => birthday; set => birthday = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Password { get => password; set => password = value; }
        public string PicUrl { get => picUrl; set => picUrl = value; }

        public string AddUser()
        {
            DBservices db = new DBservices();
            return db.AddUser(this);
        }
    }
}