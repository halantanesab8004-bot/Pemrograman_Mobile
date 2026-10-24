using System.ComponentModel.DataAnnotations;

namespace AcademicApi.Dtos;

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
    [Required]
    [StringLength(20)]
    public string Nidn { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string NamaDosen { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Prodi { get; set; } = string.Empty;
}
