CREATE TABLE dbo.InventoryItem (
	InventoryItemId				int			NOT NULL IDENTITY CONSTRAINT PK_INVENTORY PRIMARY KEY

,	InventoryDate				datetime	NOT NULL

,	CreatedDate					datetime

--,	CONSTRAINT PK_INVENTORY PRIMARY KEY (InventoryId)
)
