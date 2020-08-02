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
    public class SettingsController : ApiController
    {
        [HttpPost]
        [Route("api/Settings/PostSettings")]
        public string PostSettings([FromBody]Settings s)
        {
            return s.PostSettings();
        }

        [HttpGet]
        [Route("api/Settings/IsSettingsExist")]
        public bool IsSettingsExist(string un)
        {
            DBservices db = new DBservices();
            return db.IsUserSettingsExist(un);
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