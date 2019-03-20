using AspNetCore.Weixin;
using Demo.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Data
{
	public class WeixinDbContext:DbContext
	{
		public WeixinDbContext(DbContextOptions<WeixinDbContext> options) : base(options)
		{
		}
	
		public DbSet<WeixinSubscriber> Subscribers { get; set; }
		public DbSet<ReceivedTextMessage> ReceivedTextMessages { get; set; }
	}
}
