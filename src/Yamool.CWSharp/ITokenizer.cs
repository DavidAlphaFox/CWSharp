// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines methods to break a text into tokens.
    /// </summary>
    public interface ITokenizer
    {
        /// <summary>
        /// Gets a sequence of <see cref="Token"/>s.
        /// </summary>
        /// <param name="text">The text to breaking into a tokens.</param>
        /// <returns></returns>
        IEnumerable<Token> Traverse(string text);
    }
}
