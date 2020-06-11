using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BuildApp.Models;
using BuildApp.Models.DAL;

namespace BuildApp.Controllers
{
    public class LoginController : ApiController
    {
        // GET: api/Login
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("api/Login/isUNexist/{userName}")]
        // GET api/car/discounted/2017
        public bool IsUNexist(string userName)
        {
            DBservices db = new DBservices();
            return db.IsUserNameExist(userName);
        }
        [HttpPost]
        [Route("api/Login/addUser")]
        public string AddUser([FromBody]User u)
        {
            return u.AddUser();
        }
        [HttpGet]
        [Route("api/Login/signIn/{userName}/{password}")]
        public User SignIn(string userName, string password)
        {
            DBservices db = new DBservices();
            return db.IsSignIn(userName, password);
        }
        // GET: api/Login/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Login
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Login/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Login/5
        public void Delete(int id)
        {
        }
    }
}
