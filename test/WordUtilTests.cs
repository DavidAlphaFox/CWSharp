namespace Yamool.CWSharp.Tests
{
    using System;    
    using System.IO;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public  class WordUtilTests
    {
        [Test]
        public void SimpleTest()
        {
            var wordUtil = new WordUtil();
            var words = new string[] { "tap", "taps", "top", "tops" };
            var expectWordCount = words.Length;

            wordUtil.AddWords(words);
            Assert.AreEqual(expectWordCount, wordUtil.Count);
            var ms = new MemoryStream();

            wordUtil.SaveTo(ms);
            Assert.Greater(ms.Length, 0);

            ms.Position = 0;
            wordUtil = WordUtil.LoadFrom(ms);
            Assert.AreEqual(expectWordCount, wordUtil.Count);            
        }

        [TestCase(@"d:\1.txt")]
        public void TestFromTxtFile(string file)
        {
            var wordUtil = new WordUtil();
            var expectWordCount = 0;
            using (var sr = new StreamReader(file, Encoding.UTF8))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == string.Empty) continue;
                    wordUtil.Add(line);
                    expectWordCount++;
                }
            }
            
            var watcher = new System.Diagnostics.Stopwatch();
            watcher.Start();
            var ms = new MemoryStream();
            wordUtil.SaveTo(ms);
            watcher.Stop();

            Console.WriteLine("build dawg elapsed time:" + watcher.Elapsed.TotalMilliseconds + "'ms");

            watcher.Reset();
            watcher.Start();
            ms.Position = 0;
            wordUtil = WordUtil.LoadFrom(ms);
            watcher.Stop();
            Console.WriteLine("load dawg file elapsed time:" + watcher.Elapsed.TotalMilliseconds + "'ms");
            Assert.AreEqual(expectWordCount, wordUtil.Count);
        }
    }
}
