using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Jeevan.TextTransformers;

/// <inheritdoc cref="AffixTransform" />
[Transform("suffix")]
[TransformParameter<string>(0, "suffix",
    Description = "Suffix to append to the string.")]
public sealed class SuffixTransform : AffixTransform, ITransform<SuffixTransform>
{
    public SuffixTransform(string suffix) : base(suffix)
    {
    }

    public override string Run(string input)
    {
        return input + Affix;
    }

    public static bool TryParseParameters(IReadOnlyList<string> parameters, [NotNullWhen(true)] out Transform? transform)
    {
        transform = new SuffixTransform(parameters[0]);
        return true;
    }
}

[Transform("trim")]
[TransformParameter<string>(0, "pattern",
    Description = "Regular expression pattern to match.")]
public sealed class TrimTransform : Transform, ITransform<TrimTransform>
{
    private readonly Regex _pattern;

    public TrimTransform(string pattern)
    {
        _pattern = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
    }

    public TrimTransform(Regex pattern)
    {
        _pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
    }

    public override string Run(string input)
    {
        return _pattern.Replace(input, string.Empty);
    }

    public static bool TryParseParameters(IReadOnlyList<string> parameters, [NotNullWhen(true)] out Transform? transform)
    {
        transform = new TrimTransform(parameters[0]);
        return true;
    }
}

[Transform("replace")]
public sealed class ReplaceTransform : Transform, ITransform<ReplaceTransform>
{
    private readonly Regex _pattern;
    private readonly string _replacement;

    public ReplaceTransform(Regex pattern, string replacement)
    {
        _pattern = pattern;
        _replacement = replacement;
    }

    public ReplaceTransform(string pattern, string replacement)
    {
        _pattern = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
        _replacement = replacement;
    }

    public override string Run(string input)
    {
        return _pattern.Replace(input, _replacement);
    }

    public static bool TryParseParameters(IReadOnlyList<string> parameters, [NotNullWhen(true)] out Transform? transform)
    {
        transform = new ReplaceTransform(parameters[0], parameters[1]);
        return true;
    }
}

[Transform("split")]
public sealed class SplitTransform : Transform, ITransform<SplitTransform>
{
    private readonly string _separator;
    private readonly int _index;

    public SplitTransform(string separator, int index)
    {
        _separator = separator;
        _index = index;
    }

    public override string Run(string input)
    {
        string[] parts = input.Split(_separator);
        int partIndex = _index < 0 ? parts.Length + _index : _index;
        return partIndex < 0 || partIndex > parts.Length ? input : parts[partIndex];
    }

    public static bool TryParseParameters(IReadOnlyList<string> parameters, [NotNullWhen(true)] out Transform? transform)
    {
        transform = new SplitTransform(parameters[0], int.Parse(parameters[1]));
        return true;
    }
}
