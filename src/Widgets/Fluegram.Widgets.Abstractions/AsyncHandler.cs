namespace Fluegram.Widgets.Abstractions;

public delegate Task AsyncHandler<T>(T value, CancellationToken cancellationToken);