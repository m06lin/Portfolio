using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication.Models
{
    public class SceneViewModel
    {
        public int ID { get; set; }
        public int Account_Id { get; set; }
        public string AccountName { get; set; }
        public TypeList Job_Level { get; set; }
        public TypeList Scene_Name { get; set; }
       // public int Spend { get; set; }
        public TypeList Rank_Type { get; set; }
        public string Note { get; set; }
        public DateTime Create_Date { get; set; }
    }
    public class SceneCreateViewModel
    {
        public int ID { get; set; }
        
        public string Scene_Name { get; set; }
       
        public TypeList Area { get; set; }
        
    }
    public enum RankType
    {
        一顆星 = 1,
        二顆星 = 2,
        三顆星 = 3,
        四顆星 = 4,
        五顆星 = 5,
        
    }
    
}