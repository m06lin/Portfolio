using MvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class JapanPlayController : Controller
    {
        //
        // GET: /JapanPlay/
        CostController obj = new CostController();
        public ActionResult Index()
        {
            string accountid = obj.AccountID();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        public ActionResult PersonORAllGutsPV(string PV)
        {
            if (PV=="Personal")
            {
                return PartialView("PersonalPV");
            }
            else
                return PartialView("AllGutsPV");

        }
        public ActionResult PersonalPV()
        {
            string accountid = obj.AccountID();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }
            MvcApplication.Models.SceneViewModel model = new SceneViewModel();
            return PartialView("PersonalPV", model);
        }
        public ActionResult AllGutsPV()
        {
            string accountid = obj.AccountID();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }
            MvcApplication.Models.SceneViewModel model = new SceneViewModel();
            return PartialView("AllGutsPV",model);
        }
        public ActionResult Jobmanage()
        {
            string accountid = obj.AccountID();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }
            MvcApplication.Models.SceneViewModel model = new SceneViewModel();
            return PartialView("Jobmanage", model);
        }
        public ActionResult ScenemanagePV()
        {
            string accountid = obj.AccountID();
            if (accountid == "")
            {
                return RedirectToAction("Index", "Account");
            }
           
            List<MvcApplication.Models.TypeList> arealist = new List<TypeList>();
            for (int i = 1; i <= 5; i++ )
            {
                MvcApplication.Models.TypeList area = new TypeList();
                area.Name = ((MvcApplication.Models.AreaType)i).ToString();
                area.ID = i;
                arealist.Add(area);
            }
            ViewBag.Area = arealist;
            return View();
        }

        public ActionResult ScenemanageRead()
        {
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            var dblist = db.Scene.ToList();
            List<MvcApplication.Models.SceneCreateViewModel> scenelist = new List<SceneCreateViewModel>();
            foreach (var list in dblist)
            {
                MvcApplication.Models.SceneCreateViewModel scene = new MvcApplication.Models.SceneCreateViewModel();
                MvcApplication.Models.TypeList areaType = new MvcApplication.Models.TypeList();
                scene.ID = list.ID;
                areaType.ID = int.Parse(list.Area);
                areaType.Name = ((MvcApplication.Models.AreaType)areaType.ID).ToString() ;
                scene.Area = areaType;
                scene.Scene_Name = list.SceneName;
                scenelist.Add(scene);
            }
            db.Dispose();
            return Json(scenelist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SceneManageSave()
        {
            bool isSuccess = true;
            string Msg = "";
            WebApplication.Models.DBModels.DBContext db = null;
            try
            {
                db= new WebApplication.Models.DBModels.DBContext();
                var list = ControllerExtensions.DeserializeObject<List<MvcApplication.Models.SceneCreateViewModel>>(this, "sceneManageList", ControllerExtensions.RequestType.POST);
                foreach (var item in list)
                {
                    WebApplication.Models.DBModels.Scene olddbScene = db.Scene.Where(x => x.ID == item.ID).FirstOrDefault();
                    if (olddbScene == null)
                    {
                        WebApplication.Models.DBModels.Scene newdbScene = new WebApplication.Models.DBModels.Scene();
                        newdbScene.ID = 0;
                        newdbScene.Area = item.Area.ID.ToString();
                        newdbScene.SceneName = item.Scene_Name;
                        db.Scene.Add(newdbScene);
                    }
                    else
                    {
                        olddbScene.Area = item.Area.ID.ToString();
                        olddbScene.SceneName = item.Scene_Name;
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
