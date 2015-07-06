// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// The standard tokenizer that converting text into a sequence of tokens.
    /// </summary>
    public class StandardTokenizer : ITokenizer
    {
        private Dawg _dawg;

        private StandardTokenizer()
        {            
        }
        
        /// <summary>
        /// Initialize a new instance of StandardTokenizer.
        /// </summary>
        /// <param name="file">The dawg format file.</param>
        public StandardTokenizer(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("The file of dawg does not exist.", file);
            }
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                var decoder = new DawgDecoder(Dawg.FILEVERSION);
                _dawg = decoder.Decode(fs);
            }
        }

        /// <summary>
        /// Gets or sets the output token text whether keep a original.
        /// Default values is false.
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
                var breaker = new MaximumMatchTokenBreaker(_dawg, reader);
                var token = breaker.Next();
                do
                {
                    yield return token;
                } while ((token = breaker.Next()) != null);
            }
        }
    }
}
