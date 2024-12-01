namespace Fitness.Models
{
    public class UserWeight
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime dateMade { get; set; }
        public double Weight { get; set; }
    }
}
