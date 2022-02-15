using BulkyBook.DataAccess.Repository.iRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")] // is not really needed in .net 6 but it is safe to go ahead and declare the area for the controllers.
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }
    public IActionResult Index()
    {
        return View();
    }

    // Edit GET and POST
    // Get
    public IActionResult Upsert(int? id)
    {
        // This is a tightly bonded model instead of loosly,.
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            })
        };
        if (id == null || id == 0)
        {
            
            // create product
            /*
             * ViewBag and ViewData are two ways to pass information into a page. They transfer information from the Controller to View, but not the other way around. Ideal for situations
             * in which the temporary data is not in a model. ViewData is type case before use, ViewBag is not. Both only last during current http request. ViewData values will be null if redirection occurs.
             * 
             * The other option is TempData - it uses sesson to store the data. it can be used to store data between two consecutive requests. It must be type case before use and checked for null values to avoid runtime error.
             * tempData can be used to store only one time messages like error messages, validation messages. See our error and success messages with toastr.
             */
            // Using ViewBag to store data from category list. 
            //ViewBag.CategoryList = CategoryList;
            //// Using ViewData to pass data from covertypelist
            //ViewData["CoverTypeList"] = CoverTypeList;
            return View(productVM);
        }
        else
        {
            // Update product
        }

        return View(productVM);
    }
    // Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile file)
    {
        // Server side validation
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
            }

            _unitOfWork.Product.Add(obj.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";
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

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
        return Json(new { data = productList });
    }
    #endregion
}

