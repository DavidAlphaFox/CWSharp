// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The tokenizer that stop a specified word.
    /// </summary>
    public sealed class StopwordTokenizer : ITokenizer
    {
        private ITokenizer _instance;
        private ICollection<string> _stopwords;

        public StopwordTokenizer(ITokenizer instance, ICollection<string> stopwords)
        {
            _instance = instance;
            _stopwords = stopwords;
        }

        public bool OptionStopPuncs
        {
            get;
            set;
        }

        public IEnumerable<Token> Traverse(string text)
        {
            foreach (var token in _instance.Traverse(text))
            {
                if (_stopwords.Contains(token.Text) || (this.OptionStopPuncs && token.Type == TokenType.PUNC))
                {
                    continue;
                }
                yield return token;
            }
        }
    }
}
