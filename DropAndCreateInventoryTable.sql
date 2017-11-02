USE [GildedRose]
GO

/****** Object:  Table [dbo].[GildedRoseInventoryItems]    Script Date: 10/28/2017 5:32:10 PM ******/
DROP TABLE [dbo].[GildedRoseInventoryItems]
GO

/****** Object:  Table [dbo].[GildedRoseInventoryItems]    Script Date: 10/28/2017 5:32:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GildedRoseInventoryItems](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[category] [varchar](255) NULL,
	[sellin] [int] NOT NULL,
	[quality] [int] NOT NULL,
 CONSTRAINT [PK_GildedRoseInventory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO GildedRoseInventoryItems VALUES ('Sword', 'Weapon', 30,50)
INSERT INTO GildedRoseInventoryItems VALUES ('Axe', 'Weapon', 40,50)
INSERT INTO GildedRoseInventoryItems VALUES ('Halberd', 'Weapon', 60,40)
INSERT INTO GildedRoseInventoryItems VALUES ('Aged Brie', 'Food', 50,10)
INSERT INTO GildedRoseInventoryItems VALUES ('Aged Milk', 'Food', 20,20)
INSERT INTO GildedRoseInventoryItems VALUES ('Mutton', 'Food', 10,10)
INSERT INTO GildedRoseInventoryItems VALUES ('Hand of Ragnaros', 'Sulfuras', 80,80)
INSERT INTO GildedRoseInventoryItems VALUES ('I am Murloc', 'Backstage Passes', 20,10)
INSERT INTO GildedRoseInventoryItems VALUES ('Raging Ogre', 'Backstage Pasess', 10,10)
INSERT INTO GildedRoseInventoryItems VALUES ('Giant Slayer', 'Conjured', 15,50)
INSERT INTO GildedRoseInventoryItems VALUES ('Storm Hammer', 'Conjured', 20,50)
INSERT INTO GildedRoseInventoryItems VALUES ('Belt of Giant Strength', 'Conjured', 20,40)
INSERT INTO GildedRoseInventoryItems VALUES ('Cheese', 'Food', 5,5)
INSERT INTO GildedRoseInventoryItems VALUES ('Potion of Healing', 'Potion', 10,10)
INSERT INTO GildedRoseInventoryItems VALUES ('Bag of Holding', 'Misc', 10,50)
INSERT INTO GildedRoseInventoryItems VALUES ('TAFKAL80ETC Concert', 'Backstage Passes', 15,20)
INSERT INTO GildedRoseInventoryItems VALUES ('Elixir of the Mongoose', 'Potion', 5,7)
INSERT INTO GildedRoseInventoryItems VALUES ('+5 Dexterity Vest', 'Armor', 10,20)
INSERT INTO GildedRoseInventoryItems VALUES ('Full Plate Mail', 'Armor', 50,50)
INSERT INTO GildedRoseInventoryItems VALUES ('Wooden Shield', 'Armor',10,30)
