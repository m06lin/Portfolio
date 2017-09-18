using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication.Models
{
    public class Member
    {
        public int ID { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime Create_Date { get; set; }
        public int Job_Id { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Interest { get; set; }
        public string Skill { get; set; }
        public string Introduction { get; set; }

    }
    public enum JobType
    {
        系統管理員=0,
        團長=1,
        元老級幹部=2,
        幹部=3,
        會員=4,
        黑名單=5
    }
}