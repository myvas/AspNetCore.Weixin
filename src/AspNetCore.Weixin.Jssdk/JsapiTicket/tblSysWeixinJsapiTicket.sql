
CREATE TABLE [dbo].[tblSysWeixinJsapiTicket](
	[AccessToken] [varchar](512) NOT NULL,
	[JsapiTicket] [varchar](512) NOT NULL,
	[ExpiresIn] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tblSysWeixinJsapiTicket] ADD  CONSTRAINT [DF_tblSysWeixinJsapiTicket_ExpiresIn]  DEFAULT ((7200)) FOR [ExpiresIn]
GO

ALTER TABLE [dbo].[tblSysWeixinJsapiTicket] ADD  CONSTRAINT [DF_tblSysWeixinJsapiTicket_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

/*
SELECT AccessToken, JsapiTicket, ExpiresIn, CreateTime 
FROM tblSysWeixinJsapiTicket 

INSERT INTO tblSysWeixinJsapiTicket(AccessToken, JsapiTicket, ExpiresIn, CreateTime)
VALUES('XkC7gqcx7pctuGe5zPIita23N7dJKIfkwz_2ULReV_Pn7T09lMyhTwlgGK5cghtqGQlPUlZ7ur_nMqhGuJNXwA','HH0qQPZfY-gq0yUTlW1BYOhpLFGOBLHvxPkAQgB-X706BGG3yMdowgmTLtDZRpaUgp4_-aYuhbJOj70dvgttjhvvMeK3THP_fTnzkXp1r6g',
7200,'2014-12-05 12:55:48')

IF EXISTS(SELECT 1 FROM tblSysWeixinJsapiTicket WHERE AccessToken='XkC7gqcx7pctuGe5zPIita23N7dJKIfkwz_2ULReV_Pn7T09lMyhTwlgGK5cghtqGQlPUlZ7ur_nMqhGuJNXwA') 
print 'exists'

UPDATE tblSysWeixinJsapiTicket 
SET JsapiTicket='wrongJsapiTicketxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx', 
ExpiresIn=7200, 
CreateTime=getdate() 
WHERE AccessToken='XkC7gqcx7pctuGe5zPIita23N7dJKIfkwz_2ULReV_Pn7T09lMyhTwlgGK5cghtqGQlPUlZ7ur_nMqhGuJNXwA'


IF EXISTS(SELECT 1 FROM tblSysWeixinJsapiTicket WHERE AccessToken='XkC7gqcx7pctuGe5zPIita23N7dJKIfkwz_2ULReV_Pn7T09lMyhTwlgGK5cghtqGQlPUlZ7ur_nMqhGuJNXwA') 
BEGIN
    UPDATE tblSysWeixinJsapiTicket 
    SET JsapiTicket='tesetstesfsafsafdasfdsafasfasfddsafsafds', 
        ExpiresIn=7200, 
        CreateTime='2014-11-11' 
    WHERE AccessToken='XkC7gqcx7pctuGe5zPIita23N7dJKIfkwz_2ULReV_Pn7T09lMyhTwlgGK5cghtqGQlPUlZ7ur_nMqhGuJNXwA'
END
ELSE
BEGIN
    INSERT INTO tblSysWeixinJsapiTicket(AccessToken, JsapiTicket, ExpiresIn, CreateTime)
    VALUES('XkC7gqcx7pctuGe5zPIita23N7dJKIfkwz_2ULReV_Pn7T09lMyhTwlgGK5cghtqGQlPUlZ7ur_nMqhGuJNXwA', 'tesetstesfsafsafdasfdsafasfasfddsafsafds', 7200, '2014-11-11')
END
*/