using BackendAPI.Models;

namespace BackendAPI.Interface
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskModel>> GetAllTasks();
        Task<TaskModel> GetTaskById(int id);
        Task AddTask(TaskModel task);
        Task UpdateTask(TaskModel task);
        Task DeleteTask(int id);
    }
}
