using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.iRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        // Create Get and POST
        // Get
        public IActionResult Create()
        {
            return View();
        }
        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            // This is how we can set up a custom validation saying that name cannot be the same as DisplayOrder.
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                // name refers to the input for name, that is the first param required.
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }
            // Server side validation
           if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully"; // Allows us to send a tempdata that will go away on refresh.
                return RedirectToAction("Index");
            }
           return View(obj);
        }

        // Edit GET and POST
        // Get
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDB = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id == id); //This is the same as before but using FirstOrDefault to through exception if there is more then one. We can use the above because we know that ID is a key so unique.
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id); //Same thing as above but with SingleOrDefault

            if(categoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }
        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            // This is how we can set up a custom validation saying that name cannot be the same as DisplayOrder.
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                // name refers to the input for name, that is the first param required.
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }
            // Server side validation
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category editted successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Delete GET and POST
        // Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDB = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id==id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id == id); //This is the same as before but using FirstOrDefault to through exception if there is more then one. We can use the above because we know that ID is a key so unique.
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id); //Same thing as above but with SingleOrDefault

            if (categoryFromDB == null)
            {
                return NotFound();
            }

            return View(categoryFromDB);
        }
        // Post
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id==id);
            if(obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
