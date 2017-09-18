using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models.DBModels
{
     [Table("Cost")]
    public class Cost
    {
        [Key]
        public int ID { get; set; }
        public int Account_Id { get; set; }
        public string Cost_Name { get; set; }
        public int Spend { get; set; }
        public string Cost_Type { get; set; }
        public string Note { get; set; }
        public string Area { get; set; }
        public string Currency { get; set; }
        public DateTime Cost_Date { get; set; }
    }
}