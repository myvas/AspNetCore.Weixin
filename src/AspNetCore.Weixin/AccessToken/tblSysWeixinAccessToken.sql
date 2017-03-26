
CREATE TABLE [dbo].[tblSysWeixinAccessToken](
	[AppId] [varchar](50) NOT NULL,
	[AccessToken] [varchar](512) NOT NULL,
	[ExpiresIn] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tblSysWeixinAccessToken] ADD  CONSTRAINT [DF_tblSysWeixinAccessToken_ExpiresIn]  DEFAULT ((7200)) FOR [ExpiresIn]
GO

ALTER TABLE [dbo].[tblSysWeixinAccessToken] ADD  CONSTRAINT [DF_tblSysWeixinAccessToken_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

/*
SELECT AppId, AccessToken, ExpiresIn, CreateTime 
FROM tblSysWeixinAccessToken 

INSERT INTO tblSysWeixinAccessToken(AppId, AccessToken, ExpiresIn, CreateTime)
VALUES('wx02056e2b2b9cc4ef','HH0qQPZfY-gq0yUTlW1BYOhpLFGOBLHvxPkAQgB-X706BGG3yMdowgmTLtDZRpaUgp4_-aYuhbJOj70dvgttjhvvMeK3THP_fTnzkXp1r6g',
7200,'2014-12-05 12:55:48')

IF EXISTS(SELECT 1 FROM tblSysWeixinAccessToken WHERE AppId='wx02056e2b2b9cc4ef') 
print 'exists'

UPDATE tblSysWeixinAccessToken 
SET AccessToken='wrongaccesstokenxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx', 
ExpiresIn=7200, 
CreateTime=getdate() 
WHERE AppId='wx02056e2b2b9cc4ef'


IF EXISTS(SELECT 1 FROM tblSysWeixinAccessToken WHERE AppId='wx02056e2b2b9cc4ef') 
BEGIN
    UPDATE tblSysWeixinAccessToken 
    SET AccessToken='tesetstesfsafsafdasfdsafasfasfddsafsafds', 
        ExpiresIn=7200, 
        CreateTime='2014-11-11' 
    WHERE AppId='wx02056e2b2b9cc4ef'
END
ELSE
BEGIN
    INSERT INTO tblSysWeixinAccessToken(AppId, AccessToken, ExpiresIn, CreateTime)
    VALUES('wx02056e2b2b9cc4ef', 'tesetstesfsafsafdasfdsafasfasfddsafsafds', 7200, '2014-11-11')
END
*/