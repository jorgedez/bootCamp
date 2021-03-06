using bootCamp.Shared.Constants;
using bootCamp.Shared.Helpers;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;

namespace bootCamp.AzureFunctions
{
    public static class FlightTimer 
    {
        private static TelemetryClient _telemetry = new TelemetryClient();
        private const string functionName = "FlightTimer";

        [FunctionName(functionName)]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            FlightGenerator _flightGen = new FlightGenerator();
            var flight = _flightGen.GetAFlight();
            var runway = string.Empty;

            try
            {
                switch (flight.IATA)
                {
                    case Iatas.Iberia:
                        runway = QueueNames.Terminal4;
                        break;
                    case Iatas.Ryanair:
                        runway = QueueNames.Terminal1;
                        break;
                    default:
                        runway = QueueNames.Terminal2;
                        break;
                }

                AzureServiceBusHelper.CreateQueue(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], runway);
                AzureServiceBusHelper.SendMessage(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], runway, JsonConvert.SerializeObject(flight));

               LoggerHelper.WriteTrace(functionName, $"Enviado a control a�reo {runway} el vuelo " +
                   $"{flight.IATA}{flight.Code} a las {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")} | " +
                   $"{flight.IdTrace}", log, TraceLevel.Info, _telemetry);
            }
            catch (Exception e)
            {
                LoggerHelper.TraceException(functionName, log, _telemetry, e);
                throw e;
            }

            LoggerHelper.WriteTrace(functionName, $"C# Timer trigger function executed at: {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);
        }
    }
}
