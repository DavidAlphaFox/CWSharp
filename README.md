CWSharp
===
.Net的中英文分词组件。

##### 特性
- 内嵌多种分词算法及可扩展的自定义分词接口
	- StandardTokenizer
	- BigramTokenizer
	- StopwordTokenizer
	- 自定义分词接口
- 支持自定义词典
- 支持Lucene.Net分词
- MIT授权协议

##### 安装
- NuGet
```
nuget install CWSharp 
```
- Package Manager Console
```
PM> install-package CWSharp
```

##### 算法
- 基于最大匹配的算法。[介绍](http://technology.chtsai.org/mmseg/)
- 词典使用DAWG结构，比传统的前缀树占用更少的内存空间。[介绍](https://en.wikipedia.org/wiki/Deterministic_acyclic_finite_state_automaton)

##### TODO
- HMM算法，识别未登记词语以及人名、地名识别
- 支持跨平台Windows、Linux

##### 示例
- StandardTokenizer
```c#
var dawgFile = @"dict.dawg";
var tokenizer = new StandardTokenizer(dawgFile)
{
	OptionOutputOriginalCase = true
};
foreach (var token in tokenizer.Traverse("微软宣布它爱Linux"))
{
	Console.Write(token.Text + "/" + token.Type);
}
```
> 微软/CJK 宣布/CJK 它/CJK 爱/CJK Linux/ALPHANUM

- BigramTokenizer
```c#
var tokenizer = new BigramTokenizer();
foreach (var token in tokenizer.Traverse("微软宣布它爱Linux"))
{
	Console.Write(token.Text + "/" + token.Type);
}
```
> 微软/CJK 软宣/CJK 宣布/CJK 布它/CJK 它爱/CJK linux/ALPHANUM

- StopwordTokenizer
```c#
var tokenizer = new StopwordTokenizer(
	new StandardTokenizer(dawgFile),
	new string[] { "它", "a", "the", "an" });//停用词：它,a,the,an
foreach (var token in tokenizer.Traverse("微软宣布它爱Linux"))
{
	Console.Write(token.Text + "/" + token.Type);
	Console.Write(" ");
}
```
> 微软/CJK 宣布/CJK 爱/CJK linux/ALPHANUM

- 自定义分词接口
```c#
//一元分词
public class CustomTokenizer : ITokenizer
{
	private ITokenizer _tokenizer;
	public CustomTokenizer(ITokenizer tokenizer)
	{
		_tokenizer = tokenizer;
	}
	public IEnumerable<Token> Traverse(string text)
	{
		foreach (var token in _tokenizer.Traverse(text))
		{
			if (token.Type == TokenType.CJK)
			{
				foreach (var ch in token.Text)
					yield return new Token(ch.ToString(), TokenType.CJK);
			}
			else
				yield return token;
		}
	}
}
```
> 微/CJK 软/CJK 宣/CJK 布/CJK 它/CJK 爱/CJK linux/ALPHANUM

##### FAQ
===
- [词典](https://github.com/yamool/cwsharp/dict) - 如何生成DAWG词典文件，如何添加新的词组到DAWG词典中。
- [lucene.net插件](https://github.com/yamool/cwsharp/contrib/LuceneNet) - Lucene.Net分词接口

