using AcademicApi.Data;
using AcademicApi.Dtos;
using AcademicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DosenController : ControllerBase
{
    private readonly AcademicDbContext _context;

    public DosenController(AcademicDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DosenDto>>> GetAll()
    {
        var dosen = await _context.Dosen
            .AsNoTracking()
            .OrderBy(d => d.DosenId)
            .Select(d => new DosenDto
            {
                DosenId = d.DosenId,
                Nidn = d.Nidn,
                NamaDosen = d.NamaDosen,
                Email = d.Email,
                Prodi = d.Prodi
            })
            .ToListAsync();

        return Ok(dosen);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DosenDto>> GetById(int id)
    {
        var dosen = await _context.Dosen
            .AsNoTracking()
            .Where(d => d.DosenId == id)
            .Select(d => new DosenDto
            {
                DosenId = d.DosenId,
                Nidn = d.Nidn,
                NamaDosen = d.NamaDosen,
                Email = d.Email,
                Prodi = d.Prodi
            })
            .FirstOrDefaultAsync();

        if (dosen is null)
        {
            return NotFound(new { message = "Dosen tidak ditemukan." });
        }

        return Ok(dosen);
    }

    [HttpPost]
    public async Task<ActionResult<DosenDto>> Create(DosenRequest request)
    {
        var nidn = request.Nidn.Trim();

        if (await _context.Dosen.AnyAsync(d => d.Nidn == nidn))
        {
            return Conflict(new { message = $"NIDN '{nidn}' sudah digunakan." });
        }

        var dosen = new Dosen
        {
            Nidn = nidn,
            NamaDosen = request.NamaDosen.Trim(),
            Email = request.Email.Trim(),
            Prodi = request.Prodi.Trim()
        };

        _context.Dosen.Add(dosen);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = $"NIDN '{nidn}' sudah digunakan." });
        }

        var result = new DosenDto
        {
            DosenId = dosen.DosenId,
            Nidn = dosen.Nidn,
            NamaDosen = dosen.NamaDosen,
            Email = dosen.Email,
            Prodi = dosen.Prodi
        };

        return CreatedAtAction(nameof(GetById), new { id = dosen.DosenId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, DosenRequest request)
    {
        var dosen = await _context.Dosen.FirstOrDefaultAsync(d => d.DosenId == id);

        if (dosen is null)
        {
            return NotFound(new { message = "Dosen tidak ditemukan." });
        }

        var nidn = request.Nidn.Trim();

        if (await _context.Dosen.AnyAsync(d => d.Nidn == nidn && d.DosenId != id))
        {
            return Conflict(new { message = $"NIDN '{nidn}' sudah digunakan." });
        }

        dosen.Nidn = nidn;
        dosen.NamaDosen = request.NamaDosen.Trim();
        dosen.Email = request.Email.Trim();
        dosen.Prodi = request.Prodi.Trim();

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = $"NIDN '{nidn}' sudah digunakan." });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dosen = await _context.Dosen.FirstOrDefaultAsync(d => d.DosenId == id);

        if (dosen is null)
        {
            return NotFound(new { message = "Dosen tidak ditemukan." });
        }

        var masihMembimbing = await _context.Mahasiswa.AnyAsync(m => m.DosenId == id);

        if (masihMembimbing)
        {
            return Conflict(new { message = "Dosen tidak dapat dihapus karena masih menjadi dosen wali mahasiswa." });
        }

        _context.Dosen.Remove(dosen);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
