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
    public class RequestController : ApiController
    {
        [HttpGet]
        [Route("api/Request/GetActiveRequestsByUserName/{userName}")]
        public List<Request> GetActiveRequestsByUserName(string userName)
        {
            Request r = new Request(); 
            return r.GetActiveRequestsByUserName(userName);
        }

        [HttpGet]
        [Route("api/Request/UpdateExecutingUser/{serialNum}/{userName}")]
        public string UpdateExecutingUser(int serialNum, string userName)
        {
            Request r = new Request();
            return r.UpdateExecutingUser(serialNum, userName);
        }

        [HttpPost]
        [Route("api/Request/AddRequest")]
        public string AddRequest([FromBody]RequestToPush rtp)
        {
           
            return rtp.AddRequst();
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