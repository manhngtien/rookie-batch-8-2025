using AssetManagement.Core.Entities;
using AssetManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly AppDbContext _context;

        public AssignmentRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public IQueryable<Assignment> GetAllAsync()
        {
            return _context.Assignments
                .Include(a => a.Asset)
                .Include(a => a.AssignedToUser)
                .Include(a => a.AssignedByUser);
        }

        public async Task<Assignment?> GetByIdAsync(int assignmentId)
        {
            return await _context.Assignments
                .Include(a => a.Asset)
                .Include(a => a.AssignedToUser)
                .Include(a => a.AssignedByUser)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);
        }

        public async Task<Assignment> CreateAsync(Assignment entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Assignments.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(Assignment entity)
        {
            _context.Assignments.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Assignment entity)
        {
            _context.Assignments.Remove(entity);
            await Task.CompletedTask;
        }
    }
}