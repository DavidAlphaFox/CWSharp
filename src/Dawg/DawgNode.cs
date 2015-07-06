// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;

    internal class DawgNode
    {        
        private IDictionary<char, DawgNode> _childs;

        public DawgNode()
            : this(new char(), 0)
        {
        }

        public DawgNode(char text)
            : this(text, 0)
        {
        }

        public DawgNode(char text, int nodeCount)
        {
            this.Char = text;
            _childs = new Dictionary<char, DawgNode>(nodeCount);
        }

        public char Char
        {
            get;
            set;
        }

        public int Depth
        {
            get;
            set;
        }

        public int Frequency
        {
            get;
            set;
        }

        public bool Eow
        {
            get;
            set;
        }

        internal DawgNode Parent
        {
            get;
            private set;
        }

        public ICollection<DawgNode> ChildNodes
        {
            get
            {
                return _childs.Values;
            }
        }

        public bool HasChildNodes
        {
            get
            {
                return _childs.Count > 0;
            }
        }

        public DawgNode Next(char text)
        {
            DawgNode foundNode = null;
            if (_childs.TryGetValue(text, out foundNode))
            {
                return foundNode;
            }
            return null;
        }

        public bool AddChild(DawgNode node)
        {
            if (_childs.ContainsKey(node.Char))
            {
                return false;
            }
            if (node.Parent == null)
            {
                node.Parent = this;
            }
            _childs.Add(node.Char, node);
            return true;
        }

        public bool RemoveChild(DawgNode node)
        {
            if (!_childs.ContainsKey(node.Char))
            {
                return false;
            }
            _childs.Remove(node.Char);
            return true;
        }

        public IEnumerable<DawgNode> Descendants()
        {
            foreach (var node in _childs.Values)
            {
                yield return node;
                foreach (var node2 in node.Descendants())
                {
                    yield return node2;
                }
            }
        }

        public IEnumerable<DawgNode> DescendantsAndSelf()
        {
            yield return this;
            foreach (var node in this.Descendants())
            {
                yield return node;
            }
        }

        public override string ToString()
        {
            return this.Char + (this.Eow ? "[EOW]" : "");
        }
    }
}
