CWSharp
===
.Net中文分词组件，支持中英文、符号或者混合词组(比如：T恤,卡拉OK等)以及自定义词典。

##### 特性
- 默认支持多种分词器
	- StandardTokenizer - 默认分词，基于词典
	- BigramTokenizer - 二元分词，支持英文，数字识别
	- StopwordTokenizer - 自定义过滤词，扩展类
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
build.cmd [v4.5|v4.0|v3.5] Release
```

##### 扩展
- [自定义词典](https://github.com/yamool/CWSharp/tree/master/dict) - 介绍关于DAWG词典文件格式、生成以及如何添加新的词汇
- [Lucene.Net.CWSharp](https://github.com/yamool/CWSharp/tree/master/contrib/LuceneNet) - Lucene.Net的分词插件，支持搜索高亮显示

##### 说明
- 基于正向最大匹配的算法。[MMSEG算法](http://technology.chtsai.org/mmseg/)
- 词典基于DAWG结构，比传统的前缀树占用更少的内存空间。[DAWG算法](https://en.wikipedia.org/wiki/Deterministic_acyclic_finite_state_automaton)

##### 示例

```c#
var tokenizer = new StopwordTokenizer(new StandardTokenizer("dict.dawg"), new string[] { "的" });
foreach (var token in tokenizer.Traverse("你是我的小苹果"))
{
	Console.Write(token.Text + "/" + token.Type);
}
```

- **StandardTokenizer**
```
研究生命起源 >> 研究/CJK 生命/CJK 起源/CJK
长春市长春药店 >> 长春市/CJK 长春/CJK 药店/CJK
神秘的组织-北京朝阳群众 >> 神秘/CJK 的/CJK 组织/CJK -/PUNC 北京/CJK 朝阳/CJK 群众/CJK
一次性交一百元 >> 一次/CJK 性交/CJK 一/CJK 百/CJK 元/CJK (歧义词)
```

- **BigramTokenizer**
```
研究生命起源 >> 研究/CJK 究生/CJK 生命/CJK 命起/CJK 起源/CJK
长春市长春药店 >> 长春/CJK 春市/CJK 市长/CJK 长春/CJK 春药/CJK 药店/CJK
神秘的组织-北京朝阳群众 >> 神秘/CJK 秘的/CJK 的组/CJK 组织/CJK -/PUNC 北京/CJK 京朝/CJK 朝阳/CJK 阳群/CJK 群众/CJK
一次性交一百元 >> 一次/CJK 次性/CJK 性交/CJK 交一/CJK 一百/CJK 百元/CJK
```

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