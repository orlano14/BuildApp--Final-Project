using BuildApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BuildApp.Controllers
{
    public class StatusController : ApiController
    {
        [HttpGet]
        [Route("api/Status/GetRequestStatusBuildingByRequestId/{requestId}")]
        public List<List<List<Status>>> GetRequestStatusBuildingByRequestId(int requestId)
        {
            Status s = new Status();
            return s.GetRequestStatusBuildingByRequestId(requestId);
        }

        [HttpGet]
        [Route("api/Status/SawAllNotification/{un}")]
        public string SawAllNotification(string un)
        {
            Status s = new Status();
            return s.SawAllNotification(un);
        }
        [HttpGet]
        [Route("api/Status/GetNumOfNotification/{un}")]
        public int GetNumOfNotification(string un)
        {
            Status s = new Status();
            return s.GetNumOfNotification(un);
        }
        [HttpGet]
        [Route("api/Status/AcceptRequest/{un}/{serialNum}")]
        public string AcceptRequest(string un, int serialNum)
        {
            Status s = new Status();
            return s.AcceptRequest(un,serialNum);
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