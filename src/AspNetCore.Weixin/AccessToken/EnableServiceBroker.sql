USE master
GO

--1.检查数据库服务器版本
--必须SQL Server 2005或以上版本数据库才支持ServiceBroker特性
select @@version
GO

/*2.为数据库开启ServiceBroker功能

请将DbnameXxx修改为您的数据库名！！！

--执行过程中有可能出现一些问题，请参考以下故障排查方法：
--1.若数据库被长时间挂起
--执行exec sp_who2 查出有问题的进程号，然后kill <spid>

若有任何问题，请联系黄象尧（4848285@qq.com）。
*/
ALTER DATABASE [DbnameXxx] SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE;
GO

--3.检查数据库是否成功开启ServiceBroker功能
SELECT name, is_broker_enabled FROM sys.databases 
WHERE name='DbnameXxx'
-- ORDER BY name
GO

