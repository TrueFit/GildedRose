CREATE FUNCTION [dbo].[getCategoryIdByName](@CategoryName VARCHAR(100))
RETURNS INT
AS
BEGIN
	RETURN(SELECT 
				Id
			FROM 
				inventory.Categories
			WHERE
				[Name] = @CategoryName);
END