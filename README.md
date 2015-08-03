CWSharp
===
.Net中文分词组件，支持中英文、符号或者混合词组(比如：T恤)

##### 特性
- 默认支持多种分词器
	- StandardTokenizer - 默认分词，基于词典，支持中英文
	- BigramTokenizer - 二元分词，支持英文
	- StopwordTokenizer - 自定义过滤词分词，扩展类
	- UnigramTokenizer - 一元分词
- 可扩展的自定义分词接口
- 支持自定义词典
- 支持Lucene.Net
- 支持.NET 3.5或4.0+
- MIT授权协议

##### 安装&编译
- NuGet
```
nuget install CWSharp 
```
- Package Manager Console
```
PM> install-package CWSharp
```
- 编译
```
build.cmd [.NET版本号] Release //v4.5,v4.0,v3.5
```

##### 扩展
- [词典](https://github.com/yamool/CWSharp/tree/master/dict) - 介绍关于DAWG词典文件格式、生成以及如何添加新的词汇
- [Lucene.Net.CWSharp](https://github.com/yamool/CWSharp/tree/master/contrib/LuceneNet) - Lucene.Net的分词插件，支持搜索高亮显示

##### 算法
- 基于正向最大匹配的算法。[MMSEG算法](http://technology.chtsai.org/mmseg/)
- 词典基于DAWG结构，比传统的前缀树占用更少的内存空间。[DAWG算法](https://en.wikipedia.org/wiki/Deterministic_acyclic_finite_state_automaton)

##### 示例
- **StandardTokenizer**
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

- **BigramTokenizer**
```c#
var tokenizer = new BigramTokenizer();
foreach (var token in tokenizer.Traverse("微软宣布它爱Linux"))
{
	Console.Write(token.Text + "/" + token.Type);
}
```
> 微软/CJK 软宣/CJK 宣布/CJK 布它/CJK 它爱/CJK linux/ALPHANUM

- **StopwordTokenizer**
```c#
var tokenizer = new StopwordTokenizer(
	new StandardTokenizer(dawgFile),
	new string[] { "它", "a", "the", "an" });
foreach (var token in tokenizer.Traverse("微软宣布它爱Linux"))
{
	Console.Write(token.Text + "/" + token.Type);
	Console.Write(" ");
}
```
> 微软/CJK 宣布/CJK 爱/CJK linux/ALPHANUM

- **自定义分词接口(实现一元分词)**
```c#
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
