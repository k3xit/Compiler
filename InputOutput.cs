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
        private readonly byte _errmax;
        private readonly List<string> _programText;
        private readonly Dictionary<byte, string> _errorMessages;

        private int _currentLineIndex;
        private char _ch;
        private TextPosition _positionNow;
        private List<Err> _errList;
        private StreamReader _file;
        private StreamWriter _codeFile;
        private uint _errCount;

        public char Ch => _ch;
        public TextPosition PositionNow => _positionNow;
        public StreamReader File => _file;
        public uint ErrCount => _errCount;

        public InputOutput(string sourcePath, string codePath)
        {
            _errmax = 9;
            _programText = new List<string>();
            _errorMessages = new Dictionary<byte, string>
            {
                { 14, "Ожидалась точка с запятой" },
                { 16, "Ожидался знак равенства '=' " },
                { 51, "Ожидался знак присваивания" },
                { 203, "Целая константа превышает предел" },
                { 250, "Нарушен баланс круглых скобок" },
                { 2, "Ожидался идентификатор (имя или тип данных)" },
                { 5, "Ожидалось двоеточие ':'" },
                { 98, "Ошибка в выражении: неверный множитель" },
                { 99, "Недопустимый оператор (ожидалось присваивание)" },
                { 104, "Ожидалось ключевое слово 'end'" },
                { 113, "Ожидалось ключевое слово 'begin'" }
            };

            _currentLineIndex = -1;
            _ch = '\0';
            _positionNow = new TextPosition(0, 0);
            _errList = new List<Err>();
            _errCount = 0;

            _file = new StreamReader(sourcePath);
            _codeFile = new StreamWriter(codePath);

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

        public void WriteCode(byte code)
        {
            if (_codeFile != null && code != 0)
            {
                _codeFile.Write($"{code} ");
            }
        }

        public void NextCh()
        {
            if (_currentLineIndex == -1 || _currentLineIndex >= 
            _programText.Count)
            {
                return;
            }

            string currentLine = _programText[_currentLineIndex];

            if (_positionNow.CharNumber >= currentLine.Length - 1)
            {
                _currentLineIndex++;
                if (_currentLineIndex < _programText.Count)
                {
                    _positionNow = 
                    new TextPosition(_positionNow.LineNumber + 1, 0);
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

        public void Error(byte errorCode, TextPosition position)
        {
            if (_errList.Count <= _errmax)
            {
                _errList.Add(new Err(position, errorCode));
            }
        }

        public void OutputResult()
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

            Console.WriteLine("Компиляция завершена: " +
            $"ошибок — {_errCount}!");
        }
    }
}