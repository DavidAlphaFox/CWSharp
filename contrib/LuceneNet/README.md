Lucene.Net分词插件
=====
####安装
```
Nuget install Lucene.Net.Analysis.CWSharp
Install-package Lucene.Net.Analysis.CWSharp
```

```c#
 var textSet = new string[]{
	@"继Google App Engine for PHP在两个星期前去除了其'beta' 标签之后，Google App Engine for Go 在7月8日也以同样的方式，官方移除了'beta' 标签，扩展了 App Engine 的服务水平协议（ Service Level Agreement , SLA）。这意味着，Google App Engine for Go现在是一个通用的可用性产品，可为各种应用提供了可靠的服务。",
	@"GitHub如何征服了Google、微软及一切"
};
var dir = new RAMDirectory();
//初始化CWSharp
var tokenizer = new StandardTokenizer(@"dict.dawg"); 
var analyzer=new CwsAnalyzer(tokenizer);
//构建测试索引文件
var writer = new IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.LIMITED);
foreach (var text in textSet)
{
	var newDoc = new Document();
	newDoc.Add(new Field("text", text, Field.Store.YES, Field.Index.ANALYZED_NO_NORMS));
	writer.AddDocument(newDoc);
}
writer.Dispose();          
//搜索
var searcher = new IndexSearcher(dir, true);
var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "text", analyzer);
var query = parser.Parse("微软 google");
var hits = searcher.Search(query, 10);
//高亮显示
var highlighter = new Highlighter(new QueryScorer(query));
for (var i = 0; i < hits.TotalHits; i++)
{
	var doc = searcher.Doc(hits.ScoreDocs[i].Doc);
	var text = doc.GetField("text").StringValue;
	var tokenStream = analyzer.TokenStream("text", new StringReader(text));
	var result = highlighter.GetBestFragments(tokenStream, text, 3, "...");
	Console.WriteLine(result);
}
```
> GitHub如何征服了Google、<B>微软</B>及一切
> 继<B>Google</B> App Engine for PHP在两个星期前去除了其'beta' 标签之后.....