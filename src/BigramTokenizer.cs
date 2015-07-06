// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The standard bigram tokenizer that converting text into a sequence of tokens.
    /// </summary>
    public sealed class BigramTokenizer : ITokenizer
    {
        public BigramTokenizer() { }

        /// <summary>
        /// Gets or sets the output token text whether keep a original.
        /// </summary>
        public bool OptionOutputOriginalCase
        {
            get;
            set;
        }

        public IEnumerable<Token> Traverse(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                yield break;
            }
            using (var reader = new RewindStringReader(text, this.OptionOutputOriginalCase))
            {
                var breaker = new BigramTokenBreaker(reader);
                var token = breaker.Next();
                do
                {
                    yield return token;
                } while ((token = breaker.Next()) != null);
            }
        }

        private class BigramTokenBreaker : WhiteSpaceTokenBreaker
        {
            private RewindStringReader _reader;
            private bool _beginState = true;

            public BigramTokenBreaker(RewindStringReader reader)
                : base(reader)
            {            
                _reader = reader;
            }

            public override Token Next()
            {
                var code = _reader.Read();
                if (code.IsNull())
                {
                    return null;
                }
                if (code.IsLetterCase() || code.IsNumeralCase())
                {
                    _reader.Seek(_reader.Position - 1);
                    return base.Next();
                }
                else if (code.IsCjkCase())
                {
                    var nextCode = _reader.Read();
                    if (nextCode.IsNull())
                    {
                        if (_beginState)
                        {
                            return new Token(code.ToString(), TokenType.CJK);
                        }
                        return null;
                    }
                    if (nextCode.IsCjkCase())
                    {
                        _beginState = false;                        
                        if (_reader.Peek().IsCjkCase())
                        {
                            _reader.Seek(_reader.Position - 1);
                        }                       
                        return new Token(new string(new char[] { code, nextCode }), TokenType.CJK);
                    }
                    //may be code is a one of letter&numeral&punc.
                    _reader.Seek(_reader.Position - 2);
                    return base.Next();
                }
                _beginState = true;
                return new Token(code.ToString(), TokenType.PUNC);
            }
        }
    }
}
