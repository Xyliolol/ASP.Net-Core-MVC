using System.Collections.Concurrent;

namespace MVCApp.Models
{
    public class Catalog
    {
        private ConcurrentDictionary<int, Product> _products { get; set; } = new();
        private object _lock = new object();

        public Catalog()
        {
            _products = new ConcurrentDictionary<int, Product>();
        }
        public void ProductAdd(Product product)
        {
            _products.TryAdd(product.Id, product);
        }


        public void ProductDelete(int Id)
        {
            _products.TryRemove(Id, out _);
        }

        public int CountProducts()
        {
            return _products.Count();
        }

        public Product FindProduct(int id)
        {
            return _products[id];
        }

        public void UpdateProduct(Product product)
        {
            _products.TryUpdate(product.Id, product, _products[product.Id]);
        }

        public List<Product> ProductsGetAll()
        {
            lock (_lock)
            {
                return _products.Values.ToList();
            }
        }
    }
}
