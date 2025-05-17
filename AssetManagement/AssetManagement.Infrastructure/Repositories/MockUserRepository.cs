using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure.Repositories
{
    class MockUserRepository : IUserRepository
    {
        public Task<User> CreateAsync(User entity)
        {
            return new User
            {
                FirstName = "Test",

            }
        }

        public Task DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync<TId>(TId id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
