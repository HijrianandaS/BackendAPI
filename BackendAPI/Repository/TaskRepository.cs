using BackendAPI.Interface;
using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DatabaseContext _context;

        public TaskRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskModel>> GetAllTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskModel> GetTaskById(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task AddTask(TaskModel task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTask(TaskModel task)
        {
            try
            {
                var existingTask = await _context.Tasks.FindAsync(task.Id);
                if (existingTask != null)
                {
                    existingTask.Title = task.Title;
                    existingTask.Completed = task.Completed;
                    existingTask.Time = task.Time;
                    existingTask.UpdatedAt = DateTime.Now;
                    _context.Entry(existingTask).State = EntityState.Modified; // Mark entity as modified
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating task: " + ex.Message);
            }
        }


        public async Task DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
