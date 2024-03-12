using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5_WPF_v3
{
    /// <summary>
    /// Contains information about a planes: Number/code, Status and Timestamp
    /// </summary>
    internal class FlightInfo
    {
        /// <summary>
        /// The planes Flight number/code
        /// </summary>
        public string FlightCode { get; set; }
        /// <summary>
        /// Planes status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Display content
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = FlightCode + Status + "\n";
            return str;
        }

    }
}
