using MVCApp.Interface;

namespace MVCApp.Models
{
    public class ProductAdded : IDomainEvent
    {
        public Product Product { get; }

        public ProductAdded(Product product)
        {
            Product = product;
        }
    }
}
