using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models.DBModels
{
     [Table("Scenerank")]
    public class Scenerank
    {
        [Key]
        public int ID { get; set; }
        public int Account_Id { get; set; }
        public int Scene_Id { get; set; }
        public int Rank { get; set; }
        public string Note { get; set; }
        public DateTime Create_Date { get; set; }
    }
}