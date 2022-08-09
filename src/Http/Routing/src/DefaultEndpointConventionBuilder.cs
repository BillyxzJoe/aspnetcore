// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Routing;

internal sealed class DefaultEndpointConventionBuilder : IEndpointConventionBuilder
{
    internal EndpointBuilder EndpointBuilder { get; }

    private List<Action<EndpointBuilder>>? _conventions;
    private readonly List<Action<EndpointBuilder>> _finalConventions;

    public DefaultEndpointConventionBuilder(EndpointBuilder endpointBuilder)
    {
        EndpointBuilder = endpointBuilder;
        _conventions = new();
        _finalConventions = new();
    }

    public void Add(Action<EndpointBuilder> convention)
    {
        var conventions = _conventions;

        if (conventions is null)
        {
            throw new InvalidOperationException(Resources.RouteEndpointDataSource_ConventionsCannotBeModifiedAfterBuild);
        }

        conventions.Add(convention);
    }

    public void Finally(Action<EndpointBuilder> finalConvention)
    {
       _finalConventions.Add(finalConvention);
    }

    public Endpoint Build()
    {
        // Only apply the conventions once
        var conventions = Interlocked.Exchange(ref _conventions, null);

        if (conventions is not null)
        {
            foreach (var convention in conventions)
            {
                convention(EndpointBuilder);
            }
        }

        foreach (var finalConvention in _finalConventions)
        {
            finalConvention(EndpointBuilder);
        }

        return EndpointBuilder.Build();
    }
}
