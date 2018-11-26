SELECT 
	 [Identifier]
	,[Name]
    ,[ShelfLife]
    ,[InitialQuality]
    ,[IsLegendary]
	,[StockDate]
    ,[Created]
    ,[CreatedBy]
	,[Modified]
	,[ModifiedBy]
	,[CategoryId]
	,[CategoryName]
FROM 
	[Inventory].[ItemsView]
WHERE 
	IsDeleted = 0;