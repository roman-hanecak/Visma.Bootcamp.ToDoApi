using ToDoApi.Entities.DTO;
using ToDoApi.Entities.Model;

namespace ToDoApi.Services.Interfaces
{
    public interface ITaskListService
    {
        Task<List<TaskListDto>> GetByUserAsync(Guid userId, CancellationToken ct = default);
        Task<TaskListDto> GetAsync(Guid taskListId, CancellationToken ct = default);
        Task<TaskListDto> CreateAsync(Guid userId, TaskListModel model, CancellationToken ct = default);
        Task<TaskListDto> UpdateAsync(Guid taskListId, TaskListModel model, CancellationToken ct = default);
        Task DeleteAsync(Guid taskListId, CancellationToken ct = default);
    }
}