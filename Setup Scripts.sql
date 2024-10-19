

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