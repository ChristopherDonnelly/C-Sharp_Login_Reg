using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Login_Register.Models;
using DbConnection;

namespace Login_Register.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbConnector _dbConnector;

        public HomeController(DbConnector connect)
        {
            _dbConnector = connect;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet]
        [Route("users")]
        public JsonResult GetUsers()
        {            
            List<Dictionary<string, object>> AllUsers = _dbConnector.Query("SELECT id, first_name, last_name, email FROM users");

            return Json(AllUsers);
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel model)
        {

            if(ModelState.IsValid)
            {
                User newUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                List<Dictionary<string, object>> user = _dbConnector.Query($"SELECT email FROM users WHERE email='{newUser.Email}'");
                model.Unique = user.Count();
                TryValidateModel(model);

                if(!ModelState.IsValid){
                    return View();
                }else{
                    _dbConnector.Execute($"INSERT INTO users (first_name, last_name, email, password, created_at, updated_at) VALUES ('{newUser.FirstName}', '{newUser.LastName}', '{newUser.Email}', '{newUser.Password}', now(), now())");

                    return RedirectToAction("success");
                }
            }

            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginViewModel model)
        {

            Console.WriteLine(ModelState.IsValid);
            List<Dictionary<string, object>> user = _dbConnector.Query($"SELECT * FROM users WHERE email='{model.Email}'");
            
            model.Found = user.Count() - 1;

            if(model.Found == 0){
                model.PasswordConfirmation = ((string)user[0].GetValueOrDefault("password") == model.Password)?0:1; 
            }

            TryValidateModel(model);

            Console.WriteLine("****************************************************************");
            Console.WriteLine($"*     {model.Found}                          *");
            Console.WriteLine($"*     {model.Email}                          *");
            Console.WriteLine($"*     {model.Password}           *");
            Console.WriteLine($"*     {model.PasswordConfirmation}           *");
            Console.WriteLine($"*     {ModelState.IsValid}           *");
            Console.WriteLine("****************************************************************");


            if(ModelState.IsValid)
            {

            //    User newUser = new User
            //     {
            //         FirstName = model.FirstName,
            //         LastName = model.LastName,
            //         Email = model.Email,
            //         Password = model.Password,
            //         CreatedAt = DateTime.Now,
            //         UpdatedAt = DateTime.Now
            //     };

                return RedirectToAction("success");
                
            }

            return View();            
        }

        
    }
}
