using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using BrightIdeas.Models;


namespace BrightIdeas.Controllers
{
    using BrightIdeas.Models;
    public class HomeController : Controller
    {
    private Context _context;
        public HomeController(Context context){
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index(){
            return View("Index");
        }
        
        [HttpPost]
        [Route("RegisterUser")]
        public IActionResult RegisterUser(RegistrationValidation newUser){
            if(ModelState.IsValid){
                
                User emailValidation = _context.Users.SingleOrDefault(u => u.Email == newUser.Email);

                if(emailValidation!= null){
                    ModelState.AddModelError("Email", "Email has been taken.");
                    return View("Index");
                }

                User aliasChecker = _context.Users.SingleOrDefault(u => u.Alias == newUser.Alias);

                if(aliasChecker!= null){
                    ModelState.AddModelError("Alias", "Alias has been taken.");
                    return View("Index");
                }
                
                User addUser = new User{
                    Name = newUser.Name,
                    Alias = newUser.Alias,
                    Email = newUser.Email,
                    Password = newUser.Password,
                };

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                addUser.Password = Hasher.HashPassword(addUser, newUser.Password);

                _context.Add(addUser);
                _context.SaveChanges();

                List<User> thisUser = _context.Users.Where(u => u.Name == addUser.Name && u.Alias == addUser.Alias && u.Email == addUser.Email).ToList();
                HttpContext.Session.SetInt32("UserId", (int)thisUser[0].UserId);

                return RedirectToAction("Index", "Idea");
            }
            return View("Index");
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index"); 
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login (LoginValidation userToCheck){
            if (ModelState.IsValid){

                User IsUser = _context.Users.SingleOrDefault(u => u.Email == userToCheck.Email);
                if(IsUser != null){

                    var Hasher = new PasswordHasher<User>();
                    if(0 != Hasher.VerifyHashedPassword(IsUser, IsUser.Password, userToCheck.Password)){
                        HttpContext.Session.SetInt32("UserId", (int)IsUser.UserId);
                        return RedirectToAction("Index", "Idea");
                    }
                }
            }
            return View("Index");
        }
    }
}