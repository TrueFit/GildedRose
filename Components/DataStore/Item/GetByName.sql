SELECT 
	 [Identifier]
	,[Name]
    ,[ShelfLife]
    ,[InitialQuality]
    ,[IsLegendary]
    ,[Created]
    ,[CreatedBy]
	,[Modified]
	,[ModifiedBy]
	,[CategoryId]
	,[CategoryName]
FROM 
	[Inventory].[ItemsView]
WHERE 
	IsDeleted = 0 AND [Name] Like @itemName;