using System;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Controllers
{
    public class ExerciseViewModel
    {
        [Display(Name = "Date Recorded")]
        [DataType(DataType.Date)]
        public DateTime dateRecorded { get; set; }

        [Display(Name = "Distance Ran (in miles)")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid distance.")]
        public decimal? distanceRan { get; set; }

        [Display(Name = "Time (in hours)")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid time.")]
        public decimal time { get; set; }

        [Display(Name = "Weight Benched (in lbs)")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid weight.")]
        public decimal? weightBenched { get; set; }

        [Display(Name = "Weight Deadlift (in lbs)")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid weight.")]
        public decimal? weightDeadlift { get; set; }

        [Display(Name = "Weight Curl (in lbs)")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid weight.")]
        public decimal? weightCurl { get; set; }
    }
}
