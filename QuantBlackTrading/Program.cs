using System;


using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuantBlackTrading
{
    // Declaration
    public enum MessageType { Message, Error, Log, Update }
    public delegate void MessageDelegate(string message, MessageType type = MessageType.Message);
    public delegate void OnBackTestComplete(TestSummary testSummary);
    public delegate void OnTestComplete(TestSet testSet);

    class Program
    {
        public static string commandFile = "CommandMemory.txt";
        public static string lastReportFilename = "LastTestSummary.bin";
        public static string strategiesDLL = @"G:\My Drive\C Sharp Apps\QuantBlackTrading\TestStrategy\bin\Debug\netcoreapp2.1\TestStrategy.dll";
        public static TestSummary[] filteredTestSummary;

        public static TestSummary currentTestSummary;

        static void Main(string[] args)
        {


            /*

            //BACKTESTER and LIVE TRADER
            //Live trading through MT5 using DLL or cTrader

            //Load previous perofrmance erport
            if (File.Exists(lastReportFilename))
            {
                Console.WriteLine("Reading saved file");
                Stream openFileStream = File.OpenRead(lastReportFilename);
                BinaryFormatter deserializer = new BinaryFormatter();
                if(openFileStream.Length > 0)
                    currentTestSummary = (TestSummary)deserializer.Deserialize(openFileStream);
                openFileStream.Close();
            }


            //Load previous commands for ease of access
            string[] previousCommands = new string[100];
            if (File.Exists(commandFile))
            {
                string[] prev = File.ReadAllLines(commandFile);
                for (int i = 0; i < prev.Length; i++)
                {
                    if (prev[i] == "")
                        previousCommands[i] = null;
                    else
                        previousCommands[i] = prev[i];
                }
            }
            int commandIndex = 0;


            string command = "";

            Console.Write("->");

            while (true)
            {
                ConsoleKeyInfo ki = Console.ReadKey();

                if (ki.Key == ConsoleKey.UpArrow)
                {
                    if (commandIndex < 100)
                    {
                        command = previousCommands[commandIndex];
                        if (command != null)
                        {
                            commandIndex++;
                            Console.Write("\r" + new string(' ', Console.WindowWidth - 1));
                            Console.Write("\r->" + command);
                        }
                        else
                            Console.Write("\b");
                    }
                    else
                        Console.Write("\b");

                }
                else if (ki.Key == ConsoleKey.DownArrow)
                {
                    if (commandIndex > 0)
                    {
                        commandIndex--;
                        command = previousCommands[commandIndex];
                        Console.Write("\r" + new string(' ', Console.WindowWidth - 1));
                        Console.Write("\r->" + command);
                    }
                    else
                        Console.Write("\b");

                }
                else if (ki.Key != ConsoleKey.Enter)
                {
                    if (ki.Key == ConsoleKey.Backspace)
                    {
                        if (command.Length > 0)
                            command = command.Substring(0, command.Length - 1);
                        Console.Write(" \b");
                    }
                    if (!Char.IsControl(ki.KeyChar))
                        command += ki.KeyChar;
                }
                else
                {
                    //keep memory of previous commands
                    Array.Copy(previousCommands, 0, previousCommands, 1, previousCommands.Length - 1);
                    previousCommands[0] = command;
                    commandIndex = 0;
                    File.WriteAllLines(commandFile, previousCommands);

                    bool newLine = true;

                    if (command == "\\q")
                        break;
                    else
                    {
                        string[] cArgs = command.Split(" ");
                        if (cArgs.Length > 0)
                        {
                            //Converting CSV files to Binary files 
                            if (cArgs[0].ToLower() == "csvtobinary" && cArgs.Length > 1)
                            {
                                try
                                {
                                    csvToBinary(cArgs[1]);
                                }
                                catch (IOException ex)
                                {
                                    displayMessage(ex.Message, MessageType.Error);
                                }
                            }

                            //Backtest a strategy given the strategy name
                            else if (cArgs[0].ToLower() == "test" && cArgs.Length > 1)
                            {
                                try
                                {
                                    backTest(cArgs[1], false);
                                    newLine = false;
                                }
                                catch (Exception ex)
                                {
                                    displayMessage(ex.Message, MessageType.Error);
                                }
                            }

                            //Backtest a strategy with optimisation given the strategy name
                            else if (cArgs[0].ToLower() == "optimise" && cArgs.Length > 1)
                            {
                                try
                                {
                                    backTest(cArgs[1], true);
                                    newLine = false;
                                }
                                catch (Exception ex)
                                {
                                    displayMessage(ex.Message, MessageType.Error);
                                }
                            }

                            //Creating a feature file from python
                            else if (cArgs[0].ToLower() == "buildpythonfeatures" && cArgs.Length > 5)
                            {
                                try
                                {
                                    BuildPythonFeatures(cArgs);
                                }
                                catch (Exception ex)
                                {
                                    displayMessage(ex.Message, MessageType.Error);
                                }
                            }

                            //Post analysis wfo 
                            else if (cArgs[0].ToLower() == "wfo" && cArgs.Length > 2)
                            {
                                try
                                {
                                    WFO(cArgs);
                                }
                                catch (Exception ex)
                                {
                                    displayMessage(ex.Message, MessageType.Error);
                                }
                            }

                            //Reduce correlated parameters
                            else if (cArgs[0].ToLower() == "reduce_corr")
                            {
                                try
                                {
                                    ReduceCorrelated(cArgs);
                                }
                                catch (Exception ex)
                                {
                                    displayMessage(ex.Message, MessageType.Error);
                                }
                            }
                            //Reduce poor ranking parameters
                            else if (cArgs[0].ToLower() == "reduce_by_rank")
                            {
                                try
                                {
                                    ReduceByRank(cArgs);
                                }
                                catch (Exception ex)
                                {
                                    displayMessage(ex.Message, MessageType.Error);
                                }
                            }
                            //show performance of filtered reports in train and test sets
                            else if (cArgs[0].ToLower() == "disp_filtered")
                            {
                                try
                                {
                                    DisplayFilteredPerformance();
                                }
                                catch (Exception ex)
                                {
                                    displayMessage(ex.Message, MessageType.Error);
                                }
                            }
                            //show performance of filtered reports in train and test sets
                            else if (cArgs[0].ToLower() == "all")
                            {
                                try { 
                               
                                    RunAllByAsset();
                                }
                                catch (Exception ex)
                                {
                                    displayMessage(ex.Message, MessageType.Error);
                                }

                        }

                        }
                    }

                    //reset the command interface
                    command = "";
                    if (newLine)
                    {
                        Console.WriteLine();
                        Console.Write("->");
                    }

                }
            }



            Console.WriteLine("Exiting...");
            */
        }
        /*
        private static void RunAllByAsset()
        {

           
            File.Delete("results.txt");
            List<PerformanceReport> allAssetsPerformance = new List<PerformanceReport>();
            List<PerformanceReport> allAssetsWfoPerformance = new List<PerformanceReport>();
            string[] assets = CurrentReport.IndividualReports.Select(x => x.Asset).Distinct().ToArray();
            foreach(string asset in assets)
            {
                displayMessage(asset + "\n", MessageType.Log);
                ReduceCorrelated(new string[] {"", "80", "0.75", asset});
                ReduceByRank(new string[] { "", "220", "110", "1.0", asset });
                allAssetsPerformance.Add(DisplayFilteredPerformance(asset));
                allAssetsWfoPerformance.Add(WFO(new string[] { "", "220", "110", asset }));
            }

            PerformanceReport final = PerformanceReport.MergeReports(allAssetsPerformance.ToArray());
            final.TrainDate = allAssetsPerformance.FirstOrDefault().TrainDate;
            displayMessage("\nALL ASSETS FILTER", MessageType.Log);
            displayMessage("\nTRAIN SET\n" + final.QuickSummary(PerformanceReport.TradeSet.Train), MessageType.Log);
            displayMessage("\nTEST SET " + final.TrainDate.ToShortDateString() +  "\n" + final.QuickSummary(PerformanceReport.TradeSet.Test), MessageType.Log);
            final.ToCsv(@"C:\ForexData\TradeResults\filtered_trades.csv", PerformanceReport.TradeSet.All);

            PerformanceReport finalWfo = PerformanceReport.MergeReports(allAssetsWfoPerformance.ToArray());
            finalWfo.TrainDate = final.TrainDate; //set this so both results are from the last 20% of das
            displayMessage("\nALL ASSETS WFO", MessageType.Log);
            displayMessage("\n" + finalWfo.QuickSummary(PerformanceReport.TradeSet.Test), MessageType.Log);
            finalWfo.ToCsv(@"C:\ForexData\TradeResults\wfo_trades.csv", PerformanceReport.TradeSet.All);
            
        }


        private static void ReduceCorrelated(string[] args)
        {
            int trainPercent = Convert.ToInt32(args[1]); //80
            double r2cutoff = Convert.ToDouble(args[2]);  //0.75
                        
            string asset = null;
            if (args.Length > 3)
                asset = args[3];

            displayMessage("Reducing correlated parameter sets", MessageType.Log);
            filteredTestSummary = PostAnalysis.RemoveCorrelated(currentTestSummary, trainPercent, r2cutoff, asset);
            displayMessage("Removed " + (currentTestSummary.TestSets.Length - filteredTestSummary.Length), MessageType.Log);


        }

        private static void ReduceByRank(string[] args)
        {
            int testDays = Convert.ToInt32(args[1]); //110
            double maxRankRange = Convert.ToDouble(args[2]); // 1.0
            string asset = null;
            if (args.Length > 4)
                asset = args[3];

            filteredTestSummary = PostAnalysis.ParameterStabilityReduction(filteredTestSummary, testDays, maxRankRange, asset);

        }

        private static void RemoveLowRank(string[] args)
        {
    
            int testDays = Convert.ToInt32(args[1]); //110
            string asset = null;
            if (args.Length > 2)
                asset = args[2];

            filteredTestSummary = PostAnalysis.ParameterStabilityReduction(filteredTestSummary, testDays, 0, asset);
        }
        private static PerformanceReport DisplayFilteredPerformance(string asset = null)
        {/*
            //Show performance in train set
            PerformanceReport pr = PerformanceReport.MergeReports(filteredReports);
            pr.Description = "FINAL";
            displayMessage("\nTRAIN SET\n" + pr.QuickSummary(PerformanceReport.TradeSet.Train), MessageType.Log);
            displayMessage("\nTEST SET\n" + pr.QuickSummary(PerformanceReport.TradeSet.Test), MessageType.Log);
            pr.ToCsv(@"C:\ForexData\TradeResults\filtered_trades.csv");

            PerformanceReport[] reports = CurrentReport.IndividualReports;
            if (asset != null)
                reports = reports.Where(x => x.Asset == asset).ToArray();
            //show the performance of the test set for all static parameter sets
            foreach (PerformanceReport opr in reports)
            {
                opr.TrainDate = pr.TrainDate;
                opr.QuickSummary(PerformanceReport.TradeSet.Test);
                
            }

            List<PerformanceReport> ordered = new List<PerformanceReport>(reports);
            ordered.Add(pr);
            ordered = ordered.OrderByDescending(x => x.ProfitFactor).ToList();
            int rank = 1;
            int finalRank = 0;
            foreach (PerformanceReport opr in ordered)
            {
                displayMessage(opr.SummaryLine());
                if (opr == pr)
                    finalRank = rank;
                rank++;
            }
            displayMessage("\nTEST SET RANK: " + finalRank + "\n", MessageType.Log);

            return pr; 

            return null;

        }

        private static void BuildPythonFeatures(string[] args)
        {
            Console.WriteLine();

            int timeframe = Convert.ToInt32(args[1]);
            string assetName = args[2];
            string assetDetailsPath = args[3];
            string outputPath = args[4];
            string featureCommands = args[5];

            DataFeedType type = DataFeedType.Ask;
            if (args.Length > 7 && args[7] == "Bid")
                type = DataFeedType.Bid;

            ExternalFeatureData efd = new ExternalFeatureData(timeframe, outputPath, null);
            efd.CalculateOn = type;
            efd.FeatureCommands = featureCommands;

            //Load in the asset details and add these to the strategy if selected in TradeAssetList
            Dictionary<string, Asset> assetDetails = Asset.LoadAssetFile(assetDetailsPath);

            Asset asset = assetDetails[assetName];

            DataBuilder.PythonFeatureBuilder(asset, efd, displayMessage);

        }

        

        private static PerformanceReport WFO(string[] args)
        {
            int trainDays = Convert.ToInt32(args[1]);
            int testDays = Convert.ToInt32(args[2]);
            string asset = null;
            if (args.Length > 3)
                asset = args[3];

            PerformanceReport wfo = PostAnalysis.WFO(currentTestSummary, trainDays, testDays, asset);
            displayMessage("-----------------WFO-----------------", MessageType.Log);
            displayMessage(wfo.QuickSummary(), MessageType.Log);
            wfo.ToCsv(@"C:\ForexData\TradeResults\wfo_trades.csv");

            return wfo;
        }

        private static void OnCompleteBackTest(TestSummary testSummary)
        {
            //Keep the report in memory so we can do further commands on it
            currentTestSummary = testSummary;

            //Save to memory for later use if required
            Stream SaveFileStream = File.Create(lastReportFilename);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SaveFileStream, currentTestSummary);
            SaveFileStream.Close();

            //Show a merged performance
            displayMessage("---------Overall backtest results---------");

            displayMessage(pr.QuickSummary());

            

            Console.WriteLine("-----------------COMPLETE-----------------");

            if (pr.IndividualReports != null && pr.IndividualReports.Length > 0)
            {
                
                PerformanceReport wfo = PostAnalysis.WFO(pr.IndividualReports.ToArray(), 220, 220);
                Console.WriteLine("-----------------WFO-----------------");
                Console.WriteLine(wfo.QuickSummary());
                wfo.ToCsv(@"C:\ForexData\TradeResults\trades.csv");
                
                displayMessage("----------------------------------------------");                

                double inProfitPercent = (double)pr.IndividualReports.Where(x => x.TotalProfit >= 0).Count() / (double)pr.IndividualReports.Count() * 100;
                displayMessage(inProfitPercent.ToString("0.0") + "%");
                displayMessage("----------------------------------------------");
            }
            else
                pr.ToCsv(@"C:\ForexData\TradeResults\trades.csv");

            Console.Write("->");
        }

        private static void backTest(string strategyName, bool optimise)
        {
            //Compile all our strategies first
            Console.WriteLine();
            displayMessage("Compiling strategies...", MessageType.Message);
            bool success = CommandLineDotNetCoreBuild.BuildStrategies(@"G:\My Drive\C Sharp Apps\QuantBlackTrading\TestStrategy\bin\Debug\netcoreapp2.1\TestStrategy.dll", displayMessage);
            if (success)
                displayMessage("Success...", MessageType.Message);
            else
                displayMessage("Failed..", MessageType.Error);

            
            try
            {
                BackTest bt = new BackTest(OnCompleteBackTest, displayMessage);
                bt.Run(strategyName, strategiesDLL, optimise);
            } catch(Exception ex)
            {
                displayMessage(ex.Message, MessageType.Error);
            }

        }

        private static void csvToBinary(string filename)
        {
            Console.WriteLine();

            //can pass a single filename to convert from csv to binary
            if (filename.Contains(".csv"))
            {
                try
                {
                    DataBuilder.CsvToBinary(filename, filename.Replace(".csv", ".bin"), true, displayMessage);
                }
                catch (Exception e)
                {
                    displayMessage(filename + " was not processed: " + e.Message, MessageType.Error);
                }
            }
            //or pass a directory and all the filenames of .csv will be converted to binary
            else
            {
                string[] filenames = Directory.GetFiles(filename);
                foreach (string f in filenames)
                {
                    if (f.Contains(".csv"))
                    {

                        try
                        {
                            DataBuilder.CsvToBinary(f, f.Replace(".csv", ".bin"), true, displayMessage);
                        }
                        catch(Exception e)
                        {
                            displayMessage(f + " was not processed: " + e.Message, MessageType.Error);
                        }
                    }
                }
            }
        }


        private static void displayMessage(string message, MessageType type = MessageType.Message)
        {     
            Console.WriteLine(message);

            if (type == MessageType.Log)
                File.AppendAllText("results.txt", message);
        }
        */
    }
}
