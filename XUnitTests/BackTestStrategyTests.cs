using System;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using QuantBlackTrading;
using System.Linq;

namespace XUnitTests
{
    public class BackTestStrategyTests
    {
        private readonly ITestOutputHelper output;
        TestSummary results;

        public BackTestStrategyTests(ITestOutputHelper output)
        {

            this.output = output;

            //DataBuilder.LoadFeatures(testAsset, 60, new string[] { "SMA(4)", "SMA(20)" }, DataBuilder.DataFeedType.AskClose);
            ////////////////////////////
            ////////////
            ////
            ////
            /// NEED TO FIX SO LOAD FEATURES IS DONE IN BCKTEST from strategy features required param
            
            BackTest bt = new BackTest(OnCompleteBackTest);
            bt.Run("BackTestStrategyTest", @"G:\My Drive\C Sharp Apps\QuantBlackTrading\TestStrategy\bin\Debug\netcoreapp2.1\TestStrategy.dll");

            //wait until complete
            while (results == null)
                System.Threading.Thread.Sleep(200);
        }

        private void OnCompleteBackTest(TestSummary ts)
        {
            results = ts;
        }
        
        

        private bool TradeMatch(Trade t1, Trade t2)
        {
            if (t1.OpenTime != t2.OpenTime)
                return false;
            if (t1.CloseTime != t1.CloseTime)
                return false;
            if (!EqualityChecks.DoubleNearlyEqual(t1.OpenLevel, t2.OpenLevel, 0.00001))
                return false;
            if (!EqualityChecks.DoubleNearlyEqual(t1.CloseLevel, t2.CloseLevel, 0.00001))
                return false;
            if (!EqualityChecks.DoubleNearlyEqual(t1.PointChange, t2.PointChange, 0.00001))
                return false;
            if (!EqualityChecks.DoubleNearlyEqual(t1.Profit, t2.Profit, 0.00001))
                return false;
            if (!EqualityChecks.DoubleNearlyEqual(t1.SpreadPoints, t2.SpreadPoints, 0.00001))
                return false;
            if (t1.Direction != t2.Direction)
                return false;

            return true;
        }

        [Fact]
        public void TestBackTesterSMA()
        {

            

            //Should get trades as follows
            Trade trade1 = new Trade(DateTime.ParseExact("2017-09-18 04:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), 1.19448);
            trade1.Direction = Trade.TradeDirection.LONG;
            trade1.CloseLevel = 1.19935;
            trade1.CloseTime = DateTime.ParseExact("2017-09-19 06:10:30", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            trade1.SpreadPoints = 0.00013;
            trade1.Profit = 6.83756;

            Trade trade2 = new Trade(DateTime.ParseExact("2017-09-19 07:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), 1.19941);
            trade2.Direction = Trade.TradeDirection.SHORT;
            trade2.CloseLevel = 1.19454;
            trade2.CloseTime = DateTime.ParseExact("2017-09-20 18:05:30", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            trade2.SpreadPoints = 0.00013;
            trade2.Profit = 6.83739;

            Assert.True(TradeMatch(trade1, results.Trades[0]), "Trade 1 does not match expected results");
            Assert.True(TradeMatch(trade2, results.Trades[1]), "Trade 2 does not match expected results");
        }

       
    }
}
