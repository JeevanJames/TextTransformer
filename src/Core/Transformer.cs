using System.Reflection;

namespace Jeevan.TextTransformers;

public static class Transformer
{
    private static readonly Dictionary<string, Func<string, Transform>> _transforms =
        new(StringComparer.OrdinalIgnoreCase);

    public static void RegisterTransform<TTransform>()
        where TTransform : Transform, ITransform<TTransform>
    {
        TransformAttribute? attribute = typeof(TTransform).GetCustomAttribute<TransformAttribute>(inherit: false);
        if (attribute is null)
            throw new ArgumentException($"Transform type {typeof(TTransform)} is not decorated with the {nameof(TransformAttribute)}.");

        _transforms.Add(attribute.Name, p =>
        {
            string[] parameters = p.Split('|');
            return TTransform.TryParseParameters(parameters, out Transform? transform)
                ? transform
                : throw new InvalidOperationException();
        });
    }

    public static void RegisterTransforms(Type transformType)
    {
        ArgumentNullException.ThrowIfNull(transformType);

        if (!typeof(Transform).IsAssignableFrom(transformType))
        {
            throw new ArgumentException(
                $"Type {transformType} is not a valid transform class. It should derive from {typeof(Transform)}.",
                nameof(transformType));
        }

        TransformAttribute? attribute = transformType.GetCustomAttribute<TransformAttribute>(inherit: false);
        if (attribute is null)
            throw new ArgumentException($"Transform type {transformType} is not decorated with the {nameof(TransformAttribute)}.");

        MethodInfo? parseMethod = transformType.GetMethod("TryParseParameters", BindingFlags.Public | BindingFlags.Static, binder: null,
            [typeof(string), typeof(Transform)], modifiers: null);
        if (parseMethod is null)
            throw new ArgumentException($"Transform type {transformType} does not implement {typeof(ITransform<>)}.", nameof(transformType));
        if (parseMethod.ReturnType != typeof(bool))
            throw new ArgumentException($"Transform type {transformType} does not implement {typeof(ITransform<>)}.", nameof(transformType));

        _transforms.Add(attribute.Name, p =>
        {
            string[] parameters = p.Split('|');
            object?[] args = { parameters, null };
            object? result = parseMethod.Invoke(null, args);
            bool parsed = result is not null && (bool)result;
            return parsed && args is [.., not null] ? (Transform)args[1]! : throw new InvalidOperationException();
        });
    }

    public static IDictionary<string, string> Transform(IEnumerable<string> inputs, params Transform[] transforms)
    {
        Dictionary<string, string> results = new();

        foreach (string input in inputs)
        {
            if (results.ContainsKey(input))
                continue;

            string output = input;

            foreach (Transform transform in transforms)
                output = transform.Run(output);

            results.Add(input, output);
        }

        return results;
    }

    public static IDictionary<string, string> Transform(IEnumerable<string> inputs, params string[] transforms)
    {
        var typedTransforms = new Transform[transforms.Length];
        for (int i = 0; i < transforms.Length; i++)
        {
            string[] parts = transforms[i].Split(':', 2);
            if (!_transforms.TryGetValue(parts[0], out Func<string, Transform>? transformFunc))
                throw new InvalidOperationException($"No transformer named '{parts[0]} is registered. ({transforms[i]}).");
            typedTransforms[i] = transformFunc(parts[1]);
        }

        return Transform(inputs, typedTransforms);
    }
}
