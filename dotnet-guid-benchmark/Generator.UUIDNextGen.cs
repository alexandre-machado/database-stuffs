using UUIDNext;

namespace dotnet_guid_benchmark
{
    public class UUIDNextGen : Generator
    {
        public UUIDNextGen() : base()
        {
            Task.WaitAll(
                Task.Run(() => { MakeData($"{Database.SqlServer}", () => Uuid.NewDatabaseFriendly(Database.SqlServer)); }),
                Task.Run(() => { MakeData($"{Database.SQLite}", () => Uuid.NewDatabaseFriendly(Database.SQLite)); })
            );
        }
    }
}
