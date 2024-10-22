namespace Fitness.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public DateTime dateRecorded { get; set; }

        public decimal? distanceRan { get; set; }
        public decimal time { get; set; }

        public decimal? weightBenched { get; set; }
        public decimal? weightDeadlift { get; set; }
        public decimal? weightCurl { get; set; }

        public decimal findMPH()
        {
            if (distanceRan.HasValue && time > 0)
            {
                return distanceRan.Value / time;
            }
            return 0;
        }
    }
}
