// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;

    public sealed class Token
    {
        private Token() { }

        public Token(string text, TokenType type)
        {
            this.Text = text;
            this.Type = type;
        }

        public string Text
        {
            get;
            private set;
        }

        public TokenType Type
        {
            get;
            private set;
        }

        public int Length
        {
            get
            {
                return this.Text.Length;
            }
        }

        public Token SetBuffer(string buffer)
        {
            this.Text = buffer;
            return this;
        }

        public Token SetBuffer(string buffer, int begin, int length)
        {
            return this.SetBuffer(buffer.ToCharArray(), begin, length);
        }

        public Token SetBuffer(char[] buffer, int begin, int length)
        {
            this.Text = new string(buffer, begin, length);
            return this;
        }

        public Token SetType(TokenType type)
        {
            this.Type = type;
            return this;
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", this.Text, this.Type);
        }
    }
}
