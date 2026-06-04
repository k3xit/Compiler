using System;
using System.Collections.Generic;

namespace Компилятор
{
    public class SyntaxAnalyzer
    {
        private readonly LexicalAnalyzer _lexer;
        private readonly InputOutput _io;
        private byte _sym;

        public SyntaxAnalyzer(LexicalAnalyzer lexer, InputOutput io)
        {
            _lexer = lexer;
            _io = io;
        }

        private void NextSym()
        {
            _sym = _lexer.NextSym();
        }

        public void Parse()
        {
            NextSym();

            if (_sym == LexicalAnalyzer.Programsy)
            {
                NextSym();
                if (_sym == LexicalAnalyzer.Ident)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(2, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Semicolon, LexicalAnalyzer.Varsy,
                        LexicalAnalyzer.Constsy, LexicalAnalyzer.Functionsy,
                        LexicalAnalyzer.Beginsy);
                }

                if (_sym == LexicalAnalyzer.Semicolon)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(14, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Varsy, LexicalAnalyzer.Constsy,
                        LexicalAnalyzer.Functionsy, LexicalAnalyzer.Beginsy);
                }
            }

            ParseBlock();

            if (_sym == LexicalAnalyzer.Point)
            {
                NextSym();
            }
        }

        private void ParseBlock()
        {
            while (_sym == LexicalAnalyzer.Varsy ||
                    _sym == LexicalAnalyzer.Constsy ||
                    _sym == LexicalAnalyzer.Functionsy)
            {
                if (_sym == LexicalAnalyzer.Varsy)
                {
                    ParseVarDeclarations();
                }
                else if (_sym == LexicalAnalyzer.Constsy)
                {
                    ParseConstDeclarations();
                }
                else if (_sym == LexicalAnalyzer.Functionsy)
                {
                    ParseFunctionDeclaration();
                }
            }

            ParseCompoundStatement();
        }

        private void ParseConstDeclarations()
        {
            NextSym();
            while (_sym == LexicalAnalyzer.Ident)
            {
                NextSym();

                if (_sym == LexicalAnalyzer.Equal)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(16, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Intc, LexicalAnalyzer.Floatc,
                        LexicalAnalyzer.Semicolon, LexicalAnalyzer.Ident,
                        LexicalAnalyzer.Varsy, LexicalAnalyzer.Functionsy,
                        LexicalAnalyzer.Beginsy);
                }

                if (_sym == LexicalAnalyzer.Intc || _sym == LexicalAnalyzer.Floatc)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(98, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Semicolon, LexicalAnalyzer.Ident,
                        LexicalAnalyzer.Varsy, LexicalAnalyzer.Functionsy,
                        LexicalAnalyzer.Beginsy);
                }

                if (_sym == LexicalAnalyzer.Semicolon)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(14, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Ident, LexicalAnalyzer.Varsy,
                        LexicalAnalyzer.Functionsy, LexicalAnalyzer.Beginsy);
                }
            }
        }

        private void ParseVarDeclarations()
        {
            NextSym();

            while (_sym == LexicalAnalyzer.Ident)
            {
                while (_sym == LexicalAnalyzer.Ident)
                {
                    NextSym();
                    if (_sym == LexicalAnalyzer.Comma)
                    {
                        NextSym();
                        if (_sym != LexicalAnalyzer.Ident)
                        {
                            _io.Error(2, _lexer.Token);
                            SkipTo(LexicalAnalyzer.Colon,
                                LexicalAnalyzer.Semicolon,
                                LexicalAnalyzer.Varsy,
                                LexicalAnalyzer.Functionsy,
                                LexicalAnalyzer.Beginsy);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (_sym == LexicalAnalyzer.Colon)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(5, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Ident, LexicalAnalyzer.Semicolon,
                        LexicalAnalyzer.Varsy, LexicalAnalyzer.Functionsy,
                        LexicalAnalyzer.Beginsy);
                }

                if (_sym == LexicalAnalyzer.Ident)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(2, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Semicolon, LexicalAnalyzer.Varsy,
                    LexicalAnalyzer.Functionsy, LexicalAnalyzer.Beginsy);
                }

                if (_sym == LexicalAnalyzer.Semicolon)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(14, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Ident, LexicalAnalyzer.Varsy,
                        LexicalAnalyzer.Functionsy, LexicalAnalyzer.Beginsy);
                }
            }
        }

        private void ParseFunctionDeclaration()
        {
            NextSym();

            if (_sym == LexicalAnalyzer.Ident)
            {
                NextSym();
            }
            else
            {
                _io.Error(2, _lexer.Token);
                SkipTo(LexicalAnalyzer.Leftpar, LexicalAnalyzer.Colon,
                    LexicalAnalyzer.Semicolon, LexicalAnalyzer.Varsy,
                    LexicalAnalyzer.Functionsy, LexicalAnalyzer.Beginsy);
            }

            if (_sym == LexicalAnalyzer.Leftpar)
            {
                NextSym();
                while (_sym == LexicalAnalyzer.Ident)
                {
                    NextSym();
                    if (_sym == LexicalAnalyzer.Colon) NextSym();
                    else _io.Error(5, _lexer.Token);

                    if (_sym == LexicalAnalyzer.Ident) NextSym();
                    else _io.Error(2, _lexer.Token);

                    if (_sym == LexicalAnalyzer.Semicolon) NextSym();
                    else break;
                }

                if (_sym == LexicalAnalyzer.Rightpar)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(250, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Colon,LexicalAnalyzer.Semicolon,
                        LexicalAnalyzer.Varsy, LexicalAnalyzer.Functionsy,
                        LexicalAnalyzer.Beginsy);
                }
            }

            if (_sym == LexicalAnalyzer.Colon)
            {
                NextSym();
            }
            else
            {
                _io.Error(5, _lexer.Token);
                SkipTo(LexicalAnalyzer.Ident, LexicalAnalyzer.Semicolon,
                    LexicalAnalyzer.Varsy, LexicalAnalyzer.Functionsy,
                    LexicalAnalyzer.Beginsy);
            }

            if (_sym == LexicalAnalyzer.Ident)
            {
                NextSym();
            }
            else
            {
                _io.Error(2, _lexer.Token);
                SkipTo(LexicalAnalyzer.Semicolon, LexicalAnalyzer.Varsy,
                    LexicalAnalyzer.Functionsy, LexicalAnalyzer.Beginsy);
            }

            if (_sym == LexicalAnalyzer.Semicolon)
            {
                NextSym();
            }
            else
            {
                _io.Error(14, _lexer.Token);
                SkipTo(LexicalAnalyzer.Varsy, LexicalAnalyzer.Functionsy,
                    LexicalAnalyzer.Beginsy);
            }

            ParseBlock();

            if (_sym == LexicalAnalyzer.Semicolon)
            {
                NextSym();
            }
            else
            {
                _io.Error(14, _lexer.Token);
                SkipTo(LexicalAnalyzer.Varsy, LexicalAnalyzer.Functionsy,
                    LexicalAnalyzer.Beginsy);
            }
        }

        private void ParseCompoundStatement()
        {
            if (_sym == LexicalAnalyzer.Beginsy)
            {
                NextSym();
            }
            else
            {
                _io.Error(113, _lexer.Token);
                SkipTo(LexicalAnalyzer.Ident, LexicalAnalyzer.Endsy,
                    LexicalAnalyzer.Semicolon);
            }

            while (_sym != LexicalAnalyzer.Endsy && _sym != 0)
            {
                ParseStatement();

                if (_sym == LexicalAnalyzer.Semicolon)
                {
                    NextSym();
                }
                else if (_sym != LexicalAnalyzer.Endsy)
                {
                    _io.Error(14, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Ident, LexicalAnalyzer.Endsy,
                        LexicalAnalyzer.Semicolon);
                    if (_sym == LexicalAnalyzer.Semicolon) NextSym();
                }
            }

            if (_sym == LexicalAnalyzer.Endsy)
            {
                NextSym();
            }
            else
            {
                _io.Error(104, _lexer.Token);
            }
        }

        private void ParseStatement()
        {
            if (_sym == LexicalAnalyzer.Ident)
            {
                NextSym();

                if (_sym == LexicalAnalyzer.Assign)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(51, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Intc, LexicalAnalyzer.Floatc,
                        LexicalAnalyzer.Ident, LexicalAnalyzer.Leftpar,
                        LexicalAnalyzer.Semicolon, LexicalAnalyzer.Endsy);
                }

                ParseExpression();
            }
            else if (_sym == LexicalAnalyzer.Beginsy)
            {
                ParseCompoundStatement();
            }
            else
            {
                if (_sym != LexicalAnalyzer.Endsy && _sym !=
                    LexicalAnalyzer.Semicolon)
                {
                    _io.Error(99, _lexer.Token);
                    SkipTo(LexicalAnalyzer.Semicolon, LexicalAnalyzer.Endsy);
                }
            }
        }

        private void ParseExpression()
        {
            ParseTerm();
            while (_sym == LexicalAnalyzer.Plus ||
                    _sym == LexicalAnalyzer.Minus ||
                    _sym == LexicalAnalyzer.Orsy)
            {
                NextSym();
                ParseTerm();
            }
        }

        private void ParseTerm()
        {
            ParseFactor();
            while (_sym == LexicalAnalyzer.Star ||
                    _sym == LexicalAnalyzer.Slash || 
                    _sym == LexicalAnalyzer.Divsy ||
                    _sym == LexicalAnalyzer.Modsy ||
                    _sym == LexicalAnalyzer.Andsy)
            {
                NextSym();
                ParseFactor();
            }
        }

        private void ParseFactor()
        {
            if (_sym == LexicalAnalyzer.Ident ||
                _sym == LexicalAnalyzer.Intc || 
                _sym == LexicalAnalyzer.Floatc)
            {
                NextSym();
            }
            else if (_sym == LexicalAnalyzer.Notsy)
            {
                NextSym();
                ParseFactor();
            }
            else if (_sym == LexicalAnalyzer.Leftpar)
            {
                NextSym();
                ParseExpression();
                if (_sym == LexicalAnalyzer.Rightpar)
                {
                    NextSym();
                }
                else
                {
                    _io.Error(250, _lexer.Token);
                }
            }
            else
            {
                _io.Error(98, _lexer.Token);
                SkipTo(LexicalAnalyzer.Plus, LexicalAnalyzer.Minus,
                        LexicalAnalyzer.Star, LexicalAnalyzer.Slash, 
                        LexicalAnalyzer.Semicolon, LexicalAnalyzer.Endsy,
                        LexicalAnalyzer.Rightpar);
            }
        }

        private void SkipTo(params byte[] syncTokens)
        {
            var set = new HashSet<byte>(syncTokens);
            while (_sym != 0 && !set.Contains(_sym))
            {
                NextSym();
            }
        }
    }
}