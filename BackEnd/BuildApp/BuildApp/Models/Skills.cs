using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class Skills
    {
        int skillNum;
        string skillName;

        public int SkillNum { get => skillNum; set => skillNum = value; }
        public string SkillName { get => skillName; set => skillName = value; }

        public Skills(int skillNum, string skillName)
        {
            SkillNum = skillNum;
            SkillName = skillName;
        }
        public Skills()
        {

        }

        public List<Skills> GetSkills()
        {
            DBservices dB = new DBservices();
            return dB.GetSkills();
        }

       
    }
}