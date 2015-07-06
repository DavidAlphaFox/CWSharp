// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Lucene.Net.Analysis.CWSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Yamool.CWSharp;

    public static class TokenizerExtensions
    {
        public static IEnumerable<Token> Traverse(this ITokenizer tokenizer,TextReader reader)
        {
            using (reader)
            {
                foreach (var token in tokenizer.Traverse(reader.ReadToEnd()))
                {
                    yield return token;
                }
            }
        }
    }
}
