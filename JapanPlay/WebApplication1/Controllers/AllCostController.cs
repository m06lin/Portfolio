using MvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class AllCostController : Controller
    {
        //
        // GET: /AllCost/
        private CostController cost = new CostController();
        public ActionResult Index()
        {
            string accountid = cost.AccountID();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }
            List<MvcApplication.Models.TypeList> acNamelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> jobtypelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> costTypelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> areaTypelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> currTypelist = new List<TypeList>();
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            List<WebApplication.Models.DBModels.Job> joblist = db.Job.ToList();
            List<WebApplication.Models.DBModels.Account> accountlist = db.Account.ToList();
            MvcApplication.Models.TypeList defu = new TypeList();
            defu.ID = 0;
            defu.Name = "請選擇";
            acNamelist.Add(defu);
            costTypelist.Add(defu);
            areaTypelist.Add(defu);
            currTypelist.Add(defu);
            foreach (var account in accountlist)
            {
                MvcApplication.Models.TypeList name = new TypeList();
                name.Name = account.Name;
                name.ID = account.ID;
                acNamelist.Add(name);
            }
            foreach (var item in joblist)
            {
                MvcApplication.Models.TypeList type = new TypeList();
                int statusID = item.Joblevel;
                string Statusname = item.Jobname;
                type.ID = statusID;
                type.Name = Statusname;
                jobtypelist.Add(type);
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
            ViewBag.AcName = acNamelist;
            ViewBag.JobType = jobtypelist;
            ViewBag.CostType = costTypelist;
            ViewBag.AreaType = areaTypelist;
            ViewBag.CurrType = currTypelist;
            return View();
        }
       
        [HttpPost]
        public ActionResult CostDelete(int ID, int accountid)
        {
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            
            bool isSuccess = true;
            string Msg = "";
            try
            {
                WebApplication.Models.DBModels.Account account = db.Account.Where(x => x.ID == accountid).FirstOrDefault();
                WebApplication.Models.DBModels.Job userjob = db.Job.Where(x => x.ID == account.Job_Id).FirstOrDefault();
                if (account == null)
                {
                    isSuccess = false;
                    Msg = "請先登入!";
                }
                else
                {
                    //要被刪除的該筆資料
                    WebApplication.Models.DBModels.Cost deletecost = db.Cost.Where(x => x.ID == ID).FirstOrDefault();
                    //讀取創造筆資料的使用者
                    WebApplication.Models.DBModels.Account deletecostact = db.Account.Where(x => x.ID == deletecost.Account_Id).FirstOrDefault();
                    WebApplication.Models.DBModels.Job deletecostjob = db.Job.Where(x => x.ID == deletecostact.Job_Id).FirstOrDefault();
                    //比對權限
                    if (deletecostjob.Joblevel > userjob.Joblevel || deletecost.Account_Id == accountid)
                    {
                        db.Cost.Remove(deletecost);
                        db.SaveChanges();
                    }
                    else
                    {
                        isSuccess = false;
                        Msg = "權限不足!無法刪除";
                    }
                }
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
}
