using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using TeaTimeDemo.DataAccess.Data;
using TeaTimeDemo.DataAccess.Repository.IRepository;
using TeaTimeDemo.Models;
using TeaTimeDemo.Models.ViewModels;
using TeaTimeDemo.Utility;

namespace TeaTimeDemo.Areas.Admin.Controllers
{
    [Area("Admin")]//區域
    [Authorize(Roles = SD.Role_Admin)]//驗證身分
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;//從介面建立欄位
        private readonly IWebHostEnvironment _webHostEnvironment;//建立wwwroot訪問環境
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)//建立物件
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objCategoryList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(objCategoryList);
        }
        public IActionResult Upsert(int? id)
        {//類別清單
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u =>
                new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
                /*IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                //ViewBag.CategoryList = CategoryList;//Viewbag式下拉選單
                //ViewData["CategoryList"] = CategoryList;//ViewData式下拉選單*/
            };
            if (id == null || id == 0) return View(productVM);//如果是空值就新增
            else //否則就修改
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null) 
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl)) //判斷有無圖片
                    {
                        //刪除舊有圖片
                        var oldImagePath = Path.Combine(wwwRootPath,productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) 
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream =
                        new FileStream(Path.Combine(productPath, fileName), FileMode.Create)) 
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;//圖片路徑
                }
                if(productVM.Product.Id == 0) _unitOfWork.Product.Add(productVM.Product);
                else _unitOfWork.Product.Update(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "產品新增成功！";
                return RedirectToAction("Index");
            }
            else //避免出現錯誤驗證時的整頁異常頁面
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u =>
                new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }
/*        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null) return NotFound();
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "產品編輯成功！";
                return RedirectToAction("Index");
            }
            return View();
        }*/
        /*public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Product productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null) return NotFound();
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]//需要指定因為action 衝突
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null) return NotFound(); 
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "類別刪除成功！";
            return RedirectToAction("Index");
        }
        */
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return Json(new {data = objProductList});
        }
        //刪除的json用以製作彈窗
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted.ImageUrl != null)
            {
                if (productToBeDeleted == null) return Json(new { success = false, message = "刪除失敗" });
                var oldImagePath =
                    Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
            }  
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "刪除成功" });
        }
        #endregion    
    }
}
