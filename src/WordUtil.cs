// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    public sealed class WordUtil
    {        
        private IDictionary<string, int> _wordBag;

        public WordUtil()
        {
            _wordBag = new Dictionary<string, int>();
        }

        public int Count
        {
            get
            {
                return _wordBag.Count;
            }
        }

        public ICollection<KeyValuePair<string,int>> WordSet
        {
            get
            {
                return new ReadOnlyCollection<KeyValuePair<string,int>>(_wordBag.ToList());
            }
        }

        public void Remove(string word)
        {
            _wordBag.Remove(word);
        }

        public void Add(string word, int frequency = 0)
        {
            if (string.IsNullOrEmpty(word)) return;
            word = word.ToLower();
            this.AddInternal(word, frequency);
        }

        public bool Contains(string word)
        {
            return _wordBag.ContainsKey(word);
        }

        public void SaveTo(string file)
        {
            using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                this.SaveTo(fs);
            }
        }

        public void SaveTo(Stream output)
        {
            var builder = new DawgBuilder();
            var dawg = builder.Build(_wordBag);
            var encoder = new DawgEncoder(Dawg.FILEVERSION);
            encoder.Encode(output, dawg);
        }

        private void AddInternal(string word, int frequency)
        {
            _wordBag[word] = frequency;
        }

        public static WordUtil LoadFrom(Stream input)
        {
            var decoder = new DawgDecoder(Dawg.FILEVERSION);
            var dawg = decoder.Decode(input);
            var wordUtil = new WordUtil();

            foreach (var pair in dawg.MatchsPrefix(null))
            {                                
                wordUtil.AddInternal(pair.Key, pair.Value);
            }

            return wordUtil;
        }

        public static WordUtil LoadFrom(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("The file not found.", file);
            }
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                return LoadFrom(fs);
            }
        }
    }
}
