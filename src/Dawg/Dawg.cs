// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Dawg
    {
        internal const float FILEVERSION = 1.0f;

        private DawgNode _root;

        public Dawg(DawgNode dawgTree)
        {
            _root = dawgTree;
        }

        public DawgNode Root
        {
            get
            {
                return _root;
            }
        }       

        public bool Contains(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }

            var nextNode = _root.Next(word[0]);
            for (var i = 1; nextNode != null && i < word.Length; i++)
            {
                nextNode = nextNode.Next(word[i]);
            }
            return nextNode != null && nextNode.Eow;
        }

        public IEnumerable<KeyValuePair<string, int>> MatchsPrefix(string prefix)
        {
            if (prefix == null)
            {
                prefix = string.Empty;
            }
            var nextNode = _root;
            for (var i = 0; nextNode != null && i < prefix.Length; i++)
            {
                nextNode = nextNode.Next(prefix[i]);
            }
            if (nextNode == null)
            {
                yield break;
            }
            foreach (var matchWord in IterateNodesString(prefix, nextNode))
            {
                yield return matchWord;
            }
        }

        private static IEnumerable<KeyValuePair<string,int>> IterateNodesString(string commonPrefix, DawgNode node)
        {
            if (node == null) yield break;
            foreach (var node2 in node.ChildNodes)
            {
                var nextCommonPrefix = commonPrefix + node2.Char;
                if (node2.Eow)
                {
                    yield return new KeyValuePair<string, int>(nextCommonPrefix, node2.Frequency);
                }
                foreach(var matchWord in IterateNodesString(nextCommonPrefix,node2))
                {
                    yield return matchWord;
                }
            }
        }
    }
}
