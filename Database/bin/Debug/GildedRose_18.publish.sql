﻿/*
Deployment script for GildedRose

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "GildedRose"
:setvar DefaultFilePrefix "GildedRose"
:setvar DefaultDataPath "C:\Users\Todd\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB"
:setvar DefaultLogPath "C:\Users\Todd\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
DECLARE @Users TABLE (
	[Id] INT NOT NULL, 
    [UserName] NVARCHAR(100) NOT NULL,
	[Email] [Email] NOT NULL,
	[PasswordHash] [PWD] NOT NULL,
	[Created] [AuditDate] DEFAULT getutcdate(),
    [CreatedBy] [AuditUser]
);

 
INSERT INTO 
	@Users ([Id], [UserName], [Email], [PasswordHash], [CreatedBy]) 
VALUES
(1, 'talkersoft', 'talkersoft@gmail.com', 'abc', 1),
(2, 'jgretz', 'jgretz@truefit.io', 'efg', 1)

 
-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE membership.Users AS t
USING @Users as s
	on 	(t.[Id] = s.[Id])
WHEN NOT MATCHED BY TARGET
    THEN INSERT (UserName, Email, PasswordHash, CreatedBy)
        VALUES (s.UserName, s.Email, s.PasswordHash, s.CreatedBy)
WHEN MATCHED
    THEN UPDATE SET 
				t.UserName = s.UserName, 
				t.Email = s.Email, 
				t.PasswordHash = s.PasswordHash
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;
DECLARE @Categories TABLE (
    [Id] INT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
	[Created] [AuditDate] NOT NULL DEFAULT getutcdate(),
    [CreatedBy] [AuditUser] NOT NULL
);

 
INSERT INTO 
	@Categories ([Id], [Name], [CreatedBy]) 
VALUES
(1, 'Weapon', 1),
(2, 'Food', 1),
(3, 'Sulfuras', 1),
(4, 'Backstage Passes', 1),
(5, 'Conjured', 1),
(6, 'Potion', 1),
(7, 'Misc', 1),
(8, 'Armor', 1);

 
-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE inventory.Categories AS t
USING @Categories as s
	on 	(t.[Id] = s.[Id])
WHEN NOT MATCHED BY TARGET
    THEN INSERT ([Name], CreatedBy)
        VALUES (s.[Name], s.CreatedBy)
WHEN MATCHED
    THEN UPDATE SET t.[Name] = s.[Name]
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;

DECLARE @Items TABLE (
    [Identifier] UNIQUEIDENTIFIER NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Category] INT NOT NULL, 
	[ShelfLife] INT NOT NULL,
	[MaxQuality] INT NOT NULL,
	[IsLegendary] BIT NOT NULL,
	[IsDeleted] BIT NOT NULL,
	[Created] [AuditDate] NOT NULL DEFAULT getutcdate(),
    [CreatedBy] [AuditUser] NOT NULL
);

 
INSERT INTO 
	@Items ([Identifier] ,[Name] ,[Category] ,[ShelfLife] ,[MaxQuality] ,[IsLegendary] ,[IsDeleted] ,[CreatedBy]) 
VALUES
	('f3205dfd-55fe-4cd5-8070-b259e9db2f7b', 'Sword', dbo.getCategoryIdByName('Weapon'), 30, 50, 0, 0, 1),
	('35096084-af46-4e40-932e-655aab9bac00', 'Axe', dbo.getCategoryIdByName('Weapon'), 40, 50, 0, 0, 1),
	('e12e2698-79b6-4f15-bfa2-a1fab047aa27', 'Halberd', dbo.getCategoryIdByName('Weapon'), 60, 40, 0, 0, 1),
	('dddd657e-9617-4c82-bdc5-0ab53b5a6398', 'Aged Brie', dbo.getCategoryIdByName('Food'), 50, 10, 0, 0, 1),
	('02223851-8f9a-4b55-a925-e59be7855413', 'Aged Milk', dbo.getCategoryIdByName('Food'), 20, 20, 0, 0, 1),
	('e52e05f0-aba1-44e8-8003-435bb20bd660', 'Mutton', dbo.getCategoryIdByName('Food'), 10, 10, 0, 0, 1),
	('eea3072c-1579-469a-81c9-f2ba6302a0ca', 'Hand of Ragnaros', dbo.getCategoryIdByName('Sulfuras'), 80, 80, 1, 0, 1),
	('e2055eae-6d1f-45a9-9701-7c5fbf563e20', 'I am Murloc', dbo.getCategoryIdByName('Backstage Passes'), 20, 10, 0, 0, 1),
	('960fa030-70ac-4ff7-8ad8-07cea6ca84ce', 'Raging Ogre', dbo.getCategoryIdByName('Backstage Passes'), 10, 10, 0, 0, 1),
	('95a79f27-61e1-4c70-8039-0fdd703fc6e9', 'Giant Slayer', dbo.getCategoryIdByName('Conjured'), 15, 50, 0, 0, 1),
	('8686e4d9-a43c-4c46-bb72-81e9454609c6', 'Storm Hammer', dbo.getCategoryIdByName('Conjured'), 20, 50, 0, 0, 1),
	('3884fae6-6dd9-4e4f-bc04-bcb90f18dae7', 'Belt of Giant Strength', dbo.getCategoryIdByName('Conjured'), 20, 40, 0, 0, 1),
	('fc22fba7-7f78-4ac5-a155-d89a40a1940a', 'Cheese', dbo.getCategoryIdByName('Food'), 5, 5, 0, 0, 1),
	('f1ab5859-cd8a-46b2-9520-dd631cbde700', 'Potion of Healing', dbo.getCategoryIdByName('Potion'), 10, 10, 0, 0, 1),
	('a5091448-6848-4247-8d85-2ee5a6a8defb', 'Bag of Holding', dbo.getCategoryIdByName('Misc'), 10, 50, 0, 0, 1),
	('58c1acc3-0ce7-4a06-86f3-5052d223e48d', 'TAFKAL80ETC Concert', dbo.getCategoryIdByName('Backstage Passes'), 15, 20, 0, 0, 1),
	('49ca5c68-4fbd-4ef6-9fe8-e086c6ef81e6', 'Elixir of the Mongoose', dbo.getCategoryIdByName('Potion'), 5, 7, 0, 0, 1),
	('88bdb452-e23d-4b70-b07e-c9f3f3f0d1a9', '+5 Dexterity Vest', dbo.getCategoryIdByName('Armor'), 10, 20, 0, 0, 1),
	('15298b70-b493-43a8-b01e-2cccf5514a89', 'Full Plate Mail', dbo.getCategoryIdByName('Armor'), 50, 50, 0, 0, 1),
	('4bb48e57-245a-419e-880c-1b701dbb35c2', 'Wooden Shield', dbo.getCategoryIdByName('Armor'), 10, 30, 0, 0, 1);

 
-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE inventory.Items AS t
USING @Items as s
	on 	(t.[Identifier] = s.[Identifier])
WHEN NOT MATCHED BY TARGET
    THEN INSERT ([Identifier], [Name], Category, ShelfLife, MaxQuality, IsLegendary, IsDeleted, CreatedBy)
        VALUES (s.[Identifier], s.[Name], s.Category, s.ShelfLife, s.MaxQuality, s.IsLegendary, s.IsDeleted, s.CreatedBy)
WHEN MATCHED
    THEN UPDATE SET 
		t.[Identifier] = s.[Identifier], 
		t.[Name] = s.[Name],
		t.Category = s.Category,
		t.ShelfLife = s.ShelfLife, 
		t.MaxQuality = s.MaxQuality,
		t.IsLegandary = s.IsLegendary,
		t.IsDeleted = s.IsDeleted
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;


GO

GO
PRINT N'Update complete.';


GO
