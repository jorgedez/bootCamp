using System;

namespace bootCamp.AzureFunctions.Entity
{
    public class Flight
    {
        public string IdTrace { get; set; }
        public string IATA { get; set; }
        public string Code { get; set; }
        public string Orig { get; set; }
        public string Dest { get; set; }
        public string Time { get; set; }
    }
}
