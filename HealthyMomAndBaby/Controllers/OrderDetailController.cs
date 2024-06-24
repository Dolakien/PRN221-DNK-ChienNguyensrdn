using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Service;
using HealthyMomAndBaby.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static NuGet.Packaging.PackagingConstants;

namespace HealthyMomAndBaby.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }


        // GET: OrderDetail
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orderDetail = await _orderDetailService.ShowListOrderDetailAsync();
            return View(orderDetail);
        }

        // GET: OrderDetail/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var orderDetail = await _orderDetailService.GetDetailOrderDetailByIdAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetail/Create
        [HttpPost]
        public async Task<IActionResult> Create(OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                await _orderDetailService.AddOrderDetailAsync(orderDetail);
                return RedirectToAction(nameof(Index));
            }
            return View(orderDetail);
        }

        // GET: OrderDetail/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var orderDetail = await _orderDetailService.GetDetailOrderDetailByIdAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        // PUT: OrderDetail/Edit/5
        [HttpPut]
        public async Task<IActionResult> Edit(OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                await _orderDetailService.UpdateOrderDetailAsync(orderDetail);
                return RedirectToAction(nameof(Index));
            }
            return View(orderDetail);
        }


        // DELETE: OrderDetail/Delete/5
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderDetailService.DeleteOrderDetailAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
