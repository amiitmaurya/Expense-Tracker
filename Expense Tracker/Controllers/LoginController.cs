using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext _context;
        public LoginController(DatabaseContext context)  // dependency injection of the database context
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginSignup());
        }


        // ================= Register =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(LoginSignup model)
        {
            var user = model.SignupForm;

            // Password & Repassword check
            if (user.Password != user.ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                // Signup panel open rakho
                ViewBag.ShowSignup = true;

            }

            // Email already exists check
            var existingEmail = _context.Signups
                .FirstOrDefault(x => x.Email == user.Email);

            if (existingEmail != null)
            {
                ViewBag.Error = "Email already registered";
                ViewBag.ShowSignup = true;
            }

            // Save into database
            _context.Signups.Add(user);
            _context.SaveChanges();

            // Success message
            ViewBag.Success = "Registration Successful";

            // SAME PAGE OPEN AGAIN
            return View("Login", new LoginSignup());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginSignup model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Signups
                    .FirstOrDefault(x => x.Email == model.LoginForm.Username && x.Password == model.LoginForm.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    // Login successful, redirect to dashboard or home page
                    return RedirectToAction("Index", "Transaction");
                }
                else
                {
                    ViewBag.Error = "Invalid email or password";
                }
            }

            return View("Login", new LoginSignup());
        }


        // ================= LOGOUT =================
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");

        }
    }
}

