using System;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Models
{
    public class MealPlanViewModel
    {
        [Display(Name = "Calories")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid calorie value.")]
        public int Calories { get; set; }

        [Display(Name = "Carbohydrates (in grams)")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid carbohydrate value.")]
        public int Carbohydrates { get; set; }

        [Display(Name = "Protein (in grams)")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid protein value.")]
        public int Protein { get; set; }

        [Display(Name = "Fats (in grams)")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid fat value.")]
        public int Fats { get; set; }

        [Display(Name = "Sugar (in grams)")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid sugar value.")]
        public int Sugar { get; set; }

        [Display(Name = "Sodium (in milligrams)")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid sodium value.")]
        public int Sodium { get; set; }

        [Display(Name = "Fiber (in grams)")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid fiber value.")]
        public int Fiber { get; set; }

        [Display(Name = "Cholesterol (in milligrams)")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid cholesterol value.")]
        public int Cholesterol { get; set; }
    }
}
