using BuildApp.Models;
using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BuildApp.Controllers
{
    public class AddressController : ApiController
    {
        //[HttpGet]
        //[Route("api/Address/GetAddressByUN/{userName}")]
        //public Address GetAddressByUN(string userName)
        //{
        //    Address a = new Address();
        //    return a.GetAddressByUN(userName);
        //}

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("api/Address/AddAddress")]
        public string AddAddress([FromBody]Address a)
        {
            return a.AddAddress();
        }

        [HttpGet]
        [Route("api/Address/IsPlaceIdExist/{PlaceId}")]
        // GET api/car/discounted/2017
        public string IsPlaceIdExist(string placeId)
        {
            DBservices db = new DBservices();
            return db.IsPlaceIdExist(placeId);
        }
        [HttpGet]
        [Route("api/Address/IsAddressIdExist/{buildingCode}")]
        public bool IsAddressIdExist(string buildingCode)
        {
            DBservices db = new DBservices();
            return db.IsAddressIdExist(buildingCode);
        }
        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}