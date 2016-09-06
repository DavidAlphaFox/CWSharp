// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Text;

    internal sealed class FnvHash
    {
        private const uint fnv_prime_32 = 16777619;
        private const uint fnv_offset_32 = 2166136261;

        internal static uint GetHash(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = fnv_offset_32;
            for (int i = 0; i < bytes.Length; i++)
            {
                hash = (hash ^ bytes[i]) * fnv_prime_32;
            }
            //http://bretm.home.comcast.net/hash/6.html
            hash += hash << 13;
            hash ^= hash >> 7;
            hash += hash << 3;
            hash ^= hash >> 17;
            hash += hash << 5;
            return hash;
        }
    }
}
