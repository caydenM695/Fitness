using Fitness.Data;
using Fitness.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Claims;

namespace Fitness.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddRoutine()
        {
            return View();
        }

        [Authorize]
        public IActionResult PlanMeal()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SaveWeight()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveWeight(double weight)
        {
            // Get the user ID of the currently logged-in user
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Redirect if user is not logged in
            }

            // Get today's date without the time
            var today = DateTime.Now.Date;

            // Check if the user already has a weight entry for today
            var existingWeight = _context.UserWeights
                .FirstOrDefault(w => w.UserId == userId && w.dateMade.Date == today);

            if (existingWeight != null)
            {
                // Update the existing weight entry
                existingWeight.Weight = weight;
            }
            else
            {
                // Create a new weight entry
                var newWeight = new UserWeight
                {
                    UserId = userId,
                    dateMade = today,
                    Weight = weight
                };
                _context.UserWeights.Add(newWeight);
            }

            // Save the changes to the database
            _context.SaveChanges();

            // Redirect to a confirmation or dashboard page
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ViewWeightGraph()
        {
            // Get the user ID of the currently logged-in user
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Redirect if user is not logged in
            }

            // Fetch the user's weight entries, ordered by date
            var userWeights = _context.UserWeights
                .Where(w => w.UserId == userId)
                .OrderBy(w => w.dateMade)
                .ToList();

            var weightData = new List<(DateTime Date, double? Weight)>();

            foreach (var weight in userWeights)
            {
                weightData.Add((weight.dateMade, weight.Weight));
            }

            // Ensure there are at least 5 entries by adding placeholders if necessary
            if (weightData.Count < 5)
            {
                var additionalEntriesNeeded = 5 - weightData.Count;
                var startDate = userWeights.Any() ? userWeights.First().dateMade : DateTime.Now.Date;

                for (int i = 0; i < additionalEntriesNeeded; i++)
                {
                    weightData.Insert(0, (startDate.AddDays(-i - 1), null)); // Insert placeholder entries
                }
            }

            // Prepare data for the view
            ViewBag.Dates = weightData.Select(w => w.Date.ToShortDateString()).ToList();
            ViewBag.Weights = weightData.Select(w => w.Weight).ToList();

            return View();
        }



        [HttpPost]
        [Authorize]
        public IActionResult SaveMealPlan(MealPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Check if a meal plan already exists for the user
                var existingMealPlan = _context.MealPlans
                    .FirstOrDefault(mp => mp.UserId == userId);

                if (existingMealPlan != null)
                {
                    // Update the existing meal plan
                    existingMealPlan.Calories = model.Calories;
                    existingMealPlan.Carbohydrates = model.Carbohydrates;
                    existingMealPlan.Protein = model.Protein;
                    existingMealPlan.Fats = model.Fats;
                    existingMealPlan.Sugar = model.Sugar;
                    existingMealPlan.Sodium = model.Sodium;
                    existingMealPlan.Fiber = model.Fiber;
                    existingMealPlan.Cholesterol = model.Cholesterol;
                    existingMealPlan.DateCreated = DateTime.Now; // Update timestamp
                }
                else
                {
                    var newMealPlan = new MealPlan
                    {
                        Calories = model.Calories,
                        Carbohydrates = model.Carbohydrates,
                        Protein = model.Protein,
                        Fats = model.Fats,
                        Sugar = model.Sugar,
                        Sodium = model.Sodium,
                        Fiber = model.Fiber,
                        Cholesterol = model.Cholesterol,
                        UserId = userId,
                        DateCreated = DateTime.Now
                    };

                    _context.MealPlans.Add(newMealPlan);
                }

                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View("PlanMeal", model);
        }


        [Authorize]
        public IActionResult LogIntake()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var today = DateTime.Today;

            var existingIntake = _context.UserIntakes
                .FirstOrDefault(u => u.UserId == userId && u.DateCreated.Date == today);

            ViewBag.Today = today.ToString("yyyy-MM-dd");

            if (existingIntake != null)
            {
                var model = new MealPlanViewModel
                {
                    Calories = existingIntake.Calories,
                    Carbohydrates = existingIntake.Carbohydrates,
                    Protein = existingIntake.Protein,
                    Fats = existingIntake.Fats,
                    Sugar = existingIntake.Sugar,
                    Sodium = existingIntake.Sodium,
                    Fiber = existingIntake.Fiber,
                    Cholesterol = existingIntake.Cholesterol
                };

                return View(model);
            }

            return View(new MealPlanViewModel());
        }



        [HttpPost]
        [Authorize]
        public IActionResult SaveIntake(MealPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var today = DateTime.Today;

                var existingIntake = _context.UserIntakes
                    .FirstOrDefault(u => u.UserId == userId && u.DateCreated.Date == today);

                if (existingIntake != null)
                {
                    _context.UserIntakes.Remove(existingIntake);
                }

                var newIntake = new UserMeal
                {
                    UserId = userId,
                    DateCreated = today,
                    Calories = model.Calories,
                    Carbohydrates = model.Carbohydrates,
                    Protein = model.Protein,
                    Fats = model.Fats,
                    Sugar = model.Sugar,
                    Sodium = model.Sodium,
                    Fiber = model.Fiber,
                    Cholesterol = model.Cholesterol
                };

                _context.UserIntakes.Add(newIntake);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View("LogIntake", model);
        }




        [Authorize]
        public IActionResult ViewGraph(string mineral = "Fats")
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var today = DateTime.Now.Date;
            var past7Days = Enumerable.Range(0, 7).Select(i => today.AddDays(-i)).ToList();

   
            var dates = new List<string>();
            var mineralData = new List<int>();

            foreach (var date in past7Days)
            {
                var meal = _context.UserIntakes.FirstOrDefault(um => um.UserId == userId && um.DateCreated.Date == date);

                dates.Add(date.ToShortDateString());

                // Add the selected mineral data
                switch (mineral)
                {
                    case "Calories":
                        mineralData.Add(meal?.Calories ?? 0);
                        break;
                    case "Carbohydrates":
                        mineralData.Add(meal?.Carbohydrates ?? 0);
                        break;
                    case "Protein":
                        mineralData.Add(meal?.Protein ?? 0);
                        break;
                    case "Fats":
                        mineralData.Add(meal?.Fats ?? 0);
                        break;
                    case "Sugar":
                        mineralData.Add(meal?.Sugar ?? 0);
                        break;
                    case "Sodium":
                        mineralData.Add(meal?.Sodium ?? 0);
                        break;
                    case "Fiber":
                        mineralData.Add(meal?.Fiber ?? 0);
                        break;
                    case "Cholesterol":
                        mineralData.Add(meal?.Cholesterol ?? 0);
                        break;
                    default:
                        mineralData.Add(0);
                        break;
                }
            }

            mineralData.Reverse();
            dates.Reverse();

            var mealPlan = _context.MealPlans.FirstOrDefault(mp => mp.UserId == userId);
            int goalValue = 0;

            switch (mineral)
            {
                case "Calories":
                    goalValue = mealPlan?.Calories ?? 0;
                    break;
                case "Carbohydrates":
                    goalValue = mealPlan?.Carbohydrates ?? 0;
                    break;
                case "Protein":
                    goalValue = mealPlan?.Protein ?? 0;
                    break;
                case "Fats":
                    goalValue = mealPlan?.Fats ?? 0;
                    break;
                case "Sugar":
                    goalValue = mealPlan?.Sugar ?? 0;
                    break;
                case "Sodium":
                    goalValue = mealPlan?.Sodium ?? 0;
                    break;
                case "Fiber":
                    goalValue = mealPlan?.Fiber ?? 0;
                    break;
                case "Cholesterol":
                    goalValue = mealPlan?.Cholesterol ?? 0;
                    break;
                default:
                    break;
            }

            double averageMineralData = mineralData.Average();

            string goalStatus = averageMineralData <= goalValue
                ? $"{mineral} goal met!"
                : $"{mineral} goal not met!";

            ViewBag.Dates = dates;
            ViewBag.GoalStatus = goalStatus;
            ViewBag.Calories = mineralData;
            ViewBag.one = averageMineralData;
            ViewBag.two = goalValue;
            ViewBag.GoalValue = goalValue;

            return View("ViewGraph");
        }

        [HttpGet]
        public IActionResult ViewGraphData(string mineral = "Fats")
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Json(new { error = "User not logged in" });
            }

            var today = DateTime.Now.Date;
            var past7Days = Enumerable.Range(0, 7).Select(i => today.AddDays(-i)).ToList();

            var dates = new List<string>();
            var mineralData = new List<int>();

            foreach (var date in past7Days)
            {
                var meal = _context.UserIntakes.FirstOrDefault(um => um.UserId == userId && um.DateCreated.Date == date);

                dates.Add(date.ToShortDateString());

                switch (mineral)
                {
                    case "Calories":
                        mineralData.Add(meal?.Calories ?? 0);
                        break;
                    case "Carbohydrates":
                        mineralData.Add(meal?.Carbohydrates ?? 0);
                        break;
                    case "Protein":
                        mineralData.Add(meal?.Protein ?? 0);
                        break;
                    case "Fats":
                        mineralData.Add(meal?.Fats ?? 0);
                        break;
                    case "Sugar":
                        mineralData.Add(meal?.Sugar ?? 0);
                        break;
                    case "Sodium":
                        mineralData.Add(meal?.Sodium ?? 0);
                        break;
                    case "Fiber":
                        mineralData.Add(meal?.Fiber ?? 0);
                        break;
                    case "Cholesterol":
                        mineralData.Add(meal?.Cholesterol ?? 0);
                        break;
                    default:
                        mineralData.Add(0);
                        break;
                }
            }

            // Reverse the data so the most recent day is on the right
            mineralData.Reverse();
            dates.Reverse();

            // Get the user's meal plan for the selected mineral
            var mealPlan = _context.MealPlans.FirstOrDefault(mp => mp.UserId == userId);
            int goalValue = 0;

            switch (mineral)
            {
                case "Calories":
                    goalValue = mealPlan?.Calories ?? 0;
                    break;
                case "Carbohydrates":
                    goalValue = mealPlan?.Carbohydrates ?? 0;
                    break;
                case "Protein":
                    goalValue = mealPlan?.Protein ?? 0;
                    break;
                case "Fats":
                    goalValue = mealPlan?.Fats ?? 0;
                    break;
                case "Sugar":
                    goalValue = mealPlan?.Sugar ?? 0;
                    break;
                case "Sodium":
                    goalValue = mealPlan?.Sodium ?? 0;
                    break;
                case "Fiber":
                    goalValue = mealPlan?.Fiber ?? 0;
                    break;
                case "Cholesterol":
                    goalValue = mealPlan?.Cholesterol ?? 0;
                    break;
                default:
                    break;
            }

            // Calculate the average of the mineral data over the past 7 days
            double averageMineralData = mineralData.Average();

            // Check if the user met their goal and set the GoalStatus
            string goalStatus = averageMineralData <= goalValue
                ? $"{mineral} goal met!"
                : $"{mineral} goal not met!";

            // Return data as JSON for the frontend
            var result = new
            {
                mineralData,
                goalStatus,
                averageMineralData,
                goalValue,
                dates,
                mineralLabel = mineral
            };

            return Json(result);
        }


        [Authorize]
        public IActionResult ViewExercises()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExercises = _context.Exercises
                .Where(e => e.userId == userId)
                .ToList();

            ViewBag.UserExercises = userExercises;

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult SaveExercise(ExerciseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exercise = new Exercise
                {
                    dateRecorded = model.dateRecorded,
                    distanceRan = model.distanceRan,
                    time = model.time,
                    weightBenched = model.weightBenched,
                    weightDeadlift = model.weightDeadlift,
                    weightCurl = model.weightCurl,
                    userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                _context.Exercises.Add(exercise);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }


            return View("AddRoutine", model);
        }

        [Authorize]
        public IActionResult ViewExerciseGraph(string metric = "distanceRan")
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var today = DateTime.Now.Date;
            var past7Days = Enumerable.Range(0, 7).Select(i => today.AddDays(-i)).ToList();

            var dates = new List<string>();
            var metricData = new List<decimal>();

            foreach (var date in past7Days)
            {
                var exercise = _context.Exercises
                    .FirstOrDefault(e => e.userId == userId && e.dateRecorded.Date == date);

                dates.Add(date.ToShortDateString());

                switch (metric)
                {
                    case "distanceRan":
                        metricData.Add(exercise?.distanceRan ?? 0);
                        break;
                    case "weightBenched":
                        metricData.Add(exercise?.weightBenched ?? 0);
                        break;
                    case "weightDeadlift":
                        metricData.Add(exercise?.weightDeadlift ?? 0);
                        break;
                    case "weightCurl":
                        metricData.Add(exercise?.weightCurl ?? 0);
                        break;
                    default:
                        metricData.Add(0);
                        break;
                }
            }

            metricData.Reverse();
            dates.Reverse();

            decimal averageMetricData = metricData.Count > 0 ? metricData.Average() : 0;

            ViewBag.Dates = dates;
            ViewBag.MetricData = metricData;
            ViewBag.AverageMetricData = averageMetricData;
            ViewBag.MetricLabel = metric;

            return View("ViewExerciseGraph");
        }

        [HttpGet]
        public IActionResult ViewExerciseGraphData(string metric = "distanceRan")
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Json(new { error = "User not logged in" });
            }

            var today = DateTime.Now.Date;
            var past7Days = Enumerable.Range(0, 7).Select(i => today.AddDays(-i)).ToList();
            var dates = new List<string>();
            var metricData = new List<decimal>();

            foreach (var date in past7Days)
            {
                var exercise = _context.Exercises
                    .FirstOrDefault(e => e.userId == userId && e.dateRecorded.Date == date);

                dates.Add(date.ToShortDateString());

                switch (metric.ToLower())
                {
                    case "distanceran":
                        metricData.Add(exercise?.distanceRan ?? 0);
                        break;
                    case "weightbenched":
                        metricData.Add(exercise?.weightBenched ?? 0);
                        break;
                    case "weightdeadlift":
                        metricData.Add(exercise?.weightDeadlift ?? 0);
                        break;
                    case "weightcurl":
                        metricData.Add(exercise?.weightCurl ?? 0);
                        break;
                    default:
                        metricData.Add(0);
                        break;
                }
            }

            metricData.Reverse();
            dates.Reverse();

            decimal averageMetricData = metricData.Count > 0 ? metricData.Average() : 0;


            int? goalValue = null;

            var result = new
            {
                metricData,
                averageMetricData,
                dates,
                metricLabel = metric,
                goalValue  
            };

            return Json(result);
        }




    }
}
