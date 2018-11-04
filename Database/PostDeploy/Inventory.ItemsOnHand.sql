DECLARE @ItemsOnHand TABLE (
	[Identifier] UNIQUEIDENTIFIER NOT NULL, 
    [ItemIdentifier] UNIQUEIDENTIFIER NOT NULL, 
	[StockCount] INT NOT NULL,
	[StockDate] DATETIME2 NOT NULL,
    [CreatedBy] [AuditUser] NOT NULL
);

 
INSERT INTO 
	@ItemsOnHand ([Identifier] ,[ItemIdentifier] ,[StockCount] ,[StockDate] ,[CreatedBy]) 
VALUES
    ('8C447991-8D82-4010-8D68-CDFF4D95C48A', dbo.getItemIdentifierByName('+5 Dexterity Vest'), 1, GETDATE(), 1),
    ('90A3BEEB-15A4-4C5F-A1F6-E6205917D6FB', dbo.getItemIdentifierByName('Aged Brie'), 1, GETDATE(), 1),
    ('CC8B35B5-A363-4A31-9949-B95F201E17D4', dbo.getItemIdentifierByName('Aged Milk'), 1, GETDATE(), 1),
    ('D29AF790-7757-4003-A4F0-C5E7DF4D2C2E', dbo.getItemIdentifierByName('Axe'), 1, GETDATE(), 1),
    ('8C751912-DB71-4AC6-993C-09DE4099DBB1', dbo.getItemIdentifierByName('Bag of Holding'), 1, GETDATE(), 1),
    ('C82CC310-D2C8-4A2E-B4AC-685F8F27523D', dbo.getItemIdentifierByName('Belt of Giant Strength'), 1, GETDATE(), 1),
    ('402B3660-D010-47C9-9322-6408F0B75215', dbo.getItemIdentifierByName('Cheese'), 1, GETDATE(), 1),
    ('B07FF74D-0B57-44E5-B214-4A0341C888C6', dbo.getItemIdentifierByName('Elixir of the Mongoose'), 1, GETDATE(), 1),
    ('49379114-888E-419D-B663-2FD2AD78683A', dbo.getItemIdentifierByName('Full Plate Mail'), 1, GETDATE(), 1),
    ('AB027762-C333-4E25-AF55-7007C21C2076', dbo.getItemIdentifierByName('Giant Slayer'), 1, GETDATE(), 1),
    ('CD8739D0-D297-49BD-99E7-97CFE21763AB', dbo.getItemIdentifierByName('Halberd'), 1, GETDATE(), 1),
    ('43BFB62D-5873-4F39-B224-B4E5C3EEB285', dbo.getItemIdentifierByName('Hand of Ragnaros'), 1, GETDATE(), 1),
    ('60690BE4-32B6-45DF-B4A1-1EFCFB619C3B', dbo.getItemIdentifierByName('I am Murloc'), 1, GETDATE(), 1),
    ('4362620C-892C-42D7-8D50-57018E3366B0', dbo.getItemIdentifierByName('Mutton'), 1, GETDATE(), 1),
    ('96A3361B-3827-42B1-B151-C1174A81210F', dbo.getItemIdentifierByName('Potion of Healing'), 1, GETDATE(), 1),
    ('858A580B-7672-4B80-9227-8D161BB9D5CB', dbo.getItemIdentifierByName('Raging Ogre'), 1, GETDATE(), 1),
    ('91BD280F-B580-44C2-BCF9-C2FF7C5F01C1', dbo.getItemIdentifierByName('Storm Hammer'), 1, GETDATE(), 1),
    ('14AE9FBF-8178-4C54-AE78-8061F546C354', dbo.getItemIdentifierByName('Sword'), 1, GETDATE(), 1),
    ('9103BB66-7552-4013-BE3D-F751033FD617', dbo.getItemIdentifierByName('TAFKAL80ETC Concert'), 1, GETDATE(), 1),
    ('0AFFA6FC-FE61-40BE-A7B2-E9AC5D0F6781', dbo.getItemIdentifierByName('Wooden Shield'), 1, GETDATE(), 1);


-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE inventory.ItemsOnHand AS t
USING @ItemsOnHand as s
	on 	(t.[Identifier] = s.[Identifier])
WHEN NOT MATCHED BY TARGET
    THEN INSERT ([Identifier], [ItemIdentifier], [StockCount], [StockDate], [CreatedBy])
        VALUES (s.[Identifier], s.[ItemIdentifier], s.[StockCount], s.[StockDate], s.[CreatedBy])
WHEN MATCHED
    THEN UPDATE SET 
		t.[Identifier] = s.[Identifier], 
		t.[ItemIdentifier] = s.[ItemIdentifier],
		t.[StockCount] = s.[StockCount],
		t.[StockDate] = s.[StockDate]
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;
