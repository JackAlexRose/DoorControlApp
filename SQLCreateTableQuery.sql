CREATE DATABASE Doors;
GO

USE Doors;

CREATE TABLE [dbo].[doors](
	[iD] [uniqueidentifier] NULL,
	[label] [nvarchar](255) NULL,
	[status] [bit] NULL
) ON [PRIMARY]
GO

INSERT INTO [dbo].[doors]
           ([iD]
           ,[label]
           ,[status])
     VALUES
           (NEWID(),'Door1',0)
		   ,(NEWID(),'Door2',0)
		   ,(NEWID(),'Door3',0)
		   ,(NEWID(),'Door4',0)
		   ,(NEWID(),'Door5',0)
		   ,(NEWID(),'Door6',0)
GO