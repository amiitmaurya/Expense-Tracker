using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly DatabaseContext _context;
        public ChangePasswordController(DatabaseContext context)  // dependency injection of the database context
        {
            _context = context;
        }
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var model = new ChangePassword
            {
                Email = HttpContext.Session.GetString("UserEmail")
            };
            return View("ChangePassword", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
                return View("ChangePassword", model);
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                TempData["Error"] = "Session expired. Please login again";
                return RedirectToAction("Login", "Login");

            }
            string Email = HttpContext.Session.GetString("UserEmail");

            // ✅ SAME TABLE AS SIGNUP
            var user = _context.Signups.FirstOrDefault(u => u.Email == Email);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return View("ChangePassword", model);
            }
            // Old password check
            if (user.Password != model.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "Old password is incorrect");
                return View("ChangePassword", model);
            }
            //  NEW & CONFIRM PASSWORD CHECK (POPUP)
            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "New password and confirm password do not match");
                return View("ChangePassword", model);
            }
            // ❌ Old & New same check (extra safety)
            if (model.OldPassword == model.NewPassword)
            {
                TempData["Error"] = "New password must be different from old password";
                return RedirectToAction("ChangePassword");
            }
            // Update password
            user.Password = model.NewPassword;
            user.ConfirmPassword = model.ConfirmPassword;
            _context.SaveChanges();
            TempData["Success"] = "Password changed successfully";
            return RedirectToAction("Index", "Transaction");
        }
    }
}
