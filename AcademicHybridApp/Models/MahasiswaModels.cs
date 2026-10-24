using System.ComponentModel.DataAnnotations;

namespace AcademicHybridApp.Models;

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
    [Required(ErrorMessage = "NIM wajib diisi.")]
    [StringLength(20, ErrorMessage = "NIM maksimal 20 karakter.")]
    public string Nim { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nama mahasiswa wajib diisi.")]
    [StringLength(100, ErrorMessage = "Nama mahasiswa maksimal 100 karakter.")]
    public string NamaMahasiswa { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email wajib diisi.")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [StringLength(100, ErrorMessage = "Email maksimal 100 karakter.")]
    public string Email { get; set; } = string.Empty;

    [Range(2020, 2026, ErrorMessage = "Angkatan harus antara 2020 dan 2026.")]
    public int Angkatan { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Dosen wali wajib dipilih.")]
    public int DosenId { get; set; }
}
