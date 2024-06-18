namespace WebBuilder2.Client.Models;

public class Job
{
    public Job(JobType jobType)
    {
        Type = jobType;
    }

    public Job(JobType jobType, JobStatus jobStatus)
    {
        Type = jobType;
        Status = jobStatus;
    }

    public JobType Type { get; set; } 
    public JobStatus Status { get; set; } = JobStatus.NotStarted;
}
