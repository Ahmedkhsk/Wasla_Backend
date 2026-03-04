[ApiController]
[Route("api/hangfire")]
public class HangfireController : ControllerBase
{
    private readonly JobStorage _storage;

    public HangfireController()
    {
        _storage = JobStorage.Current;
    }

    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        var monitor = _storage.GetMonitoringApi();

        var stats = new
        {
            Enqueued = monitor.EnqueuedCount("default"),
            Scheduled = monitor.ScheduledCount(),
            Processing = monitor.ProcessingCount(),
            Failed = monitor.FailedCount(),
        };

        return Ok(stats);
    }

    [HttpGet("scheduled")]
    public IActionResult GetScheduled()
    {
        var monitor = _storage.GetMonitoringApi();
        var jobs = monitor.ScheduledJobs(0, 50);

        var result = jobs.Select(j => new
        {
            Key = j.Key,
            MethodName = j.Value?.Job?.Method?.Name ?? "Unknown",
            EnqueueAt = j.Value?.EnqueueAt,
            ScheduledAt = j.Value?.ScheduledAt
        });

        return Ok(result);
    }

    [HttpGet("enqueued")]
    public IActionResult GetEnqueued()
    {
        var monitor = _storage.GetMonitoringApi();
        var jobs = monitor.EnqueuedJobs("default", 0, 50);

        var result = jobs.Select(j => new
        {
            Key = j.Key,
            MethodName = j.Value?.Job?.Method?.Name ?? "Unknown",
            EnqueuedAt = j.Value?.EnqueuedAt
        });

        return Ok(result);
    }

    [HttpGet("processing")]
    public IActionResult GetProcessing()
    {
        var monitor = _storage.GetMonitoringApi();
        var jobs = monitor.ProcessingJobs(0, 50);

        var result = jobs.Select(j => new
        {
            Key = j.Key,
            MethodName = j.Value?.Job?.Method?.Name ?? "Unknown",
            StartedAt = j.Value?.StartedAt
        });

        return Ok(result);
    }

    [HttpGet("failed")]
    public IActionResult GetFailed()
    {
        var monitor = _storage.GetMonitoringApi();
        var jobs = monitor.FailedJobs(0, 50);

        var result = jobs.Select(j => new
        {
            Key = j.Key,
            MethodName = j.Value?.Job?.Method?.Name ?? "Unknown",
        });

        return Ok(result);
    }

    [HttpPost("retry/{jobId}")]
    public IActionResult Retry(string jobId)
    {
        BackgroundJob.Requeue(jobId);
        return Ok("Job retried");
    }

    [HttpDelete("{jobId}")]
    public IActionResult Delete(string jobId)
    {
        BackgroundJob.Delete(jobId);
        return Ok("Job deleted");
    }
}
