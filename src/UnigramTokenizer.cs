//----------------------------------------------------------------
// Copyright (c) Yamool Inc. All rights reserved.
//----------------------------------------------------------------

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The standard unigram tokenizer that converting text into a sequence of tokens.
    /// </summary>
    public sealed class UnigramTokenizer : ITokenizer
    {
        public UnigramTokenizer() { }

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
                var breaker = new UnigramTokenBreaker(reader);
                var token = breaker.Next();
                do
                {
                    yield return token;
                } while ((token = breaker.Next()) != null);
            }
        }

        private class UnigramTokenBreaker : WhiteSpaceTokenBreaker
        {
            private RewindStringReader _reader;            

            public UnigramTokenBreaker(RewindStringReader reader)
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
                    return new Token(code.ToString(), TokenType.CJK);
                }
                return new Token(code.ToString(), TokenType.PUNC);
            }
        }
    }
}
