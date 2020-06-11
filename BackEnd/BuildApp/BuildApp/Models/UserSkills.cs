using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class UserSkills
    {
        string userName;
        int skillNum;

        public string UserName { get => userName; set => userName = value; }
        public int SkillNum { get => skillNum; set => skillNum = value; }

        public UserSkills(string userName, int skillNum)
        {
            UserName = userName;
            SkillNum = skillNum;
        }
        public UserSkills()
        {

        }
    }
}