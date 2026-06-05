using System;
using System.Collections.Generic;

namespace Компилятор
{
    public class LexicalAnalyzer
    {
        public const byte Star = 21;
        public const byte Slash = 60;
        public const byte Equal = 16;
        public const byte Comma = 20;
        public const byte Semicolon = 14;
        public const byte Colon = 5;
        public const byte Point = 61;
        public const byte Arrow = 62;
        public const byte Leftpar = 9;
        public const byte Rightpar = 4;
        public const byte Lbracket = 11;
        public const byte Rbracket = 12;
        public const byte Flpar = 63;
        public const byte Frpar = 64;
        public const byte Later = 65;
        public const byte Greater = 66;
        public const byte Laterequal = 67;
        public const byte Greaterequal = 68;
        public const byte Latergreater = 69;
        public const byte Plus = 70;
        public const byte Minus = 71;
        public const byte Lcomment = 72;
        public const byte Rcomment = 73;
        public const byte Assign = 51;
        public const byte Twopoints = 74;
        public const byte Ident = 2;
        public const byte Floatc = 82;
        public const byte Intc = 15;
        
        public const byte Casesy = 31;
        public const byte Elsesy = 32;
        public const byte Filesy = 57;
        public const byte Gotosy = 33;
        public const byte Thensy = 52;
        public const byte Typesy = 34;
        public const byte Untilsy = 53;
        public const byte Dosy = 54;
        public const byte Withsy = 37;
        public const byte Ifsy = 56;
        public const byte Insy = 100;
        public const byte Ofsy = 101;
        public const byte Orsy = 102;
        public const byte Tosy = 103;
        public const byte Endsy = 104;
        public const byte Varsy = 105;
        public const byte Divsy = 106;
        public const byte Andsy = 107;
        public const byte Notsy = 108;
        public const byte Forsy = 109;
        public const byte Modsy = 110;
        public const byte Nilsy = 111;
        public const byte Setsy = 112;
        public const byte Beginsy = 113;
        public const byte Whilesy = 114;
        public const byte Arraysy = 115;
        public const byte Constsy = 116;
        public const byte Labelsy = 117;
        public const byte Downtosy = 118;
        public const byte Packedsy = 119;
        public const byte Recordsy = 120;
        public const byte Repeatsy = 121;
        public const byte Programsy = 122;
        public const byte Functionsy = 123;
        public const byte Procedurensy = 124;

        private readonly InputOutput _io;
        private readonly Keywords _keywordsObj;
        private byte _symbol;
        private TextPosition _token;
        private string _addrName;
        private int _nmbInt;

        private int _parenthesisBalance;
        private TextPosition _lastLeftParPos;

        public byte Symbol => _symbol;
        public TextPosition Token => _token;

        public LexicalAnalyzer(InputOutput io)
        {
            _io = io;
            _keywordsObj = new Keywords();
            _symbol = 0;
            _token = new TextPosition(0, 0);
            _addrName = string.Empty;
            _nmbInt = 0;
            _parenthesisBalance = 0;
            _lastLeftParPos = new TextPosition(0, 0);
        }

        public byte NextSym()
        {
            while (_io.Ch == ' ' || _io.Ch == '\n' || _io.Ch == '\r')
            {
                _io.NextCh();
            }

            _token = _io.PositionNow;

            if (_io.Ch == '\0')
            {
                if (_parenthesisBalance > 0)
                {
                    _io.Error(250, _lastLeftParPos);
                    _parenthesisBalance = 0;
                }
                _symbol = 0;
                return _symbol;
            }

            if (_io.Ch >= '0' && _io.Ch <= '9')
            {
                byte digit;
                short maxint = short.MaxValue;
                _nmbInt = 0;

                while (_io.Ch >= '0' && _io.Ch <= '9')
                {
                    digit = (byte)(_io.Ch - '0');
                    if (_nmbInt < maxint / 10 || 
                    (_nmbInt == maxint / 10 && digit <= maxint % 10))
                    {
                        _nmbInt = 10 * _nmbInt + digit;
                    }
                    else
                    {
                        _io.Error(203, _io.PositionNow);
                        _nmbInt = 0;
                        while (_io.Ch >= '0' && _io.Ch <= '9')
                        {
                            _io.NextCh();
                        }
                    }
                    _io.NextCh();
                }
                _symbol = Intc;
            }
            else if ((_io.Ch >= 'a' && _io.Ch <= 'z') || 
            (_io.Ch >= 'A' && _io.Ch <= 'Z'))
            {
                string name = "";
                while ((_io.Ch >= 'a' && _io.Ch <= 'z') ||
                       (_io.Ch >= 'A' && _io.Ch <= 'Z') ||
                       (_io.Ch >= '0' && _io.Ch <= '9'))
                {
                    name += _io.Ch;
                    _io.NextCh();
                }

                _addrName = name.ToLower();
                _symbol = Ident;

                byte wordLength = (byte)_addrName.Length;
                if (_keywordsObj.Kw.ContainsKey(wordLength))
                {
                    var innerDict = _keywordsObj.Kw[wordLength];
                    if (innerDict.ContainsKey(_addrName))
                    {
                        _symbol = innerDict[_addrName];
                    }
                }
            }
            else
            {
                switch (_io.Ch)
                {
                    case '(':
                        _symbol = Leftpar;
                        _parenthesisBalance++;
                        _lastLeftParPos = _io.PositionNow;
                        _io.NextCh();
                        break;
                    case ')':
                        _symbol = Rightpar;
                        _parenthesisBalance--;
                        if (_parenthesisBalance < 0)
                        {
                            _io.Error(250, _io.PositionNow);
                            _parenthesisBalance = 0;
                        }
                        _io.NextCh();
                        break;
                    case '<':
                        _io.NextCh();
                        if (_io.Ch == '=')
                        {
                            _symbol = Laterequal;
                            _io.NextCh();
                        }
                        else if (_io.Ch == '>')
                        {
                            _symbol = Latergreater;
                            _io.NextCh();
                        }
                        else
                        {
                            _symbol = Later;
                        }
                        break;
                    case '>':
                        _io.NextCh();
                        if (_io.Ch == '=')
                        {
                            _symbol = Greaterequal;
                            _io.NextCh();
                        }
                        else
                        {
                            _symbol = Greater;
                        }
                        break;
                    case ':':
                        _io.NextCh();
                        if (_io.Ch == '=')
                        {
                            _symbol = Assign;
                            _io.NextCh();
                        }
                        else
                        {
                            _symbol = Colon;
                        }
                        break;
                    case ';':
                        _symbol = Semicolon;
                        _io.NextCh();
                        break;
                    case ',':
                        _symbol = Comma;
                        _io.NextCh();
                        break;
                    case '=':
                        _symbol = Equal;
                        _io.NextCh();
                        break;
                    case '+':
                        _symbol = Plus;
                        _io.NextCh();
                        break;
                    case '-':
                        _symbol = Minus;
                        _io.NextCh();
                        break;
                    case '*':
                        _symbol = Star;
                        _io.NextCh();
                        break;
                    case '/':
                        _symbol = Slash;
                        _io.NextCh();
                        break;
                    case '.':
                        _io.NextCh();
                        if (_io.Ch == '.')
                        {
                            _symbol = Twopoints;
                            _io.NextCh();
                        }
                        else
                        {
                            _symbol = Point;
                        }
                        break;
                    default:
                        _symbol = 0;
                        _io.NextCh();
                        break;
                }
            }

            return _symbol;
        }
    }
}