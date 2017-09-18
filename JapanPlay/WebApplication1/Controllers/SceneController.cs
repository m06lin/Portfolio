using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MvcApplication.Models;
namespace Application.Controllers
{
    public class SceneController : Controller
    {
        //
        // GET: /Scene/
        public CostController obj = new CostController();
        

        public ActionResult Index()
        {
            string accountid = obj.AccountID();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }

            List<MvcApplication.Models.TypeList> acNamelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> rankTypelist = new List<TypeList>();
            List<MvcApplication.Models.TypeList> sceneTypelist = new List<TypeList>();
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
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
            ViewBag.RankType = rankTypelist;
            ViewBag.SceneType = sceneTypelist;
            return View();
        }

        public ActionResult Scene()
        {

            return View();
        }
        public ActionResult SceneRead(int AcID, string Type)
        {
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            List<WebApplication.Models.DBModels.Scenerank> scenerankdblist = new List<WebApplication.Models.DBModels.Scenerank>();
            List<MvcApplication.Models.SceneViewModel> sceneList = new List<SceneViewModel>();
            try
            {
                int rank = int.Parse(Type);
                scenerankdblist = db.Scenerank.ToList();
                List<WebApplication.Models.DBModels.Account> accountlist = db.Account.ToList();
                if (AcID != null && AcID > 0)
                {
                    accountlist = accountlist.Where(x => x.ID == AcID).ToList();
                    scenerankdblist = scenerankdblist.Where(x => x.Account_Id == AcID).ToList();
                }
                if (!string.IsNullOrEmpty(Type) && Type != "0")
                {
                    scenerankdblist = scenerankdblist.Where(x => x.Rank == rank).ToList();
                }
                if (scenerankdblist.Count > 0)
                {
                    List<int> costactid = scenerankdblist.Select(x => x.Account_Id).ToList();
                    accountlist = accountlist.Where(x => costactid.Contains(x.ID)).ToList();
                }
                foreach(var scenerank in scenerankdblist)
                {
                    WebApplication.Models.DBModels.Account account = accountlist.Where(x => x.ID == scenerank.Account_Id).FirstOrDefault();
                    if (account != null)
                    {
                        WebApplication.Models.DBModels.Job job = db.Job.Where(x => x.ID == account.Job_Id).FirstOrDefault();
                        if (job != null)
                        {
                            WebApplication.Models.DBModels.Scene scene = db.Scene.Where(x=>x.ID == scenerank.Scene_Id).FirstOrDefault();
                            if (scene != null)
                            {
                                MvcApplication.Models.TypeList ranktypelist = new TypeList();
                                MvcApplication.Models.TypeList jobtypelist = new TypeList();
                                MvcApplication.Models.TypeList scenetypelist = new TypeList();
                                ranktypelist.ID = scenerank.Rank;
                                ranktypelist.Name = ((MvcApplication.Models.RankType)ranktypelist.ID).ToString();
                                jobtypelist.ID = job.Joblevel;
                                jobtypelist.Name = ((MvcApplication.Models.JobType)jobtypelist.ID).ToString();
                                scenetypelist.ID = scenerank.Scene_Id;
                                scenetypelist.Name = scene.SceneName;
                                MvcApplication.Models.SceneViewModel sceneview = new SceneViewModel();
                                sceneview.ID = scenerank.ID;
                                sceneview.Account_Id = scenerank.Account_Id;
                                sceneview.Scene_Name = scenetypelist;
                                sceneview.Rank_Type = ranktypelist;
                                sceneview.Note = scenerank.Note;
                                sceneview.Create_Date = scenerank.Create_Date;
                                sceneview.AccountName = account.Name;
                                sceneview.Job_Level = jobtypelist;
                                sceneList.Add(sceneview);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                db.Dispose();
            }
            return Json(sceneList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SceneSave()
        {
            bool isSuccess = true;
            string Msg = "";
            WebApplication.Models.DBModels.DBContext db = null;
            try
            {
                db = new WebApplication.Models.DBModels.DBContext();
                
                int accountid = int.Parse(obj.AccountID());
                var scenedblist = db.Scenerank.Where(x => x.Account_Id == accountid).ToList();
                var list = ControllerExtensions.DeserializeObject<List<MvcApplication.Models.SceneViewModel>>(this, "SceneList", ControllerExtensions.RequestType.POST);
                List<int> updatedata = list.Where(x => x.ID > 0).Select(x => x.ID).ToList();
                if (scenedblist.Count > 0)
                {
                    var deletedata = scenedblist.Where(x => !updatedata.Contains(x.ID));
                    if (deletedata.Count() > 0)
                        db.Scenerank.RemoveRange(deletedata);
                }
                
                foreach (var item in list)
                {
                    WebApplication.Models.DBModels.Scenerank olddata = db.Scenerank.Where(x => x.ID == item.ID).FirstOrDefault();
                    if (olddata != null)
                    {
                        olddata.Create_Date = item.Create_Date;
                        olddata.Scene_Id = item.Scene_Name.ID;
                        olddata.Rank = item.Rank_Type.ID;
                        olddata.Note = item.Note;

                    }
                    else
                    {
                        WebApplication.Models.DBModels.Scenerank newdata = new WebApplication.Models.DBModels.Scenerank();
                        newdata.ID = 0;
                        newdata.Create_Date = item.Create_Date;
                        newdata.Note = item.Note;
                        newdata.Rank = item.Rank_Type.ID;
                        newdata.Scene_Id = item.Scene_Name.ID;
                        newdata.Account_Id = accountid;
                        db.Scenerank.Add(newdata);
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

    
}
