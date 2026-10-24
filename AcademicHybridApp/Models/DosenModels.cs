using System.ComponentModel.DataAnnotations;

namespace AcademicHybridApp.Models;

public class DosenDto
{
    public int DosenId { get; set; }

    public string Nidn { get; set; } = string.Empty;

    public string NamaDosen { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Prodi { get; set; } = string.Empty;
}

public class DosenRequest
{
    [Required(ErrorMessage = "NIDN wajib diisi.")]
    [StringLength(20, ErrorMessage = "NIDN maksimal 20 karakter.")]
    public string Nidn { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nama dosen wajib diisi.")]
    [StringLength(100, ErrorMessage = "Nama dosen maksimal 100 karakter.")]
    public string NamaDosen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [StringLength(100, ErrorMessage = "Email maksimal 100 karakter.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Prodi wajib diisi.")]
    [StringLength(100, ErrorMessage = "Prodi maksimal 100 karakter.")]
    public string Prodi { get; set; } = string.Empty;
}
