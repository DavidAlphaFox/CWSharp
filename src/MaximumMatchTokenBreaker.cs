// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;

    //http://technology.chtsai.org/mmseg/

    internal sealed class MaximumMatchTokenBreaker : WhiteSpaceTokenBreaker
    {
        private static readonly IChunkFilter[] Filters = new IChunkFilter[] { new LawlFilter(), new SvwlFilter() };
        private RewindStringReader _reader;
        private Dawg _dawg;

        public MaximumMatchTokenBreaker(Dawg dawg, RewindStringReader reader)
            : base(reader)
        {
            _reader = reader;
            _dawg = dawg;
        }

        public override Token Next()
        {
            var baseOffset = _reader.Position;
            var code = _reader.Peek();
            if (code.IsNull())
            {
                return null;
            }
            DawgNode node = _dawg.Root.Next(code);
            //check char type before 
            if (node == null || !node.HasChildNodes)
            {
                _reader.Seek(baseOffset);
                return base.Next();
            }
              
                var firstOfNodes = this.MatchedNodes(baseOffset);
                if (firstOfNodes.Count == 0)
                {
                    _reader.Seek(baseOffset);
                    return base.Next();
                }
                var maxLength = 0;
                var chunks = new List<Chunk>(3);

                for (var i = firstOfNodes.Count - 1; i >= 0; i--)
                {
                    var offset1 = baseOffset + firstOfNodes[i].Depth + 1;                    
                    var secondOfNodes = this.MatchedNodes(offset1);
                    if (secondOfNodes.Count > 0)
                    {
                        for (var j = secondOfNodes.Count - 1; j >= 0; j--)
                        {
                            var offset2 = offset1 + secondOfNodes[j].Depth + 1;
                            var thirdOfNodes = this.MatchedNodes(offset2);
                            if (thirdOfNodes.Count > 0)
                            {
                                for (var k = thirdOfNodes.Count - 1; k >= 0; k--)
                                {
                                    var offset3 = offset2 + thirdOfNodes[k].Depth + 1;
                                    var length = offset3 - baseOffset;
                                    //Rule 1: Maximum matching
                                    if (length >= maxLength)
                                    {
                                        maxLength = length;
                                        var chunk = new Chunk(length,
                                            new WordPoint[]{ new WordPoint(baseOffset, offset1 - baseOffset,firstOfNodes[i].Frequency),
                                            new WordPoint(offset1, offset2 - offset1,secondOfNodes[j].Frequency),
                                            new WordPoint(offset2, offset3 - offset2,thirdOfNodes[k].Frequency)});
                                        chunks.Add(chunk);
                                    }
                                }
                            }
                            else
                            {
                                var length = offset2 - baseOffset;
                                //Rule 1: Maximum matching
                                if (length >= maxLength)
                                {
                                    maxLength = length;
                                    var chunk = new Chunk(length, new WordPoint[] { 
                                    new WordPoint(baseOffset, offset1 - baseOffset,firstOfNodes[i].Frequency),
                                    new WordPoint(offset1, offset2 - offset1,secondOfNodes[j].Frequency) });
                                    chunks.Add(chunk);
                                }                                
                            }
                        }
                    }
                    else
                    {
                        var length = offset1 - baseOffset;
                        //Rule 1: Maximum matching
                        if (length >= maxLength)
                        {
                            maxLength = length;
                            var chunk = new Chunk(length, new WordPoint[] { new WordPoint(baseOffset, offset1 - baseOffset, firstOfNodes[i].Frequency) });
                            chunks.Add(chunk);
                        }                       
                    }
                }               
                if (chunks.Count > 1)
                {
                    var count = chunks.Count;
                    foreach (var filter in Filters)
                    {
                        if ((count = filter.Apply(chunks, count)) == 1)
                        {                            
                            break;
                        }
                    }
                }
                //seek and read and move to next point to start.  
                var bestChunk = chunks[0];  
                _reader.Seek(bestChunk[0].Offset);
                return new Token(new string(_reader.Read(bestChunk[0].Length)), TokenType.CJK);                           
        }

        private IList<DawgNode> MatchedNodes(int offset)
        {
            _reader.Seek(offset);
            var matchedNodes = new List<DawgNode>(1);
            var code = _reader.Read();
            var node = _dawg.Root;
            while (!code.IsNull())
            {
                node = node.Next(code);
                if (node == null)
                {
                    break;
                }
                if (node.Eow)
                {
                    matchedNodes.Add(node);
                }
                code = _reader.Read();
            }           
            return matchedNodes;
        }

        private interface IChunkFilter
        {
            int Apply(IList<Chunk> chunks, int count);
        }

        /// <summary>
        /// The largest average word length filter.
        /// </summary>
        private class LawlFilter : IChunkFilter
        {
            public LawlFilter() { }

            public int Apply(IList<Chunk> chunks, int count)
            {
                var maxLength = 0d;
                var nextCount = 0;
                for (var i = 0; i < count; i++)
                {
                    var chunk = chunks[i];
                    var averageLength = chunk.WordAverageLength();
                    if (averageLength > maxLength)
                    {
                        maxLength = averageLength;
                        nextCount = 0;
                        chunks[nextCount++] = chunk;
                    }
                    else if (averageLength == maxLength)
                    {
                        chunks[nextCount++] = chunk;
                    }
                }
                return nextCount;
            }
        }

        /// <summary>
        /// The Smallest variance of word lengths filter.
        /// </summary>
        private class SvwlFilter : IChunkFilter
        {
            public SvwlFilter() { }

            public int Apply(IList<Chunk> chunks, int count)
            {
                var minVariance = double.MaxValue;
                var nextCount = 0;
                for (var i = 0; i < count; i++)
                {
                    var chunk = chunks[i];
                    var variance = chunk.Variance();
                    if (variance < minVariance)
                    {
                        nextCount = 0;
                        minVariance = variance;
                        chunks[nextCount++] = chunk;
                    }
                    else if (variance == minVariance)
                    {
                        chunks[nextCount++] = chunk;
                    }
                }
                return nextCount;
            }
        }

        private class LsdmfocwFilter : IChunkFilter
        {
            public LsdmfocwFilter() { }

            public int Apply(IList<Chunk> chunks, int count)
            {
                var maxDegree = 0d;
                var nextCount = 0;
                for (var i = 0; i < count; i++)
                {
                    var chunk = chunks[i];
                    var degree = chunk.Degree();
                    if (degree > maxDegree)
                    {
                        nextCount = 0;
                        maxDegree = degree;
                        chunks[nextCount++] = chunk;
                    }
                    else if (degree == maxDegree)
                    {
                        chunks[nextCount++] = chunk;
                    }
                }
                return nextCount;
            }
        }
    }
}
