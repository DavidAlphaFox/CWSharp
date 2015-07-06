// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Lucene.Net.Analysis.CWSharp
{
    using System;
    using System.IO;
    using Lucene.Net.Analysis;
    using Yamool.CWSharp;

    /// <summary>
    /// The CWSharp analyzer for Lucene.Net.
    /// </summary>
    public sealed class CwsAnalyzer : Analyzer
    {
        private ITokenizer _tokenizer;

        private CwsAnalyzer() { }

        public CwsAnalyzer(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }
        
        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            return new CwsTokenizer(_tokenizer.Traverse(reader).GetEnumerator());
        }

        public override TokenStream ReusableTokenStream(string fieldName, TextReader reader)
        {
            return base.ReusableTokenStream(fieldName, reader);
        }
    }
}
