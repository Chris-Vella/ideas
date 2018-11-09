using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrightIdeas.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace BrightIdeas.Controllers
{
    using BrightIdeas.Models;
    using System.Security.Claims;
    public class IdeaController : Controller
    {
        private Context _context;
        public IdeaController(Context context){
            _context = context;
        }
        
        [HttpGet]
        [Route("bright_ideas")]
        public IActionResult Index (){
            int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null){
                return RedirectToAction("Index", "Home");
            }

            User loggedUser = _context.Users.SingleOrDefault(u => u.UserId == uId);
            ViewBag.LoggedUser = loggedUser;

            List<User> UsersList = _context.Users.ToList();
            ViewBag.Idea = UsersList;

            List<Post> IdeaList = _context.Posts.Include(p => p.Creator).Include(p => p.Likes).OrderByDescending(p => p.Likes.Count).ToList();
            ViewBag.Ideas = IdeaList;

            return View("Ideas");
        }

        [HttpPost]
        [Route("CreatePost")]
        public IActionResult CreatePost(string Content){
            int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null){
                return RedirectToAction("Index", "Home");
            }
            Post newPost = new Post{
                Content = Content,
                CreatorId = (int) uId,
            };
            _context.Add(newPost);
            _context.SaveChanges();
            return RedirectToAction("Index", "Idea");
        }

        [HttpGet]
        [Route("RemovePost/{PostId}")]
        public IActionResult RemovePost(int PostId){
            int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null){
                return RedirectToAction("Index", "Home");
            }

            Post DeletePost = _context.Posts.SingleOrDefault(p => p.PostId == PostId);
            
            if((int)uId == DeletePost.CreatorId){
                _context.Posts.Remove(DeletePost);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Idea");
        }

        [HttpGet]
        [Route("LikePost/{PostId}")]
        public IActionResult LikePost (int PostId){
            int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null){
                return RedirectToAction("Index", "Idea");
            }

            Like newLike = new Like{
                UserId = (int) uId,
                PostId = PostId,
                };
            _context.Add(newLike);
            _context.SaveChanges();

            return RedirectToAction("Index", "Idea");
        }

        [HttpGet]
        [Route("bright_ideas/{PostId}")]
        public IActionResult PostInfo(int PostId){
            int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null){
                return RedirectToAction("Index", "Home");
            }
            
            Post thisPost = _context.Posts.Include(p => p.Creator).Include(p => p.Likes).ThenInclude(like => like.User).SingleOrDefault(p => p.PostId == PostId);
            ViewBag.ThisPost = thisPost;

            //HOW DO I HANDLE THE LINQ QUERIES TO NOT REPEAT USERS WITH MY FOREACH STATEMENT
            //PROBABLY NEED TO ADD SOMETHING IN MODELS / USE DIFFERENT QUERIES / FUTILE

            return View("PostInfo");
        }

        [HttpGet]
        [Route("users/{UserId}")]
        public IActionResult UserInfo(int UserId){
            int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null){
                return RedirectToAction("Index", "Home");
            }

            User thisUser = _context.Users.Include(u => u.Likes).Include(u => u.MyPosts).SingleOrDefault(u => u.UserId == UserId);
            ViewBag.ThisUser = thisUser;

            return View("UserInfo");
        }
    }
}