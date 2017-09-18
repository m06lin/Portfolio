using MvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace Application.Controllers
{
    public class CostController : Controller
    {
        //
        // GET: /Cost/
        public string AccountID()
        {
            try
            {
                string accountid = System.Web.HttpContext.Current.Request.Cookies["Account"].Value;
                return accountid;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public int AccountLevel()
        {
            int level = 6;
            string accountID = AccountID();
            WebApplication.Models.DBModels.DBContext db = null;
            try
            {
                if (accountID != "")
                {
                    db = new WebApplication.Models.DBModels.DBContext();
                    int actid_int = int.Parse(accountID);
                    var account = db.Account.Where(x => x.ID == actid_int);
                    if (account.Count() > 0)
                    {
                        var job = db.Job.Where(x => x.ID == account.FirstOrDefault().Job_Id);
                        if (job.Count() > 0)
                            level = job.FirstOrDefault().Joblevel;
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                db.Dispose();
            }
            return level;
        }
        public string AccountName()
        {
            WebApplication.Models.DBModels.DBContext db = null;
            try
            {
                string accountName = "";
                string accountID = AccountID();
                db = new WebApplication.Models.DBModels.DBContext();
                int actid = 0;
                if (accountID != "")
                    actid = int.Parse(accountID);
                var account = db.Account.Where(x => x.ID == actid);
                if (account.Count() > 0)
                    accountName = account.FirstOrDefault().Name;
                return accountName;
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                db.Dispose();
            }
        }
        public string JobName()
        {
            WebApplication.Models.DBModels.DBContext db = null;
            try
            {
                db = new WebApplication.Models.DBModels.DBContext();
                string jobName = "無";
                string actid = AccountID();
                int actid_int = int.Parse(actid);
                var account = db.Account.Where(x => x.ID == actid_int);
                if (account.Count() > 0)
                {
                    var job = db.Job.Where(x => x.ID == account.FirstOrDefault().Job_Id);
                    if (job.Count() > 0)
                        jobName = job.FirstOrDefault().Jobname;
                }
                return jobName;
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                db.Dispose();
            }
        }
        public ActionResult Index()
        {
            string accountid = AccountID();
            if (accountid=="")
            {
                return RedirectToAction("Index", "Account");
            }
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            List<MvcApplication.Models.TypeList> acNamelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> costTypelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> areaTypelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> currTypelist = new List<TypeList>();
            List<WebApplication.Models.DBModels.Account> accountlist = db.Account.ToList();
            MvcApplication.Models.TypeList defu = new TypeList();
            defu.ID = 0;
            defu.Name = "請選擇";
            acNamelist.Add(defu);
            costTypelist.Add(defu);
            areaTypelist.Add(defu);
            currTypelist.Add(defu);
            foreach(var account in accountlist)
            {
                MvcApplication.Models.TypeList name = new TypeList();
                name.Name = account.Name;
                name.ID = account.ID;
                acNamelist.Add(name);
            }
            foreach (var item in Enum.GetValues(typeof(MvcApplication.Models.CostType)))
            {
                MvcApplication.Models.TypeList type = new TypeList();
                int statusID = (int)item;
                string Statusname = ((MvcApplication.Models.CostType)item).ToString();
                type.ID = statusID;
                type.Name = Statusname;
                costTypelist.Add(type);
            }
            foreach (var item in Enum.GetValues(typeof(MvcApplication.Models.AreaType)))
            {
                MvcApplication.Models.TypeList type = new TypeList();
                int statusID = (int)item;
                string Statusname = ((MvcApplication.Models.AreaType)item).ToString();
                type.ID = statusID;
                type.Name = Statusname;
                areaTypelist.Add(type);
            }
            foreach (var item in Enum.GetValues(typeof(MvcApplication.Models.CurrencyType)))
            {
                MvcApplication.Models.TypeList type = new TypeList();
                int statusID = (int)item;
                string Statusname = ((MvcApplication.Models.CurrencyType)item).ToString();
                type.ID = statusID;
                type.Name = Statusname;
                currTypelist.Add(type);
            }
            ViewBag.AcName = acNamelist;
            ViewBag.CostType = costTypelist;
            ViewBag.AreaType = areaTypelist;
            ViewBag.CurrType = currTypelist;
            db.Dispose();
            return View();
        }
        public ActionResult Cost()
        {
            string accountid = AccountID();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }
            
            return View();
        }
        public ActionResult CostRead(int AcID, string Type)
        {
            List<MvcApplication.Models.CostViewModel> costList = new List<CostViewModel>();
            List<WebApplication.Models.DBModels.Cost> costdblist = new List<WebApplication.Models.DBModels.Cost>();
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            costdblist = db.Cost.ToList();
            List<WebApplication.Models.DBModels.Account> accountlist = db.Account.ToList();
            if (AcID != null && AcID > 0)
            {
                accountlist = accountlist.Where(x => x.ID == AcID).ToList();
                costdblist = costdblist.Where(x => x.Account_Id == AcID).ToList();
            }
            if (!string.IsNullOrEmpty(Type) && Type!="0")
            {
                costdblist = costdblist.Where(x => x.Cost_Type == Type).ToList();
            }
            if (costdblist.Count > 0)
            {
                List<int> costactid = costdblist.Select(x => x.Account_Id).ToList();
                accountlist = accountlist.Where(x => costactid.Contains(x.ID)).ToList();
            }
            foreach(var costdb in costdblist)
            {
                WebApplication.Models.DBModels.Account account = accountlist.Where(x => x.ID == costdb.Account_Id).FirstOrDefault();
                if (account != null)
                {
                    WebApplication.Models.DBModels.Job job = db.Job.Where(x => x.ID == account.Job_Id).FirstOrDefault();
                    if (job != null)
                    {
                        MvcApplication.Models.TypeList costtypelist = new TypeList();
                        MvcApplication.Models.TypeList areatypelist = new TypeList();
                        MvcApplication.Models.TypeList jobtypelist = new TypeList();
                        MvcApplication.Models.TypeList currtypelist = new TypeList();
                        costtypelist.ID = int.Parse(costdb.Cost_Type);
                        costtypelist.Name = ((MvcApplication.Models.CostType)costtypelist.ID).ToString();
                        areatypelist.ID = int.Parse(costdb.Area);
                        areatypelist.Name = ((MvcApplication.Models.AreaType)areatypelist.ID).ToString();
                        jobtypelist.ID = job.Joblevel;
                        jobtypelist.Name = ((MvcApplication.Models.JobType)jobtypelist.ID).ToString();
                        currtypelist.ID = int.Parse(costdb.Currency);
                        currtypelist.Name = ((MvcApplication.Models.CurrencyType)currtypelist.ID).ToString();
                        MvcApplication.Models.CostViewModel cost = new CostViewModel();
                        cost.ID = costdb.ID;
                        cost.Account_Id = costdb.Account_Id;
                        cost.Cost_Name = costdb.Cost_Name;
                        cost.Spend = costdb.Spend;
                        cost.Cost_Type = costtypelist;
                        cost.Note = costdb.Note;
                        cost.Area = areatypelist;
                        cost.Cost_Date = costdb.Cost_Date;
                        cost.AccountName = account.Name;
                        cost.Job_Level = jobtypelist;
                        cost.Currency = currtypelist;
                        costList.Add(cost);
                    }
                }
            }
            db.Dispose();
            return Json(costList, JsonRequestBehavior.AllowGet);
            //return PartialView("Cost", costList);
        }
        
        [HttpPost]
        public ActionResult CostSave()
        {
            bool isSuccess = true;
            string Msg = "";
            WebApplication.Models.DBModels.DBContext db = null;
            try
            {
                db = new WebApplication.Models.DBModels.DBContext();
                int accountid = int.Parse(this.AccountID());
                var costdblist = db.Cost.Where(x => x.Account_Id == accountid).ToList();
                var list = ControllerExtensions.DeserializeObject<List<MvcApplication.Models.CostViewModel>>(this, "CostList", ControllerExtensions.RequestType.POST);
                List<int> updatedata = list.Where(x => x.ID > 0).Select(x=>x.ID).ToList();
                if(costdblist.Count >0)
                {
                    var deletedata = costdblist.Where(x => !updatedata.Contains(x.ID));
                    if(deletedata.Count()>0) 
                       db.Cost.RemoveRange(deletedata);
                }
                foreach (var cost in list)
                {
                    WebApplication.Models.DBModels.Cost oldcostdb = costdblist.Where(x => x.ID == cost.ID).FirstOrDefault();
                    if (oldcostdb == null)
                    {
                        WebApplication.Models.DBModels.Cost costdb = new WebApplication.Models.DBModels.Cost();
                        costdb.ID = 0;
                        costdb.Account_Id = accountid;
                        costdb.Cost_Name = cost.Cost_Name;
                        costdb.Spend = cost.Spend;
                        costdb.Cost_Type = cost.Cost_Type.ID.ToString();
                        costdb.Note = cost.Note;
                        costdb.Area = cost.Area.ID.ToString();
                        costdb.Cost_Date = cost.Cost_Date;
                        costdb.Currency = cost.Currency.ID.ToString();
                        db.Cost.Add(costdb);
                    }
                    else
                    {
                        
                        oldcostdb.Cost_Name = cost.Cost_Name;
                        oldcostdb.Spend = cost.Spend;
                        oldcostdb.Cost_Type = cost.Cost_Type.ID.ToString();
                        oldcostdb.Note = cost.Note;
                        oldcostdb.Area = cost.Area.ID.ToString();
                        oldcostdb.Cost_Date = cost.Cost_Date;
                        oldcostdb.Currency = cost.Currency.ID.ToString();
                        
 
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccess = false;
                if (Msg == "")
                {
                    Msg = ex.ToString();
                }
            }
            finally
            {
                db.Dispose();
            }
            return Json(new { isSuccess = isSuccess, Msg = Msg }, JsonRequestBehavior.AllowGet);
        }



       
        
    }
    public static class ControllerExtensions
    {
        public enum RequestType
        {
            GET = 0,
            POST = 1
        }

        public static T DeserializeObject<T>(this Controller controller, string key, RequestType rType = RequestType.GET) where T : class
        {
            var value = "";// controller.HttpContext.Request.QueryString.Get(key);
            switch (rType)
            {
                case RequestType.POST:
                    value = controller.HttpContext.Request.Form.Get(key);
                    break;
                default:
                    value = controller.HttpContext.Request.QueryString.Get(key);
                    break;
            }

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Deserialize<T>(value);
        }

    }
}
