// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The tokenizer that used a whitespace to split text into tokens.
    /// </summary>
    internal class WhiteSpaceTokenBreaker : ITokenBreaker
    {
        private static readonly IDictionary<char, bool> AlphaNumStops = new Dictionary<char, bool>()
        {
            {'#',false},{'+',true},{'-',true},{'_',true}
        };

        private static readonly IDictionary<char, bool> NumStops = new Dictionary<char, bool>()
        {
            {'.',true}
        };

        private RewindStringReader _reader;

        private WhiteSpaceTokenBreaker() { }

        public WhiteSpaceTokenBreaker(RewindStringReader stringReader)
        {
            _reader = stringReader;
        }

        public virtual Token Next()
        {
            var offset = _reader.Position;
            var code = _reader.Read();
            if (code.IsNull())
            {
                return null;
            }           
            if (code.IsCjkCase())
            {
                return new Token(char.ToString(code), TokenType.CJK);
            }
            if (code.IsLetterCase())
            {
                while (!(code = _reader.Read()).IsNull())
                {
                    if (code.IsLetterCase() || code.IsNumeralCase())
                    {
                        continue;
                    }
                    var period = false;
                    if (AlphaNumStops.TryGetValue(code, out period))
                    {
                        if (period)
                        {
                            var nextCode = _reader.Peek();
                            if (nextCode.IsLetterCase() || code.IsNumeralCase() || AlphaNumStops.ContainsKey(nextCode))
                            {
                                continue;
                            }
                        }
                        break;
                    }
                    _reader.Seek(_reader.Position - 1);
                    break;
                }
                var length = _reader.Position - offset;
                _reader.Seek(offset);
                return new Token(new string(_reader.Read(length)), TokenType.ALPHANUM);
            }
            else if (code.IsNumeralCase())
            {
                var mixed = false;
                while (!(code = _reader.Read()).IsNull())
                {
                    if (code.IsNumeralCase() || (code.IsLetterCase() && (mixed = true)))
                    {
                        continue;
                    }
                    var period = false;
                    if (NumStops.TryGetValue(code, out period) && period)
                    {
                        var nextCode = _reader.Peek();
                        if (nextCode.IsNumeralCase())
                        {
                            continue;
                        }
                    }
                    _reader.Seek(_reader.Position - 1);
                    break;
                }
                var length = _reader.Position - offset;
                _reader.Seek(offset);
                return new Token(new string(_reader.Read(length)), mixed ? TokenType.ALPHANUM : TokenType.NUM);
            }
            return new Token(char.ToString(code), TokenType.PUNC);
        }
    }
}
