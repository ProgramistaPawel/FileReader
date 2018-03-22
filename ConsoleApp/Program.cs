using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool continueProgram = true;
            string fileName = "testFile.txt";

            while (continueProgram)
            {
                ShowMenu();
                var pressedKey = Console.ReadKey();

                if (pressedKey.KeyChar == '1')
                {
                    CreateTestFile(fileName);
                }
                else if (pressedKey.KeyChar == '2')
                    RunTests(fileName);
                else
                    continueProgram = false;
            }

        }

        private static void CreateTestFile(string fileName)
        {
            PrintOnTheScreen("Creating test file...");
            RandomFileGenerator.Generate(fileName, 100, 20000);
            PrintOnTheScreen($"Created file: {new FileInfo(fileName).Length >> 10} MB");
            PrintOnTheScreen("");
        }

        private static void RunTests(string fileName)
        {
            Action LoadFileToOneString = () => PerformTest("File.ReadAllText"
                                                            , () => SimulateDoingSomethingWithData(File.ReadAllText(fileName)));
            LoadFileToOneString.CatchOutOfMemoryEx();

            PerformTest("File.ReadAllLines"
                , () => File.ReadAllLines(fileName)
                            .Select((x) => SimulateDoingSomethingWithData(x))
                            .ToList());

            PerformTest("File.ReadLines"
                , () => File.ReadLines(fileName)
                            .Select((x) => SimulateDoingSomethingWithData(x))
                            .ToList());

            PerformTest("FileReader.ReadLineByLineWithBuffer buffer: 128"
                , () => FileReader.ReadLineByLineWithBuffer(128, fileName)
                                  .Select((x) => SimulateDoingSomethingWithData(x))
                                  .ToList());

            PerformTest("FileReader.ReadLineByLineWithBuffer buffer: 4096"
                , () => FileReader.ReadLineByLineWithBuffer(4096, fileName)
                                  .Select((x) => SimulateDoingSomethingWithData(x))
                                  .ToList());

            PerformTest("FileReader.ReadLineByLineWithBuffer buffer: 131072"
                , () => FileReader.ReadLineByLineWithBuffer(131072, fileName)
                                  .Select((x) => SimulateDoingSomethingWithData(x))
                                  .ToList());


            PerformTest("File.ReadAllLines + degree of parallelism 2"
                , () => Parallel.ForEach(File.ReadAllLines(fileName)
                                        , new ParallelOptions { MaxDegreeOfParallelism = 2 }
                                        , (x) => SimulateDoingSomethingWithData(x)));

            PerformTest("File.ReadAllLines + degree of parallelism 4"
                , () => Parallel.ForEach(File.ReadAllLines(fileName)
                                        , new ParallelOptions { MaxDegreeOfParallelism = 4 }
                                        , (x) => SimulateDoingSomethingWithData(x)));

            PerformTest("File.ReadAllLines + degree of parallelism 8"
                , () => Parallel.ForEach(File.ReadAllLines(fileName)
                                        , new ParallelOptions { MaxDegreeOfParallelism = 8 }
                                        , (x) => SimulateDoingSomethingWithData(x)));

            PerformTest("File.ReadLines + degree of parallelism 2"
                , () => Parallel.ForEach(File.ReadLines(fileName)
                                        , new ParallelOptions { MaxDegreeOfParallelism = 2 }
                                        , (x) => SimulateDoingSomethingWithData(x)));


            PerformTest("File.ReadLines + degree of parallelism 4"
                , () => Parallel.ForEach(File.ReadLines(fileName)
                                        , new ParallelOptions { MaxDegreeOfParallelism = 4 }
                                        , (x) => SimulateDoingSomethingWithData(x)));

            PerformTest("File.ReadLines + degree of parallelism 8"
                , () => Parallel.ForEach(File.ReadLines(fileName)
                                        , new ParallelOptions { MaxDegreeOfParallelism = 8 }
                                        , (x) => SimulateDoingSomethingWithData(x)));
        }

        private static void PerformTest(string testName, Action test)
        {
            int numberOfSpaces = 50 - testName.Length;
            Console.Write($"{testName} in progress. " + GenerateSpaces(numberOfSpaces));
            var execTime = Timer.MesureExecutionTime(() => test());
            Console.WriteLine($"Execution time: {execTime}");
            GC.Collect();
            Thread.Sleep(3000);
        }

        private static string GenerateSpaces(int numberOfSpaces)
        {
            return new string(' ', numberOfSpaces);
        }


        private static void ShowMenu()
        {
            PrintOnTheScreen("");
            PrintOnTheScreen("************************************************");
            PrintOnTheScreen("File reader");
            PrintOnTheScreen("1. Create test file");
            PrintOnTheScreen("2. Execute test");
            PrintOnTheScreen("3. Close program");
            PrintOnTheScreen("************************************************");
            PrintOnTheScreen("Chose option: ");
            PrintOnTheScreen("");
        }

        private static double SimulateDoingSomethingWithData(string data)
            => data.Split(',')
                   .Select(x => Encoding.UTF8.GetBytes(x))
                   .Select(x => MD5.Create().ComputeHash(x))
                   .Select(x => Math.Log(Math.Sqrt(Math.Pow(Math.Log10(Math.Sqrt(Math.Log(x.Sum(c => (int)c)) * 2.3D) + 22), 7D) - 3)))
                    .Sum();

        private static void PrintOnTheScreen(string message)
            => Console.WriteLine(message);
    }

}
