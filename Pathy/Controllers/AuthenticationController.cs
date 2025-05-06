using Microsoft.AspNetCore.Mvc;
using Pathy.Models;
using Pathy_BusinessAccessLayer;

namespace Pathy.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(clsLoginViewModel model)
        {


            if(!ModelState.IsValid)
            {
                return View(model);

            } else if(!clsUser.doesUserExistByUsername(model.username))
            {
                ModelState.AddModelError("username", "Username does not exist !");
                return View(model);
            }


            clsUser user = clsUser.loginByUsernameAndPassword(model.username, model.password);
            if(user == null)
            {
                ModelState.AddModelError("username", "Incorrect password");
                return View(model);
            }
            

            clsGlobal.logedInUser = user;
            clsGlobal.logedInUser.loadUserCompositeObjects();
            clsGlobal.account = clsAccount.getAccountByUserID(user.userID);
            clsGlobal.account.loadCompositeObjects();
            
          
            clsLoginLog loginLog = new clsLoginLog();
            loginLog.mode = clsLoginLog.enMode.AddNew;
            loginLog.accountID = clsGlobal.account.accountID;
            loginLog.loginDateTime = DateTime.Now;

            loginLog.save();

            return RedirectToAction("TimeLine","TimeLine");
        }





        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(clsRegisterViewModel model)
        {
          
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            if (clsUser.doesUserExistByUsername(model.username))
            {
                Console.WriteLine("DEBUG: Reached");
                ModelState.AddModelError("username", "Username already exist");
                return View(model);
            }

            if(clsPerson.doesEmailExist(model.email))
            {
                ModelState.AddModelError("email", "Email already exist");
                return View(model);
            }



            clsPerson person = new clsPerson();
            person.firstName = model.firstName;
            person.secondName = model.secondName;
            person.thirdName = model.thirdName;
            person.lastName = model.lastName;
            person.email = model.email;
            person.phoneNumber = model.phoneNumber;
            person.gender = model.gender ?? -1;


            if(person.save())
            {
                clsUser user = new clsUser();
                user.personID = person.personID;
                user.username = model.username;
                user.password = model.password;
                user.photoURL = "";

                if (user.save())
                {
                    clsAccount account = new clsAccount();
                    account.userID = user.userID;
                    account.accountTypeID = (int)clsAccountType.enType.Normal;
                    account.accountNumber = Guid.NewGuid().ToString();

                    if(account.save())
                    {
                        TempData["Message"] = "New account created successfully!";                        
                    } else
                    {
                        TempData["Message"] = "Failed to create new account";
                    }

                } else
                {
                    TempData["Message"] = "Failed to create new account";
                }

            } else
            {
                TempData["Message"] = "Failed to create new account";
            }
                                                        
            return RedirectToAction("Login", "Authentication"); ;
        }

    }
}
