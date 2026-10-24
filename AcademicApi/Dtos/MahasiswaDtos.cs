using System.ComponentModel.DataAnnotations;

namespace AcademicApi.Dtos;

public class DosenWaliDto
{
    public int DosenId { get; set; }

    public string Nidn { get; set; } = string.Empty;

    public string NamaDosen { get; set; } = string.Empty;
}

public class MahasiswaDto
{
    public int MahasiswaId { get; set; }

    public string Nim { get; set; } = string.Empty;

    public string NamaMahasiswa { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int Angkatan { get; set; }

    public int DosenId { get; set; }

    public DosenWaliDto DosenWali { get; set; } = new();
}

public class MahasiswaRequest
{
    [Required]
    [StringLength(20)]
    public string Nim { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string NamaMahasiswa { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Range(2020, 2026)]
    public int Angkatan { get; set; }

    public int DosenId { get; set; }
}
