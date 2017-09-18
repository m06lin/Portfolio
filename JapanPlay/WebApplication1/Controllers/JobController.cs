using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication.Models;

namespace Application.Controllers
{
    public class JobController : Controller
    {
        Application.Controllers.CostController cost = new CostController();
        public ActionResult Index()
        {            
            string accountid = cost.AccountID();
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                int act_int = int.Parse(accountid);
                var account = db.Account.Where(x => x.ID == act_int).FirstOrDefault();
                if (account == null)
                    return RedirectToAction("Index", "Account");
                else
                {
                    var job = db.Job.Where(x => x.ID == account.Job_Id).FirstOrDefault();
                    if (job == null)
                        return RedirectToAction("Index", "Account");
                    else
                        if(job.Joblevel>1)
                            return RedirectToAction("Index", "Account");
                }
            }
            List<MvcApplication.Models.TypeList> joblevellist = new List<MvcApplication.Models.TypeList>();
            for (int i = 0; i < 6; i++)
            {
                MvcApplication.Models.TypeList level = new MvcApplication.Models.TypeList();
                level.Name = i.ToString() + " ( " + ((MvcApplication.Models.JobType)i).ToString() + " ) ";
                level.ID = i;
                joblevellist.Add(level);
            }
            ViewBag.JobLevel = joblevellist;
            return View();
        }
        public ActionResult AccountPower()
        {
            string accountid = cost.AccountID();
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                int act_int = int.Parse(accountid);
                var account = db.Account.Where(x => x.ID == act_int).FirstOrDefault();
                if (account == null)
                    return RedirectToAction("Index", "Account");
                else
                {
                    var job = db.Job.Where(x => x.ID == account.Job_Id).FirstOrDefault();
                    if (job == null)
                        return RedirectToAction("Index", "Account");
                    else
                        if (job.Joblevel > 1)
                            return RedirectToAction("Index", "Account");
                }
            }
            var joblist = db.Job.ToList();
            List<MvcApplication.Models.TypeList> joblevellist = new List<MvcApplication.Models.TypeList>();
            foreach(var item in joblist)
            {
                MvcApplication.Models.TypeList level = new MvcApplication.Models.TypeList();
                level.Name = item.Jobname+ " (" + item.Joblevel + ") ";
                level.ID = item.ID;
                joblevellist.Add(level);
            }
            db.Dispose();
            ViewBag.JobName = joblevellist;
            return View();
        }
        public ActionResult JobRead()
        {
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            var dbjoblist = db.Job.ToList();
            List<MvcApplication.Models.JobViewModel> joblist = new List<MvcApplication.Models.JobViewModel>();
            foreach (var dbjob in dbjoblist)
            {
                MvcApplication.Models.JobViewModel job = new MvcApplication.Models.JobViewModel();
                MvcApplication.Models.TypeList level = new MvcApplication.Models.TypeList();
                job.ID = dbjob.ID;
                level.ID = dbjob.Joblevel;
                level.Name = dbjob.Joblevel.ToString() + " ( " + ((MvcApplication.Models.JobType)level.ID).ToString() + " ) ";
                job.Joblevel = level;
                job.Jobname = dbjob.Jobname;
                joblist.Add(job);
            }
            db.Dispose();
            return Json(joblist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccountPowerRead(string Keyword, int Type)
        {
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            List<MvcApplication.Models.AccountPowerViewModel> result = new List<AccountPowerViewModel>();
            IEnumerable<MvcApplication.Models.AccountPowerViewModel> list = (from account in db.Account
                                                                       join job in db.Job on account.Job_Id equals job.ID
                                                                       select new MvcApplication.Models.AccountPowerViewModel
                                                                       {
                                                                           ID = account.ID,
                                                                           AccountNum = account.AccountNum,
                                                                           Name = account.Name,
                                                                           Jobname = new MvcApplication.Models.TypeList {Name = job.Jobname+" ("+job.Joblevel+")",ID = job.ID }

                                                                       }).AsQueryable();
            if (Type == 1)
            {
                list = list.Where(x => x.Name.Contains(Keyword));
            }
            else if (Type == 2)
            {
                list = list.Where(x => x.AccountNum.Contains(Keyword));
            }
            else 
            {
               
            }
            result = list.ToList();
            db.Dispose();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult JobSave()
        {
            bool isSuccess = true;
            string Msg = "";
            WebApplication.Models.DBModels.DBContext db = null;
            try
            {
                db = new WebApplication.Models.DBModels.DBContext();
                var list = ControllerExtensions.DeserializeObject<List<MvcApplication.Models.JobViewModel>>(this, "JobList", ControllerExtensions.RequestType.POST);
                foreach (var item in list)
                {
                    WebApplication.Models.DBModels.Job olddbjob = db.Job.Where(x => x.ID == item.ID).FirstOrDefault();
                    if (olddbjob == null)
                    {
                        WebApplication.Models.DBModels.Job newdbjob = new WebApplication.Models.DBModels.Job();
                        newdbjob.ID = 0;
                        newdbjob.Joblevel = item.Joblevel.ID;
                        newdbjob.Jobname = item.Jobname;
                        db.Job.Add(newdbjob);
                    }
                    else
                    {
                        olddbjob.Joblevel = item.Joblevel.ID;
                        olddbjob.Jobname = item.Jobname;
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
        [HttpPost]
        public ActionResult AccountPowerSave()
        {
            bool isSuccess = true;
            string Msg = "";
            WebApplication.Models.DBModels.DBContext db = null;
            try
            {
                db = new WebApplication.Models.DBModels.DBContext();
                var list = ControllerExtensions.DeserializeObject<List<MvcApplication.Models.AccountPowerViewModel>>(this, "AccountPowerList", ControllerExtensions.RequestType.POST);
                foreach (var item in list)
                {
                    WebApplication.Models.DBModels.Account olddbaccount = db.Account.Where(x => x.ID == item.ID).FirstOrDefault();
                    olddbaccount.Job_Id = item.Jobname.ID;
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
}