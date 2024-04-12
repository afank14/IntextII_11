using Microsoft.EntityFrameworkCore;

namespace IntexII_11.Models;

public class EFAuroraRepository : IAuroraRepository
{
    private ApplicationDbContext _context;

    public EFAuroraRepository(ApplicationDbContext temp)
    {
        _context = temp;
    }

    public IQueryable<Product> Products => _context.Products;
    public IQueryable<Customer> Customers => _context.Customers;
    public IQueryable<Order> Orders => _context.Orders;
    public IQueryable<LineItem> LineItems => _context.LineItems;
    // public IQueryable<ProductCategory> ProductCategories => _context.ProductCategories;
    public IQueryable<User60Rec> User60Recs => _context.User60Recs;
    public void AddOrder(Order order)
    {
        _context.Orders.Add(order);
    }

    public void SaveChanges(Order order)
    {
        _context.SaveChanges();
    }

    // Method to get the stats and include Category
    // public IQueryable<ProductCategory> GetProductsWithCategory()
    // {
    //     return _context.ProductCategories.Include(p => p.Product)
    //         .Include(p => p.Category);
    // }
}