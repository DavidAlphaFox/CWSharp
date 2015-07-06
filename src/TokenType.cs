// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    public class TokenType : IComparable, IComparable<TokenType>, IEquatable<TokenType>
    {
        public static readonly TokenType PUNC = new TokenType("PUNC");
        public static readonly TokenType ALPHANUM = new TokenType("ALPHANUM");
        public static readonly TokenType NUM = new TokenType("NUM");
        public static readonly TokenType CJK = new TokenType("CJK");

        private readonly string _value;

        private TokenType(string value)
        {
            _value = value;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj is string)
            {
                return this.CompareTo((string)obj);
            }
            return -1;
        }

        public int CompareTo(TokenType other)
        {
            return _value.CompareTo(other._value);
        }

        public bool Equals(TokenType other)
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            if (obj is TokenType)
            {
                return this.Equals(((TokenType)obj));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public static bool operator !=(TokenType x, TokenType y)
        {
            return !(x == y);
        }

        public static bool operator ==(TokenType x, TokenType y)
        {
            return x.Equals(y);
        }

        public static implicit operator TokenType(string value)
        {
            return new TokenType(value);
        }

        public static explicit operator string(TokenType value)
        {
            return value._value;
        }        
    }
}
