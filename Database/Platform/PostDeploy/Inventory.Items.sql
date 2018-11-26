DECLARE @Items TABLE (
    [Identifier] UNIQUEIDENTIFIER NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Category] INT NOT NULL, 
	[ShelfLife] INT NOT NULL,
	[IsDeleted] BIT NOT NULL,
    [CreatedBy] [AuditUser] NOT NULL
);

 
INSERT INTO 
	@Items ([Identifier] ,[Name] ,[Category] ,[ShelfLife] ,[IsDeleted] ,[CreatedBy]) 
VALUES
	('f3205dfd-55fe-4cd5-8070-b259e9db2f7b', 'Sword', dbo.getCategoryIdByName('Weapon'), 30, 0, 1),
	('35096084-af46-4e40-932e-655aab9bac00', 'Axe', dbo.getCategoryIdByName('Weapon'), 40, 0, 1),
	('e12e2698-79b6-4f15-bfa2-a1fab047aa27', 'Halberd', dbo.getCategoryIdByName('Weapon'), 60, 0, 1),
	('dddd657e-9617-4c82-bdc5-0ab53b5a6398', 'Aged Brie', dbo.getCategoryIdByName('Food'), 50, 0, 1),
	('02223851-8f9a-4b55-a925-e59be7855413', 'Aged Milk', dbo.getCategoryIdByName('Food'), 20, 0, 1),
	('e52e05f0-aba1-44e8-8003-435bb20bd660', 'Mutton', dbo.getCategoryIdByName('Food'), 10, 0, 1),
	('eea3072c-1579-469a-81c9-f2ba6302a0ca', 'Hand of Ragnaros', dbo.getCategoryIdByName('Sulfuras'), 80, 0, 1),
	('e2055eae-6d1f-45a9-9701-7c5fbf563e20', 'I am Murloc', dbo.getCategoryIdByName('Backstage Passes'), 20, 0, 1),
	('960fa030-70ac-4ff7-8ad8-07cea6ca84ce', 'Raging Ogre', dbo.getCategoryIdByName('Backstage Passes'), 10, 0, 1),
	('95a79f27-61e1-4c70-8039-0fdd703fc6e9', 'Giant Slayer', dbo.getCategoryIdByName('Conjured'), 15, 0, 1),
	('8686e4d9-a43c-4c46-bb72-81e9454609c6', 'Storm Hammer', dbo.getCategoryIdByName('Conjured'), 20, 0, 1),
	('3884fae6-6dd9-4e4f-bc04-bcb90f18dae7', 'Belt of Giant Strength', dbo.getCategoryIdByName('Conjured'), 20, 0, 1),
	('fc22fba7-7f78-4ac5-a155-d89a40a1940a', 'Cheese', dbo.getCategoryIdByName('Food'), 5, 0, 1),
	('f1ab5859-cd8a-46b2-9520-dd631cbde700', 'Potion of Healing', dbo.getCategoryIdByName('Potion'), 10, 0, 1),
	('a5091448-6848-4247-8d85-2ee5a6a8defb', 'Bag of Holding', dbo.getCategoryIdByName('Misc'), 10, 0, 1),
	('58c1acc3-0ce7-4a06-86f3-5052d223e48d', 'TAFKAL80ETC Concert', dbo.getCategoryIdByName('Backstage Passes'), 15, 0, 1),
	('49ca5c68-4fbd-4ef6-9fe8-e086c6ef81e6', 'Elixir of the Mongoose', dbo.getCategoryIdByName('Potion'), 5, 0, 1),
	('88bdb452-e23d-4b70-b07e-c9f3f3f0d1a9', '+5 Dexterity Vest', dbo.getCategoryIdByName('Armor'), 10, 0, 1),
	('15298b70-b493-43a8-b01e-2cccf5514a89', 'Full Plate Mail', dbo.getCategoryIdByName('Armor'), 50, 0, 1),
	('4bb48e57-245a-419e-880c-1b701dbb35c2', 'Wooden Shield', dbo.getCategoryIdByName('Armor'), 10, 0, 1);

 
-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE inventory.Items AS t
USING @Items as s
	on 	(t.[Identifier] = s.[Identifier])
WHEN NOT MATCHED BY TARGET
    THEN INSERT ([Identifier], [Name], [Category], [ShelfLife], [IsDeleted], [CreatedBy])
        VALUES (s.[Identifier], s.[Name], s.[Category], s.[ShelfLife], s.[IsDeleted], s.[CreatedBy])
WHEN MATCHED
    THEN UPDATE SET 
		t.[Identifier] = s.[Identifier], 
		t.[Name] = s.[Name],
		t.Category = s.Category,
		t.ShelfLife = s.ShelfLife, 
		t.IsDeleted = s.IsDeleted
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;
