using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    public class TransactionController : Controller
    {

        private readonly DatabaseContext _context;

        public TransactionController(DatabaseContext context)
        {
            _context = context;
        }



        public IActionResult Index(string sortOrder, string typeFilter, string categoryFilter, DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.DateSort = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";

            var data = from t in _context.Transactions
                       select t;

            //  Check user actually selected dates
            var hasFrom = Request.Query.ContainsKey("fromDate") && !string.IsNullOrWhiteSpace(Request.Query["fromDate"]);
            var hasTo = Request.Query.ContainsKey("toDate") && !string.IsNullOrWhiteSpace(Request.Query["toDate"]);

            //  Validate
            if (hasFrom && hasTo && fromDate > toDate)
            {

                ViewBag.DateError = true;
                ViewBag.FromDate = "";
                ViewBag.ToDate = "";
                return View(data.ToList());
            }

            //  Future date prevent
            if (fromDate.HasValue && fromDate.Value > DateTime.Today)
            {
                fromDate = DateTime.Today;
            }

            if (toDate.HasValue && toDate.Value > DateTime.Today)
            {
                toDate = DateTime.Today;
            }

            // Apply filters
            if (fromDate.HasValue)
            {
                data = data.Where(x => x.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                data = data.Where(x => x.Date <= toDate.Value);
            }

            // existing filters
            if (!string.IsNullOrEmpty(typeFilter))
            {
                data = data.Where(x => x.Type == typeFilter);
            }

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                data = data.Where(x => x.Category.Contains(categoryFilter));
            }


            if (sortOrder == "date_desc")
            {
                data = data.OrderByDescending(x => x.Date);
            }
            else
            {
                data = data.OrderBy(x => x.Date);
            }

            var list = data.ToList();

            // summary
            ViewBag.TotalIncome = list.Where(x => x.Type == "Income").Sum(x => x.Amount);
            ViewBag.TotalExpense = list.Where(x => x.Type == "Expense").Sum(x => x.Amount);
            ViewBag.Balance = ViewBag.TotalIncome - ViewBag.TotalExpense;

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Transaction t)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(t);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t);
        }

        public IActionResult Edit(Guid id)
        {
            var data = _context.Transactions.Find(id);

            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Transaction t)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Update(t);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(t);
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            var data = _context.Transactions.Find(id);
            if (data == null)
            {
                return NotFound(); // or RedirectToAction("Index");
            }
            _context.Transactions.Remove(data);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
