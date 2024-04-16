using Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public static class PrometheusMetricsExtensions
{
    public static IApplicationBuilder UsePrometheusHttpMetrics(this IApplicationBuilder app)
    {
        var requestCounter = Metrics.CreateCounter(
            "http_requests_total",
            "Total number of HTTP requests.",
            new CounterConfiguration
            {
                LabelNames = new[] { "method", "path" }
            });

        var requestDuration = Metrics.CreateHistogram(
            "http_request_duration_seconds",
            "The duration in seconds of HTTP requests.",
            new HistogramConfiguration
            {
                LabelNames = new[] { "method", "path" },
                Buckets = Histogram.ExponentialBuckets(0.01, 2, 10)
            });

        var inProgressGauge = Metrics.CreateGauge(
            "http_requests_in_progress_total",
            "Total number of HTTP requests in progress.",
            new GaugeConfiguration
            {
                LabelNames = new[] { "method", "path" }
            });

        app.Use(async (context, next) =>
        {
            var path = NormalizePath(context.Request.Path);
            var method = context.Request.Method;

            try
            {
                inProgressGauge.WithLabels(method, path).Inc();

                var timer = requestDuration.WithLabels(method, path).NewTimer();

                await next();

                timer.ObserveDuration();
            }
            finally
            {
                inProgressGauge.WithLabels(method, path).Dec();
            }
        });

        return app;
    }

    private static string NormalizePath(PathString path)
    {
        var segments = path.ToString().Trim('/').Split('/');

        for (int i = 0; i < segments.Length; i++)
        {
            if (long.TryParse(segments[i], out _))
            {
                segments[i] = "{id}";
            }
        }

        return "/" + string.Join("/", segments);
    }
}
