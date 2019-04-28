using System;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using QuantBlackTrading;

namespace XUnitTests
{
    public class AssetTests
    {
        private readonly ITestOutputHelper output;

        private string localCurrency;
        private Dictionary<string, double> assetRates;

        public AssetTests(ITestOutputHelper output)
        {
            this.output = output;

            localCurrency = "AUD";

            assetRates = new Dictionary<string, double>();
            assetRates.Add("AUDJPY", 79.50); //12.57
            assetRates.Add("AUDUSD", 0.7102); // 14.08
            assetRates.Add("AUDCAD", 0.951); //10.52
            assetRates.Add("AUDNZD", 1.054); //9.49
            assetRates.Add("AUDCHF", 0.712); //14.04
            assetRates.Add("GBPAUD", 1.837); //10
            assetRates.Add("EURAUD", 1.577); //10
            assetRates.Add("EURGBP", 1.577); //18.37
            assetRates.Add("NZDUSD", 1.577); //14.08

        }

        
        private void checkAssetPipCost(string assetName, double expectedValue)
        {
            Asset asset = new Asset();
            asset.Name = assetName;
            Assert.True(EqualityChecks.DoubleNearlyEqual(asset.GetPipCost(localCurrency, assetRates), expectedValue, 0.01));
        }

        [Fact]
        public void PipCostCalculationTest()
        {
            Dictionary<string, double> assetChecks = new Dictionary<string, double>();
            assetChecks.Add("AUDJPY", 12.57);
            assetChecks.Add("AUDUSD", 14.08);
            assetChecks.Add("AUDCAD", 10.52);
            assetChecks.Add("AUDNZD", 9.49);
            assetChecks.Add("AUDCHF", 14.04);
            assetChecks.Add("GBPAUD", 10);
            assetChecks.Add("EURAUD", 10);
            assetChecks.Add("EURGBP", 18.37);
            assetChecks.Add("NZDUSD", 14.08);

            foreach (KeyValuePair<string, double> asset in assetChecks)
                checkAssetPipCost(asset.Key, asset.Value);
            
        }
    }
}
