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
    
    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void EditProduct(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    // Method to delete a product and save changes to the db
    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
    }
    
    public void AddCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }
    
    public void EditCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
    }

    // Method to delete a task and save changes to the db
    public void DeleteCustomer(Customer customer)
    {
        _context.Customers.Remove(customer);
        _context.SaveChanges();
    }
}