namespace Jeevan.TextTransformers;

/// <inheritdoc />
public abstract class AffixTransform : Transform
{
    protected AffixTransform(string affix)
    {
        ArgumentException.ThrowIfNullOrEmpty(affix);
        Affix = affix;
    }

    protected string Affix { get; }
}
