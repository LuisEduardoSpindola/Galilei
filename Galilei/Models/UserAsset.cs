using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Galilei.Models
{
    public class UserAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required(ErrorMessage = "O Ticker (Ativo) é obrigatório.")]
        [StringLength(10)]
        public string Ticker { get; set; } = string.Empty;

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "O preço médio é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço médio deve ser maior que zero.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal AveragePrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço desejado deve ser maior que zero.")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal DesiredPrice { get; set; }

        [Required]
        public DesiredPriceType DesiredPriceType { get; set; } = DesiredPriceType.Compra;

        public bool IsTargetNotified { get; set; }
    }

    public enum DesiredPriceType
    {
        Compra = 1,
        Venda = 2
    }
}
