using FirstWebApiProject.DTO.Cat;
using FirstWebApiProject.DTO.Category;
using FirstWebApiProject.DTO.Order;
using FirstWebApiProject.DTO.OrderItem;
using FirstWebApiProject.DTO.Payment;
using FirstWebApiProject.Interfaces;
using FirstWebApiProject.Models;
using FirstWebApiProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApiProject.Controllers
{
    [Route("Order/[controller]")]
    [ApiController]
 
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllOrders()
        {
            // Retrieve all orders
            var orders = unitOfWork.Repositry<Order>().GetALL();

            // Map orders to DTOs
            var orderDtos = orders.Select(order => new OrederDTO
            {
               Id=order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
             

                // Map OrderStatus (bool) to OrderStatus (enum)
                OrderStatus = order.OrderStatus ? OrderStatus.Completed : OrderStatus.InCompleted
            }).ToList();

            return Ok(orderDtos);
        }



        [HttpGet("{id:int}")]
        public IActionResult GetOrderById(int id)
        {
            var Order = unitOfWork.Repositry<Order>().GetFirstOrDefault(e=>e.Id==id,includeProperties : "OrderItems");
            if (Order == null)
            {
                return NotFound(new { Message = "Order not found." });
            }

            var result = new OrderWithOrderItemsDTO
            {
              Id= Order.Id,
              OrderDate=Order.OrderDate,
              TotalAmount=Order.TotalAmount,
              ShippingAddress=Order.ShippingAddress,
              OrderItemsList=Order.OrderItems.Select(e=>e.Name).ToList(),
              OrderStatus = Order.OrderStatus ? OrderStatus.Completed : OrderStatus.InCompleted
            };

            return Ok(result);

        }


        [HttpPost]
        public IActionResult CreateOrder(OrederDTO orederDto)
        {
            // Validate the incoming DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = orederDto.TotalAmount,
                ShippingAddress = orederDto.ShippingAddress,

                // Convert OrderStatus to OrderStatus (bit)
                OrderStatus = orederDto.OrderStatus == OrderStatus.Completed
            };
            try
            {
                unitOfWork.Repositry<Order>().Add(order);


                unitOfWork.Complete();

                // Return a success response
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, orederDto);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }



        }


        [HttpPost("OrderWithItems")]
        public IActionResult CreateOrder(OrderCreateDTO orderCreateDTO)
        {
            // Step 1: Validate input
            if (orderCreateDTO == null)
            {
                return BadRequest("Order data is required.");
            }

            // Step 2: Map DTO to Entity
            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = orderCreateDTO.TotalAmount,
                ShippingAddress = orderCreateDTO.ShippingAddress,
                OrderStatus = orderCreateDTO.OrderStatus == OrderStatus.Completed,
                OrderItems = orderCreateDTO.OrderItems.Select(item => new OrderItem
                {
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            // Step 3: Save the new order
            try
            {
                unitOfWork.Repositry<Order>().Add(order); // Add the order to the database
                unitOfWork.Complete(); // Commit the transaction

                // Step 4: Map and return the result
                var createdOrderDTO = new OrderCreateDTO
                {
                    OrderDate = DateTime.Now,
                    TotalAmount = order.TotalAmount,
                    ShippingAddress = order.ShippingAddress,
                    OrderStatus = order.OrderStatus ? OrderStatus.Completed : OrderStatus.InCompleted,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemCreateDTO
                    {
                        Name = oi.Name,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                };

                // Return the created order with 201 status code
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, createdOrderDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPut]
        public IActionResult EditOrder(OrederDTO orederDTO , int id)
        {
            // Step 1: Validate input
            if (orederDTO == null)
            {
                return BadRequest("Oreder data is required.");
            }

            // Step 2: Retrieve the existing category from the database
            var order = unitOfWork.Repositry<Order>().GetById(id);

            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            // Step 3: Update category properties
            

            order.OrderDate = DateTime.Now;
            order.OrderStatus = orederDTO.OrderStatus == OrderStatus.Completed;
            order.TotalAmount = orederDTO.TotalAmount;
            order.ShippingAddress = orederDTO.ShippingAddress;
            try
            {
                // Step 4: Save changes to the database
                unitOfWork.Complete();

                // Step 5: Return the updated order
                var OrderDTO = new OrederDTO
                {
                    Id=order.Id,
                    OrderDate=order.OrderDate,
                    OrderStatus = order.OrderStatus ? OrderStatus.Completed : OrderStatus.InCompleted  ,                 
                    TotalAmount =order.TotalAmount,
                    ShippingAddress=order.ShippingAddress
                };

                // Returning the updated order with HTTP 200 OK
                return Ok(orederDTO);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the update
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpDelete]
        public IActionResult DeleteOrder(int id)
        {
            // Step 1: Retrieve the existing Order from the database
            var order = unitOfWork.Repositry<Order>().GetById(id);

            if (order == null)
            {
                // If the category doesn't exist, return NotFound (404)
                return NotFound($"Order with ID {id} not found.");
            }

            try
            {
                // Step 2: Create a  OrederDTO to return before deletion
                var orederDto = new OrederDTO
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus ? OrderStatus.Completed : OrderStatus.InCompleted,
                    TotalAmount = order.TotalAmount,
                    ShippingAddress = order.ShippingAddress

                };

                // Step 3: Delete the Order from the repository
                unitOfWork.Repositry<Order>().Delete(order);

                // Step 4: Save changes to the database
                unitOfWork.Complete();

                // Step 5: Return a successful response (204 No Content) along with the deleted CategoryDTO
                return Ok(orederDto); // 200 OK and return the deleted orederDto
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the delete operation
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{orderId}/payment")]
        public IActionResult AddOrUpdatePayment(int orderId, PaymentUpdateDTO paymentUpdateDto)
        {
            // Step 1: Validate the input
            if (paymentUpdateDto == null)
            {
                return BadRequest("Payment data is required.");
            }

            // Step 2: Retrieve the order from the database
            var order = unitOfWork.Repositry<Order>().GetById(orderId);
            if (order == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            // Step 3: Check if the payment already exists for the order
            var existingPayment = unitOfWork.Repositry<Payment>().Find(p => p.OrderId == orderId);

            if (existingPayment != null)
            {
                // Update existing payment
                existingPayment.Amount = paymentUpdateDto.Amount;
                existingPayment.PaymentDate = paymentUpdateDto.PaymentDate;
                existingPayment.PaymentStatus = paymentUpdateDto.PaymentStatus == PaymentStatus.Paid;
            }
            else
            {
                // Add a new payment
                var newPayment = new Payment
                {
                    Amount = paymentUpdateDto.Amount,
                    PaymentDate = paymentUpdateDto.PaymentDate,
                    PaymentStatus = paymentUpdateDto.PaymentStatus == PaymentStatus.Paid,
                    OrderId = orderId
                };

                unitOfWork.Repositry<Payment>().Add(newPayment);
            }

            try
            {
                // Save changes to the database
                unitOfWork.Complete();

                // Return the updated or newly added payment details
                var paymentDto = new PaymentDetailsDTO
                {
                    Amount = paymentUpdateDto.Amount,
                    PaymentDate = paymentUpdateDto.PaymentDate,
                    PaymentStatus = paymentUpdateDto.PaymentStatus,
                    OrderId = orderId
                };

                return Ok(paymentDto);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }

}

