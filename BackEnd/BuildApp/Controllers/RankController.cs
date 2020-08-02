using BuildApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BuildApp.Controllers
{
    public class RankController : ApiController
    {
        [HttpGet]
        [Route("api/Rank/GetRanks/{userName}")]
        public List<Rank> GetRanks(string userName)
        {
            Rank r = new Rank();
            return r.GetRanks(userName);
        }

        [HttpGet]
        [Route("api/Rank/PostRank/{requestSerialNum}/{rate}")]
        public string PostRank(int requestSerialNum, int rate)
        {
            Rank r = new Rank();
            return r.PostRank(requestSerialNum, rate);
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