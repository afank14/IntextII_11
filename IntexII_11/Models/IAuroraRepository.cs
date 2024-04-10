namespace IntexII_11.Models;

public interface IAuroraRepository
{
    public IQueryable<Product> Products { get; }
    public IQueryable<Customer> Customers { get; }
    public IQueryable<Order> Orders { get; }
    public IQueryable<LineItem> LineItems { get; }
    // public IQueryable<ProductCategory> ProductCategories { get; }
    public IQueryable<User60Rec> User60Recs { get; }

    // public IQueryable<ProductCategory> GetProductsWithCategory();
}