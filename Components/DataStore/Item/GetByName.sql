SELECT 
	 [Identifier]
	,[Name]
    ,[Category]
    ,[ShelfLife]
    ,[MaxQuality]
    ,[IsLegendary]
    ,[Created]
    ,[CreatedBy]
	,[Modified]
	,[ModifiedBy]
FROM 
	[Inventory].[Items]
WHERE 
	IsDeleted = 0 AND [Name] Like @itemName;