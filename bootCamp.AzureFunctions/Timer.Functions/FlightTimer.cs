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
                if (flight.IATA.Equals(Iatas.Iberia)) {
                    runway = QueueNames.Terminal4;
                }
                else if (flight.IATA.Contains(Iatas.Ryanair)) {
                    runway = QueueNames.Terminal1;
                }
                else if (flight.IATA.Equals(Iatas.AirEuropa)) {
                    runway = QueueNames.Terminal2;
                }

                AzureServiceBusHelper.CreateQueue(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], runway);
                AzureServiceBusHelper.SendMessage(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], runway, JsonConvert.SerializeObject(flight));

               LoggerHelper.WriteTrace(functionName, $"Enviado a control aéreo {runway} el vuelo " +
                   $"{flight.IATA}{flight.Code} : {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")} | " +
                   $"{flight.IdTrace}", log, TraceLevel.Info, _telemetry);
            }
            catch (Exception e)
            {
                LoggerHelper.TraceException(functionName, log, _telemetry, e);
                throw e;
            }

            LoggerHelper.WriteTrace(functionName, $"C# Timer trigger function executed at: {DateTime.Now}", log, TraceLevel.Info, _telemetry);
        }
    }
}
