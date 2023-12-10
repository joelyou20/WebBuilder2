using Microsoft.EntityFrameworkCore;
using WebBuilder2.Server.Data.Models;
using WebBuilder2.Server.Data;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Shared.Models;
using Serilog;

namespace WebBuilder2.Server.Repositories;

public class LogRepository : ILogRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<LogRepository> _logger;

    public LogRepository(AppDbContext db, ILogger<LogRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IQueryable<LogModel>? Get(IEnumerable<long>? exclude = null)
    {
        var query = _db.Logs
            .Where(s => s.DeletedDateTime == null);

        if (exclude != null) query = query.Where(s => !exclude.Any(e => s.Id == e));

        return query.Select(l => new LogModel()
        {
            Id = l.Id,
            Message = l.Message,
            StackTrace = l.StackTrace,
            Exception = l.Exception,
            CreatedDateTime = l.CreatedDateTime,
            ModifiedDateTime = l.ModifiedDateTime,
            DeletedDateTime = l.DeletedDateTime
        });
    }

    public IEnumerable<LogModel> AddRange(IEnumerable<LogModel> values)
    {
        if (!values.Any()) throw new ArgumentNullException(paramName: "values");
        var dtos = values.Select(ToDto).ToArray();

        _db.AddRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return dtos.Select(x => x.FromDto());
    }

    public IEnumerable<LogModel> UpdateRange(IEnumerable<LogModel> values)
    {
        var dtos = values.Select(ToDto).ToList();
        dtos.ForEach(x => _db.Entry(x).Property("CreatedDateTime").IsModified = false);
        _db.Logs.UpdateRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return dtos.Select(x => x.FromDto());
    }

    public IEnumerable<LogModel> UpsertRange(IEnumerable<LogModel> values)
    {
        IEnumerable<long> valuesList = values.Select(x => x.Id);
        List<LogModel> existingValues = Get()?.Where(x => valuesList.Contains(x.Id)).ToList() ?? new List<LogModel>();
        List<LogModel> newValues = values.Where(x => !existingValues.Any(y => y.Id.Equals(x.Id))).ToList();

        var result = new List<LogModel>();

        if (existingValues.Any()) result.AddRange(UpdateRange(values));
        if (newValues.Any()) result.AddRange(AddRange(newValues));
        return result;
    }

    public IEnumerable<LogModel> DeleteRange(IEnumerable<LogModel> values)
    {
        var dtos = values.Select(ToDto).ToArray(); ;
        _db.RemoveRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return dtos.Select(x => x.FromDto());
    }

    public IEnumerable<LogModel> SoftDeleteRange(IEnumerable<LogModel> values)
    {
        var softDeletedValues = values.Select(x =>
        {
            var copy = ToDto(x);
            copy.DeletedDateTime = DateTime.UtcNow;
            return copy;
        }).ToArray();
        _db.Logs.UpdateRange(softDeletedValues);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return softDeletedValues.Select(x => x.FromDto());
    }

    public Data.Models.Log ToDto(LogModel log) => new()
    {
        Id = log.Id,
        Message = log.Message,
        StackTrace = log.StackTrace,
        Exception = log.Exception,
        ModifiedDateTime = log.ModifiedDateTime,
        CreatedDateTime = log.CreatedDateTime,
        DeletedDateTime = log.DeletedDateTime,
    };
}
