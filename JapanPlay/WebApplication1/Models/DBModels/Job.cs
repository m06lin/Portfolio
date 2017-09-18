using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models.DBModels
{
     [Table("Job")]
    public class Job
    {
        [Key]
        public int ID { get; set; }
        public int Joblevel { get; set; }
        public string Jobname { get; set; }
    }
}