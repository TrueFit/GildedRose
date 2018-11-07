SELECT 
	 i.[Identifier]
    ,i.[Name]
    ,i.[Category]
    ,i.[ShelfLife]
    ,i.[MaxQuality]
    ,i.[IsLegendary]
    ,i.[Created]
    ,i.[CreatedBy]
    ,i.[Modified]
	,i.[ModifiedBy]
FROM 
	[GildedRose].[inventory].[Items] i
INNER JOIN 
	inventory.Categories c on i.Category = c.Id
WHERE 
	i.IsDeleted = 0 AND c.Id = @categoryId;