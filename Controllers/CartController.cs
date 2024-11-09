using Microsoft.AspNetCore.Mvc;
using teeze.Services;
using teeze.Models;
using System.Collections.Generic;

namespace teeze.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartServices;

        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        // Get all products in the cart
        [HttpGet]
        public ActionResult<List<CartModel>> GetAllProduct()
        {
            return _cartServices.GetAllProduct();
        }

        // Add or update a product in the cart
        [HttpPost("AddToCart")]
        public IActionResult UploadProduct([FromBody] AddToCartRequest request)
        {
            var newProduct = new CartModel
            {
                Name = request.Name,
                Price = request.Price,
                ThumbnaiL = request.Thumbnail,
                Category = request.Category,
                Id_ = request.Id_,
                Quantity = request.Quantity,
            };

            _cartServices.CreateOrUpdateProduct(newProduct);
            return Ok(new { message = "Product added or updated successfully" });
        }

        // Increase quantity
        [HttpPut("{itemId}/increase")]
        public IActionResult IncreaseQuantity(string itemId)
        {
            var product = _cartServices.GetAllProduct().Find(p => p.Id_ == itemId);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _cartServices.IncreaseQuantity(itemId); // Use service method directly
            return Ok(new { message = "Quantity increased" });
        }

        // Decrease quantity
        [HttpPut("{itemId}/decrease")]
        public IActionResult DecreaseQuantity(string itemId)
        {
            var product = _cartServices.GetAllProduct().Find(p => p.Id_ == itemId);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            try
            {
                _cartServices.DecreaseQuantity(itemId); // Use service method directly
                return Ok(new { message = "Quantity decreased" });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { message = "Cannot decrease quantity below 1" });
            }
        }

        // Remove product from cart
        [HttpDelete("{itemId}")]
        public IActionResult RemoveFromCart(string itemId)
        {
            var product = _cartServices.GetAllProduct().Find(p => p.Id_ == itemId);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _cartServices.RemoveProduct(itemId);
            return Ok(new { message = "Product removed from cart" });
        }
        [HttpDelete]
        public IActionResult RemoveAllFromCart()
        {
            var products = _cartServices.GetAllProduct();
            if (products == null || !products.Any())
            {
                return NotFound(new { message = "No products in cart" });
            }
            _cartServices.RemoveAllProduct();
            return Ok(new { message = "All products removed from cart" });
        }

        public class AddToCartRequest
        {
            public string? Name { get; set; }
            public double Price { get; set; }
            public string? Thumbnail { get; set; }
            public string? Category { get; set; }
            public string? Id_ { get; set; }
            public int Quantity { get; set; }
        }
    }
}
