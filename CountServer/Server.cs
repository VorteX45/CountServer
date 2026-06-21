namespace CountServer
{
    static public class Server
    {
        private static int count = 0;
        static SemaphoreSlim writeSemaphore = new SemaphoreSlim(1, 1);


        public async static Task<int> GetCount()
        {
            await Task.Delay(1000);
            return count;
        }


        public async static Task<int> AddToCount(int value)
        {
            await writeSemaphore.WaitAsync();

            await Task.Delay(2000);
            // чтобы сервер не упал из-за переполнения
            if (count + value > int.MaxValue)
                count = int.MaxValue;
            else
                count += value;

            writeSemaphore.Release();
            return count;
        }
    }
}