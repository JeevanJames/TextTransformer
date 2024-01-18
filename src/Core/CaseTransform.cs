using System.Diagnostics.CodeAnalysis;

namespace Jeevan.TextTransformers;

/// <inheritdoc cref="Transform" />
[Transform("case")]
public sealed class CaseTransform : Transform, ITransform<CaseTransform>
{
    private readonly CaseType[] _caseTypes;

    public CaseTransform(params CaseType[] caseTypes)
    {
        ArgumentNullException.ThrowIfNull(caseTypes);
        _caseTypes = caseTypes;
    }

    public override string Run(string input)
    {
        string output = input;
        foreach (CaseType caseType in _caseTypes)
        {
            output = caseType switch
            {
                CaseType.Upper => output.ToUpperInvariant(),
                CaseType.Lower => output.ToLowerInvariant(),
                _ => output,
            };
        }

        return output;
    }

    public static bool TryParseParameters(IReadOnlyList<string> parameters,
        [NotNullWhen(true)] out Transform? transform)
    {
        transform = default;

        if (parameters.Count == 0)
            return false;

        var caseTypes = new CaseType[parameters.Count];
        for (int i = 0; i < parameters.Count; i++)
        {
            if (!Enum.TryParse(parameters[i], ignoreCase: true, out CaseType caseType))
                return false;
            caseTypes[i] = caseType;
        }

        transform = new CaseTransform(caseTypes);
        return true;
    }
}

public enum CaseType
{
    Upper,
    Lower,
    Camel,
    Pascal,
    Snake,
    Kebab,
    Title,
}
