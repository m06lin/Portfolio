using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.CompilerServices;
using MvcApplication.Models;

namespace Application.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public CostController obj = new CostController();
        public ActionResult Index()
        {
            //MvcApplication.Controllers.CostController cost =new CostController();
            //string accountid = cost.AccountID();
            //if (accountid == "")
            //{
            //    return RedirectToAction("Index", "Account");
            //}
            return View();
        }
        public ActionResult Login()
        {
            return View();
            

        }
        [HttpPost]
        public ActionResult Login(string account,string password)
        {
            if(string.IsNullOrEmpty(account)||string.IsNullOrEmpty(password))
                return Json(new { isSuccess = false, Msg = "帳號密碼皆為必填欄位!" }, JsonRequestBehavior.AllowGet);
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            //撈DB資料比對
            var member = db.Account.Where(x=> x.AccountNum == account&&x.Password == password).FirstOrDefault();
            if (member == null)
            {
                return Json(new { isSuccess = false, Msg = "無效的帳號或密碼!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Response.Cookies["Account"].Value = member.ID.ToString();
            }
            
             db.Dispose();
            
            return Json(new { isSuccess = true, Msg = "" }, JsonRequestBehavior.AllowGet);


        }
        public ActionResult Logout()
        {
            HttpCookie User = new HttpCookie("Account");
            User.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(User);
            return RedirectToAction("Login", "Account");
            
      
        }
        public ActionResult ForgetPassWord()
        {
            return View();
        }
         [HttpPost]
        public ActionResult ForgetPassWord(string ForgetAccountNum, string ForgetEmail)
        {
            bool isSuccess = true;
            string Msg = "";
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            try
            {
                WebApplication.Models.DBModels.Account account = db.Account.Where(x => x.AccountNum == ForgetAccountNum).FirstOrDefault();
                if (account != null)
                {
                    
                    account.Password = "iamnowb";
                    
                    db.SaveChanges();
                    Msg = "密碼修改為預設，iamnowb";
                }
                else
                {
                    isSuccess = false;
                    Msg = "忘記密碼傳送失敗!";
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
         public ActionResult Register()
         {
             return View();
         }
        [HttpPost]
         public ActionResult Register(string registerAC, string registerPW, string registerEM, string registerName, string registerSex, string registerPhone)
        {
            bool isSuccess = false;
            string Msg = "";
            WebApplication.Models.DBModels.Account account = new WebApplication.Models.DBModels.Account();
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            if (string.IsNullOrEmpty(registerAC))
                Msg += "帳號為必填!\n";
            if (string.IsNullOrEmpty(registerPW))
                Msg += "密碼為必填!\n";
            if (string.IsNullOrEmpty(registerEM))
                Msg += "信箱為必填!\n";
            if (string.IsNullOrEmpty(registerName))
                Msg += "姓名為必填!\n";
            if (string.IsNullOrEmpty(registerPhone))
                Msg += "電話為必填!\n";
            if (Msg != "")
                throw new Exception(Msg);
            try
            {
                WebApplication.Models.DBModels.Account olddata = db.Account.Where(x => x.AccountNum == registerAC).FirstOrDefault();
                if (olddata == null)
                {
                    account.AccountNum = registerAC;
                    account.Create_Date = DateTime.Now;
                    account.Email = registerEM;
                    account.Interest = "";
                    account.Introduction = "";
                    account.Job_Id = 9;
                    account.Sex = registerSex;
                    account.Password = registerPW;
                    account.Name = registerName;
                    account.Skill = "";
                    account.Phone = registerPhone;
                    db.Account.Add(account);
                    db.SaveChanges();
                    isSuccess = true;


                }
                else
                {
                    isSuccess = false;
                    Msg = "註冊失敗!帳號重複!";
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                if(Msg =="")
                   Msg = "註冊失敗!";
            }
            finally
            {
                db.Dispose();
            }
            return Json(new { isSuccess = isSuccess, Msg = Msg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditPersonInfo()
        {
            string accountid = obj.AccountID();
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            string actID = obj.AccountID();
            int id=0;
            if (actID != "")
                id = int.Parse(actID);
            MvcApplication.Models.Member member = new Member();
            WebApplication.Models.DBModels.Account account = new WebApplication.Models.DBModels.Account();
            List<WebApplication.Models.DBModels.Account> accountlist = new List<WebApplication.Models.DBModels.Account>();
            accountlist = db.Account.ToList();

            account = accountlist.Where(x => x.ID == id).FirstOrDefault();
            db.Dispose();
            return View(account);
        }
        [HttpPost]
        public ActionResult EditPersonInfoUpdate(int ID, string Name, string Password, string Sex, string Interest, string Skill, string Introduction)
        {
            bool isSuccess = true;
            string Msg = "";
            WebApplication.Models.DBModels.DBContext db = new WebApplication.Models.DBModels.DBContext();
            try
            {
                WebApplication.Models.DBModels.Account account = db.Account.Where(x => x.ID == ID).FirstOrDefault();
                if (account != null)
                {
                    account.Name = Name.Trim();
                    account.Password = Password.Trim();
                    account.Sex = Sex;
                    account.Interest = Interest.Trim();
                    account.Skill = Skill.Trim();
                    account.Introduction = Introduction.Trim();
                    db.SaveChanges();
                }
                else
                {
                    isSuccess = false;
                    Msg = "個人資料修改失敗!";
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
