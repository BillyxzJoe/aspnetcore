// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.HttpLogging;

internal static partial class HttpLoggingExtensions
{
    public static void RequestLog(this ILogger logger, HttpRequestLog requestLog) => logger.Log(
        LogLevel.Information,
        new EventId(1, "RequestLog"),
        requestLog,
        exception: null,
        formatter: HttpRequestLog.Callback);
    public static void ResponseLog(this ILogger logger, HttpResponseLog responseLog) => logger.Log(
        LogLevel.Information,
        new EventId(2, "ResponseLog"),
        responseLog,
        exception: null,
        formatter: HttpResponseLog.Callback);

    [LoggerMessage(3, LogLevel.Information, "RequestBody: {Body}", EventName = "RequestBody")]
    public static partial void RequestBody(this ILogger logger, string body);

    [LoggerMessage(4, LogLevel.Information, "ResponseBody: {Body}", EventName = "ResponseBody")]
    public static partial void ResponseBody(this ILogger logger, string body);

    [LoggerMessage(5, LogLevel.Debug, "Decode failure while converting body.", EventName = "DecodeFailure")]
    public static partial void DecodeFailure(this ILogger logger, Exception ex);

    [LoggerMessage(6, LogLevel.Debug, "Unrecognized Content-Type for request body.", EventName = "RequestUnrecognizedMediaType")]
    public static partial void RequestUnrecognizedMediaType(this ILogger logger);
    
    [LoggerMessage(7, LogLevel.Debug, "Unrecognized Content-Type for response body.", EventName = "ResponseUnrecognizedMediaType")]
    public static partial void ResponseUnrecognizedMediaType(this ILogger logger);
}
