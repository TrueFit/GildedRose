CREATE TABLE dbo.InventoryItem (
	InventoryItemId				int				NOT NULL IDENTITY
,	InventoryItemTypeId			smallint		NOT NULL 

,	Name						varchar(100)	NOT NULL
,	Description					varchar(1000)

,	InitialQuality				float			NOT NULL
,	CurrentQuality				float			NOT NULL

,	InventoryDate				datetime		NOT NULL
,	SellByDate					datetime
,	SaleDate					datetime
,	DiscardDate					datetime

,	CreatedDate					datetime		NOT NULL 
,	ModifiedDate				datetime

,	CONSTRAINT PK_INVENTORY PRIMARY KEY (InventoryItemId)
,	CONSTRAINT FK_InventoryItem_InventoryItemType FOREIGN KEY (InventoryItemTypeId) REFERENCES dbo.InventoryItemType (InventoryItemTypeId)
)
