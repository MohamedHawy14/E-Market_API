using FirstWebApiProject.DTO.Order;
using FirstWebApiProject.DTO.Payment;
using FirstWebApiProject.Interfaces;
using FirstWebApiProject.Models;
using FirstWebApiProject.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstWebApiProject.Controllers
{
    [Route("Payment/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PaymentController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpPost]
        public IActionResult CreatePayment(PaymentCreateDTO paymentDto)
        {
            if (paymentDto == null)
            {
                return BadRequest("Payment data is required.");
            }

            var order = unitOfWork.Repositry<Order>().GetById(paymentDto.OrderId);
            if (order == null)
            {
                return NotFound($"Order with ID {paymentDto.OrderId} not found.");
            }

            var payment = new Payment
            {
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
                PaymentStatus = paymentDto.PaymentStatus == PaymentStatus.Paid,
                OrderId = paymentDto.OrderId
            };

            try
            {
                unitOfWork.Repositry<Payment>().Add(payment);
                unitOfWork.Complete();

                var createdPayment = new PaymentDetailsDTO
                {
                    Id = payment.Id,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentStatus = payment.PaymentStatus ? PaymentStatus.Paid : PaymentStatus.Failed ,
                    OrderId = payment.Order.Id,
                };

                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, createdPayment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = unitOfWork.Repositry<Payment>()
                .Find(p => p.Id == id, include: q => q.Include(p => p.Order));

            if (payment == null)
            {
                return NotFound($"Payment with ID {id} not found.");
            }

            if (payment.Order == null)
            {
                return NotFound($"Order related to Payment ID {id} not found.");
            }

            var paymentDto = new PaymentDetailsDTO
            {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate, // Use the actual payment date
                PaymentStatus = payment.PaymentStatus ? PaymentStatus.Paid : PaymentStatus.Failed,
                OrderId = payment.Order.Id
            };

            return Ok(paymentDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, PaymentUpdateDTO paymentUpdateDto)
        {
            // Step 1: Validate the input
            if (paymentUpdateDto == null)
            {
                return BadRequest("Payment data is required.");
            }

            // Step 2: Retrieve the payment record from the database
            var payment = unitOfWork.Repositry<Payment>().GetById(id);
            if (payment == null)
            {
                return NotFound($"Payment with ID {id} not found.");
            }

            // Step 3: Update the payment properties
            payment.Amount = paymentUpdateDto.Amount;
            payment.PaymentDate = paymentUpdateDto.PaymentDate;
            payment.PaymentStatus = paymentUpdateDto.PaymentStatus == PaymentStatus.Paid;

            try
            {
                // Step 4: Save changes to the database
                unitOfWork.Complete();

                // Step 5: Create a DTO to return the updated payment
                var paymentDto = new PaymentDetailsDTO
                {
                    Id = payment.Id,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentStatus = payment.PaymentStatus ? PaymentStatus.Paid : PaymentStatus.Failed,
                    OrderId = payment.Order.Id
                };

                // Return the updated payment with HTTP 200 OK
                return Ok(paymentDto);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the update
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}
