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
    public class HomeController : Controller
    {
        private MyContext _context;
 
        public HomeController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            LoginView loginReg = new LoginView()
            {
                regUser = new User(),
                logUser = new Login(),
            };
            if(HttpContext.Session.GetInt32("User") != null)
            {
                return RedirectToAction("Ideas");
            }
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(LoginView DataForm)
        {
            User user = DataForm.regUser; 
            User check = _context.User.SingleOrDefault(u => u.Email == user.Email);
            if (check != null)
            {
                ModelState.AddModelError("regUser.Email", "Email already in use");
                return View("Index",DataForm);
            }
            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                _context.Add(user);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("User", user.id);
                return RedirectToAction("Ideas");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginView FormData)
        {
            Login loginUser = FormData.logUser;
            var user = _context.User.SingleOrDefault(u => u.Email == loginUser.Email);
            if(user != null && loginUser.Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(user, user.Password, loginUser.Password))
                {
                    HttpContext.Session.SetInt32("User", user.id);
                    return RedirectToAction("Ideas");
                }
            }
            ModelState.AddModelError("logUser.Password", "Username and password do not match");
            return View("Index", FormData);
        }
        [HttpGet("ideas")]
        public IActionResult Ideas()
        {
            int? id = HttpContext.Session.GetInt32("User");
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            User user = _context.User.SingleOrDefault(u => u.id == (int)id);
            ViewModel Views = new ViewModel()
            {
                User = user,
                AllIdeas = _context.Idea
                            .Include(l => l.LikedBy)
                            .ThenInclude(u => u.LikedByUser)
                            .OrderByDescending(x => x.LikedBy.Count)
                            .ToList(),
                Idea = new Idea()
            };
            return View(Views);
        }

        [HttpPost("share")]
        public IActionResult Share(ViewModel FormData)
        {
            System.Console.WriteLine(FormData.Idea.Content);
            Idea idea = FormData.Idea;
            int? id = HttpContext.Session.GetInt32("User");
            User user = _context.User.SingleOrDefault(u => u.id == (int)id);
            if(ModelState.IsValid)
            {
                idea.Userid = (int)id;
                idea.Author = user;
                _context.Idea.Add(idea);
                _context.SaveChanges();
                user.UsersIdeas.Add(idea);
                return RedirectToAction("Ideas");            
            }
            else
            {
                ViewModel Views = new ViewModel()
                {
                    User = user,
                    AllIdeas = _context.Idea
                                .Include(l => l.LikedBy)
                                .ThenInclude(u => u.LikedByUser)
                                .OrderByDescending(x => x.LikedBy.Count)
                                .ToList(),
                    Idea = new Idea()
                };
                return View("Ideas", Views);
            }
        }

        [HttpGet]
        [Route("like/{num}")]
        public IActionResult Like(int num)
        {
            int? id = HttpContext.Session.GetInt32("User");
            User user = _context.User.SingleOrDefault(u => u.id == (int)id);
            Idea idea = _context.Idea.SingleOrDefault(i => i.id == (int)num);
            Like like = new Like()
            {
                Userid = user.id,
                Ideaid = idea.id,
                LikedByUser = user,
                LikedIdea = idea
            };
            _context.Like.Add(like);
            _context.SaveChanges();
            user.LikedIdeas.Add(like);
            _context.SaveChanges();
            return RedirectToAction("Ideas");
        }

        [HttpGet]
        [Route("delete/{num}")]
        public IActionResult Delete(int num)
        {
            if(HttpContext.Session.GetInt32("User") == null)
            {
                return RedirectToAction("Index"); 
            }
            Idea idea = _context.Idea.SingleOrDefault(i => i.id == (int)num);
            _context.Idea.Remove(idea);
            _context.SaveChanges();
            return RedirectToAction("Ideas");
        }

        [HttpGet("idea/{num}")]
        public IActionResult IdeaID(int num)
        {
            Idea idea = _context.Idea.SingleOrDefault(i =>i.id == num);
            ViewModel Views = new ViewModel()
            {
                Idea = idea,
                LikedBy = _context.Idea
                        .Include(l => l.LikedBy)
                        .ThenInclude(u => u.LikedByUser)
                        .ToList()
            };
            return View(Views);
        }

        [HttpGet("user/{num}")]
        public IActionResult UserID(int num)
        {
            User user = _context.User.SingleOrDefault(u =>u.id == num);
            ViewModel Views = new ViewModel()
            {
                User = user,
                UsersIdeas = _context.Idea
                        .Where(i => i.Userid == user.id)
                        .ToList(),
                LikedIdeas = _context.Like
                        .Where(l => l.LikedByUser == user)
                        .ToList()
            };
            return View(Views);
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
