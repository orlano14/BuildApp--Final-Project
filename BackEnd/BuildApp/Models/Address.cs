using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class Address
    {
       
        string placeId;
        string country;
        string locality;
        string street;
        int houseNum;
        int floorsNum;
        string addedUser;

        public string PlaceId { get => placeId; set => placeId = value; }
        public string Country { get => country; set => country = value; }
        public string Locality { get => locality; set => locality = value; }
        public string Street { get => street; set => street = value; }
        public int HouseNum { get => houseNum; set => houseNum = value; }
        public int FloorsNum { get => floorsNum; set => floorsNum = value; }
        public string AddedUser { get => addedUser; set => addedUser = value; }

        public Address(string placeId, string country, string locality, string street, int houseNum, int floorsNum, string addedUser)
        {
            PlaceId = placeId;
            Country = country;
            Locality = locality;
            Street = street;
            HouseNum = houseNum;
            FloorsNum = floorsNum;
            AddedUser = addedUser;
        }

        public Address()
        {

        }

        public string AddAddress()
        {
            DBservices db = new DBservices();
            return "B"+(10000-db.AddAddress(this));
        }

        //public Address GetAddressByUN(string un)
        //{
        //    DBservices db = new DBservices();
        //    return db.GetAddressByUN(un);
        //}
       

    }
}