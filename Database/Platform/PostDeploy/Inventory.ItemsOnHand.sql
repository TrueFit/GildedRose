DECLARE @ItemsOnHand TABLE (
	[Identifier] UNIQUEIDENTIFIER NOT NULL, 
    [ItemIdentifier] UNIQUEIDENTIFIER NOT NULL,
	[InitialQuality] INT NOT NULL,
	[StockDate] DATETIME2 NOT NULL,
    [CreatedBy] [AuditUser] NOT NULL
);

 
INSERT INTO 
	@ItemsOnHand ([Identifier] ,[ItemIdentifier] ,[InitialQuality], [StockDate] ,[CreatedBy]) 
VALUES
	('C3289840-3B10-439F-9BC0-DCFB808BDFFE', dbo.getItemIdentifierByName('Bag of Holding'), 50, GETDATE(), 1),
	('72E15B2E-2062-435E-81FE-A1D815C636F5', dbo.getItemIdentifierByName('I am Murloc'), 10, GETDATE(), 1),
	('91871496-0CA2-4395-B35E-5E20FF201E23', dbo.getItemIdentifierByName('Full Plate Mail'), 50, GETDATE(), 1),
	('0715E0BC-6410-4CDF-9F37-2E8910B3D0FD', dbo.getItemIdentifierByName('Elixir of the Mongoose'), 7, GETDATE(), 1),
	('337D9931-BD01-461E-A251-E6765D7705AD', dbo.getItemIdentifierByName('Mutton'), 10, GETDATE(), 1),
	('05154EBA-6960-4B28-9F5A-F56FC4D68FC9', dbo.getItemIdentifierByName('Cheese'), 5, GETDATE(), 1),
	('10ED98DF-7940-47CD-9037-7B1AB6F3108F', dbo.getItemIdentifierByName('Belt of Giant Strength'), 40, GETDATE(), 1),
	('6E2FD542-B102-4C44-83E3-3F7C19BF9A44', dbo.getItemIdentifierByName('Giant Slayer'), 50, GETDATE(), 1),
	('04E968B1-7DC1-46EE-9717-6F6132E0690F', dbo.getItemIdentifierByName('Sword'), 50, GETDATE(), 1),
	('7FB55244-5DCE-4A4D-A063-EEEEC489537B', dbo.getItemIdentifierByName('Raging Ogre'), 10, GETDATE(), 1),
	('8CC6609B-3907-4999-8D65-AA2BE873A95B', dbo.getItemIdentifierByName('Halberd'), 40, GETDATE(), 1),
	('C36F2D57-F8CA-4D4B-9908-5485B794D4CC', dbo.getItemIdentifierByName('Hand of Ragnaros'), 80, GETDATE(), 1),
	('E3C0CABA-5302-4897-886A-BB5C58C90D69', dbo.getItemIdentifierByName('Aged Milk'), 20, GETDATE(), 1),
	('03C2B461-4254-43AF-BD21-AC677D3F4C87', dbo.getItemIdentifierByName('Potion of Healing'), 10, GETDATE(), 1),
	('C55E76E7-7FCB-4A95-BF74-9685B78430D5', dbo.getItemIdentifierByName('Storm Hammer'), 50, GETDATE(), 1),
	('CF3FB7DC-1220-4E57-9405-968C20D737CA', dbo.getItemIdentifierByName('Axe'), 50, GETDATE(), 1),
	('64BD78B2-C6F7-4EBC-87B4-2C7961F91D2D', dbo.getItemIdentifierByName('+5 Dexterity Vest'), 20, GETDATE(), 1),
	('9B523CEB-5329-48A7-9355-45B7AB523842', dbo.getItemIdentifierByName('Aged Brie'), 10, GETDATE(), 1),
	('AEB878B0-7EEE-485D-B088-7ADDD96613FC', dbo.getItemIdentifierByName('Wooden Shield'), 30, GETDATE(), 1),
	('46637321-E10D-4E77-AAC0-4399840032A8', dbo.getItemIdentifierByName('TAFKAL80ETC Concert'), 20, GETDATE(), 1)



-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE inventory.ItemsOnHand AS t
USING @ItemsOnHand as s
	on 	(t.[Identifier] = s.[Identifier])
WHEN NOT MATCHED BY TARGET
    THEN INSERT ([Identifier], [ItemIdentifier], [InitialQuality], [StockDate], [CreatedBy])
        VALUES (s.[Identifier], s.[ItemIdentifier], s.[InitialQuality], s.[StockDate], s.[CreatedBy])
WHEN MATCHED
    THEN UPDATE SET 
		t.[Identifier] = s.[Identifier], 
		t.[ItemIdentifier] = s.[ItemIdentifier],
		t.[InitialQuality] = s.[InitialQuality],
		t.[StockDate] = s.[StockDate]
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;
