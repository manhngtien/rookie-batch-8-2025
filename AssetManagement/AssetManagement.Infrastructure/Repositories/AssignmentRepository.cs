using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using AssetManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            await _context.Assignments.AddAsync(entity);
            
            return await _context.Assignments
                .Include(a => a.Asset)
                .Include(a => a.AssignedToUser)
                .Include(a => a.AssignedByUser)
                .FirstOrDefaultAsync(a => a.Id == entity.Id) ?? entity;
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

        public async Task<bool> IsUserInViewAssignments(string staffCode)
        {
            return await _context.Assignments
                .Include(a => a.ReturningRequest)
                .Where(a => a.AssignedToUser.StaffCode == staffCode || a.AssignedByUser.StaffCode == staffCode)
                .Where(a => a.State != AssignmentStatus.Accepted ||
                            (
                                a.State == AssignmentStatus.Accepted &&
                                a.ReturningRequest != null &&
                                a.ReturningRequest.State != ReturningRequestStatus.Completed)
                            )
                .AnyAsync();
        }
    }
}