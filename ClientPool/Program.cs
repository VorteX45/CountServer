namespace ClientPool
{
    internal class Program
    {
        const int clientCount = 10;
        const int loopsPerTask = 5;

        async static Task Main(string[] args)
        {
            // создаем лист задач
            var pool = new List<Task>();
            // на чтение
            for (int i = 0; i < clientCount - 2; i++)
                pool.Add(ImitateReader(i));
            // парочку на запись
            pool.Add(ImitateWriter(clientCount - 2));
            pool.Add(ImitateWriter(clientCount - 1));
            // запускаем
            await Task.WhenAll(pool);

            Console.WriteLine("----- завершено -----");
            Console.ReadLine();
        }


        async static Task ImitateReader(int taskIndex)
        {
            int count;
            Random random = new Random();

            for (int i = 0; i < loopsPerTask; i++)
            {
                await Task.Delay(random.Next(1000, 3000));
                Console.WriteLine($"#{taskIndex}: читаю...");
                count = await CountServer.Server.GetCount();
                Console.WriteLine($"#{taskIndex}: прочитано {count}");
            }
        }


        async static Task ImitateWriter(int taskIndex)
        {
            int count;
            int increment;
            Random random = new Random();

            for (int i = 0; i < loopsPerTask; i++)
            {
                await Task.Delay(random.Next(1000, 3000));
                
                increment = random.Next(10);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"#{taskIndex}: увеличиваю на {increment}...");
                Console.ResetColor();
                count = await CountServer.Server.AddToCount(increment);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"#{taskIndex}: увеличено до {count}");
                Console.ResetColor();
            }
        }
    }
}