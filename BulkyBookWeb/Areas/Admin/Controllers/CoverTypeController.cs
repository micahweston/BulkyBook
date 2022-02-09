using BulkyBook.DataAccess.Repository.iRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")] // is not really needed in .net 6 but it is safe to go ahead and declare the area for the controllers.
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypeObj = _unitOfWork.CoverType.GetAll();
        return View(objCoverTypeObj);
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
    public IActionResult Create(CoverType obj)
    {
        // Server side validation
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cover created successfully"; // Allows us to send a tempdata that will go away on refresh.
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // Edit GET and POST
    // Get
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        //var categoryFromDB = _db.Categories.Find(id);
        var coverFromDBFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id); //This is the same as before but using FirstOrDefault to through exception if there is more then one. We can use the above because we know that ID is a key so unique.
                                                                                            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id); //Same thing as above but with SingleOrDefault

        if (coverFromDBFirst == null)
        {
            return NotFound();
        }

        return View(coverFromDBFirst);
    }
    // Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CoverType obj)
    {
        // Server side validation
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cover editted successfully";
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
        var categoryFromDB = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id==id);
        //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id == id); //This is the same as before but using FirstOrDefault to through exception if there is more then one. We can use the above because we know that ID is a key so unique.
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id); //Same thing as above but with SingleOrDefault

        if (categoryFromDB == null)
        {
            return NotFound();
        }

        return View(categoryFromDB);
    }
    // Post
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id==id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.CoverType.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Cover deleted successfully";
        return RedirectToAction("Index");
    }
}

