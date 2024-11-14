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
ORDER BY ips.avg_fragmentation_in_percent DESC