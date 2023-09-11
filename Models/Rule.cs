using System.ComponentModel.DataAnnotations;

namespace WebAppCarClub.Models
{
    public class Rule
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Rule> Rules { get; set; }
    }
}
