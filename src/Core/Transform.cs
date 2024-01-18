namespace Jeevan.TextTransformers;

public abstract class Transform
{
    /// <summary>
    ///     Runs the transform on the given <paramref name="input"/> and returns the transformed value.
    /// </summary>
    /// <param name="input">The input string to transform.</param>
    /// <returns>The transform value.</returns>
    public abstract string Run(string input);
}
