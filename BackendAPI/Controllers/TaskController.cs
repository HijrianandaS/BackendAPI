using BackendAPI.Interface;
using BackendAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskRepository.GetAllTasks();
                var response = new ApiResponse<IEnumerable<TaskModel>>(
                    status: StatusCodes.Status200OK,
                    message: "Tasks retrieved successfully",
                    data: tasks
                );
                return Ok(response);  // Mengembalikan response dengan status 200 OK
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>(
                    status: StatusCodes.Status400BadRequest,
                    message: ex.Message,
                    data: null
                );
                return BadRequest(response);  // Mengembalikan error dengan status 400 Bad Request
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskRepository.GetTaskById(id);
            if (task == null)
            {
                var response = new ApiResponse<string>(
                    status: StatusCodes.Status404NotFound,
                    message: "Task not found",
                    data: null
                );
                return NotFound(response);
            }
            var successResponse = new ApiResponse<TaskModel>(
                status: StatusCodes.Status200OK,
                message: "Task retrieved successfully",
                data: task
            );
            return Ok(successResponse);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTask([FromBody] TaskModel task)
        {
            if (task == null)
            {
                var response = new ApiResponse<string>(
                    status: StatusCodes.Status400BadRequest,
                    message: "Task cannot be null",
                    data: null
                );
                return BadRequest(response);
            }

            await _taskRepository.AddTask(task);

            var successResponse = new ApiResponse<TaskModel>(
                status: StatusCodes.Status201Created,
                message: "Task created successfully",
                data: task
            );
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, successResponse);
        }

        /*[HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModel task)
        {
            if (task == null || task.Id != id)
            {
                var response = new ApiResponse<string>(
                    status: StatusCodes.Status400BadRequest,
                    message: "Task ID mismatch",
                    data: null
                );
                return BadRequest(response);
            }

            var existingTask = await _taskRepository.GetTaskById(id);
            if (existingTask == null)
            {
                var response = new ApiResponse<string>(
                    status: StatusCodes.Status404NotFound,
                    message: "Task not found",
                    data: null
                );
                return NotFound(response);
            }

            await _taskRepository.UpdateTask(task);

            var successResponse = new ApiResponse<TaskModel>(
                status: StatusCodes.Status204NoContent,
                message: "Task updated successfully",
                data: existingTask
            );
            return Ok(successResponse); 
        }*/

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] TaskUpdateDto dto)
        {
            if (dto == null)
            {
                var response = new ApiResponse<string>(
                    status: StatusCodes.Status400BadRequest,
                    message: "Invalid request body",
                    data: null
                );
                return BadRequest(response);
            }

            // Cek apakah task dengan ID tertentu ada
            var existingTask = await _taskRepository.GetTaskById(id);
            if (existingTask == null)
            {
                var response = new ApiResponse<string>(
                    status: StatusCodes.Status404NotFound,
                    message: "Task not found",
                    data: null
                );
                return NotFound(response);
            }

            // Update hanya field `completed`
            existingTask.Completed = dto.Completed;
            existingTask.UpdatedAt = DateTime.UtcNow;

            // Simpan perubahan
            await _taskRepository.UpdateTask(existingTask);

            var successResponse = new ApiResponse<TaskModel>(
                status: StatusCodes.Status200OK,
                message: "Task status updated successfully",
                data: existingTask
            );

            return Ok(successResponse);
        }

        // DTO untuk menerima data pembaruan status
        public class TaskUpdateDto
        {
            public bool Completed { get; set; }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var existingTask = await _taskRepository.GetTaskById(id);
            if (existingTask == null)
            {
                var response = new ApiResponse<string>(
                    status: StatusCodes.Status404NotFound,
                    message: "Task not found",
                    data: null
                );
                return NotFound(response);
            }

            await _taskRepository.DeleteTask(id);

            var successResponse = new ApiResponse<string>(
                status: StatusCodes.Status204NoContent,
                message: "Task deleted successfully",
                data: null
            );
            return Ok(successResponse);
        }
    }
}
