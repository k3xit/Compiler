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
            string testFile3 = "test3.pas";
            string resultFile3 = "test3.txt";

            Console.WriteLine("=== ЗАПУСК ТЕСТА 1 ===");
            RunTest(testFile1, resultFile1);
            Console.WriteLine("=== ЗАПУСК ТЕСТА 2 ===");
            RunTest(testFile2, resultFile2);
            Console.WriteLine("=== ЗАПУСК ТЕСТА 3 ===");
            RunTest(testFile3, resultFile3);
            Console.ReadLine();
        }

        private static void RunTest(string srcPath, string resPath)
        {
            InputOutput io = new InputOutput(srcPath, resPath);
            LexicalAnalyzer analyzer = new LexicalAnalyzer(io);
            
            SyntaxAnalyzer syntax = new SyntaxAnalyzer(analyzer, io);
            
            syntax.Parse();
            
            io.OutputResult(); 
        }
    }
}