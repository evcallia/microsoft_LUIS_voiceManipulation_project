using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using theWall.Models;
using theWall.Factory;
using Microsoft.AspNetCore.Http;
using CryptoHelper;
using System.ComponentModel.DataAnnotations;

namespace theWall.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserRepository userRepository;
        private readonly MessageRepository messageRepository;
        private readonly CommentRepository commentRepository;

        public HomeController()
        {
            userRepository = new UserRepository();
            messageRepository = new MessageRepository();
            commentRepository = new CommentRepository();
        }


        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {   
            if(TempData["errors"] != null){
                ViewBag.errors = TempData["errors"];
            }
            return View("index");
        }

        [HttpGet]
        [Route("/success")]
        public IActionResult Success()
        {   
            if(HttpContext.Session.GetInt32("id") != null){
                User user = userRepository.FindByID((int)HttpContext.Session.GetInt32("id"));
                ViewBag.name = user.first_name;
                ViewBag.user_id = HttpContext.Session.GetInt32("id");

                ViewBag.comments = commentRepository.FindAll();

                return View("wall", messageRepository.FindAll());
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User newUser)
        {   
            if(ModelState.IsValid){
                //check to see if email already exists
                if(userRepository.FindByEmail(newUser.email) == null){
                    newUser.encPass = Crypto.HashPassword(newUser.password);
                    userRepository.Add(newUser);
                    // return RedirectToAction("Login", new {email = newUser.email, password = newUser.password});
                    return Login(newUser.email, newUser.password);
                }else{
                    TempData["errors"] = new List<string>{"Email is already in use"};
                }
            }else{
                List<string> errors = new List<string>();
                foreach(var error in ModelState.Values){
                    if(error.Errors.Count > 0){
                        errors.Add(error.Errors[0].ErrorMessage);
                    }
                }
                TempData["errors"] = errors;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {   
            User user = userRepository.FindByEmail(email);
            if(user != null){
                if(Crypto.VerifyHashedPassword(user.password, password)){
                    HttpContext.Session.SetInt32("id", user.id);
                    return RedirectToAction("Success");
                }else{
                    TempData["errors"] = new List<string>{"Wrong password"};
                }
            }else{
                TempData["errors"] = new List<string>{"Email invalid"};
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("new-message")]
        public IActionResult NewMessage(Message newMessage)
        {   
            messageRepository.Add(newMessage);
            return RedirectToAction("Success");
        }

        [HttpPost]
        [Route("new-comment")]
        public IActionResult NewComment(Comment newComment)
        {   
            commentRepository.Add(newComment);
            return RedirectToAction("Success");
        }

        [HttpGet]
        [Route("delete-message/{message_id}")]
        public IActionResult DeleteMessage(int message_id)
        {   
            if(HttpContext.Session.GetInt32("id") == null){
                return RedirectToAction("Index");
            }

            messageRepository.Delete(message_id);
            return RedirectToAction("Success");
        }

        [HttpGet]
        [Route("delete-comment/{comment_id}")]
        public IActionResult DeleteComment(int comment_id)
        {   
            if(HttpContext.Session.GetInt32("id") == null){
                return RedirectToAction("Index");
            }

            commentRepository.Delete(comment_id);
            return RedirectToAction("Success");
        }


        [HttpGet]
        [Route("logoff")]
        public IActionResult Logoff()
        {   
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }



    
}
