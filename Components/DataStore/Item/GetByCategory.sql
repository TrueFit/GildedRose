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
	i.IsDeleted = 0 AND CategoryId = @categoryId;