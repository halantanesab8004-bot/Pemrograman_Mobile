using AcademicApi.Data;
using AcademicApi.Dtos;
using AcademicApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MahasiswaController : ControllerBase
{
    private readonly AcademicDbContext _context;

    public MahasiswaController(AcademicDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MahasiswaDto>>> GetAll()
    {
        var mahasiswa = await _context.Mahasiswa
            .AsNoTracking()
            .OrderBy(m => m.MahasiswaId)
            .Select(m => new MahasiswaDto
            {
                MahasiswaId = m.MahasiswaId,
                Nim = m.Nim,
                NamaMahasiswa = m.NamaMahasiswa,
                Email = m.Email,
                Angkatan = m.Angkatan,
                DosenId = m.DosenId,
                DosenWali = new DosenWaliDto
                {
                    DosenId = m.Dosen.DosenId,
                    Nidn = m.Dosen.Nidn,
                    NamaDosen = m.Dosen.NamaDosen
                }
            })
            .ToListAsync();

        return Ok(mahasiswa);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MahasiswaDto>> GetById(int id)
    {
        var mahasiswa = await _context.Mahasiswa
            .AsNoTracking()
            .Where(m => m.MahasiswaId == id)
            .Select(m => new MahasiswaDto
            {
                MahasiswaId = m.MahasiswaId,
                Nim = m.Nim,
                NamaMahasiswa = m.NamaMahasiswa,
                Email = m.Email,
                Angkatan = m.Angkatan,
                DosenId = m.DosenId,
                DosenWali = new DosenWaliDto
                {
                    DosenId = m.Dosen.DosenId,
                    Nidn = m.Dosen.Nidn,
                    NamaDosen = m.Dosen.NamaDosen
                }
            })
            .FirstOrDefaultAsync();

        if (mahasiswa is null)
        {
            return NotFound(new { message = "Mahasiswa tidak ditemukan." });
        }

        return Ok(mahasiswa);
    }

    [HttpPost]
    public async Task<ActionResult<MahasiswaDto>> Create(MahasiswaRequest request)
    {
        var nim = request.Nim.Trim();

        if (await _context.Mahasiswa.AnyAsync(m => m.Nim == nim))
        {
            return Conflict(new { message = $"NIM '{nim}' sudah digunakan." });
        }

        var dosenWali = await _context.Dosen
            .AsNoTracking()
            .Where(d => d.DosenId == request.DosenId)
            .Select(d => new DosenWaliDto
            {
                DosenId = d.DosenId,
                Nidn = d.Nidn,
                NamaDosen = d.NamaDosen
            })
            .FirstOrDefaultAsync();

        if (dosenWali is null)
        {
            return BadRequest(new { message = "Dosen wali tidak ditemukan." });
        }

        var mahasiswa = new Mahasiswa
        {
            Nim = nim,
            NamaMahasiswa = request.NamaMahasiswa.Trim(),
            Email = request.Email.Trim(),
            Angkatan = request.Angkatan,
            DosenId = request.DosenId
        };

        _context.Mahasiswa.Add(mahasiswa);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = $"NIM '{nim}' sudah digunakan." });
        }

        var result = new MahasiswaDto
        {
            MahasiswaId = mahasiswa.MahasiswaId,
            Nim = mahasiswa.Nim,
            NamaMahasiswa = mahasiswa.NamaMahasiswa,
            Email = mahasiswa.Email,
            Angkatan = mahasiswa.Angkatan,
            DosenId = mahasiswa.DosenId,
            DosenWali = dosenWali
        };

        return CreatedAtAction(nameof(GetById), new { id = mahasiswa.MahasiswaId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MahasiswaRequest request)
    {
        var mahasiswa = await _context.Mahasiswa.FirstOrDefaultAsync(m => m.MahasiswaId == id);

        if (mahasiswa is null)
        {
            return NotFound(new { message = "Mahasiswa tidak ditemukan." });
        }

        var nim = request.Nim.Trim();

        if (await _context.Mahasiswa.AnyAsync(m => m.Nim == nim && m.MahasiswaId != id))
        {
            return Conflict(new { message = $"NIM '{nim}' sudah digunakan." });
        }

        if (!await _context.Dosen.AnyAsync(d => d.DosenId == request.DosenId))
        {
            return BadRequest(new { message = "Dosen wali tidak ditemukan." });
        }

        mahasiswa.Nim = nim;
        mahasiswa.NamaMahasiswa = request.NamaMahasiswa.Trim();
        mahasiswa.Email = request.Email.Trim();
        mahasiswa.Angkatan = request.Angkatan;
        mahasiswa.DosenId = request.DosenId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = $"NIM '{nim}' sudah digunakan." });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var mahasiswa = await _context.Mahasiswa.FirstOrDefaultAsync(m => m.MahasiswaId == id);

        if (mahasiswa is null)
        {
            return NotFound(new { message = "Mahasiswa tidak ditemukan." });
        }

        _context.Mahasiswa.Remove(mahasiswa);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
