using System.Security;
using V2.Models;

namespace V2.Services
{
    public interface IDoSomethingService
    {
        public Something GenerateRandomEnumerable();

        public string GetDateAndValues();
    }
    public class DoSomethingService : IDoSomethingService, IHostedService, IDisposable
    {
        //Решил в этом примере варианте не делать базу данных и оставить все статической переменной
        public static List<Something> Somethings { get; set; }

        private int executionCount = 0;
        private readonly ILogger<DoSomethingService> _logger;
        private Timer? _timer = null;

        public DoSomethingService(ILogger<DoSomethingService> logger)
        {
            _logger = logger;
            Somethings = new List<Something> { new Something() };
        }

        private static readonly string[] Values = new[]
        {
           "Hello", "My", "Name", "is", "Gustafo"
        };

        //Генерация рандомного свойства Name/Value
        public Something GenerateRandomEnumerable() =>
            new Something { Name = Values[Random.Shared.Next(Values.Length)], Value = 10 };


        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        //Вывод в лог строку Дата/Значение
        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            var result = GetDateAndValues();

            _logger.LogInformation(result);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        //Формирует строку Дата/Сум(значения)
        public string GetDateAndValues()
        {
            var sum = Somethings.Select(x => x.Value).Sum();

            return String.Format($"Date: {DateTime.Now}, Sum values: {sum}");
        }
    }
}
