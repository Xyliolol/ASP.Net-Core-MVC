namespace MVCApp.Models
{
    public class AddProduct
    {
        public List<GoodsCategory> Categories { get; set; } = new();
    }

    public class GoodsCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
