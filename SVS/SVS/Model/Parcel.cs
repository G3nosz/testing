namespace SVS.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Parcel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public int Number { get; set; }

        [Required]
        [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid Weight")]
        public float Weight { get; set; }

        [Required]
        [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid Length")]
        public float Length { get; set; }

        [Required]
        [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid Width")]
        public float Width { get; set; }

        [Required]
        [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid Height")]
        public float Height { get; set; }

        public bool Fragile { get; set; }

        public string Status { get; set; }

        public string Driver { get; set; }

        public string FormatDimensions => string.Format(this.Length + " x " + this.Width + " x " + this.Height);

        public string Size
        {
            get
            {
                if (this.Length <= 45 && this.Width <= 35 && this.Height <= 16 && this.Weight <= 2)
                {
                    return "S";
                }

                if (this.Length <= 61 && this.Width <= 46 && this.Height <= 46 && this.Weight <= 20)
                {
                    return "M";
                }

                return "L";
            }
        }

        public Receiver Receiver { get; set; }

        public Sender Sender { get; set; }
    }
}