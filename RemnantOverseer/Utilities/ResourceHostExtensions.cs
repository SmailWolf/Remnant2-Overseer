using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RemnantOverseer.Utilities;
public static class ResourceHostExtensions
{
    public static IServiceProvider GetServiceProvider(this IResourceHost control)
    {
        return (IServiceProvider?)control.FindResource(typeof(IServiceProvider)) ??
            throw new Exception("Service provider was not found");
    }

    public static T CreateInstance<T>(this IResourceHost control)
    {
        return ActivatorUtilities.CreateInstance<T>(control.GetServiceProvider());
    }
}
