using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Database.Entitty;

[Table("Contacts")]
public class Contact
{
    [Key]
    public int Id { get; set; }
    [MaxLength(15)]
    public string Name { get; set; }
    [MaxLength(11)]
    public string Phonenumber { get; set; }
    [MaxLength(15)]
    public string JobTitle { get; set; }
    public DateTime BirthDate { get; set; }
}