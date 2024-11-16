using UUIDNext;

namespace dotnet_guid_benchmark
{
    public class UUIDNextGen : Generator
    {
        public UUIDNextGen() : base()
        {
            Task.WaitAll(
                Task.Run(async () => { await MakeData($"{Database.SqlServer}", () => Uuid.NewDatabaseFriendly(Database.SqlServer)); }),
                Task.Run(async () => { await MakeData($"{Database.SQLite}", () => Uuid.NewDatabaseFriendly(Database.SQLite)); })
            );
        }
    }
}
