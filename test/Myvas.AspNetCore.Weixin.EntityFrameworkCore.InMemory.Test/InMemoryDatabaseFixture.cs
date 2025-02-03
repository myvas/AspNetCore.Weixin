using Microsoft.Data.Sqlite;
using System;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.InMemory.Test
{
    public class InMemoryDatabaseFixture : IDisposable
    {
        private readonly SqliteConnection _connection = new SqliteConnection($"DataSource=:memory:");

        public InMemoryDatabaseFixture()
        {
            _connection.Open();
        }

        public SqliteConnection Connection => _connection;

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
