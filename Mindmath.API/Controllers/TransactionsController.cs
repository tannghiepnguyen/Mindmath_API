﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mindmath.Repository.Constant;
using Mindmath.Repository.Parameters;
using Mindmath.Service.IService;
using Mindmath.Service.Transactions.DTO;
using System.Text.Json;

namespace Mindmath.API.Controllers
{
	[Route("api/transactions")]
	[ApiController]
	public class TransactionsController : ControllerBase
	{
		private readonly IServiceManager serviceManager;

		public TransactionsController(IServiceManager serviceManager)
		{
			this.serviceManager = serviceManager;
		}

		[HttpGet]
		[Authorize(Roles = Roles.Admin)]
		public async Task<IActionResult> GetTransactions([FromQuery] TransactionParameters transactionParameters)
		{
			var transactions = await serviceManager.TransactionService.GetTransactions(transactionParameters, false);
			Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(transactions.metaData));
			return Ok(transactions.transactions);
		}

		[HttpGet("{userId}")]
		[Authorize(Roles = Roles.Teacher)]
		public async Task<IActionResult> GetTransactionsByUserId([FromRoute] string userId, [FromQuery] TransactionParameters transactionParameters)
		{
			var transactions = await serviceManager.TransactionService.GetTransactionsByUserId(userId, transactionParameters, false);
			Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(transactions.metaData));
			return Ok(transactions.transactions);
		}

		[HttpPost("web/create")]
		[Authorize(Roles = Roles.Teacher)]
		public async Task<IActionResult> CreatePaymentWeb(string userId, [FromBody] TransactionForCreationDto transactionDto)
		{
			var paymentUrl = await serviceManager.TransactionService.CreatePaymentWebAsync(Guid.Parse(userId), transactionDto);
			return Ok(new { PaymentUrl = paymentUrl });
		}

        [HttpPost("mobile/create")]
        public async Task<IActionResult> CreatePaymentMobile(string userId, [FromBody] TransactionForCreationDto transactionDto)
		{
			var paymentUrl = await serviceManager.TransactionService.CreatePaymentMobileAsync(Guid.Parse(userId), transactionDto);
			return Ok(new { PaymentUrl = paymentUrl });
        }

        [HttpGet("IPN")]
		public async Task<IActionResult> IPN()
		{
			var result = await serviceManager.TransactionService.IPNAsync(Request.Query);
			return Ok(result);
		}


		[HttpGet("ReturnUrl")]
		public async Task<IActionResult> ReturnUrl()
		{
			// Xử lý thông tin từ query string
			var responseCode = Request.Query["vnp_ResponseCode"];
			var transactionId = Request.Query["vnp_TxnRef"];

			// Kiểm tra mã phản hồi và thực hiện logic cần thiết
			if (responseCode == "00")
			{
				// Thanh toán thành công
				await serviceManager.TransactionService.UpdateTransaction(Guid.Parse(transactionId), "Success", trackChange: true);
				return Content("Thanh toán thành công. Mã giao dịch: " + transactionId);
			}
			else
			{
				// Thanh toán thất bại
				await serviceManager.TransactionService.UpdateTransaction(Guid.Parse(transactionId), "Failed", trackChange: true);
				return Content("Thanh toán không thành công. Mã lỗi: " + responseCode);
			}
		}

	}
}
