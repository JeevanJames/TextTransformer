namespace Jeevan.TextTransformers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class TransformAttribute : Attribute
{
    public TransformAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public abstract class BaseTransformParameterAttribute : Attribute
{
    protected BaseTransformParameterAttribute(int order, string name, Type dataType)
    {
        Order = order;
        Name = name;
        DataType = dataType;
    }

    public int Order { get; }

    public string Name { get; }

    public Type DataType { get; }

    public string? Description { get; set; }
}

public sealed class TransformParameterAttribute : BaseTransformParameterAttribute
{
    public TransformParameterAttribute(int order, string name, Type dataType) : base(order, name, dataType)
    {
    }
}

public sealed class TransformParameterAttribute<TDataType> : BaseTransformParameterAttribute
{
    public TransformParameterAttribute(int order, string name) : base(order, name, typeof(TDataType))
    {
    }
}
