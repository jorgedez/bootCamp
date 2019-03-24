using bootCamp.Shared.Constants;
using bootCamp.Shared.Entities;
using bootCamp.Shared.Helpers;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace bootCamp.AzureFunctions.ServiceBus.Functions
{
    public static class FlightSorterT1
    {
        private static TelemetryClient _telemetry = new TelemetryClient();
        private const string functionName = "FlightSorterT1";

        [FunctionName(functionName)]
        public static void Run([ServiceBusTrigger(QueueNames.Terminal1, AccessRights.Manage, Connection = "AzureWebJobsServiceBus")]string sbMessage, TraceWriter log)
        {
            try
            {
                var flight = JsonConvert.DeserializeObject<Flight>(sbMessage);
                LoggerHelper.WriteTrace(functionName, $"{QueueNames.Terminal1}-Informa: Se procesa el vuelo {flight.IATA}{flight.Code} procedente de " +
                      $"{flight.Orig} a las {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")} | {flight.IdTrace}", log, TraceLevel.Info, _telemetry);

            }
            catch (Exception e)
            {
                LoggerHelper.TraceException(functionName, log, _telemetry, e);
                throw e;
            }
        }
    }
}
