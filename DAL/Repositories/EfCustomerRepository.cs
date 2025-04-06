using System;
using System.Collections.Generic;
using System.Linq;
using CW2.DAL.EF;
using CW2.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CW2.DAL.Repositories
{
    public class EfCustomerRepository : ICustomerRepository
    {
        private readonly ArtRentDbContext _context;

        public EfCustomerRepository(ArtRentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers
                .AsNoTracking()
                .ToList();
        }

        public Customer GetById(int id)
        {
            return _context.Customers
                .FirstOrDefault(c => c.CustomerId == id);
        }

        public void Insert(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            _context.Customers.Add(customer);
            Save();
        }

        public void Update(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            _context.Entry(customer).State = EntityState.Modified;
            Save();
        }

        public void Delete(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            if (_context.Entry(customer).State == EntityState.Detached)
            {
                _context.Customers.Attach(customer);
            }
            _context.Customers.Remove(customer);
            Save();
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Add more specific exception handling here
                throw new Exception("Database update failed", ex);
            }
        }
    }
}