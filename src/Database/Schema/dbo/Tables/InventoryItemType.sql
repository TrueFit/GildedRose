CREATE TABLE dbo.InventoryItemType (
	InventoryItemTypeId			smallint		NOT NULL IDENTITY CONSTRAINT PK_InventoryItemType PRIMARY KEY
,	Name						varchar(50)		NOT NULL
,	Description					varchar(500)

)