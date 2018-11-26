DECLARE @Categories TABLE (
    [Id] INT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
	[IsLegendary] BIT NOT NULL,
    [CreatedBy] [AuditUser] NOT NULL
);

 
INSERT INTO 
	@Categories ([Id], [Name], [IsLegendary], [CreatedBy]) 
VALUES
(1, 'Weapon', 0, 0),
(2, 'Food', 0, 0),
(3, 'Sulfuras', 1, 1),
(4, 'Backstage Passes', 0, 0),
(5, 'Conjured', 0, 0),
(6, 'Potion', 0, 0),
(7, 'Misc', 0, 0),
(8, 'Armor', 0, 0);

 
-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE inventory.Categories AS t
USING @Categories as s
	on 	(t.[Id] = s.[Id])
WHEN NOT MATCHED BY TARGET
    THEN INSERT ([Name], [IsLegendary], [CreatedBy])
        VALUES (s.[Name], [IsLegendary], s.CreatedBy)
WHEN MATCHED
    THEN UPDATE 
	SET 
		t.[Name] = s.[Name],
		t.[IsLegendary] = s.[IsLegendary]
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;
