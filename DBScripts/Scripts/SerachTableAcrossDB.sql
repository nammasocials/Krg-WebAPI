CREATE TABLE #Results (
    DatabaseName SYSNAME,
    SchemaName SYSNAME,
    TableName SYSNAME
);

EXEC sp_msforeachdb '
DECLARE @TableName NVARCHAR(256) = ''InvCustomers_Audit''
IF ''?'' NOT IN (''master'',''tempdb'',''model'',''msdb'')
BEGIN
    INSERT INTO #Results
    SELECT ''?'' AS DatabaseName,
           s.name AS SchemaName,
           t.name AS TableName
    FROM [?].sys.tables t
    INNER JOIN [?].sys.schemas s ON t.schema_id = s.schema_id
    WHERE t.name = @TableName
END
';

SELECT * FROM #Results;
DROP TABLE #Results;