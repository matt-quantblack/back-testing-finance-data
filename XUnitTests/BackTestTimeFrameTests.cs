using System;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using QuantBlackTrading;
using System.Linq;

namespace XUnitTests
{
    public class BackTestTimeFrameTests
    {
        private readonly ITestOutputHelper output;
        TestSummary results;
        

        public BackTestTimeFrameTests(ITestOutputHelper output)
        {

            this.output = output;

           
            BackTest bt = new BackTest(OnCompleteBackTest);
            bt.Run("BackTestTimeFrameTest", @"G:\My Drive\C Sharp Apps\QuantBlackTrading\TestStrategy\bin\Debug\netcoreapp2.1\TestStrategy.dll");

            //wait until complete
            while (results == null)
                System.Threading.Thread.Sleep(200);
        }

        private void OnCompleteBackTest(TestSummary ts)
        {
            results = ts;
        }



        [Fact]
        public void TestBar_BidOpen_60_first()
        {            
            Trade result = results.Trades.Where(x => x.Comment == "BidOpen_60_first").FirstOrDefault();

            Assert.True(result != null, "Bid Open (60min first) test did not find trade info");
            if (result != null)
            {
                output.WriteLine("Bid Open (60min first) actual " + result.StopPoints + " does not match expected " + result.TakeProfitPoints);
                Assert.Equal(result.StopPoints, result.TakeProfitPoints, 5);
            }

        }

        [Fact]
        public void TestBar_BidClose_60_first()
        {
            Trade result = results.Trades.Where(x => x.Comment == "BidClose_60_first").FirstOrDefault();

            Assert.True(result != null, "Bid Close (60min first) test did not find trade info");
            if (result != null)
            {
                output.WriteLine("Bid Close (60min first) actual " + result.StopPoints + " does not match expected " + result.TakeProfitPoints);
                Assert.Equal(result.StopPoints, result.TakeProfitPoints, 5);
            }

        }

        [Fact]
        public void TestBar_BidHigh_60_first()
        {
            Trade result = results.Trades.Where(x => x.Comment == "BidHigh_60_first").FirstOrDefault();

            Assert.True(result != null, "Bid High (60min first) test did not find trade info");
            if (result != null)
            {
                output.WriteLine("Bid High (60min first) actual " + result.StopPoints + " does not match expected " + result.TakeProfitPoints);
                Assert.Equal(result.StopPoints, result.TakeProfitPoints, 5);
            }

        }

        [Fact]
        public void TestBar_BidLow_60_first()
        {
            Trade result = results.Trades.Where(x => x.Comment == "BidLow_60_first").FirstOrDefault();

            Assert.True(result != null, "Bid Low (60min first) test did not find trade info");
            if (result != null)
            {
                output.WriteLine("Bid Low (60min first) actual " + result.StopPoints + " does not match expected " + result.TakeProfitPoints);
                Assert.Equal(result.StopPoints, result.TakeProfitPoints, 5);
            }

        }

        [Fact]
        public void TestBar_AskOpen_60_first()
        {
            Trade result = results.Trades.Where(x => x.Comment == "AskOpen_60_first").FirstOrDefault();

            Assert.True(result != null, "Ask Open (60min first) test did not find trade info");
            if (result != null)
            {
                output.WriteLine("Ask Open (60min first) actual " + result.StopPoints + " does not match expected " + result.TakeProfitPoints);
                Assert.Equal(result.StopPoints, result.TakeProfitPoints, 5);
            }

        }

        [Fact]
        public void TestBar_AskClose_60_first()
        {
            Trade result = results.Trades.Where(x => x.Comment == "AskClose_60_first").FirstOrDefault();

            Assert.True(result != null, "Ask Close (60min first) test did not find trade info");
            if (result != null)
            {
                output.WriteLine("Ask Close (60min first) actual " + result.StopPoints + " does not match expected " + result.TakeProfitPoints);
                Assert.Equal(result.StopPoints, result.TakeProfitPoints, 5);
            }

        }

        [Fact]
        public void TestBar_AskHigh_60_first()
        {
            Trade result = results.Trades.Where(x => x.Comment == "AskHigh_60_first").FirstOrDefault();

            Assert.True(result != null, "Ask High (60min first) test did not find trade info");
            if (result != null)
            {
                output.WriteLine("Ask High (60min first) actual " + result.StopPoints + " does not match expected " + result.TakeProfitPoints);
                Assert.Equal(result.StopPoints, result.TakeProfitPoints, 5);
            }

        }

        [Fact]
        public void TestBar_AskLow_60_first()
        {
            Trade result = results.Trades.Where(x => x.Comment == "AskLow_60_first").FirstOrDefault();

            Assert.True(result != null, "Ask Low (60min first) test did not find trade info");
            if (result != null)
            {
                output.WriteLine("Ask Low (60min first) actual " + result.StopPoints + " does not match expected " + result.TakeProfitPoints);
                Assert.Equal(result.StopPoints, result.TakeProfitPoints, 5);
            }

        }

        [Fact]
        public void TestBar_volume_60_first()
        {
            Trade result = results.Trades.Where(x => x.Comment == "Volume_60_first").FirstOrDefault();

            Assert.True(result != null, "volume (60min first) test did not find trade info");
            if (result != null)
            {
                output.WriteLine("Volume (60min first) actual " + result.StopPoints + " does not match expected " + result.TakeProfitPoints);
                Assert.Equal(result.StopPoints, result.TakeProfitPoints, 5);
            }

        }
    }
}
