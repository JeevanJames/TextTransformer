using System.Diagnostics.CodeAnalysis;

namespace Jeevan.TextTransformers;

/// <inheritdoc cref="AffixTransform" />
[Transform("prefix")]
[TransformParameter<string>(0, "prefix",
    Description = "Prefix to prepend to the string.")]
public sealed class PrefixTransform : AffixTransform, ITransform<PrefixTransform>
{
    public PrefixTransform(string prefix) : base(prefix)
    {
    }

    public override string Run(string input) => Affix + input;

    public static bool TryParseParameters(IReadOnlyList<string> parameters, [NotNullWhen(true)] out Transform? transform)
    {
        transform = new PrefixTransform(parameters[0]);
        return true;
    }
}
