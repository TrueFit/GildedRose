CREATE TABLE dbo.Config (
	[Key]				varchar(200)		NOT NULL
,	Value				varchar(MAX)

,	CONSTRAINT PK_Config PRIMARY KEY ([Key])
)