using System;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using QuantBlackTrading;

namespace XUnitTests
{
    public class StrategyTests
    {
        public StrategyTests()
        {

        }

        [Fact]
        public void BruteForceVariantTest3_3()
        {
            //Get the strategy
            Strategy strategy = Strategy.Load("BackTestStrategyTest", @"G:\My Drive\C Sharp Apps\QuantBlackTrading\TestStrategy\bin\Debug\netcoreapp2.1\TestStrategy.dll");
            StrategyVariant[] variants = StrategyVariant.BruteForceGeneration(strategy.OptimiseParameters.ToArray());

            string paramMatrix = "";
            foreach(StrategyVariant v in variants)
            {
                paramMatrix += v.Parameters[0] + " " + v.Parameters[1] + " " + v.Parameters[2] + ";";
            }

            Assert.True(paramMatrix == "3 15 1.3;3 15 1.4;3 15 1.5;3 16 1.3;3 16 1.4;3 16 1.5;3 17 1.3;3 17 1.4;3 17 1.5;4 15 1.3;4 15 1.4;4 15 1.5;4 16 1.3;4 16 1.4;4 16 1.5;4 17 1.3;4 17 1.4;4 17 1.5;5 15 1.3;5 15 1.4;5 15 1.5;5 16 1.3;5 16 1.4;5 16 1.5;5 17 1.3;5 17 1.4;5 17 1.5;",
                "Variant 3x3 matrix not as expected.");

        }
    }
}
