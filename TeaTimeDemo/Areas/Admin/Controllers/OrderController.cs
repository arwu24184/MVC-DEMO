using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using System.Security.Claims;
using TeaTimeDemo.DataAccess.Repository.IRepository;
using TeaTimeDemo.Models;
using TeaTimeDemo.Models.ViewModels;
using TeaTimeDemo.Utility;

namespace TeaTimeDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int orderId) 
        {
            OrderVM = new OrderVM
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties:
                "Product")
            };
            return View(OrderVM);
        }

        [HttpPost]
        //只允許這三種身分用","添加額外
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee + "," + SD.Role_Manager)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.Address = OrderVM.OrderHeader.Address;
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "訂購人資訊更新成功！";
            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee + "," + SD.Role_Manager)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusProcess);
            _unitOfWork.Save();
            TempData["Success"] = "訂單狀態更新成功！";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee + "," + SD.Role_Manager)]
        public IActionResult OrderReady()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusReady);
            _unitOfWork.Save();
            TempData["Success"] = "訂單狀態更新成功！";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee + "," + SD.Role_Manager)]
        public IActionResult OrderCompleted()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusCompleted);
            _unitOfWork.Save();
            TempData["Success"] = "訂單狀態更新成功！";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee + "," + SD.Role_Manager)]
        public IActionResult CancelOrder()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusCanclled);
            _unitOfWork.Save();
            TempData["Success"] = "訂單狀態更新成功！";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }
        #region API CALLS

        //修改為選中的status顯示
        public IActionResult GetAll(string status ) 
        {
            //取得所選訂單資訊
            IEnumerable<OrderHeader> objOrderHeaders;
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Manager) || User.IsInRole(SD.Role_Employee))
            {
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties:
                "ApplicationUser").ToList();
            }
            else
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                //找到第一個匹配ID
                var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId, 
                    includeProperties: "ApplicationUser");
            }
            switch(status){
                case "Pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusPending);
                    break;
                case "Processing":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusProcess);
                    break;
                case "Ready":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusReady);
                    break;
                case "Completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusCompleted);
                    break;
                case "Cancel":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusCanclled);
                    break;
                default:
                    break;
            }
            return Json(new { data = objOrderHeaders });
        }
        #endregion
    }
}
