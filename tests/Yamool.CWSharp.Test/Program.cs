namespace Yamool.CWSharp.Tests
{
    using System;
    using System.IO;

    static class Program
    {
        static void Main(string[] args)
        {
            var dawgFile = args[0];
            Console.WriteLine("reading draw file: " + dawgFile);

            using (var stream = new FileStream(dawgFile, FileMode.Open, FileAccess.Read))
            {
                var tokenizer = new StandardTokenizer(stream);
                foreach (var token in tokenizer.Traverse("研究生命起源"))
                {
                    Console.Write(token.Text + "/" + token.Type);
                    Console.Write(" ");
                }
            }
        }

        /**      
        static void BuildDawgFile(string file)
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\test\bin\Debug\", "");
            var wordUtil = new WordDict();
            //加载默认的词频
            using (var sr = new StreamReader(rootPath + @"\dict\cwsharp.freq", Encoding.UTF8))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == string.Empty) continue;
                    var array = line.Split(' ');
                    wordUtil.Add(array[0], int.Parse(array[1]));
                }            
            }
            //加载新的词典
            using (var sr = new StreamReader(rootPath + @"\dict\cwsharp.dic", Encoding.UTF8))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == string.Empty) continue;
                    wordUtil.Add(line);
                }
            }
            //保存新的dawg文件
            wordUtil.SaveTo(file);
        }
        **/
    }
}
