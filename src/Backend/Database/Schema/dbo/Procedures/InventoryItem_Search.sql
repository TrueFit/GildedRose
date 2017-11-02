CREATE PROCEDURE dbo.InventoryItem_Search (
	@IncludeAvailable bit
,	@IncludeExpired bit
,	@IncludeSold bit
,	@IncludeDiscarded bit
,	@OrderByClause varchar(MAX)
,	@Skip int
,	@Take int
,	@Now datetime
,	@TotalRows int OUTPUT
) AS

--Creates a dynamic query against InventoryItems

SET @Now = ISNULL(@Now, GETDATE())

DECLARE @WhereClause varchar(MAX) = null

IF @IncludeAvailable = 0 AND @IncludeExpired = 0 AND @IncludeSold = 0 AND @IncludeDiscarded = 0
	--Do what now?
	SET @WhereClause = '1 = 0'
ELSE IF @IncludeAvailable = 1
BEGIN
	--If IncludAvailable is true, start from the set of all records and exclude things we don't want
	IF @IncludeExpired = 0
		SET @WhereClause = ISNULL(@WhereClause + '
    AND ', '') + 'II.CurrentQuality > 0'

	
	IF @IncludeSold = 0
		SET @WhereClause = ISNULL(@WhereClause + '
    AND ', '') + 'II.SaleDate IS NULL'

	IF @IncludeDiscarded = 0
		SET @WhereClause = ISNULL(@WhereClause + '
    AND ', '') + 'II.DiscardDate IS NULL'		
END
ELSE
BEGIN
	--If IncludeAvailable is false, start from an empty set and add the the things we do want
	IF @IncludeExpired = 1
		SET @WhereClause = ISNULL(@WhereClause + '
    OR ', '(') + 'II.CurrentQuality <= 0'

	IF @IncludeSold = 1
		SET @WhereClause = ISNULL(@WhereClause + '
    OR ', '(') + 'II.SaleDate IS NOT NULL'

	IF @IncludeDiscarded = 1
		SET @WhereClause = ISNULL(@WhereClause + '
    OR ', '(') + 'II.DiscardDate IS NOT NULL'		

	SET @WhereClause = @WhereClause + ')'
	
	IF @IncludeExpired = 0
		SET @WhereClause = ISNULL(@WhereClause + '
    AND ', '') + 'II.CurrentQuality > 0'

	IF @IncludeSold = 0
		SET @WhereClause = ISNULL(@WhereClause + '
    AND ', '') + 'II.SaleDate IS NULL'

	IF @IncludeDiscarded = 0
		SET @WhereClause = ISNULL(@WhereClause + '
    AND ', '') + 'II.DiscardDate IS NULL'		

END

DECLARE @sqlCount nvarchar(MAX) = 'SELECT @TotalRows = COUNT(1)
FROM dbo.InventoryItem II' + ISNULL('
WHERE
	' + @WhereClause, '')

DECLARE @sqlRows nvarchar(MAX) = 'SELECT II.*
FROM 
	dbo.InventoryItem		II
	 INNER JOIN
	dbo.InventoryItemType	IIT	ON	IIT.InventoryItemTypeId = II.InventoryItemTypeId' + ISNULL('
WHERE
	' + @WhereClause, '')
	+ CASE WHEN NULLIF(RTRIM(@OrderByClause), '') IS NOT NULL THEN '
ORDER BY ' + @OrderByClause 
		+ CASE WHEN @Skip >= 0 AND @Take > 0 THEN '
OFFSET ' + CAST(@Skip as varchar(10)) + ' ROWS FETCH NEXT ' + CAST(@Take AS varchar(10)) + ' ROWS ONLY'
			ELSE ''
		END
		ELSE ''
	END

PRINT 'Executing SQL:
' + @sqlRows

EXEC sp_executesql @sqlCount, N'@Now datetime, @TotalRows int OUTPUT', @Now, @TotalRows OUTPUT
EXEC sp_executesql @sqlRows, N'@Now datetime', @Now
