using teeze.Models;
using System.Collections.Generic;

namespace teeze.Services
{
    public interface ICartServices
    {
        List<CartModel> GetAllProduct();
        void CreateOrUpdateProduct(CartModel newProducts);
        public void IncreaseQuantity(string itemId);
        public void DecreaseQuantity(string itemId);
        public void RemoveProduct(string itemId);
        public void RemoveAllProduct();

    }
}
