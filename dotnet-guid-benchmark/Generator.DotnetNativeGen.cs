namespace dotnet_guid_benchmark
{
    public class DotnetNativeGen : Generator
    {
        public DotnetNativeGen() : base()
        {
            Task.WaitAll(
                Task.Run(() => { MakeData($"DotnetV4", () => Guid.NewGuid()); }),
                Task.Run(() => { MakeData($"DotnetV7", () => Guid.CreateVersion7()); })
            );
        }
    }
}
