using System.ComponentModel.DataAnnotations;

namespace AccountManagementSystem.Models
{
    public class accountDto
    {
        [Required]
        public string Name { get; set; } = "";

        [Required,EmailAddress]
        public string Email { get; set; } = "";

        [Phone]
        public string PhoneNumber { get; set; } = "";

        public int? ParentId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string CreatedBy { get; set; } = "";
    }
}
