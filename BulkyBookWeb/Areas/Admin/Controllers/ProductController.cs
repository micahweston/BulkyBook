using BulkyBook.DataAccess.Repository.iRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")] // is not really needed in .net 6 but it is safe to go ahead and declare the area for the controllers.
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<Product> objProductObj = _unitOfWork.Product.GetAll();
        return View(objProductObj);
    }

    // Edit GET and POST
    // Get
    public IActionResult Upsert(int? id)
    {
        Product product = new();
        if (id == null || id == 0)
        {
            // create product
            return View(product);
        }
        else
        {
            // Update product
        }

        return View(product);
    }
    // Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Product obj)
    {
        // Server side validation
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product editted successfully";
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
        var productFromDB = _unitOfWork.Product.GetFirstOrDefault(u => u.Id==id);
        //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id == id); //This is the same as before but using FirstOrDefault to through exception if there is more then one. We can use the above because we know that ID is a key so unique.
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id); //Same thing as above but with SingleOrDefault

        if (productFromDB == null)
        {
            return NotFound();
        }

        return View(productFromDB);
    }
    // Post
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id==id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product deleted successfully";
        return RedirectToAction("Index");
    }
}

