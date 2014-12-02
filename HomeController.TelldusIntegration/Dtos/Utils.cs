using System.Collections.Generic;
using System.Text;

namespace HomeController.TelldusIntegration.Dtos
{
    public static class Utils
    {
        public static StringBuilder ToStringy(this IEnumerable<TemperatureSensor> sensors)
        {
            var sb = new StringBuilder();

            foreach (var sensor in sensors)
                sb.AppendLine(sensor.ToString());
            return sb;
        }
    }
}
