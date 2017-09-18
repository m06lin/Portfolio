using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication.Models
{
     
    public class JobViewModel
    {
       
        public int ID { get; set; }
        public MvcApplication.Models.TypeList Joblevel { get; set; }
        public string Jobname { get; set; }
    }
    public class AccountPowerViewModel
    {
        public int ID { get; set; }
        public string AccountNum { get; set; }
        public string Name { get; set; }
        public MvcApplication.Models.TypeList Jobname { get; set; } //Name = JobName+Joblevel  ID = Job_ID
    }
}