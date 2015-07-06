// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class DawgBuilder
    {
        public DawgBuilder()
        {
        }

        public Dawg Build(IEnumerable<KeyValuePair<string,int>> wordBag)
        {
            var root = new DawgNode();
            var levelNodeCollections = new Dictionary<int, List<DawgNode>>();          
            foreach (var pair in wordBag)
            {
                var word = pair.Key;
                var nextNode = root;
                var level = 0;
                for (var i = 0; i < word.Length; i++, level++)
                {
                    var ch = word[i];
                    var currNode = nextNode.Next(ch);
                    if (currNode == null)
                    {
                        currNode = new DawgNode(ch) { Depth = level };
                        nextNode.AddChild(currNode);
                    }

                    //collection nodes with level
                    List<DawgNode> nodes = null;
                    if (!levelNodeCollections.TryGetValue(level, out nodes))
                    {
                        nodes = new List<DawgNode>();
                        levelNodeCollections[level] = nodes;
                    }
                    nodes.Add(currNode);
                    nextNode = currNode;
                }
                //make sure this node is EOW(end of word).
                nextNode.Eow = true;
                nextNode.Frequency = pair.Value;
            }
            //for fast to traverse all nodes,we should tracking a node branch which is been merged.
            var trackingNodes = new HashSet<DawgNode>();
            for (var j = levelNodeCollections.Count - 1; j >= 0; j--)
            {
                var nextNode = root;
                var uniqNodeTables = new Dictionary<int, DawgNode>();
                foreach (var node in levelNodeCollections[j])
                {
                    if (node.Eow || trackingNodes.Contains(node))
                    {
                        DawgNode foundNode = null;
                        var nodeId = GetDawgNodeId(node);
                        if (uniqNodeTables.TryGetValue(nodeId, out foundNode))
                        {
                            //merge two node that with has same value.
                            if (node != foundNode)
                            {
                                node.Parent.RemoveChild(node);
                                node.Parent.AddChild(foundNode);
                            }
                            foundNode.Eow |= node.Eow;
                            //tracking merge node status
                            trackingNodes.Add(node.Parent);
                            trackingNodes.Add(foundNode.Parent);
                        }
                        else
                        {
                            uniqNodeTables[nodeId] = node;
                        }
                    }
                }
            }            
            var dawg = new Dawg(root);
            return dawg;
        }

        private static int GetDawgNodeId(DawgNode node)
        {
            var sb = new StringBuilder();
            foreach (var node2 in node.DescendantsAndSelf())
            {
                sb.Append(node2.Char);
                sb.Append(node2.Eow ? '1' : '0');
            }
            return (int)FnvHash.GetHash(sb.ToString());
        }
    }
}
