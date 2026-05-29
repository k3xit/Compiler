using System;
using System.Collections.Generic;
using System.IO;

namespace Компилятор
{
    public struct TextPosition
    {
        private uint _lineNumber;
        private byte _charNumber;

        public uint LineNumber => _lineNumber;
        public byte CharNumber => _charNumber;

        public TextPosition(uint ln = 0, byte c = 0)
        {
            _lineNumber = ln;
            _charNumber = c;
        }
    }

    public struct Err
    {
        private TextPosition _errorPosition;
        private byte _errorCode;

        public TextPosition ErrorPosition => _errorPosition;
        public byte ErrorCode => _errorCode;

        public Err(TextPosition errorPosition, byte errorCode)
        {
            _errorPosition = errorPosition;
            _errorCode = errorCode;
        }
    }

    public class InputOutput
    {
        private const byte Errmax = 9;
        private static readonly List<string> _programText = 
            new List<string>();
        private static readonly Dictionary<byte, string> _errorMessages = 
            new Dictionary<byte, string>
            {
                { 14, "Ожидалась точка с запятой" },
                { 51, "Ожидался знак присваивания" },
                { 203, "Целая константа превышает предел" },
                { 250, "Нарушен баланс круглых скобок" }
            };

        private static int _currentLineIndex = -1;
        private static char _ch;
        private static TextPosition _positionNow = new TextPosition();
        private static List<Err> _errList = new List<Err>();
        private static StreamReader _file;
        private static StreamWriter _codeFile;
        private static uint _errCount;

        public static char Ch => _ch;
        public static TextPosition PositionNow => _positionNow;
        public static StreamReader File => _file;
        public static uint ErrCount => _errCount;

        public static void Initialize(string filePath)
        {
            _file = new StreamReader(filePath);
            _programText.Clear();
            _errList.Clear();
            _errCount = 0;
            _positionNow = new TextPosition(0, 0);
            _currentLineIndex = -1;

            while (!_file.EndOfStream)
            {
                _programText.Add(_file.ReadLine());
            }
            _file.Close();

            if (_programText.Count > 0)
            {
                _currentLineIndex = 0;
                _positionNow = new TextPosition(1, 0);
                _ch = _programText[0].Length > 0 
                    ? _programText[0][0] 
                    : '\n';
            }
        }
        public static void InitializeCodeFile(string filePath)
        {
            _codeFile = new StreamWriter(filePath);
        }

        public static void WriteCode(byte code)
        {
            if (_codeFile != null && code != 0)
            {
                _codeFile.Write($"{code} ");
            }
        }

        public static void NextCh()
        {
            if (_currentLineIndex == -1 
                || _currentLineIndex >= _programText.Count)
            {
                return;
            }

            string currentLine = _programText[_currentLineIndex];

            if (_positionNow.CharNumber >= currentLine.Length - 1)
            {
                _currentLineIndex++;
                if (_currentLineIndex < _programText.Count)
                {
                    _positionNow = new TextPosition(
                        _positionNow.LineNumber + 1, 0);
                    _ch = _programText[_currentLineIndex].Length > 0 
                        ? _programText[_currentLineIndex][0] 
                        : '\n';
                }
                else
                {
                    _currentLineIndex = -1;
                    _ch = '\0';
                }
            }
            else
            {
                _positionNow = new TextPosition(
                    _positionNow.LineNumber, 
                    (byte)(_positionNow.CharNumber + 1));
                _ch = currentLine[_positionNow.CharNumber];
            }
        }

        public static void Error(byte errorCode, TextPosition position)
        {
            if (_errList.Count <= Errmax)
            {
                _errList.Add(new Err(position, errorCode));
            }
        }

        public static void OutputResult()
        {
            if (_codeFile != null)
            {
                _codeFile.Close();
            }

            for (int i = 0; i < _programText.Count; i++)
            {
                string lineText = _programText[i];
                Console.WriteLine(lineText);

                uint currentLineNum = (uint)(i + 1);
                List<Err> errorsInLine = _errList.FindAll(
                    e => e.ErrorPosition.LineNumber == currentLineNum);

                foreach (Err item in errorsInLine)
                {
                    _errCount++;
                    string pointerLine = "";
                    for (int j = 0; j < item.ErrorPosition.CharNumber; j++)
                    {
                        pointerLine += " ";
                    }

                    string msg = _errorMessages.ContainsKey(item.ErrorCode)
                        ? _errorMessages[item.ErrorCode]
                        : "Неизвестная ошибка";

                    pointerLine += 
                        $"^ ошибка: код {item.ErrorCode} ({msg})";
                    Console.WriteLine(pointerLine);
                }
            }

            Console.WriteLine(
                $"Компиляция завершена: ошибок — {_errCount}!");
        }
    }
}