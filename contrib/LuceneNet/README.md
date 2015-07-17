Lucene.Net分词插件
=====
#### 安装
>Nuget install Lucene.Net.Analysis.CWSharp

> Install-package Lucene.Net.Analysis.CWSharp

####示例

```c#
 var textSet = new string[]{
	@"Google发布App Engine的Go语言通用版",
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
var query = parser.Parse("微软");
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