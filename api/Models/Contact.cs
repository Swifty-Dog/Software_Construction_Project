using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Contact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
}