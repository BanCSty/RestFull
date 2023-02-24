using System.Security;
using V2.Models;

namespace V2.Services
{
    public interface IDoSomethingService
    {
        public SomethingModel GenerateRandomEnumerable();

        public LoggerModel GetDateAndValues();
    }
    public class DoSomethingService : IDoSomethingService, IHostedService, IDisposable
    {
        //Решил в этом примере варианте не делать базу данных и оставить все статической переменной
        public static List<SomethingModel> Somethings { get; set; }

        private int executionCount = 0;
        private readonly ILogger<DoSomethingService> _logger;
        private Timer? _timer = null;

        public DoSomethingService(ILogger<DoSomethingService> logger)
        {
            _logger = logger;
            Somethings = new List<SomethingModel> { new SomethingModel() };
        }

        private static readonly string[] Values = new[]
        {
           "Hello", "My", "Name", "is", "Gustafo"
        };

        //Генерация рандомного свойства Name/Value
        public SomethingModel GenerateRandomEnumerable() =>
            new SomethingModel { Name = Values[Random.Shared.Next(Values.Length)], Value = 10 };


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

            var result = GetDateAndValues().ToString();

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
        public LoggerModel GetDateAndValues()
        {
            var sum = Somethings.Select(x => x.Value).Sum();

            return new LoggerModel {Date = DateTime.Now.ToString(), Value = sum };
        }
    }
}
