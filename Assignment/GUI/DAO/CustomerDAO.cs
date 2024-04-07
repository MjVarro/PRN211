
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Models;

namespace Assignment.DAO;

public class CustomerDAO
{
    private static CustomerDAO instance;
    public static CustomerDAO Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CustomerDAO();
            }
            return instance;
        }

    }
    private readonly NorthwindContext _context;

    CustomerDAO()
    {
        _context = new NorthwindContext();
    }

    public List<Customer> GetCustomers()
    {
        var customers = _context.Customers.ToList();
        return customers;
    }

 
    public void AddCustomer(Customer customer)
    {
        try
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }

    public Customer CheckContainsPhone(string phone)
    {
        try
        {
            var cus = _context.Customers.Where(x => x.Phone.Equals(phone)).FirstOrDefault();
            if (cus != null)
            {
                return cus;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }


}
