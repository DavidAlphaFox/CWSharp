// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal class DawgEncoder
    {
        private float _version;

        private DawgEncoder() { }

        public DawgEncoder(float version)
        {
            _version = version;
        }

        public void Encode(Stream output, Dawg dawg)
        {
            //make a label for the each node.
            var count = 0;
            var nodeLabels = new Dictionary<DawgNode, int>();
            foreach (var node in dawg.Root.Descendants())
            {
                if (nodeLabels.ContainsKey(node))
                {
                    continue;
                }
                nodeLabels[node] = count++;
            }
            var count2 = 0;
            var writer = new BinaryWriter(output, Encoding.UTF8);
            //the header of dawg.
            writer.Write(_version);
            writer.Write(count);
            foreach (var pair in nodeLabels)
            {
                var node = pair.Key;
                writer.Write((ushort)node.Char);
                writer.Write(node.Frequency);
                writer.Write(node.Depth);
                writer.Write(node.Eow);
                writer.Write(node.ChildNodes.Count);
                if (node.HasChildNodes)
                {
                    count2++;
                }
            }
            //the top node of dawg.
            writer.Write(dawg.Root.ChildNodes.Count);
            foreach (var node in dawg.Root.ChildNodes)
            {
                writer.Write(nodeLabels[node]);
            }
            //the child node of dawg.
            writer.Write(count2);
            foreach (var pair in nodeLabels)
            {
                var node = pair.Key;
                if (!node.HasChildNodes)
                {
                    continue;
                }
                writer.Write(pair.Value);
                writer.Write(node.ChildNodes.Count);
                foreach (var node2 in node.ChildNodes)
                {
                    writer.Write(nodeLabels[node2]);
                }
            }
        }
    }

    internal class VersionConflictException : Exception
    {
        public VersionConflictException(string msg) : base(msg)
        {
        }
    }

    internal class DawgDecoder
    {
        private float _version;

        private DawgDecoder() { }

        public DawgDecoder(float version)
        {
            _version = version;
        }

        public Dawg Decode(Stream stream)
        {            
            
            using (var reader = new BinaryReader(stream))
            {
                var fileVersion = reader.ReadSingle();
                if (_version != fileVersion)
                {
                    throw new VersionConflictException(string.Format("The file version of dawg is not match.\rThe decoder version is {0},but file version is {1}", _version, fileVersion));
                }
                var allNodes = new DawgNode[reader.ReadInt32()];
                //read header of dawg file
                for (var i = 0; i < allNodes.Length; i++)
                {
                    var text = (char)reader.ReadInt16();
                    var freq = reader.ReadInt32();
                    var depth = reader.ReadInt32();
                    var eow = reader.ReadBoolean();                   
                    var size = reader.ReadInt32();
                    var node = new DawgNode(text, size);
                    node.Eow = eow;
                    node.Depth = depth;
                    node.Frequency = freq;
                    allNodes[i] = node;
                }                
                //build a dawg.
                
                var count = reader.ReadInt32();
                var root = new DawgNode(new char(), count);
                for (var i = 0; i < count; i++)
                {
                    var node = allNodes[reader.ReadInt32()];
                    root.AddChild(node);
                }
                count = reader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var label = reader.ReadInt32();
                    var node = allNodes[label];
                    var childNodeCount = reader.ReadInt32();
                    for (var j = 0; j < childNodeCount; j++)
                    {
                        var childNode = allNodes[reader.ReadInt32()];
                        node.AddChild(childNode);
                    }
                }
                return new Dawg(root);
            }            
        }
    }
}
