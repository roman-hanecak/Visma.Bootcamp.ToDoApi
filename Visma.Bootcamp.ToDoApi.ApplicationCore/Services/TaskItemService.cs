using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;
using Visma.Bootcamp.ToDoApi.ApplicationCore.Database;
using Visma.Bootcamp.ToDoApi.ApplicationCore.Entities.Domain;
using Visma.Bootcamp.ToDoApi.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.ToDoApi.ApplicationCore.Entities.Model;
using Visma.Bootcamp.ToDoApi.ApplicationCore.Exceptions;
using Visma.Bootcamp.ToDoApi.ApplicationCore.Services.Interfaces;
using Visma.Bootcamp.ToDoApi.ApplicationCore.Repositories.Interfaces;

namespace Visma.Bootcamp.ToDoApi.ApplicationCore.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly ITaskListRepository _taskListRepository;

        public TaskItemService(ITaskItemRepository taskItemRepository, ITaskListRepository taskListRepository = null)
        {
            _taskItemRepository = taskItemRepository;
            _taskListRepository = taskListRepository;
        }

        public async Task<TaskItemDto> CreateAsync(Guid taskListId, TaskItemModel model, CancellationToken ct = default)
        {
            var taskList = await _taskListRepository.Get(taskListId);
            if (taskList == null)
            {
                throw new NotFoundException($"TaskList with Id: {taskListId} doesnt exists!");
            }

            var taskItem = new TaskItem
            {
                PublicId = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                CreatedDate = model.CreatedDate,
                EndDate = model.EndDate,
                Completed = false,
                TaskListId = taskList.Id
            };

            await _taskItemRepository.Create(taskItem);
            return taskItem.ToDto();
        }

        public async Task DeleteAsync(Guid taskId, CancellationToken ct = default)
        {
            var taskItem = await _taskItemRepository.Get(taskId);
            if (taskItem == null)
            {
                throw new NotFoundException($"Task with Id {taskId} doesnt exists!");
            }

            await _taskItemRepository.Delete(taskItem);
        }

        public async Task<TaskItemDto> GetAsync(Guid taskId, CancellationToken ct = default)
        {
            var task = await _taskItemRepository.Get(taskId);
            if (task == null)
            {
                throw new NotFoundException($"Task with {taskId} wasnt found");
            }
            return task.ToDto();
        }

        public async Task<TaskItemDto> UpdateAsync(Guid taskId, TaskItemModel model, CancellationToken ct = default)
        {
            var taskItem = await _taskItemRepository.Get(taskId);
            if (taskItem == null)
            {
                throw new NotFoundException($"Task with Id {taskId} doesnt exists!");
            }
            taskItem.Title = model.Title;
            taskItem.Description = model.Description;
            taskItem.EndDate = model.EndDate;
            taskItem.Completed = model.Completed;

            await _taskItemRepository.Update(taskItem);

            return taskItem.ToDto();
        }

        public async Task<List<TaskItemDto>> GetTasksByTaskList(Guid taskListId, CancellationToken ct = default)
        {
            var taskList = await _taskListRepository.Get(taskListId);
            if (taskList == null)
            {
                throw new NotFoundException($"TaskList with Id {taskListId} wasnt found!");
            }

            var tasks = await _taskItemRepository.GetByTaskList(taskList.Id);
            if (taskList == null)
            {
                throw new NotFoundException("Tasks werent found!");
            }

            List<TaskItemDto> taskItems = tasks.Select(x => x.ToDto()).ToList();
            return taskItems;
        }
    }
}