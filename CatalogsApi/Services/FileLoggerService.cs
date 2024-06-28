namespace CatalogsApi.Services
{
    public class FileLoggerService : IHostedService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _fileName = "logs.txt";
        private Timer _timer;

        public FileLoggerService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            WriteToFile("Starting");
            return Task.CompletedTask;
        }

        public void DoWork(object state)
        {
            _timer.Dispose();
            WriteToFile("Executing timer." + DateTime.Now.ToString("hh:mm:ss"));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteToFile("Stopping");
            return Task.CompletedTask;
        }

        public void WriteToFile(string message)
        {
            var path = $@"{_env.ContentRootPath}\wwwroot\{_fileName}";
            using StreamWriter sw = new StreamWriter(path, append: true);
            sw.WriteLine(message);
            sw.Close();
        }
    }
}
