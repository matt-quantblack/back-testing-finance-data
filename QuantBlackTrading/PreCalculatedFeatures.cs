using System;
using System.Collections.Generic;
using System.Text;

namespace QuantBlackTrading
{
    public class PreCalculatedFeatures
    {
        public Dictionary<DateTime, Dictionary<string, double?>>  Data { get; set; }

        public PreCalculatedFeatures()
        {
            Data = new Dictionary<DateTime, Dictionary<string, double?>>();
        }
    }
}
