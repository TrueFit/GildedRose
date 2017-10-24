PRINT 'Executing Data/PostDeploymentData.sql...'

--Declare constants used in BaseData scripts
:r BaseData\_Constants.sql

--Load Base Data for all environemnts


--Declare constants used in the environment specific scripts
:r .\TestData\_Constants.sql

--Load Test Data for the appropriate environment
IF '$(Environment)' = 'Dev'
BEGIN
:r .\TestData\Dev.sql
END

ELSE IF '$(Environment)' = 'QA'
BEGIN
:r .\TestData\QA.sql
END

ELSE IF '$(Environment)' = 'Prod'
BEGIN
:r .\TestData\Prod.sql
END


