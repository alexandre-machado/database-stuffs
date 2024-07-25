using System.Diagnostics;
using UUIDNext;

namespace dotnet_guidv7
{
    public class UUIDNextSample
    {
        private int counter;

        public UUIDNextSample()
        {
            Task.WaitAll(
                Task.Run(() => { MakeData($"UUIDs_UUIDNext_{Database.SqlServer}"); }),
                Task.Run(() => { MakeData($"UUIDs_UUIDNext_{Database.SQLite}"); })
            );

            foreach (var item in Sql.Query<FragResult>(@"
                SELECT DDIPS.avg_fragmentation_in_percent, T.name as table_name, I.name as index_name
                FROM sys.dm_db_index_physical_stats(DB_ID(N'SampleDB'), NULL, NULL, NULL, NULL) AS DDIPS
	                INNER JOIN sys.tables T on T.object_id = DDIPS.object_id
	                INNER JOIN sys.schemas S on T.schema_id = S.schema_id
	                INNER JOIN sys.indexes I ON I.object_id = DDIPS.object_id
		                AND DDIPS.index_id = I.index_id
                ORDER BY DDIPS.avg_fragmentation_in_percent DESC;")
            )
                Console.WriteLine(
                    $"avg_fragmentation_in_percent: {item.avg_fragmentation_in_percent}" +
                    $", table: {item.table}" +
                    $", index: {item.index}"
                );

        }

        private void MakeData(string tableName)
        {
            Sql.Query($@"if not exists (select * from sysobjects where name='{tableName}' and xtype='U')
                CREATE TABLE {tableName} (Id UNIQUEIDENTIFIER PRIMARY KEY, Ordered INT IDENTITY(1,1))");
            Sql.Query($"delete from {tableName}");
            EnableStatics(tableName);

            var size = 5_000;
            //Enumerable.Range(0, size).AsParallel().ForAll(i =>
            Enumerable.Range(0, size).ToList().ForEach(i =>
            {
                var sequentialUuid = Uuid.NewDatabaseFriendly(Database.SQLite);

                Sql.Query<int>($"Insert into {tableName} (Id) values (@guid)", new { guid = sequentialUuid });
                Interlocked.Increment(ref counter);
            });

            foreach (var item in Sql.Query<Guid>($"select top 10 Id from {tableName}"))
                Console.WriteLine(item);
        }

        private void EnableStatics(string tableName)
        {
            // run code async every 5 seconds
            var timer = new Timer(async _ =>
            {
                // show all parallelquery threads
                var count = Sql.Query<int>($"select count(*) from {tableName}").FirstOrDefault();

                Console.WriteLine(
                    $"[{tableName}] -> bd count: {count}" +
                    $", insert counter: {counter}" +
                    $", worker threads: {Process.GetCurrentProcess().Threads.Count}"
                );

                await Task.Delay(5000);
            }, null, 0, 5000);
        }
    }
}
