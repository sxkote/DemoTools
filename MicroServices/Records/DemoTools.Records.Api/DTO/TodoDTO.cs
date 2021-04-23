using System;

namespace DemoTools.Records.Api.DTO
{
    public class ModifyTodoListDTO
    {
        public string Title { get; set; }
    }

    public class ModifyTodoItemDTO
    {
        public string Title { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
