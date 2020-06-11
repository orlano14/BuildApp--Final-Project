using BuildApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BuildApp.Controllers
{
    public class UserInAddressController : ApiController
    {

        [HttpPost]
        [Route("api/UserInAddress/AddUserInAddress/{buildingCode}")]
        public string AddUserInAddress([FromBody]UserInAddress u, string buildingCode)
        {
            u.AddressId= 10000-int.Parse(buildingCode.Substring(1));
            return u.AddUserInAddress();
        }


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