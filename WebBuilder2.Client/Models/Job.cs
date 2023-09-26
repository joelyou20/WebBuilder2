namespace WebBuilder2.Client.Models;

public class Job
{
    public string Name { get; set; } = string.Empty;
    public JobStatus Status { get; set; } = JobStatus.NotStarted;
}
