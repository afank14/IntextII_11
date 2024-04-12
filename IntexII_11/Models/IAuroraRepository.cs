namespace IntexII_11.Models;

public interface IAuroraRepository
{
    public IQueryable<Product> Products { get; }
    public IQueryable<Customer> Customers { get; }
    public IQueryable<Order> Orders { get; }
    public IQueryable<LineItem> LineItems { get; }
    // public IQueryable<ProductCategory> ProductCategories { get; }
    public IQueryable<User60Rec> User60Recs { get; }

    public void AddOrder(Order order);

    public void SaveChanges(Order order);

    // public IQueryable<ProductCategory> GetProductsWithCategory();
}