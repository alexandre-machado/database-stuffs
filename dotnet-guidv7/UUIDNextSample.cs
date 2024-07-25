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

            foreach (var item in Sql.Query<decimal>(@"
                SELECT ips.avg_fragmentation_in_percent
                FROM sys.dm_db_index_physical_stats(DB_ID(N'SampleDB'), NULL, NULL, NULL, NULL) AS ips
                ORDER BY ips.avg_fragmentation_in_percent DESC;"))
                Console.WriteLine($"avg_fragmentation_in_percent: {item}");

        }

        private void MakeData(string tableName)
        {
            Sql.Query($@"if not exists (select * from sysobjects where name='{tableName}' and xtype='U')
                CREATE TABLE {tableName} (Id UNIQUEIDENTIFIER PRIMARY KEY, Ordered INT IDENTITY(1,1))");
            //Sql.Query($"delete from {tableName}");
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
                ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
                var count = Sql.Query<int>($"select count(*) from {tableName}").FirstOrDefault();

                Console.WriteLine(
                    $"{tableName} -> bd count: {count}" +
                    $", thread counter: {counter}" +
                    $", worker threads: {workerThreads}" +
                    $", completion port threads: {completionPortThreads}");

                await Task.Delay(5000);
            }, null, 0, 5000);
        }
    }
}
