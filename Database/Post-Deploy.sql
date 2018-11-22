/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
:r .\PostDeploy\Membership.Organization.sql
:r .\PostDeploy\Membership.Users.sql
:r .\PostDeploy\Inventory.Categories.sql
:r .\PostDeploy\Inventory.Items.sql
:r .\PostDeploy\Inventory.ItemsOnHand.sql
