using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Myvas.AspNetCore.Weixin.EfCore.Tests;

public class InMemoryTests
{
    [Fact]
    public void Subscribers_AddEntity()
    {
        var randomDatabaseName = Guid.NewGuid().ToString("N");

        // Arrange
        var options = new DbContextOptionsBuilder<WeixinDbContext>()
            .UseInMemoryDatabase(databaseName: randomDatabaseName) // Use in-memory database
            .Options;

        var randomOpenId = Guid.NewGuid().ToString("N");
        var randomNickname = Guid.NewGuid().ToString("N");

        // Act
        using (var context = new WeixinDbContext(options))
        {
            var entity = new WeixinSubscriberEntity
            {
                OpenId = randomOpenId,
                Nickname = randomNickname
            };
            context.WeixinSubscribers.Add(entity);
            context.SaveChanges();
        }

        // Assert
        using (var context = new WeixinDbContext(options))
        {
            var savedEntity = context.WeixinSubscribers.FirstOrDefault(x => x.OpenId == randomOpenId);
            Assert.NotNull(savedEntity);
            Assert.Equal(randomNickname, savedEntity.Nickname);
        }
    }

    [Fact]
    public void ReceivedMessages_AddEntity()
    {
        var randomDatabaseName = Guid.NewGuid().ToString("N");

        // Arrange
        var options = new DbContextOptionsBuilder<WeixinDbContext>()
            .UseInMemoryDatabase(databaseName: randomDatabaseName) // Use in-memory database
            .Options;

        var randomMsgId = new Random().Next(int.MaxValue);
        var randomContent = Guid.NewGuid().ToString("N");

        // Act
        using (var context = new WeixinDbContext(options))
        {
            var entity = new WeixinReceivedMessageEntity
            {
                MsgId = randomMsgId,
                Content = randomContent
            };
            context.WeixinReceivedMessages.Add(entity);
            context.SaveChanges();
        }

        // Assert
        using (var context = new WeixinDbContext(options))
        {
            var savedEntity = context.WeixinReceivedMessages.FirstOrDefault(x => x.MsgId == randomMsgId);
            Assert.NotNull(savedEntity);
            Assert.Equal(randomContent, savedEntity.Content);
        }
    }
}