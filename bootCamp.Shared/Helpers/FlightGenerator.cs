using bootCamp.Shared.Constants;
using bootCamp.Shared.Entities;
using System;

namespace bootCamp.Shared.Helpers
{
    public class FlightGenerator
    {
        public Flight GetAFlight()
        {
            Random rnd = new Random();
            string[] iatas = { Iatas.Iberia, Iatas.AirEuropa, Iatas.Ryanair };
            string[] destinations = { "MAD" };
            string[] origins = { "TFN", "TFS", "MAD", "BOG", "MIA", "NYK", "MAL", "BAC" };

            int mIatas = rnd.Next(iatas.Length);
            int mDestinations = rnd.Next(destinations.Length);
            int mOrigins = rnd.Next(origins.Length);
            int mFlightNumber = rnd.Next(1000, 9999);

            return new Flight
            {
                IdTrace = Guid.NewGuid().ToString(),
                IATA = iatas[mIatas],
                Code = mFlightNumber.ToString(),
                Orig = origins[mOrigins],
                Dest = destinations[mDestinations],
                Time = DateTime.UtcNow.ToString("yyyyMMddHHmmss")
            };
        }
    }
}
