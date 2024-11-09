using Microsoft.AspNetCore.Mvc;
using teeze.Services;
using teeze.Models;
using static teeze.Controllers.UsersController;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace teeze.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IWishlistServices _wishlistServices;
        public ProductController(IProductServices productServices, IWishlistServices wishlistServices)
        {
            _productServices = productServices;
            _wishlistServices = wishlistServices;
        }
        [HttpGet]
        public ActionResult<List<ProductModel>> GetAllProducts()
        {
            return _productServices.GetAllProducts();
        }
        [HttpPost("UploadProduct")]
        public IActionResult UploadProduct([FromBody] UploadProductRequest request)
        {
            var newProduct = new ProductModel
            {
                Name = request.Name,
                Price = request.Price,
                Thumbnail = request.Thumbnail,
                Category = request.Category,
                Id_ = request.Id_,
            };

            _productServices.CreateProduct(newProduct);
            return Ok(new { message = "upload successful" });
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string search)
        {
            var products = await _productServices.GetProductsByNameAsync(search);
            return Ok(products);
        }

        [HttpPost("UploadWishlist")]
        public IActionResult UploadWishlist([FromBody] UploadProductRequest request)
        {
            var newProduct = new ProductModel
            {
                Name = request.Name,
                Price = request.Price,
                Thumbnail = request.Thumbnail,
                Category = request.Category,
                Id_ = request.Id_,
            };
            _wishlistServices.CreateProduct(newProduct);
            return Ok(new { message = "upload successful" });
        }
        [HttpDelete("DeleteFromWishlist/{itemId}")]
        public IActionResult RemoveFromWishlist(string itemId)
        {
            var product = _wishlistServices.GetAllProducts().Find(p => p.Id_ == itemId);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _wishlistServices.RemoveProduct(itemId);
            return Ok(new { message = "Product removed from Wishlist" });
        }
        [HttpGet("Wishlist")]
        public ActionResult<List<ProductModel>> GetAllWishes()
        {
            return _wishlistServices.GetAllProducts();
        }

        [HttpPost("update-images/{id}")]
        public async Task<IActionResult> UpdateImages(string id, [FromBody] List<string> images)
        {
            if (!ObjectId.TryParse(id, out ObjectId Id))
            {
                return BadRequest("Invalid product ID.");
            }

            await _productServices.UpdateProductImagesAsync(Id, images);
            return Ok("Images updated successfully.");
        }


        public class UploadProductRequest
        {
            public string? Name { get; set; }
            public double Price { get; set; }
            public string? Thumbnail { get; set; }
            public string? Category { get; set; }
            public string? Id_ { get; set; }
        }
    }
}
