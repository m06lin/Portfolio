using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models.DBModels
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int ID { get; set; }
        public string AccountNum { get; set; }
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
}