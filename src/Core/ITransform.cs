using System.Diagnostics.CodeAnalysis;

namespace Jeevan.TextTransformers;

public interface ITransform<TSelf> where TSelf : ITransform<TSelf>
{
    static abstract bool TryParseParameters(IReadOnlyList<string> parameters,
        [NotNullWhen(true)] out Transform? transform);
}
