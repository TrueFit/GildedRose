CREATE TABLE dbo.QualityDeltaStrategy (
	QualityDeltaStrategyId			tinyint			NOT NULL
,	Name							varchar(100)	NOT NULL
,	Description						varchar(1000)

,	CONSTRAINT PK_QualityDeltaStrategy PRIMARY KEY (QualityDeltaStrategyId)
)
