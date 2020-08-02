using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class Settings
    {
        string userName;
        bool babySitter;
        bool dogwalker;
        bool carpool;
        bool groceries;
        bool availability;
        List<Skills> skills;

        public string UserName { get => userName; set => userName = value; }
        public bool BabySitter { get => babySitter; set => babySitter = value; }
        public bool Dogwalker { get => dogwalker; set => dogwalker = value; }
        public bool Carpool { get => carpool; set => carpool = value; }
        public bool Groceries { get => groceries; set => groceries = value; }
        public bool Availability { get => availability; set => availability = value; }
        public List<Skills> Skills { get => skills; set => skills = value; }

        public Settings(string userName, bool babySitter, bool dogwalker, bool carpool, bool groceries, bool availability, List<Skills> skills)
        {
            UserName = userName;
            BabySitter = babySitter;
            Dogwalker = dogwalker;
            Carpool = carpool;
            Groceries = groceries;
            Availability = availability;
            Skills = skills;
        }

        public Settings()
        {

        }


        public string PostSettings()
        {
            DBservices db = new DBservices();
            if (db.IsUserSettingsExist(this.UserName))
                return db.UpdateUserSettings(this);
            else
                return db.AddUserSettings(this);
        }
    }
}