using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromptLog.Db;

namespace promptlog.Controllers;

public class PromptDto
{
    public Guid Id { get; set; }
    public int Raiting { get; set; }
    public string Created { get; set; }
    public string Experiment { get; set; }
}

public class PromptPatchDto 
{
    public Guid Id { get; set; }
    public int? Raiting { get; set; }
}

public class PromptDetailDto
{
    public Guid Id { get; set; }
    public string Experiment { get; set; }
    public string Created { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
    public int Raiting { get; set; }
}

[ApiController]
//[Authorize]
public class PromptController : ControllerBase
{

    private readonly PromptLogDbContext _db;

    public PromptController(PromptLogDbContext db)
    {
        _db = db;
    }

    [HttpGet("api/prompts")]
    public async Task<PromptDto[]> GetAll(int? raiting = null)
    {
        return await _db.Prompts
            .Where(i => raiting == null || i.Raiting >= raiting)
            .OrderByDescending(p => p.Created)
            .Select(p => new PromptDto
            {
                Id = p.Id,
                Experiment = p.Experiment,
                Raiting = p.Raiting,
                Created = p.Created.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToArrayAsync();
    }

    [HttpGet("api/prompts/{id:guid}")]
    public async Task<ActionResult<PromptDetailDto>> Get(Guid id)
    {
        var prompt = await _db.Prompts.FindAsync(id);
        if (prompt == null)
        {
            return NotFound();
        }

        return new PromptDetailDto
        {
            Id = prompt.Id,
            Experiment = prompt.Experiment,
            Created = prompt.Created.ToString("yyyy-MM-dd HH:mm:ss"),
            Request = prompt.Request,
            Response = prompt.Response,
            Raiting = prompt.Raiting,
        };
    }

    [HttpPatch("api/prompts")]
    public async Task<ActionResult<PromptPatchDto>> Patch(PromptPatchDto prompt)
    {
        var entity = await _db.Prompts.FindAsync(prompt.Id);
        if (entity == null)
        {
            return NotFound();
        }
        
        entity.Raiting = prompt.Raiting ?? 0;
        await _db.SaveChangesAsync();
        return new PromptPatchDto
        {
            Id = entity.Id,
            Raiting = entity.Raiting,
        };
    }
}
