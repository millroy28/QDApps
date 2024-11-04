/*
 Scaffold-DbContext "Server=.\SQLExpress;Database=QDApps;Trusted_Connection=True;encrypt=false;"  Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force -ContextDir Context;

*/

CREATE TABLE TimeZones (
 TimeZoneId INT IDENTITY(1,1) NOT NULL
,CONSTRAINT [PK_TimeZoneId] PRIMARY KEY CLUSTERED (TimeZoneId)
,TimeZoneName VARCHAR(50) NOT NULL
,UTCOffset INT NOT NULL
)


INSERT INTO TimeZones (TimeZoneName, UTCOffset)
VALUES ('Hawaii Standard Time', -10),
       ('Alaska Standard Time', -9),
       ('Pacific Standard Time', -8),
       ('Mountain Standard Time', -7),
       ('Central Standard Time', -6),
       ('Eastern Standard Time', -5)

	   		  
CREATE TABLE Users (
 UserId INT IDENTITY (1,1) NOT NULL
,CONSTRAINT [PK_UserId] PRIMARY KEY CLUSTERED (UserId)
,AspNetUserId NVARCHAR(450) NOT NULL
,CONSTRAINT [FK_Users_AspNetUserId] FOREIGN KEY (AspNetUserId) REFERENCES AspNetUsers(Id)
,UserName NVARCHAR(300)
,TimeZoneId INT NOT NULL DEFAULT 6
,CONSTRAINT [FK_Users_TimeZoneId] FOREIGN KEY (TimeZoneId) REFERENCES TimeZones(TimeZoneId)

)



-- WHERE IT APP TABLES
go
CREATE SCHEMA wia
go


CREATE TABLE wia.Stashes(
 StashId INT IDENTITY (1,1) NOT NULL
,CONSTRAINT [PK_StashId] PRIMARY KEY CLUSTERED (StashId)
,UserId INT NOT NULL
,CONSTRAINT [FK_Stashes_UserId] FOREIGN KEY (UserId) REFERENCES Users(UserId)
,StashName NVARCHAR(200) NOT NULL
,CreatedAt DATETIME
,UpdatedAt DATETIME
)

CREATE TABLE wia.Tags(
 TagId INT IDENTITY (1,1) NOT NULL
,CONSTRAINT [PK_TagId] PRIMARY KEY CLUSTERED (TagId)
,UserId INT NOT NULL
,CONSTRAINT [FK_Tags_UserId] FOREIGN KEY (UserId) REFERENCES Users(UserId)
,TagName NVARCHAR(200) NOT NULL
,CreatedAt DATETIME
,UpdatedAt DATETIME

)


CREATE TABLE wia.Items(
 ItemId INT IDENTITY (1,1) NOT NULL
,CONSTRAINT [PK_ItemId] PRIMARY KEY CLUSTERED (ItemId)
,StashId INT 
,CONSTRAINT [FK_Items_StashId] FOREIGN KEY (StashId) REFERENCES wia.Stashes(StashId)
,ItemName NVARCHAR(200) NOT NULL
,CreatedAt DATETIME
,UpdatedAt DATETIME
)

CREATE TABLE wia.ItemTags(
 ItemTagId INT IDENTITY (1,1) NOT NULL
,CONSTRAINT [PK_ItemTagId] PRIMARY KEY CLUSTERED (ItemTagId)
,TagId INT NOT NULL
,CONSTRAINT [FK_ItemTags_TagId] FOREIGN KEY (TagId) REFERENCES wia.Tags(TagId)
,ItemId INT NOT NULL
,CONSTRAINT [FK_ItemTags_ItemId] FOREIGN KEY (ItemId) REFERENCES wia.Items(ItemId)
,CreatedAt DATETIME

)

GO;
CREATE VIEW wia.ViewStashes
AS

	SELECT s.UserId
		  ,s.StashId
		  ,s.StashName
		  ,ic.Items AS ItemCount

	  FROM wia.Stashes s
	
	 CROSS APPLY (
		SELECT COUNT(i.ItemId) AS Items
		      ,i.StashId

		  FROM wia.Items i

		 WHERE i.StashId = s.StashId
		
		 GROUP BY i.StashId
	 ) AS ic


GO;
CREATE VIEW wia.StashTags 
AS

	SELECT 
	    s.UserId
	   ,s.StashId
	   ,t.TagId
	   ,t.TagName
	
	  FROM wia.Tags t
	  
	 INNER JOIN wia.ItemTags it
	    ON it.TagId = t.TagId

	 INNER JOIN wia.Items i
	    ON i.ItemId = it.TagId

	 INNER JOIN wia.Stashes s
	    ON s.StashId = i.StashId
		
	 GROUP BY s.UserId, s.StashId, t.TagId, t.TagName

GO;

GO;
CREATE VIEW wia.ViewItems AS
	SELECT i.ItemId
		  ,i.ItemName
		  ,s.StashName
		  ,s.StashId
		  ,s.UserId

	  FROM wia.Items i

	 INNER JOIN wia.Stashes s
	    ON s.StashId = i.StashId




GO;

CREATE VIEW wia.ViewTags  AS
	SELECT t.UserId
	      ,t.TagId
		  ,t.TagName
		  ,tc.ItemCount

	  FROM wia.Tags t

	 CROSS APPLY (
		SELECT COUNT(i.ItemId) AS ItemCount
		  FROM wia.Items i

		 INNER JOIN wia.ItemTags it
		    ON it.ItemId = i.ItemId

		 WHERE it.TagId = t.TagId
		 GROUP BY it.TagId
	 ) AS tc

	 


CREATE VIEW wia.ItemTagNames AS

SELECT i.ItemId
      ,s.UserId
	  ,t.TagId
	  ,t.TagName
       

  FROM wia.Items i

 INNER JOIN wia.ItemTags it
    ON it.ItemId = i.ItemId

 INNER JOIN wia.Tags t
    ON t.TagId = it.TagId
 
 INNER JOIN wia.Stashes s
    ON i.StashId = s.StashId

GO;

