using teeze.Models;
using System.Collections.Generic;

namespace teeze.Services
{
    public interface IWishlistServices
    {
        public void RemoveProduct(string itemId);
        List<ProductModel> GetAllProducts();
        void CreateProduct(ProductModel newProduct);
    }

}
