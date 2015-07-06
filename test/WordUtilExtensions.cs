namespace Yamool.CWSharp.Tests
{
    using System;
    using System.Collections.Generic;

    public static class WordUtilExtensions
    {
        public static void AddWords(this WordUtil wordUtil, params string[] words)
        {
            foreach (var word in words)
            {
                wordUtil.Add(word);
            }
        }
    }
}
