using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
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
    public class StoreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;//從介面建立欄位
        public StoreController(IUnitOfWork unitOfWork)//建立物件
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Store> objStoreList = _unitOfWork.Store.GetAll().ToList();
            return View(objStoreList);
        }
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0) return View(new Store());//如果是空值就新增
            else //否則就修改
            {
                Store storeObj = _unitOfWork.Store.Get(u => u.Id == id);
                return View(storeObj);//回傳
            }
        }
        [HttpPost]
        public IActionResult Upsert(Store storeObj)
        {
            if (ModelState.IsValid)
            {
                if(storeObj.Id == 0) _unitOfWork.Store.Add(storeObj);
                else _unitOfWork.Store.Update(storeObj);
                _unitOfWork.Save();
                TempData["success"] = "店鋪新增成功！";
                return RedirectToAction("Index");
            }
            else return View(storeObj);
        }
/*        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Store? productFromDb = _unitOfWork.Store.Get(u => u.Id == id);
            if (productFromDb == null) return NotFound();
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Store obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Store.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "產品編輯成功！";
                return RedirectToAction("Index");
            }
            return View();
        }*/
        /*public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Store productFromDb = _unitOfWork.Store.Get(u => u.Id == id);
            if (productFromDb == null) return NotFound();
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]//需要指定因為action 衝突
        public IActionResult DeletePOST(int? id)
        {
            Store? obj = _unitOfWork.Store.Get(u => u.Id == id);
            if (obj == null) return NotFound(); 
            _unitOfWork.Store.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "類別刪除成功！";
            return RedirectToAction("Index");
        }
        */
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Store> objStoreList = _unitOfWork.Store.GetAll().ToList();
            return Json(new {data = objStoreList});
        }
        //刪除的json用以製作彈窗
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var storeToBeDeleted = _unitOfWork.Store.Get(u => u.Id == id);
            if (storeToBeDeleted == null) return Json(new { success = false, message = "刪除失敗" });
            _unitOfWork.Store.Remove(storeToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "刪除成功" });
        }
        #endregion    
    }
}
