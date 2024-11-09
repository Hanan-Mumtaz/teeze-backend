using MongoDB.Driver;
using teeze.Models;
using System.Collections.Generic;

namespace teeze.Services
{
    public class WishlistServices : IWishlistServices
    {
        private IMongoCollection<ProductModel> _product;

        public WishlistServices(IOnlineStoreDB OnlineStoreDB, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(OnlineStoreDB.DatabaseName);
            _product = database.GetCollection<ProductModel>("Wishlist");
        }
        public List<ProductModel> GetAllProducts()
        {
            return _product.Find(ProductModel => true).ToList();
        }
        public void CreateProduct(ProductModel newProduct)
        {
                _product.InsertOne(newProduct);
            }
        public void RemoveProduct(string itemId)
        {
            _product.DeleteOne(p => p.Id_ == itemId);
        }

    }
    }

