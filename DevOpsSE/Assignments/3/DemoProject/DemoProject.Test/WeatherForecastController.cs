using DemoProject.Controllers;
using Microsoft.Extensions.Logging;

namespace DemoProject.Test
{
    [TestClass]
    public class WeatherForecastController
    {
        private ILogger<Controllers.WeatherForecastController> _logger;

        public WeatherForecastController()
        {
            InitialzeTests();
        }        

        [TestMethod]
        public void TestMethod1()
        {

            Controllers.WeatherForecastController wc = new Controllers.WeatherForecastController(_logger);
            var weatherForecasts = wc.Get();
            Assert.IsTrue(weatherForecasts.Count() > 0);
        }

        private void InitialzeTests()
        {
            var factory = LoggerFactory.Create(b => b.AddConsole());
            var logger = factory.CreateLogger<Controllers.WeatherForecastController>();
            _logger = logger;
        }
    }
}