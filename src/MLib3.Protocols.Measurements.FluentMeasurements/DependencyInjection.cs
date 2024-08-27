using Microsoft.Extensions.DependencyInjection;

namespace MLib3.Protocols.Measurements.FluentMeasurements;

public static class DependencyInjection
{
    /// <summary>
    /// <para>
    /// Register Fluent Measurements
    /// </para>
    /// <para>
    /// <list type="bullet">
    /// <item><description><see cref="IProtocolBuilderFactory"/></description></item>
    /// <item><description><see cref="IMetaBuilderFactory"/></description></item>
    /// <item><description><see cref="IProductBuilderFactory"/></description></item>
    /// <item><description><see cref="IResultsBuilderFactory"/></description></item>
    /// <item><description><see cref="ISectionBuilderFactory"/></description></item>
    /// <item><description><see cref="IValueBuilderFactory"/></description></item>
    /// <item><description><see cref="ICommentBuilderFactory"/></description></item>
    /// <item><description><see cref="IInfoBuilderFactory"/></description></item>
    /// <item><description><see cref="IFlagBuilderFactory"/></description></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="services">The service collection to register the dependency injection services</param>
    /// <returns>The same service collection</returns>
    public static IServiceCollection AddFluentMeasurements(this IServiceCollection services)
    {
        services.AddTransient<IProtocolBuilderFactory, ProtocolBuilderFactory>();
        services.AddTransient<IMetaBuilderFactory, MetaBuilderFactory>();
        services.AddTransient<IProductBuilderFactory, ProductBuilderFactory>();
        services.AddTransient<IResultsBuilderFactory, ResultsBuilderFactory>();

        services.AddTransient<ISectionBuilderFactory, SectionBuilderFactory>();
        services.AddTransient<IValueBuilderFactory, ValueBuilderFactory>();
        services.AddTransient<ICommentBuilderFactory, CommentBuilderFactory>();
        services.AddTransient<IInfoBuilderFactory, InfoBuilderFactory>();
        services.AddTransient<IFlagBuilderFactory, FlagBuilderFactory>();
        return services;
    }
}