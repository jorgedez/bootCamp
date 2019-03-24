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

namespace bootCamp.AzureFunctions.ServiceBus.Functions.DeadLetter
{
    public static class DeadLetterSorterT2
    {
        private static readonly TelemetryClient _telemetry = new TelemetryClient();
        private const string functionName = "DeadLetterSorterT2";

        [FunctionName(functionName)]
        public static void Run([ServiceBusTrigger(QueueNames.DeadLetterSorterT2, AccessRights.Manage, Connection = "AzureWebJobsServiceBus")]string sbMessage, TraceWriter log)
        {
            LoggerHelper.WriteTrace(functionName, $"Atención se ha enviado a {QueueNames.DeadLetterSorterT2} | {sbMessage} | a las {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);

            try
            {
                var flightMessage = JsonConvert.DeserializeObject<Flight>(sbMessage);
                var message = $@"
            Hola. {Environment.NewLine}
            No se ha podido procesar el vuelo {flightMessage.IATA}{flightMessage.Code}. {Environment.NewLine}
            Un saludo.";
                var lista = "jorge.fernandez@encamina.com;jorgeffernandez@gmail.com";
                var listTo = lista.Split(';');
                EmailHelper.SendMail("Error en FlightBus", message, listTo);
                LoggerHelper.WriteTrace(functionName, $"Se ha enviado mail a {lista} avisando de la recepción errónea del vuelo {flightMessage.IATA}{flightMessage.Code} | {flightMessage.IdTrace} a las {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteTrace(functionName, $"Se ha producido un error al enviar el mensaje: {ex.Message}, {ex.InnerException}", log, TraceLevel.Error, _telemetry);
            }
            LoggerHelper.WriteTrace(functionName, $"C# ServiceBus queue trigger function finished at {DateTime.UtcNow.ToString("dd/MM/yyyy HH-mm-ss")}", log, TraceLevel.Info, _telemetry);
        }
    }
}
