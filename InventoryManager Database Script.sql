/**********************************************/

/*               CREATE DATABASE              */

/**********************************************/
CREATE DATABASE [InventoryManager]
GO
USE [InventoryManager]



/**********************************************/

/*              CREATE ROLE & USER            */

/**********************************************/
-- Create READ/WRITE Role for Security of Web Application 
CREATE ROLE [ReadWrite]
-- give role READ permission to ALL tables
EXEC sp_addrolemember 'db_datareader', 'ReadWrite'
-- give role WRITE permission to ALL tables
EXEC sp_addrolemember 'db_datawriter', 'ReadWrite'
-- give role permission to run procedures
GRANT EXECUTE TO [ReadWrite]
-- Create user for Web Application
IF (NOT EXISTS ( SELECT * FROM sys.server_principals WHERE name = 'InvMgrWebApp' ) )
	BEGIN
		CREATE LOGIN [InvMgrWebApp] WITH PASSWORD = 'Th1$1$Cryp+1C'
	END
-- Add User to DB and Role
EXEC sp_adduser [InvMgrWebApp], [InvMgrWebApp], [ReadWrite]



/**********************************************/

/*                 CREATE SCHEMA              */

/**********************************************/
IF (NOT EXISTS ( SELECT * FROM sys.schemas WHERE name = 'InvMgr' ) )
	BEGIN
		EXEC ('CREATE SCHEMA [InvMgr] AUTHORIZATION [dbo]')
	END



/**********************************************/

/*                CREATE TABLES               */

/**********************************************/

-- Use tbl_ prefix as DB could have thousands of tables, stored procedures, views, ect
-- GUID as Primary Key
-- DateCreated so we know when every record was made
-- Active flag to turn records on/off, we never delete anything for reporting purposes
-- Stores table allows managment of multiple locations

CREATE TABLE [InvMgr].[tbl_Stores](
	[Guid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Address] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[Zipcode] [nvarchar](15) NULL,
	[PhoneNumber] [nvarchar](15) NULL,
	[EmailAddress] [nvarchar](255) NULL,
	[Logo] [varbinary](max) NULL,
	[LogoName] [nvarchar](255) NULL,
	[DateCreated] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Stores] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [InvMgr].[tbl_Items](
	[Guid] [uniqueidentifier] NOT NULL,
	[StoreGuid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Category] [nvarchar](255) NOT NULL,
	[SellIn] [int] NOT NULL,
	[Quality] [int] NOT NULL,
	[Legendary] [bit] NOT NULL,
	[BetterWithAge] [bit] NOT NULL,
	[Image] [varbinary](max) NULL,
	[ImageName] [nvarchar](255) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateSold] [datetime] NULL,
	[DateTrashed] [datetime] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [InvMgr].[tbl_Items]  WITH CHECK ADD  CONSTRAINT [FK_ItemsStore] FOREIGN KEY([StoreGuid])
REFERENCES [InvMgr].[tbl_Stores] ([Guid])
GO

ALTER TABLE [InvMgr].[tbl_Items] CHECK CONSTRAINT [FK_ItemsStore]
GO


CREATE TABLE [InvMgr].[tbl_DailyUpdateLog](
	[Guid] [uniqueidentifier] NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Type] [nvarchar](255) NULL,
 CONSTRAINT [PK_DailyUpdateLog] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]



/**********************************************/

/*                  SEED DATA                 */

/**********************************************/

INSERT INTO [InvMgr].tbl_Stores VALUES
       (NEWID()
            , 'Gilded Rose'
            , 'The Gilded Rose is an inn located in the Trade District. It is a popular resting point for travelers passing through the human capital of Stormwind City due to its proximity to the bank, Auction House, and an ever-popular mailbox. Its name is readable from the sign outside.  Innkeeper Allison is the host and offers a Hearthstone point as well as being a vendor of food and drink.'
            , 'Trade District'
            , 'Stormwind City'
            , 'Warcraft'
            , 'BZ'
			, '800-592-5499'
			, 'GildedRose@blizzard.com'
            , NULL
			, 'The_Gilded_Rose.jpg'
            , GETDATE()
            , 1
       )

-- seeding Guid for this store to seed items below
INSERT INTO [InvMgr].tbl_Stores VALUES
       ('6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d'
            , 'Lion''s Pride Inn'
            , 'Lion''s Pride Inn, aka Goldshire Inn, is the main inn of the town of Goldshire, located in Elwynn Forest. It is known for the frequent duels that take place just outside its entrance, and the just as frequent "exotic entertainment" that Goldshire is infamous for.  Quite possibly the busiest non-city inn of all the zones, Lion''s Pride offers a wide range of services from class trainers and quest givers to drinks aplenty.'
            , 'Goldshire'
            , 'Elywnn Forest'
            , 'Warcraft'
            , 'BZ'
			, '800-592-5499'
			, 'LionsPride@blizzard.com'
            ,  NULL
			, 'Lions_Pride_Inn.jpg'
            , GETDATE()
            , 1
       )

INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Sword', 'Weapon',30,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Axe', 'Weapon',40,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Halberd', 'Weapon',60,40, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Aged Brie', 'Food',50,10, 0, 1, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Aged Milk', 'Food',20,20, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Mutton', 'Food',10,10, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Hand of Ragnaros', 'Sulfuras',80,80, 1, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'I am Murloc', 'Backstage Passes',20,10, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Raging Ogre', 'Backstage Pasess',10,10, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Giant Slayer', 'Conjured',15,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Storm Hammer', 'Conjured',20,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Belt of Giant Strength', 'Conjured',20,40, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Cheese', 'Food',5,5, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Potion of Healing', 'Potion',10,10, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Bag of Holding', 'Misc',10,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'TAFKAL80ETC Concert', 'Backstage Passes',15,20, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Elixir of the Mongoose', 'Potion',5,7, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', '+5 Dexterity Vest', 'Armor',10,20, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Full Plate Mail', 'Armor',50,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Wooden Shield', 'Armor',10,30, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)

INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Sword', 'Weapon',30,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Axe', 'Weapon',40,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Halberd', 'Weapon',60,40, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Aged Brie', 'Food',50,10, 0, 1, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Aged Milk', 'Food',20,20, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Mutton', 'Food',10,10, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Hand of Ragnaros', 'Sulfuras',80,80, 1, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'I am Murloc', 'Backstage Passes',20,10, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Raging Ogre', 'Backstage Pasess',10,10, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Giant Slayer', 'Conjured',15,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Storm Hammer', 'Conjured',20,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Belt of Giant Strength', 'Conjured',20,40, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Cheese', 'Food',5,5, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Potion of Healing', 'Potion',10,10, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Bag of Holding', 'Misc',10,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'TAFKAL80ETC Concert', 'Backstage Passes',15,20, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Elixir of the Mongoose', 'Potion',5,7, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', '+5 Dexterity Vest', 'Armor',10,20, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Full Plate Mail', 'Armor',50,50, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)
INSERT INTO [InvMgr].tbl_Items VALUES (NEWID(), '6c9b11a4-3c37-4953-8cc7-f7ddcd3dd71d', 'Wooden Shield', 'Armor',10,30, 0, 0, NULL, NULL, GETDATE(), NULL, NULL, 1)