using bootCamp.AzureFunctions.Constants;
using bootCamp.AzureFunctions.Entity;
using bootCamp.AzureFunctions.Helpers;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace bootCamp.AzureFunctions.ServiceBus.Functions
{
    public static class FlightSorterT2
    {
        private static TelemetryClient _telemetry = new TelemetryClient();
        private const string functionName = "FlightSorterT2";

        [FunctionName(functionName)]
        public static async Task Run([ServiceBusTrigger(QueueNames.Terminal2, AccessRights.Manage, Connection = "AzureWebJobsServiceBus")]string sbMessage, TraceWriter log)
        {
            try
            {
                var flight = JsonConvert.DeserializeObject<Flight>(sbMessage);
                LoggerHelper.WriteTrace(functionName, $"{QueueNames.Terminal2}-Informa: Se procesa el vuelo {flight.IATA}{flight.Code} procedente de " +
                     $"{flight.Orig} y con salida programada a las {flight.Time} ", log, TraceLevel.Info, _telemetry);

            }
            catch (Exception e)
            {
                LoggerHelper.TraceException(functionName, log, _telemetry, e);
                throw e;
            }
        }
    }
}
