using Microsoft.EntityFrameworkCore;
using WebBuilder2.Server.Data;
using WebBuilder2.Server.Data.Models;
using WebBuilder2.Server.Repositories.Contracts;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Repositories;

public class ScriptRepository : IScriptRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<ScriptRepository> _logger;

    public ScriptRepository(AppDbContext db, ILogger<ScriptRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IQueryable<ScriptModel>? Get(IEnumerable<long>? exclude = null)
    {
        var query = _db.Script
            .Where(s => s.DeletedDateTime == null);

        if (exclude != null) query = query.Where(s => !exclude.Any(e => s.Id == e));

        return query.Select(s => new ScriptModel()
        {
            Id = s.Id,
            Data = s.Data,
            Name = s.Name,
            CreatedDateTime = s.CreatedDateTime,
            ModifiedDateTime = s.ModifiedDateTime,
            DeletedDateTime = s.DeletedDateTime
        });
    }

    public IEnumerable<ScriptModel> AddRange(IEnumerable<ScriptModel> values)
    {
        if (!values.Any()) throw new ArgumentNullException(paramName: "values");
        var dtos = values.Select(ToDto).ToArray();

        _db.AddRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return dtos.Select(x => x.FromDto());
    }

    public IEnumerable<ScriptModel> UpdateRange(IEnumerable<ScriptModel> values)
    {
        var dtos = values.Select(ToDto).ToList();
        dtos.ForEach(x => _db.Entry(x).Property("CreatedDateTime").IsModified = false);
        _db.Script.UpdateRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return dtos.Select(x => x.FromDto());
    }

    public IEnumerable<ScriptModel> UpsertRange(IEnumerable<ScriptModel> values)
    {
        IEnumerable<long> valuesList = values.Select(x => x.Id);
        List<ScriptModel> existingValues = Get()?.Where(x => valuesList.Contains(x.Id)).ToList() ?? new List<ScriptModel>();
        List<ScriptModel> newValues = values.Where(x => !existingValues.Any(y => y.Id.Equals(x.Id))).ToList();

        var result = new List<ScriptModel>();

        if (existingValues.Any()) result.AddRange(UpdateRange(values));
        if (newValues.Any()) result.AddRange(AddRange(newValues));
        return result;
    }

    public IEnumerable<ScriptModel> DeleteRange(IEnumerable<ScriptModel> values)
    {
        var dtos = values.Select(ToDto).ToArray(); ;
        _db.RemoveRange(dtos);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return dtos.Select(x => x.FromDto());
    }

    public IEnumerable<ScriptModel> SoftDeleteRange(IEnumerable<ScriptModel> values)
    {
        var softDeletedValues = values.Select(x =>
        {
            var copy = ToDto(x);
            copy.DeletedDateTime = DateTime.UtcNow;
            return copy;
        }).ToArray();
        _db.Script.UpdateRange(softDeletedValues);
        var result = _db.SaveChanges();
        if (result <= 0) throw new DbUpdateException("Failed to save changes.");
        return softDeletedValues.Select(x => x.FromDto());
    }

    public Script ToDto(ScriptModel script) => new()
    {
        Id = script.Id,
        Data = script.Data,
        Name = script.Name,
        ModifiedDateTime = script.ModifiedDateTime,
        CreatedDateTime = script.CreatedDateTime,
        DeletedDateTime = script.DeletedDateTime,
    };
}
