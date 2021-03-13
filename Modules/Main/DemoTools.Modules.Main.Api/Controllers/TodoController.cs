using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Domain.Entities.Todos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DemoTools.Modules.Main.Api.Controllers
{
    [Route("api/todo")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }


        [Authorize(AuthenticationSchemes ="Token", Roles = "User")]
        [HttpGet]
        [Route("")]
        public IEnumerable<TodoList> GetTodoLists()
        {
            return _todoService.GetAllLists();
        }

        [Authorize(AuthenticationSchemes = "Token", Roles = "User")]
        [HttpGet]
        [Route("{id}")]
        public TodoList GetTodoList(Guid id)
        {
            return _todoService.GetList(id);
        }
    }
}
