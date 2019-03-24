# Global Integration Bootcamp - Tenerife

# Procesamiento de simulador de llegadas de vuelos y derivación según IATA en distintas terminales.

Se generarán aleatoriamente vuelos con un simulador de codificación tal que IATA+NumVuelo y dependiendo del tipo de IATA:
- RYR (Ryanair), IB (Iberia), UX (AirEuropa) irán destinados a distintas terminales "colas" (T1,T4,T2).
- Se procesaran la entrada correctamente mediante logs de ApplicationInsights.

# Requisitos de Azure:
  - Service Bus + Queue  : https://docs.microsoft.com/es-es/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues
  - Functions (Plan de consumo) + ApplicationInsights
  
#######
Para nuestro ejemplo usaremos el service bus "bootsb" donde crearemos 3 queue (t1,t2,t4)
Crearemos una Function App "bootcamptfe" asociada a plan de consumo (para abaratar costes) y publicaremos las functions:
  - FlightSorterT1, FlightSorterT2, FlightSorterT4, FlightTimer (tened en cuenta que ahora mismo se dispara cada 1m)
  
La entrada a la aplicación se realizará mediante el TimerTrigger que si dispara cada 1m creando el vuelo y distribuyendolo.
  
 ######
 
 Una vez ejecutada la aplicación deberiamos comenzar a recibir trazas en AppInsight con la ordenación de los vuelos.
