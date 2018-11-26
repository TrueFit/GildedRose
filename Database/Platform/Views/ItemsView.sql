CREATE VIEW [inventory].[ItemsView]
	AS 
SELECT 
	   i.[Identifier]
      ,i.[Name]
      ,i.[ShelfLife]
      ,oh.[InitialQuality]
      ,i.[IsDeleted]
	  ,c.[IsLegendary]
	  ,oh.StockDate
      ,i.[Created]
      ,i.[CreatedBy]
      ,i.[Modified]
      ,i.[ModifiedBy]
	  ,c.[Id] as CategoryId
      ,c.[Name] as CategoryName
  FROM 
	[inventory].[ItemsOnHand] oh
  INNER JOIN
	[inventory].[Items] i
  ON
	oh.ItemIdentifier = i.Identifier
  INNER JOIN 
	[inventory].[Categories] c 
  ON
	c.Id = i.Category
  WHERE
	oh.Sold = 0;