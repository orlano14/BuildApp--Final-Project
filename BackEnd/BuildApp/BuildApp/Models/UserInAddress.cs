using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class UserInAddress
    {
        string userName;
        int addressId;
        int floor;
        int apartment;
        string phoneNum;

        public UserInAddress(string userName, int addressId, int floor, int apartment, string phoneNum)
        {
            UserName = userName;
            AddressId = addressId;
            Floor = floor;
            Apartment = apartment;
            PhoneNum = phoneNum;
        }

        public string UserName { get => userName; set => userName = value; }
        public int AddressId { get => addressId; set => addressId = value; }
        public int Floor { get => floor; set => floor = value; }
        public int Apartment { get => apartment; set => apartment = value; }
        public string PhoneNum { get => phoneNum; set => phoneNum = value; }


        public string AddUserInAddress()
        {
            DBservices db = new DBservices();
            int addressFloor = db.GetAddressFloor(AddressId);
            if (Floor > addressFloor)
            {
                db.UpdateAddressFloor(AddressId, Floor);
            }
            if (db.IsUserInAddressExist(this.UserName))
                return db.UpdateUserInAddress(this);
            else
                return db.AddUserInAddress(this);
        }
    }
}