using System;
using System.Collections.Generic;
using System.Text;

namespace QuantBlackTrading
{
    [Serializable()]
    public class ReduceByRankParams
    {
        public int PeriodDays { get; set; }
        public double MaxRankDifference { get; set; }

        public ReduceByRankParams() { }

        public ReduceByRankParams(int periodDays, double maxRankDifference)
        {
            PeriodDays = periodDays;
            MaxRankDifference = maxRankDifference;
        }
    }
}
