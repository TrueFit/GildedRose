SET IDENTITY_INSERT dbo.InventoryItemType ON
INSERT INTO dbo.InventoryItemType (InventoryItemTypeId, Name, QualityDeltaStrategyId, BaseDelta, MinQuality, MaxQuality, CreatedDate, Description)
VALUES
	(@Dev_InventoryItemType_Weapon       , 'Weapon'        , @QualityDeltaStrategy_Linear       , 1.0,  0.0,   50.0, @Now, 'A thing to hit things.')
,	(@Dev_InventoryItemType_Armor        , 'Armor'         , @QualityDeltaStrategy_Linear       , 1.0,  0.0,   50.0, @Now, 'A thing to stop getting hit with things.')
,	(@Dev_InventoryItemType_Food         , 'Food'          , @QualityDeltaStrategy_Linear       , 1.0,  0.0,   50.0, @Now, 'Something to eat.')
,	(@Dev_InventoryItemType_StinkyCheese , 'Stinky Cheese' , @QualityDeltaStrategy_InverseLinear, 1.0,  0.0,   50.0, @Now, 'The older and stinkier, the better.')
,	(@Dev_InventoryItemTYpe_Potion       , 'Potion'        , @QualityDeltaStrategy_Linear       , 1.0,  0.0,   50.0, @Now, 'Quaff me.')
,	(@Dev_InventoryItemType_Sulfuras     , 'Sulfuras'      , @QualityDeltaStrategy_Static       , 1.0, 80.0,   80.0, @Now, 'It''s big! It''s heavy!  It''s sulfuron!.')
,	(@Dev_InventoryItemType_BackStagePass, 'Backstage Pass', @QualityDeltaStrategy_Event        , 1.0,  0.0,   50.0, @Now, 'VIP ONLY.')
,	(@Dev_InventoryItemType_Conjured     , 'Conjured'      , @QualityDeltaStrategy_Linear       , 2.0,  0.0,   50.0, @Now, 'Made from the finest crap available.')
,	(@Dev_InventoryItemType_Miscellania  , 'Miscellania'   , @QualityDeltaStrategy_Linear       , 1.0,  0.0,   50.0, @Now, 'The finest crap available.')
SET IDENTITY_INSERT dbo.InventoryItemType OFF



