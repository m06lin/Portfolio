using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication.Models
{
    public class CostViewModel
    {
        public int ID { get; set; }
        public int Account_Id { get; set; }
        public string AccountName { get; set; }
        public TypeList Job_Level { get; set; }
        public string Cost_Name{ get; set; }
        public int Spend { get; set; }
        public TypeList Cost_Type { get; set; }
        public string Note { get; set; }
        public TypeList Area { get; set; }
        public TypeList Currency { get; set; }
        public DateTime Cost_Date { get; set; }
    }
    public class TypeList
    {
        public int ID { get; set; }
        public string Name { get; set; }
 
    }
    public enum CostType
    {
        食=1,
        玩=2,
        爽=3,
        交通=4,
        伴手禮=5,
        住=6,
        其他=7
    }
    public enum AreaType
    {
        大阪=1,
        奈良=2,
        京都=3,
        機場=4,
        奇怪地方=5
    }
    public enum CurrencyType
    {
        台幣=1,
        日幣=2
    }
}