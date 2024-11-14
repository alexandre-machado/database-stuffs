using System.Diagnostics;

namespace dotnet_guid_benchmark
{
    public class Generator
    {
        private int counter;

        public Generator()
        {

        }

        private static void ListFragment(string tableName)
        {
            foreach (var item in Sql.Query<FragResult>(@$"
                SELECT (DDIPS.avg_fragmentation_in_percent / 100.00) as AvgFragmentationInPercent
                    , T.name as TableName
                    , I.name as IndexName
                FROM sys.dm_db_index_physical_stats(DB_ID(N'SampleDB'), OBJECT_ID(N'{tableName}'), NULL, NULL, NULL) AS DDIPS
	                INNER JOIN sys.tables T on T.object_id = DDIPS.object_id
	                INNER JOIN sys.schemas S on T.schema_id = S.schema_id
	                INNER JOIN sys.indexes I ON I.object_id = DDIPS.object_id
		                AND DDIPS.index_id = I.index_id
                ORDER BY DDIPS.avg_fragmentation_in_percent DESC;")
            )
                Console.WriteLine(
                    $"table: {item.TableName}" +
                    $", avg_fragmentation_in_percent: {item.AvgFragmentationInPercent:P}" +
                    $", index: {item.IndexName}"
                );
        }

        public void MakeData(string sufix, Func<Guid> sequentialMaker)
        {
            var tableName = $"{this.GetType().Name}_{sufix}";

            Console.WriteLine($"droping table: {tableName}");
            Sql.Query($@"if not exists (select * from sysobjects where name='{tableName}' and xtype='U')
                CREATE TABLE {tableName} (Id UNIQUEIDENTIFIER PRIMARY KEY, Ordered INT IDENTITY(1,1))");

            Console.WriteLine($"delete table: {tableName}");
            Sql.Query($"delete from {tableName}");
            EnableStatics(tableName);

            var size = 5_000;

            Console.WriteLine($"insert into table: {tableName}");
            Enumerable.Range(0, size).ToList().ForEach(i =>
            {
                var sequentialUuid = sequentialMaker();

                Sql.Query<int>($"Insert into {tableName} (Id) values (@guid)", new { guid = sequentialUuid });
                Interlocked.Increment(ref counter);
            });

            Console.WriteLine($"select top 10 Id from table: {tableName}");
            foreach (var item in Sql.Query<Guid>($"select top 10 Id from {tableName}"))
                Console.WriteLine(item);

            ListFragment(tableName);
        }

        private void EnableStatics(string tableName)
        {
            // run code async every x seconds
            var timer = new Timer(async _ =>
            {
                // show all parallelquery threads
                var count = Sql.Query<int>($"select count(*) from {tableName}").FirstOrDefault();

                Console.WriteLine(
                    $"[{tableName}] -> bd count: {count}" +
                    $", insert counter: {counter}" +
                    $", worker threads: {Process.GetCurrentProcess().Threads.Count}"
                );

                await Task.Delay(0);
            }, null, 0, 1000);
        }
    }
}
