using System;
using System.IO;

namespace Компилятор
{
    public class Program
    {
        public static void Main()
        {
            string testFile1 = "test1.pas";
            string testFile2 = "test2.pas";
            string testFile3 = "test3.pas";

            File.WriteAllText(testFile1, 
                "program Test1;\nbegin\n  var x := 10;\n  " +
                "if x > 5 then\n    x := 0;\nend.");
            File.WriteAllText(testFile2, 
                "program Test2;\nvar\n  a : integer;\n" +
                "begin\n  a := 123;\nend.");
            File.WriteAllText(testFile3, 
                "program Test3;\nbegin\n  while true do\n" +
                "    begin\n    end;\nend.");

            Console.WriteLine("=== ЗАПУСК ТЕСТА 1 ===");
            RunTest(testFile1);

            Console.WriteLine("\n=== ЗАПУСК ТЕСТА 2 ===");
            RunTest(testFile2);

            Console.WriteLine("\n=== ЗАПУСК ТЕСТА 3 ===");
            RunTest(testFile3);

            if (File.Exists(testFile1)) File.Delete(testFile1);
            if (File.Exists(testFile2)) File.Delete(testFile2);
            if (File.Exists(testFile3)) File.Delete(testFile3);

            Console.ReadLine();
        }

        private static void RunTest(string filePath)
        {
            InputOutput.Initialize(filePath);

            while (InputOutput.File != null)
            {
                uint lastLine = InputOutput.PositionNow.lineNumber;
                byte lastChar = InputOutput.PositionNow.charNumber;

                InputOutput.NextCh();

                if (InputOutput.PositionNow.lineNumber == lastLine 
                    && InputOutput.PositionNow.charNumber == lastChar)
                {
                    break;
                }
            }
        }
    }
}