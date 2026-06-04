using System.Collections.Generic;

namespace Компилятор
{
    public class Keywords
    {
        private readonly Dictionary<byte, Dictionary<string, byte>> _kw;

        public Dictionary<byte, Dictionary<string, byte>> Kw => _kw;

        public Keywords()
        {
            _kw = new Dictionary<byte, Dictionary<string, byte>>();
            Dictionary<string, byte> tmp = new Dictionary<string, byte>();
            tmp["do"] = LexicalAnalyzer.Dosy;
            tmp["if"] = LexicalAnalyzer.Ifsy;
            tmp["in"] = LexicalAnalyzer.Insy;
            tmp["of"] = LexicalAnalyzer.Ofsy;
            tmp["or"] = LexicalAnalyzer.Orsy;
            tmp["to"] = LexicalAnalyzer.Tosy;
            _kw[2] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["end"] = LexicalAnalyzer.Endsy;
            tmp["var"] = LexicalAnalyzer.Varsy;
            tmp["div"] = LexicalAnalyzer.Divsy;
            tmp["and"] = LexicalAnalyzer.Andsy;
            tmp["not"] = LexicalAnalyzer.Notsy;
            tmp["for"] = LexicalAnalyzer.Forsy;
            tmp["mod"] = LexicalAnalyzer.Modsy;
            tmp["nil"] = LexicalAnalyzer.Nilsy;
            tmp["set"] = LexicalAnalyzer.Setsy;
            _kw[3] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["then"] = LexicalAnalyzer.Thensy;
            tmp["else"] = LexicalAnalyzer.Elsesy;
            tmp["case"] = LexicalAnalyzer.Casesy;
            tmp["file"] = LexicalAnalyzer.Filesy;
            tmp["goto"] = LexicalAnalyzer.Gotosy;
            tmp["type"] = LexicalAnalyzer.Typesy;
            tmp["with"] = LexicalAnalyzer.Withsy;
            _kw[4] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["begin"] = LexicalAnalyzer.Beginsy;
            tmp["while"] = LexicalAnalyzer.Whilesy;
            tmp["array"] = LexicalAnalyzer.Arraysy;
            tmp["const"] = LexicalAnalyzer.Constsy;
            tmp["label"] = LexicalAnalyzer.Labelsy;
            tmp["until"] = LexicalAnalyzer.Untilsy;
            _kw[5] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["downto"] = LexicalAnalyzer.Downtosy;
            tmp["packed"] = LexicalAnalyzer.Packedsy;
            tmp["record"] = LexicalAnalyzer.Recordsy;
            tmp["repeat"] = LexicalAnalyzer.Repeatsy;
            _kw[6] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["program"] = LexicalAnalyzer.Programsy;
            _kw[7] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["function"] = LexicalAnalyzer.Functionsy;
            _kw[8] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["procedure"] = LexicalAnalyzer.Procedurensy;
            _kw[9] = tmp;
        }
    }
}