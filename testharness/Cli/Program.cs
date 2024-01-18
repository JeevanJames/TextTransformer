using Jeevan.TextTransformers;

Transformer.RegisterTransform<CaseTransform>();
Transformer.RegisterTransform<PrefixTransform>();
Transformer.RegisterTransform<SuffixTransform>();
Transformer.RegisterTransform<TrimTransform>();
Transformer.RegisterTransform<ReplaceTransform>();
Transformer.RegisterTransform<SplitTransform>();

IDictionary<string, string> results = Transformer.Transform(
    ["2023-04-12", "2009-06-14", "2015-11-18"],
    "split:-|0");
foreach ((string original, string transformed) in results)
{
    Console.WriteLine($"{original} ==> {transformed}");
}
