using System;
using System.IO;

namespace Компилятор
{
    public class Program
    {
        public static void Main()
        {
            string testFile1 = "test1.pas";
            string resultFile1 = "test1.txt";
            string testFile2 = "test2.pas";
            string resultFile2 = "test2.txt";

            Console.WriteLine("=== ЗАПУСК ТЕСТА 1 ===");
            RunTest(testFile1, resultFile1);
            Console.WriteLine("=== ЗАПУСК ТЕСТА 2 ===");
            RunTest(testFile2, resultFile2);
            Console.ReadLine();
        }

        private static void RunTest(string srcPath, string resPath)
        {
            InputOutput.Initialize(srcPath);
            InputOutput.InitializeCodeFile(resPath);
            
            LexicalAnalyzer analyzer = new LexicalAnalyzer();

            while (true)
            {
                byte code = analyzer.NextSym();
                
                if (code == 0)
                {
                    break;
                }

                InputOutput.WriteCode(code);
            }
            
            InputOutput.OutputResult();
        }
    }
}