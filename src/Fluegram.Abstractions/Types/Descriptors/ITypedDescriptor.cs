namespace Fluegram.Abstractions.Types.Descriptors;

/// <summary>
///     Descriptor which holds information about type.
/// </summary>
public interface ITypedDescriptor
{
    Type? Type { get; }

    string? Name { get; }
}