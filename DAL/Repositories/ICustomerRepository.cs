using CW2.DAL.Entities;
using System.Collections.Generic;

namespace CW2.DAL.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer GetById(int id);
        void Insert(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
        void Save();
    }
}