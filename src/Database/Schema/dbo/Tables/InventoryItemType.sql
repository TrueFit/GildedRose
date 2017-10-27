CREATE TABLE dbo.InventoryItemType (
	InventoryItemTypeId			smallint		NOT NULL IDENTITY
,	Name						varchar(50)		NOT NULL
,	Description					varchar(500)
,	QualityDeltaStrategyId		tinyint			NOT NULL
,	BaseDelta                   real            NOT NULL
,	MinQuality					real			NOT NULL
,	MaxQuality					real			NOT NULL

,	CreatedDate					datetime		NOT NULL
,	ModifiedDate				datetime

,	CONSTRAINT PK_InventoryItemType PRIMARY KEY (InventoryItemTypeId)
,	CONSTRAINT FK_InventoryItemType_QualityDeltaStrategy FOREIGN KEY (QualityDeltaStrategyId) REFERENCES dbo.QualityDeltaStrategy (QualityDeltaStrategyId)
)