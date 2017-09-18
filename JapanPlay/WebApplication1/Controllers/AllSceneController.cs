using MvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class AllSceneController : Controller
    {
        //
        // GET: /AllScene/
        private CostController cost = new CostController();
        public ActionResult Index()
        {
            string accountid = cost.AccountID();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }
            List< MvcApplication.Models.TypeList> acNamelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> jobtypelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> rankTypelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> sceneTypelist = new List<TypeList>();
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            List<WebApplication.Models.DBModels.Job> joblist = db.Job.ToList();
            List<WebApplication.Models.DBModels.Account> accountlist = db.Account.ToList();
            List<WebApplication.Models.DBModels.Scene> scenelist = db.Scene.ToList();
            MvcApplication.Models.TypeList defu = new TypeList();
            defu.ID = 0;
            defu.Name = "請選擇";
            acNamelist.Add(defu);
            rankTypelist.Add(defu);
            sceneTypelist.Add(defu);
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
            foreach (var scene in scenelist)
            {
                MvcApplication.Models.TypeList type = new TypeList();
                type.Name = scene.SceneName;
                type.ID = scene.ID;
                sceneTypelist.Add(type);
            }
            foreach (var item in Enum.GetValues(typeof(MvcApplication.Models.RankType)))
            {
                MvcApplication.Models.TypeList type = new TypeList();
                int statusID = (int)item;
                string Statusname = ((MvcApplication.Models.RankType)item).ToString();
                type.ID = statusID;
                type.Name = Statusname;
                rankTypelist.Add(type);
            }
            ViewBag.AcName = acNamelist;
            ViewBag.JobType = jobtypelist;
            ViewBag.RankType = rankTypelist;
            ViewBag.SceneType = sceneTypelist;
            return View();
          
        }

        [HttpPost]
        public ActionResult SceneDelete(int ID, int accountid)
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
                    WebApplication.Models.DBModels.Scenerank deletedata = db.Scenerank.Where(x => x.ID == ID).FirstOrDefault();
                    //讀取創造筆資料的使用者
                    WebApplication.Models.DBModels.Account deletedataact = db.Account.Where(x => x.ID == deletedata.Account_Id).FirstOrDefault();
                    WebApplication.Models.DBModels.Job deletedatajob = db.Job.Where(x => x.ID == deletedataact.Job_Id).FirstOrDefault();
                    //比對權限
                    if (deletedatajob.Joblevel > userjob.Joblevel || deletedata.Account_Id ==accountid)
                    {
                        db.Scenerank.Remove(deletedata);
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
