// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Linq;

    internal struct Chunk
    {
        private WordPoint[] _wordPoints;
        private int _length;

        public Chunk(int length, WordPoint[] wordPoints)
            : this()
        {
            _length = length;
            _wordPoints = wordPoints;
        }

        public int Length
        {
            get
            {
                return _length;
            }
        }

        public int Count
        {
            get
            {
                return _wordPoints.Length;
            }
        }
       
        public WordPoint this[int index]
        {
            get
            {
                return _wordPoints[index];
            }
        }

        public double WordAverageLength()
        {
            return (double)_length / this.Count;
        }

        public double Variance()
        {
            var averageLength = this.WordAverageLength();
            return Math.Sqrt(_wordPoints.Sum(k => Math.Pow(k.Length - averageLength, 2)) / this.Count);
        }

        public double Degree()
        {
            return _wordPoints.Sum(k => Math.Log10(k.Frequency));
        }
    }
}
