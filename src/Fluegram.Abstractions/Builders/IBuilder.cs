namespace Fluegram.Abstractions.Builders;

public interface IBuilder<T>
{
    T Build();
}

public interface IBuilder<T, TBuildOptions>
{
    T Build(TBuildOptions buildOptions);
}