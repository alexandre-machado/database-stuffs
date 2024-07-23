-- ALTER DATABASE SampleDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE IF EXISTS SampleDB;
CREATE DATABASE SampleDB;
USE SampleDB;
CREATE TABLE Inventory (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    Name NVARCHAR(50),
    Quantity INT
);

-- delete all rows from the guidv4 table [SampleDB].[dbo].[Inventory]
DELETE FROM [SampleDB].[dbo].[Inventory];
-- add 1MM rows of data to the guidv4 table [SampleDB].[dbo].[Inventory]
INSERT INTO [SampleDB].[dbo].[Inventory] ([Id], [Name], [Quantity])
SELECT 
    NEWID(),
    'Product ' + CAST(ABS(CHECKSUM(NEWID())) % 1000000 AS VARCHAR(10)), 
    ABS(CHECKSUM(NEWID())) % 1000
FROM sys.all_columns AS ac1
CROSS JOIN sys.all_columns AS ac2

-- add with default value
INSERT INTO [SampleDB].[dbo].[Inventory] ([Name], [Quantity])
SELECT 
    'Product ' + CAST(ABS(CHECKSUM(NEWID())) % 1000000 AS VARCHAR(10)), 
    ABS(CHECKSUM(NEWID())) % 1000
FROM sys.all_columns AS ac1
CROSS JOIN sys.all_columns AS ac2


select count(*) from [SampleDB].[dbo].[Inventory];
select top 10 * from [SampleDB].[dbo].[Inventory];


SELECT OBJECT_NAME(ps.object_id) AS TableName
    ,i.name AS IndexName
    ,ips.index_type_desc
    ,ips.avg_fragmentation_in_percent
    ,ips.page_count
FROM sys.dm_db_index_physical_stats(DB_ID(N'SampleDB'), NULL, NULL, NULL, NULL) AS ips
INNER JOIN sys.indexes AS i ON ips.object_id = i.object_id
    AND ips.index_id = i.index_id
INNER JOIN sys.partitions AS ps ON ips.object_id = ps.object_id
    AND ips.index_id = ps.index_id
WHERE OBJECT_NAME(ps.object_id) = 'Inventory'
ORDER BY ips.avg_fragmentation_in_percent DESC;