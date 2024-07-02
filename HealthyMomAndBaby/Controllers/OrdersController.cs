using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthyMomAndBaby.DataContext;
using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Service;
using HealthyMomAndBaby.Service.Impl;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HealthyMomAndBaby.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.ShowListOrderAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetDetailOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Create
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddOrderAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetDetailOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.UpdateOrderAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
