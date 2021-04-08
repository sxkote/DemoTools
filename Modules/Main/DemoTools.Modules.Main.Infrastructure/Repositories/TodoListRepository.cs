﻿using DemoTools.Modules.Main.Domain.Contracts.Repositories;
using DemoTools.Modules.Main.Domain.Entities.Todo;
using DemoTools.Modules.Main.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SX.Common.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoTools.Modules.Main.Infrastructure.Repositories
{
    public class TodoListRepository : DomainRepositoryGuid<MainDbContext, TodoList>, ITodoListRepository
    {
        public TodoListRepository(MainDbContext dbContext)
               : base(dbContext)
        { }

        protected override IQueryable<TodoList> GetQuery(bool multiple, bool tracking = false)
        {
            var query = this.DbSet.AsQueryable();

            if (!multiple)
            {
                query = query.Include(t => t.Items);
            }

            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }

        protected override void DeleteReferences(TodoList entity)
        {
            this.DbContext.Set<TodoItem>()
                .RemoveRange(entity.Items);
        }

        public IEnumerable<TodoList> GetAll(Guid subscriptionID)
        {
            return this.QueryAll
                .Where(t => t.SubscriptionID == subscriptionID)
                .ToList();
        }
    }
}
