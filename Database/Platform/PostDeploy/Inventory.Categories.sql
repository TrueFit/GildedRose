DECLARE @Categories TABLE (
    [Id] INT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
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
