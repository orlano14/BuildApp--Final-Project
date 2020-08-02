using BuildApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using BuildApp.Models.DAL;
using System;
using System.Linq;
using System.Net.Http;

namespace BuildApp.Controllers
{
    public class PushNotificationController : ApiController
    {
        //TOREMOVE
        [HttpPost]
        [Route("api/sendpushnotification")]
        public int Sendpushnotification([FromBody]PushNotData pnd)
        {
            List<PushNotData> listPnd = new List<PushNotData>();
            listPnd.Add(pnd);
            return PushNotData.SendListPushNotification(listPnd);
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