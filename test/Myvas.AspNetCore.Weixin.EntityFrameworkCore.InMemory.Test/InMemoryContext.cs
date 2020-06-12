using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net;
using System.Text;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.InMemory.Test
{
    public class InMemoryContext : WeixinDbContext<WeixinSubscriber, string>
    {
        private readonly DbConnection _connection;

        private InMemoryContext(DbConnection connection)
        {
            _connection = connection;
        }

        public static InMemoryContext Create(DbConnection connection)
             => InMemoryContext.Initialize(new InMemoryContext(connection));

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite(_connection);

        public static TContext Initialize<TContext>(TContext context)
            where TContext : DbContext
        {
            context.Database.EnsureCreated();
            return context;
        }
    }

    public abstract class InMemoryContext<TWeixinSubscriber, TKey> : WeixinDbContext<TWeixinSubscriber, TKey>
        where TWeixinSubscriber : WeixinSubscriber<TKey>
        where TKey : IEquatable<TKey>
    {
        protected InMemoryContext(DbContextOptions options) : base(options) { }
    }
}
