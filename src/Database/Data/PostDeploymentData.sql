PRINT 'Executing Data/PostDeploymentData.sql...'

--Declare constants used in BaseData scripts
DECLARE @Now datetime = GETDATE()
:r BaseData\_Constants.sql

--Load Base Data for all environemnts
:r BaseData\QualityDeltaStrategy.sql

--Declare constants used in the environment specific scripts
:r .\SeedData\_Constants.sql

--Load Test Data for the appropriate environment
IF '$(Environment)' = 'Dev'
BEGIN
:r .\SeedData\Dev.sql
END

ELSE IF '$(Environment)' = 'QA'
BEGIN
:r .\SeedData\QA.sql
END

ELSE IF '$(Environment)' = 'Prod'
BEGIN
:r .\SeedData\Prod.sql
END


