// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;

    internal static class CharExtensions
    {
        public static bool IsLetterCase(this char code)
        {
            return (code >= 0x41 && code <= 0x5A) || (code >= 0x61 && code <= 0x7A);
        }

        public static bool IsUpperCase(this char code)
        {
            return code >= 0x41 && code <= 0x5A;
        }

        public static bool IsLowercase(this char code)
        {
            return code >= 0x61 && code <= 0x7A;
        }

        public static bool IsNumeralCase(this char code)
        {
            return code >= 0x30 && code <= 0x39;
        }

        public static bool IsCjkCase(this char code)
        {
            return code >= 0x4e00 && code <= 0x9fa5;
        }

        public static bool IsNull(this char code)
        {
            return code == '\0';
        }
    }
}
