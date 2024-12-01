namespace Fitness.Models
{
    public class UserMeal
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public int Calories { get; set; }
        public int Carbohydrates { get; set; }
        public int Protein { get; set; }
        public int Fats { get; set; }
        public int Sugar { get; set; }
        public int Sodium { get; set; }
        public int Fiber { get; set; }
        public int Cholesterol { get; set; }
    }
}