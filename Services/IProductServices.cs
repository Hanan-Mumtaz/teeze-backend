using teeze.Models;
using System.Collections.Generic;
using MongoDB.Bson;

namespace teeze.Services
{
    public interface IProductServices
    {
        List<ProductModel> GetAllProducts();
        void CreateProduct(ProductModel newProduct);
        public Task<List<ProductModel>> GetProductsByNameAsync(string search);
        public Task UpdateProductImagesAsync(ObjectId Id, List<string> images);

    }


}
