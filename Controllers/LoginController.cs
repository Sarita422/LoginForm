using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UserAuthentication.Models;

namespace UserAuthentication.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            LoginModelManager loginModelManager = new LoginModelManager();
            loginModel = loginModelManager.UserAuth(loginModel);
            if (loginModel.Isvalid == 1)
            {
                Session["UserEmail"] = loginModel.UserEmail;
                FormsAuthentication.SetAuthCookie(loginModel.Username, false);

                var authTicket = new FormsAuthenticationTicket(1, loginModel.UserEmail, DateTime.Now, DateTime.Now.AddMinutes(20), false, loginModel.UserRole);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);


                HttpContext.Response.Cookies.Add(authCookie);
                return RedirectToAction("Index", "Home");

                
            }
            else
            {
                loginModel.LoginErrorMessage = "Wrong Username or Password";
                return View("Login", loginModel);
            }
        }
    }
}