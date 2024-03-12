using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5_WPF_v3
{
    /// <summary>
    /// Class that contains information that is sent when an Event is triggered.
    /// </summary>
    public class FlightEventInfo : EventArgs
    {
        private string flightID;
        private Countries destination;

        /// <summary>
        /// Constructor. Recives a Flight number for identity
        /// </summary>
        /// <param name="flightNR"></param>
        public FlightEventInfo(string flightNR)
        {
            this.flightID = flightNR;
        }

        /// <summary>
        /// Constructor. Specify Identity and Destination
        /// </summary>
        /// <param name="flightCode"></param>
        /// <param name="dest"></param>
        public FlightEventInfo(string flightCode, Countries dest) : this(flightCode)
        {
            this.destination = dest;
        }
        /// <summary>
        /// The flights number/code/identity
        /// </summary>
        public string FlightID
        {
            get { return flightID; }
            set { flightID = value; }
        }
        /// <summary>
        /// The flights destination
        /// </summary>
        public Countries Destination
        {
            get { return destination; }
            set { destination = value; }
        }


    }
}
