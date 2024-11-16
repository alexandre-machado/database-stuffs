namespace dotnet_guid_benchmark;

public class DotnetNativeGen : Generator
{
    public DotnetNativeGen() : base()
    {
        var start = DateTimeOffset.Now;
        Task.WaitAll(
            Task.Run(async () => { await MakeData($"DotnetV4", () => Guid.NewGuid()); }),
            Task.Run(async () => { await MakeData($"DotnetV7", () => Guid.CreateVersion7()); }),
            Task.Run(async () =>
            {
                await MakeData($"DotnetV7_OffSet", () =>
                {
                    var now = DateTimeOffset.Now;
                    var diff = (now - start);

                    var offSet = now.AddTicks(diff.Ticks * 10000);

                    //var offSet = new DateTimeOffset(newDate);

                    Console.WriteLine(
                        $"now: {now:hh:mm:ss}, " +
                        $"newDate: {offSet:hh:mm:ss}, " +
                        $"diff: {diff}, " +
                        $"ticks: {diff.Ticks}, ");

                    return Guid.CreateVersion7(offSet);
                });
            })
        );
    }
}