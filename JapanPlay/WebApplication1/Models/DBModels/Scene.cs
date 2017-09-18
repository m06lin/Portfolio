using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models.DBModels
{
     [Table("Scene")]
    public class Scene
    {
        [Key]
        public int ID { get; set; }
        public int Account_Id { get; set; }
        public string SceneName { get; set; }
        public string Area { get; set; }
    }
}