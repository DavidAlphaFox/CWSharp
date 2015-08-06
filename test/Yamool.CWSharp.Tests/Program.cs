namespace Yamool.CWSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Text;

    static class Program
    {
        static void Main(string[] args)
        {

            var dawgFile = args.Length > 0 ? args[0] : "https://raw.githubusercontent.com/yamool/CWSharp/master/data/cwsharp.dawg";
            //BuildDawgFile(dawgFile);           
            var tokenizer = new StandardTokenizer(dawgFile);
            foreach (var token in tokenizer.Traverse("研究生命起源"))
            {
                Console.Write(token.Text + "/" + token.Type);
                Console.Write(" ");
            }
            Console.ReadLine();
        }


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
    }
}
