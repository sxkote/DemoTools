using DemoTools.Records.Domain.Contracts;
using DemoTools.Records.Domain.Entities;
using DemoTools.Records.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SX.Common.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTools.Records.Infrastructure.Services
{
    public class TodoService : ITodoService
    {
        private readonly RecordsDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;

        protected DbSet<TodoList> TodoLists => _dbContext.Set<TodoList>();

        public TodoService(RecordsDbContext dbContext, ITokenProvider tokenProvider)
        {
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
        }

        protected void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        protected TodoList GetListTracking(Guid listID)
        {
            var token = _tokenProvider.GetToken();

            var list = this.TodoLists
                .Include(t => t.Items)
                .FirstOrDefault(t => t.ID == listID);

            if (!list.HasAccess(token))
                return null;

            return list;
        }

        public IEnumerable<TodoList> GetAllLists()
        {
            var token = _tokenProvider.GetToken();

            return this.TodoLists
                .Where(t => t.SubscriptionID == token.SubscriptionID)
                .ToList()
                .Where(t => t.HasAccess(token))
                .ToList();
        }
        public TodoList GetList(Guid listID)
        {
            return this.GetListTracking(listID);
        }
        public TodoList CreateList(string title)
        {
            var token = _tokenProvider.GetToken();

            var list = TodoList.Create(token.SubscriptionID, title);

            this.TodoLists.Add(list);

            this.SaveChanges();

            return list;
        }
        public void ChangeList(Guid listID, string title)
        {
            var list = this.GetListTracking(listID);
            if (list == null)
                return;

            list.Change(title);

            this.TodoLists.Update(list);

            this.SaveChanges();
        }
        public void DeleteList(Guid listID)
        {
            var list = this.GetListTracking(listID);
            if (list == null)
                return;

            this.TodoLists.Remove(list);

            this.SaveChanges();
        }

        public void CreateListItem(Guid listID, string title, DateTime dueDate)
        {
            var list = this.GetListTracking(listID);
            if (list == null)
                return;

            var item = TodoItem.Create(title);
            list.Items.Add(item);

            this.SaveChanges();
        }

        public void ChangeListItem(Guid listID, Guid itemID, string title, DateTime dueDate)
        {
            var list = this.GetListTracking(listID);
            if (list == null)
                return;

            var item = list.Items.FirstOrDefault(i => i.ID == itemID);
            item.Change(title);

            this.TodoLists.Update(list);
            this.SaveChanges();
        }

        public void ToggleListItem(Guid listID, Guid itemID)
        {
            var list = this.GetListTracking(listID);
            if (list == null)
                return;

            var item = list.Items.FirstOrDefault(i => i.ID == itemID);
            item.ToggleMark();

            this.TodoLists.Update(list);
            this.SaveChanges();
        }

        public void DeleteListItem(Guid listID, Guid itemID)
        {
            var list = this.GetListTracking(listID);
            if (list == null)
                return;

            var item = list.Items.FirstOrDefault(i => i.ID == itemID);
            list.Items.Remove(item);

            this.TodoLists.Update(list);
            this.SaveChanges();
        }
    }
}
