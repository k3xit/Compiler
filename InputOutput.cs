using System;
using System.Collections.Generic;
using System.IO;

namespace Компилятор
{
    public struct TextPosition
    {
        public uint lineNumber;
        public byte charNumber;

        public TextPosition(uint ln = 0, byte c = 0)
        {
            lineNumber = ln;
            charNumber = c;
        }
    }

    public struct Err
    {
        public TextPosition errorPosition;
        public byte errorCode;

        public Err(TextPosition errorPosition, byte errorCode)
        {
            this.errorPosition = errorPosition;
            this.errorCode = errorCode;
        }
    }

    public class InputOutput
    {
        private const byte Errmax = 9;
        private static readonly List<string> ProgramText = 
            new List<string>();
        private static readonly Dictionary<uint, Err> PlannedErrors = 
            new Dictionary<uint, Err>();
        private static readonly Dictionary<byte, string> ErrorMessages = 
            new Dictionary<byte, string>
            {
                { 14, "Ожидалась точка с запятой" },
                { 51, "Ожидался знак присваивания" },
                { 203, "Целая константа превышает предел" }
            };

        private static int currentLineIndex = -1;
        private static bool isFinished;

        public static char Ch { get; set; }
        public static TextPosition PositionNow = new TextPosition();
        public static List<Err> ErrList { get; set; } = new List<Err>();
        public static StreamReader File { get; set; }
        public static uint ErrCount { get; set; }

        public static void Initialize(string filePath)
        {
            File = new StreamReader(filePath);
            ProgramText.Clear();
            PlannedErrors.Clear();
            ErrList.Clear();
            ErrCount = 0;
            PositionNow = new TextPosition(0, 0);
            currentLineIndex = -1;
            isFinished = false;

            uint lineNum = 1;
            List<uint> validLines = new List<uint>();

            while (!File.EndOfStream)
            {
                string currentLine = File.ReadLine();
                ProgramText.Add(currentLine);

                if (!string.IsNullOrEmpty(currentLine))
                {
                    validLines.Add(lineNum);
                }
                lineNum++;
            }

            File.Close();

            Random random = new Random();
            byte[] possibleCodes = { 14, 51, 203 };
            int errorsToPlace = Math.Min(3, validLines.Count);

            for (int i = 0; i < errorsToPlace; i++)
            {
                int targetIndex = random.Next(0, validLines.Count);
                uint targetLine = validLines[targetIndex];
                validLines.RemoveAt(targetIndex);

                string lineText = ProgramText[(int)targetLine - 1];
                byte randomChar = (byte)random.Next(0, lineText.Length);
                byte randomCode = possibleCodes[
                    random.Next(0, possibleCodes.Length)];

                TextPosition errPos = new TextPosition(
                    targetLine, randomChar);
                PlannedErrors[targetLine] = new Err(errPos, randomCode);
            }

            if (ProgramText.Count > 0)
            {
                currentLineIndex = 0;
                PositionNow.lineNumber = 1;
                PositionNow.charNumber = 0;
                Ch = ProgramText[0].Length > 0 
                    ? ProgramText[0][0] 
                    : '\n';
            }
        }

        public static void NextCh()
        {
            if (isFinished)
            {
                return;
            }

            if (currentLineIndex == -1 
                || currentLineIndex >= ProgramText.Count)
            {
                End();
                return;
            }

            string currentLine = ProgramText[currentLineIndex];

            if (PlannedErrors.ContainsKey(PositionNow.lineNumber))
            {
                Err planned = PlannedErrors[PositionNow.lineNumber];
                if (planned.errorPosition.charNumber == 
                    PositionNow.charNumber)
                {
                    Error(planned.errorCode, PositionNow);
                }
            }

            if (PositionNow.charNumber >= currentLine.Length - 1)
            {
                currentLineIndex++;
                if (currentLineIndex < ProgramText.Count)
                {
                    PositionNow.lineNumber++;
                    PositionNow.charNumber = 0;
                    Ch = ProgramText[currentLineIndex].Length > 0 
                        ? ProgramText[currentLineIndex][0] 
                        : '\n';
                }
                else
                {
                    End();
                }
            }
            else
            {
                PositionNow.charNumber++;
                Ch = currentLine[PositionNow.charNumber];
            }
        }

        public static void Error(byte errorCode, TextPosition position)
        {
            if (ErrList.Count <= Errmax)
            {
                ErrList.Add(new Err(position, errorCode));
            }
        }

        private static void End()
        {
            isFinished = true;

            for (int i = 0; i < ProgramText.Count; i++)
            {
                string lineText = ProgramText[i];
                Console.WriteLine(lineText);

                uint currentLineNum = (uint)(i + 1);
                List<Err> errorsInLine = ErrList.FindAll(
                    e => e.errorPosition.lineNumber == currentLineNum);

                foreach (Err item in errorsInLine)
                {
                    ErrCount++;
                    string pointerLine = "";
                    for (int j = 0; j < item.errorPosition.charNumber; j++)
                    {
                        pointerLine += " ";
                    }

                    string msg = ErrorMessages.ContainsKey(item.errorCode)
                        ? ErrorMessages[item.errorCode]
                        : "Неизвестная ошибка";

                    pointerLine += 
                        $"^ ошибка: код {item.errorCode} ({msg})";
                    Console.WriteLine(pointerLine);
                }
            }

            Console.WriteLine(
                $"Компиляция завершена: ошибок — {ErrCount}!");
        }
    }
}