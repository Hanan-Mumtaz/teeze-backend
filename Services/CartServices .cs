using MongoDB.Driver;
using teeze.Models;
using System.Collections.Generic;

namespace teeze.Services
{
    public class CartServices : ICartServices
    {
        private IMongoCollection<CartModel> _product;

        public CartServices(IOnlineStoreDB OnlineStoreDB, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(OnlineStoreDB.DatabaseName);
            _product = database.GetCollection<CartModel>("Cart");
        }

        public List<CartModel> GetAllProduct()
        {
            return _product.Find(CartModel => true).ToList();
        }

        public void CreateOrUpdateProduct(CartModel newProduct)
        {
            var existingProduct = _product.Find(p => p.Id_ == newProduct.Id_).FirstOrDefault();

            if (existingProduct != null)
            {
                var updatedQuantity = existingProduct.Quantity + newProduct.Quantity;
                var updateDefinition = Builders<CartModel>.Update.Set(p => p.Quantity, updatedQuantity);

                _product.UpdateOne(p => p.Id_ == existingProduct.Id_, updateDefinition);
            }
            else
            {
                _product.InsertOne(newProduct);
            }
        }
        public void IncreaseQuantity(string itemId)
        {
            var updateDefinition = Builders<CartModel>.Update.Inc(p => p.Quantity, 1);
            _product.UpdateOne(p => p.Id_ == itemId, updateDefinition);
        }
        public void DecreaseQuantity(string itemId)
        {
            var updateDefinition = Builders<CartModel>.Update.Inc(p => p.Quantity, -1);
            // Ensure quantity does not go below 1
            var existingProduct = _product.Find(p => p.Id_ == itemId).FirstOrDefault();
            if (existingProduct != null && existingProduct.Quantity > 1)
            {
                _product.UpdateOne(p => p.Id_ == itemId, updateDefinition);
            }
            else
            {
                throw new InvalidOperationException("Cannot decrease quantity below 1");
            } 
        }
        public void RemoveProduct(string itemId)
        {
            _product.DeleteOne(p => p.Id_ == itemId);
        }
        public void RemoveAllProduct()
        {
            _product.DeleteMany(FilterDefinition<CartModel>.Empty);
        }
    }
}
