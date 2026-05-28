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
            

            Console.WriteLine("=== ЗАПУСК ТЕСТА ===");
            RunTest(testFile1, resultFile1);

            Console.ReadLine();
        }

        private static void RunTest(string srcPath, string resPath)
        {
            InputOutput.Initialize(srcPath);
            InputOutput.InitializeCodeFile(resPath);
            
            LexicalAnalyzer analyzer = new LexicalAnalyzer();

            Console.WriteLine("--- Поток кодов символов ---");
            
            while (InputOutput.Ch != '\0')
            {
                byte code = analyzer.NextSym();
                
                if (code != 0)
                {
                    Console.Write($"{code} ");
                    InputOutput.WriteCode(code); // Запись в Файл 2
                }
            }
            
            Console.WriteLine("\n\n--- Листинг программы ---");
            InputOutput.OutputResult();
            Console.WriteLine($"\nКоды символов сохранены в: {resPath}");
        }
    }
}