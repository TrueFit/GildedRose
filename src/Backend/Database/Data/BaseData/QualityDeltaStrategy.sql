INSERT INTO dbo.QualityDeltaStrategy (QualityDeltaStrategyId, Name, Description)
VALUES
	(@QualityDeltaStrategy_Linear       , 'Linear'        , 'Quality decreases by a fixed amount per day until Sell By Date when it begins decreaseing at double the rate.')
,	(@QualityDeltaStrategy_InverseLinear, 'Inverse Linear', 'Quality increases by a fixed amount per day.')
,	(@QualityDeltaStrategy_Static       , 'Static'        , 'Quality is a fixed constant.')
,	(@QualityDeltaStrategy_Event        , 'Event'         , 'Quality decrease by a fixed amount per day.  Ten days before the event, the rate doubles.  Five days before an event, the rate triples.  The day after the event, quality goes to zero.')
