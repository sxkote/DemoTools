using DemoTools.Modules.Main.Api.DTO;
using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Domain.Entities.Todo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DemoTools.Modules.Main.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "Token", Roles = "User")]
    [Route("api/todo")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }


        [HttpGet]
        [Route("")]
        public IEnumerable<TodoList> GetTodoLists()
        {
            return _todoService.GetAllLists();
        }

        [HttpPost]
        [Route("")]
        public void AddTodoList([FromBody] ModifyTodoListDTO dto)
        {
            _todoService.CreateList(dto.Title);
        }

        [HttpPost]
        [Route("{listID}")]
        public void ModifyTodoList(Guid listID, [FromBody] ModifyTodoListDTO dto)
        {
            _todoService.ChangeList(listID, dto.Title);
        }

        [HttpDelete]
        [Route("{listID}")]
        public void DeleteTodoList(Guid listID)
        {
            _todoService.DeleteList(listID);
        }





        [HttpGet]
        [Route("{listID}")]
        public TodoList GetTodoList(Guid listID)
        {
            return _todoService.GetList(listID);
        }

        [HttpPost]
        [Route("{listID}/items")]
        public void AddTodoItem(Guid listID, [FromBody] ModifyTodoItemDTO dto)
        {
            _todoService.CreateListItem(listID, dto.Title, dto.Date);
        }

        [HttpPost]
        [Route("{listID}/items/{itemID}")]
        public void ModifyTodoItem(Guid listID, Guid itemID, [FromBody] ModifyTodoItemDTO dto)
        {
            _todoService.ChangeListItem(listID, itemID, dto.Title, dto.Date);
        }

        [HttpPost]
        [Route("{listID}/items/{itemID}/toggle")]
        public void ToggleTodoItem(Guid listID, Guid itemID)
        {
            _todoService.ToggleListItem(listID, itemID);
        }

        [HttpDelete]
        [Route("{listID}/items/{itemID}")]
        public void DeleteTodoItem(Guid listID, Guid itemID)
        {
            _todoService.DeleteListItem(listID, itemID);
        }

    }
}
