// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;

    internal partial class RewindStringReader : IDisposable
    {
        private static readonly char EOF = new char();
        private string _s;
        private int _cursor;
        private int _length;
        private bool _ignoreCase;

        private RewindStringReader() { }

        public RewindStringReader(string s, bool ignoreCase)
        {
            _s = s;
            _length = s == null ? 0 : s.Length;
            _cursor = 0;
            _ignoreCase = ignoreCase;
        }

        public int Position
        {
            get
            {
                return _cursor;
            }
        }

        public char Read()
        {
            if (_cursor >= _length)
            {
                return EOF;
            }
            var ch = _s[_cursor++];
            if (!_ignoreCase)
            {
                return this.Normalize(ch);
            }
            return ch;
        }

        public char[] Read(int count)
        {
            if (_cursor + count > _length)
            {
                throw new IndexOutOfRangeException("Offset and length were out of bounds of string.");
            }
            var array = new char[count];
            for (var i = 0; i < count; i++)
            {
                array[i] = this.Read();
            }
            return array;
        }

        public char Peek()
        {
            if (_cursor >= _length)
            {
                return EOF;
            }
            var ch = _s[_cursor];

            return _ignoreCase ? ch : this.Normalize(ch);
        }        

        public void Seek(int offset)
        {
            _cursor = offset;
        }        

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private char Normalize(char ch)
        {
            if (ch.IsUpperCase())
            {
                //convert a upper letter to lower.
                ch = (char)((int)ch + 32);
            }
            else if (ch >= 0xff01 && ch <= 0xff5d)
            {
                //convert a fullwidth char to halfwidth.
                ch = (char)((int)ch - 0xFEE0);
            }
            return ch;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _s = null;
                _length = 0;
                _cursor = 0;
                _ignoreCase = false;
            }
        }
    }
}
