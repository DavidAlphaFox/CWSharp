// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;

    internal struct WordPoint
    {
        private int _offset;
        private int _length;
        private int _freq;

        public WordPoint(int offset, int length,int freq)
        {
            _offset = offset;
            _length = length;
            _freq = freq;
        }

        public int Offset
        {
            get
            {
                return _offset;
            }
        }

        public int Length
        {
            get
            {
                return _length;
            }
        }

        public int Frequency
        {
            get
            {
                return _freq;
            }
        }
    }
}
