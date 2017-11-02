PRINT 'Executing Data/BaseData/_Constants.sql...'

DECLARE @Today date = CAST(@Now AS date)
DECLARE @Tomorrow date = DATEADD(DAY, 1, @Today)
DECLARE @EndOfToday datetime = DATEADD(SECOND, -1, CAST(@Tomorrow AS datetime))
DECLARE @ThisYear date = DATEADD(DAY, -DATEPART(DAYOFYEAR, @Today) + 1, @Today)

DECLARE @Config_SimulationDateOffset varchar(200) = 'SimulationDateOffset'

DECLARE @QualityDeltaStrategy_Linear        tinyint = 1
DECLARE @QualityDeltaStrategy_InverseLinear tinyint = 2
DECLARE @QualityDeltaStrategy_Static        tinyint = 3
DECLARE @QualityDeltaStrategy_Event         tinyint = 4

