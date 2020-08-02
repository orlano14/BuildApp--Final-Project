using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class Token
    {
        string userName;
        string tokenString;

        public Token(string userName, string tokenString)
        {
            UserName = userName;
            TokenString = tokenString;
        }
        public Token()
        {

        }
        public string UserName { get => userName; set => userName = value; }
        public string TokenString { get => tokenString; set => tokenString = value; }

        public string IsTokenExist(string userName, string token)
        {
            DBservices db = new DBservices();
            if (db.IsTokenExist(userName))
            {
                return db.UpdateToken(userName, token);
            }
            else
            {
                return db.InsertToken(userName, token);
            }
        }
    }
}