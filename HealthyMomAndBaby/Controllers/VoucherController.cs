using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;
using HealthyMomAndBaby.Service;
using HealthyMomAndBaby.Service.Impl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthyMomAndBaby.Api.Controllers
{
    [Route("Voucher")]
    public class VouchersController : Controller
    {
        private readonly IVoucherService _voucherService;

        public VouchersController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddVoucher(CreateVoucherRequest voucher)
        {
            try
            {
                await _voucherService.AddVoucherAsync(voucher);
                return RedirectToAction("Vouchers", "Admin"); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateVoucher([FromBody] Voucher voucher)
        {
            try
            {
                await _voucherService.UpdateVoucherAsync(voucher);
                return Ok(new { message = "Voucher updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("delete")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            try
            {
                await _voucherService.DeleteVoucherAsync(id);
                return Ok(new { message = "Voucher deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetVoucherDetails(int id)
        {
            var voucher = await _voucherService.GetVoucherAsync(id);
            if (voucher == null)
            {
                return NotFound(new { message = $"Voucher with id {id} not found" });
            }
            return Ok(voucher);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllVouchers()
        {
            var vouchers = await _voucherService.GetAllVouchersAsync();
            return Ok(vouchers);
        }
    }
}
