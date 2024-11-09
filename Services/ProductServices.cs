using MongoDB.Driver;
using teeze.Models;
using System.Collections.Generic;
using MongoDB.Bson;

namespace teeze.Services
{
    public class ProductServices : IProductServices
    {
        private IMongoCollection<ProductModel> _product;

        public ProductServices(IOnlineStoreDB OnlineStoreDB, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(OnlineStoreDB.DatabaseName);
            _product = database.GetCollection<ProductModel>("Products");
        }
        public async Task<List<ProductModel>> GetProductsByNameAsync(string search)
        {
            var filter = Builders<ProductModel>.Filter.Regex("name", new BsonRegularExpression(search, "i"));
            return await _product.Find(filter).ToListAsync();
        }
        public List<ProductModel> GetAllProducts()
        {
            return _product.Find(ProductModel => true).ToList();
        }
        public void CreateProduct(ProductModel newProduct)
        {
                _product.InsertOne(newProduct);
            }
        public async Task UpdateProductImagesAsync(ObjectId Id , List<string> images)
        {
            var update = Builders<ProductModel>.Update.Set(p => p.Images, images);

            var filter = Builders<ProductModel>.Filter.Eq(p => p.Id, Id);

            await _product.UpdateOneAsync(filter, update);
        }

    }
}

