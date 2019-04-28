using System;
using Xunit;
using Xunit.Abstractions;
using QuantBlackTrading;
using System.Collections.Generic;


namespace XUnitTests
{
    public class DataBuilderTests
    {
        private readonly ITestOutputHelper output;

        public DataBuilderTests(ITestOutputHelper output)
        {
            this.output = output;

        }

        

        

        public bool BarEqual(Bar b1, Bar b2)
        {
            if (b1.OpenTime != b2.OpenTime)
                return false;
            if (!EqualityChecks.FloatNearlyEqual(b1.BidOpen, b2.BidOpen, 0.00000001f))
                return false;
            if (!EqualityChecks.FloatNearlyEqual(b1.BidClose, b2.BidClose, 0.00000001f))
                return false;
            if (!EqualityChecks.FloatNearlyEqual(b1.BidHigh, b2.BidHigh, 0.00000001f))
                return false;
            if (!EqualityChecks.FloatNearlyEqual(b1.BidLow, b2.BidLow, 0.00000001f))
                return false;
            if (!EqualityChecks.FloatNearlyEqual(b1.AskOpen, b2.AskOpen, 0.00000001f))
                return false;
            if (!EqualityChecks.FloatNearlyEqual(b1.AskClose, b2.AskClose, 0.00000001f))
                return false;
            if (!EqualityChecks.FloatNearlyEqual(b1.AskHigh, b2.AskHigh, 0.00000001f))
                return false;
            if (!EqualityChecks.FloatNearlyEqual(b1.AskLow, b2.AskLow, 0.00000001f))
                return false;
            if (b1.Volume != b2.Volume)
                return false;

            return true;
        }

        [Fact]
        public void TestCsvToBinaryConversion()
        {
            //try to convert a csv to binary
            DataBuilder.CsvToBinary(@"G:\My Drive\C Sharp Apps\QuantBlackTrading\XUnitTests\test_data\test_data_midnight_start.csv",
                @"G:\My Drive\C Sharp Apps\QuantBlackTrading\XUnitTests\test_data\EURUSD_m1.bin", true);

            byte[] bytes = DataBuilder.LoadBinary(@"G:\My Drive\C Sharp Apps\QuantBlackTrading\XUnitTests\test_data\EURUSD_m1.bin");

            
            Bar firstBar = DataBuilder.ReadBinaryBar(bytes, 0);
            Bar middleBar = DataBuilder.ReadBinaryBar(bytes, 50);            
            Bar lastBar = DataBuilder.ReadBinaryBar(bytes, 7242);

            Bar expectedFirst = new Bar("2017-09-15 00:00:00,1.19207,1.19226,1.19232,1.19205,1.19219,1.19239,1.19244,1.19217,292");
            Bar expectedMiddleBar = new Bar("2017-09-15 00:50:00,1.19079,1.19068,1.19086,1.19067,1.19092,1.1908,1.19098,1.1908,161");
            Bar expectedLastBar = new Bar("2017-09-22 00:19:00,1.19409,1.19414,1.19417,1.19408,1.19422,1.19428,1.1943,1.19422,60");

            Assert.True(BarEqual(firstBar, expectedFirst), "First bar is not as expected!");
            Assert.True(BarEqual(middleBar, expectedMiddleBar), "First bar is not as expected!");
            Assert.True(BarEqual(lastBar, expectedLastBar), "First bar is not as expected!");

        }

        [Fact]
        public void TestBarBuilder()
        {
            //The Bar building function is slightly different to the backtesting one, so test that the correct data is built here.

            Bar expectedBar1_1H = new Bar();
            expectedBar1_1H.OpenTime = DateTime.ParseExact("2017-09-15 00:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            expectedBar1_1H.BidOpen = 1.19207f;
            expectedBar1_1H.BidClose = 1.19075f;
            expectedBar1_1H.BidHigh = 1.19232f;
            expectedBar1_1H.BidLow = 1.19067f;
            expectedBar1_1H.AskOpen = 1.19219f;
            expectedBar1_1H.AskClose = 1.19088f;
            expectedBar1_1H.AskHigh = 1.19244f;
            expectedBar1_1H.AskLow = 1.1908f;
            expectedBar1_1H.Volume = 9217;

            Asset testAsset = new Asset();
            testAsset.Name = "EURUSD";
            testAsset.DataPath = @"G:\My Drive\C Sharp Apps\QuantBlackTrading\XUnitTests\test_data\EURUSD_m1.bin";
            
            Dictionary<int, Bar[]> timeframes = DataBuilder.BuildTimeFrames(testAsset, new int[] { 60, 240, 1440 });

            Assert.True(timeframes.Count == 3, "Expected 3 timeframes but got " + timeframes.Count);
            Assert.True(BarEqual(expectedBar1_1H, timeframes[60][0]), "First 60 min bar not as expected");
        }
        

    }
}
