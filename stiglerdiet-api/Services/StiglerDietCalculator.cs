using System.Collections.Generic;
using stiglerdiet;
using Google.OrTools.LinearSolver;
public class StiglerDietCalculator
{
    public CalculateResult? CalculateDiet(Calculate calculate, List<Food> foods2)
    {
        var result = new CalculateResult();
        /// Convert the list of nutrients to a tuple array.
        (string Name, double Value)[] nutrients = new (string Name, double Value)[]
        {
            ("Calories", calculate.Calories ?? 0),
            ("Protein", calculate.Protein ?? 0),
            ("Carbohydrates", calculate.Carbohydrates ?? 0),
            ("Fat", calculate.Fat ?? 0),
            ("Fiber", calculate.Fiber ?? 0)
        };
        /// Convert the list of foods to a tuple array.
        (string Name, double Price, double[] Nutrients)[] data = new (string Name, double Price, double[] Nutrients)[foods2.Count];
        for (int i = 0; i < foods2.Count; ++i)
        {
            data[i] = (foods2[i].Name, foods2[i].Price, new double[] { foods2[i].Calories, foods2[i].Protein, foods2[i].Carbohydrates, foods2[i].Fat, foods2[i].Fiber });
        }

        Solver solver = Solver.CreateSolver("GLOP");

        if (solver is null)
        {
            throw new Exception("Solver could not be created.");
        }
        List<Variable> foods = new List<Variable>();
        for (int i = 0; i < data.Length; ++i)
        {
            foods.Add(solver.MakeNumVar(0.0, double.PositiveInfinity, data[i].Name));
        }
        Console.WriteLine($"Number of variables = {solver.NumVariables()}");
        List<Constraint> constraints = new List<Constraint>();
        for (int i = 0; i < nutrients.Length; ++i)
        {
            Constraint constraint =
                solver.MakeConstraint(nutrients[i].Value, double.PositiveInfinity, nutrients[i].Name);
            for (int j = 0; j < data.Length; ++j)
            {
                constraint.SetCoefficient(foods[j], data[j].Nutrients[i]);
            }
            constraints.Add(constraint);
        }

        Console.WriteLine($"Number of constraints = {solver.NumConstraints()}");

        Objective objective = solver.Objective();
        for (int i = 0; i < data.Length; ++i)
        {
            objective.SetCoefficient(foods[i], 1);
        }
        objective.SetMinimization();

        Solver.ResultStatus resultStatus = solver.Solve();

        // Check that the problem has an optimal solution.
        if (resultStatus != Solver.ResultStatus.OPTIMAL)
        {
            Console.WriteLine("The problem does not have an optimal solution!");
            if (resultStatus == Solver.ResultStatus.FEASIBLE)
            {
                Console.WriteLine("A potentially suboptimal solution was found.");
            }
            else
            {
                Console.WriteLine("The solver could not solve the problem.");
                return null;
            }
        }

        // Display the amounts (in dollars) to purchase of each food.
        double[] nutrientsResult = new double[nutrients.Length];
        Console.WriteLine("\nAnnual Foods:");
        for (int i = 0; i < foods.Count; ++i)
        {
            if (foods[i].SolutionValue() > 0.0)
            {
                Console.WriteLine($"{data[i].Name}: ${365 * foods[i].SolutionValue():N2}");
                if (result.Foods == null) {
                    result.Foods = new List<FoodList>();
                }
                result.Foods.Add(new FoodList { Name = data[i].Name, Price = 365 * foods[i].SolutionValue() });
                for (int j = 0; j < nutrients.Length; ++j)
                {
                    nutrientsResult[j] += data[i].Nutrients[j] * foods[i].SolutionValue();
                }
            }
        }
        Console.WriteLine($"\nOptimal annual price: ${365 * objective.Value():N2}");
        result.OptimalAnnualPrice = 365 * objective.Value();
        Console.WriteLine("\nNutrients per day:");
        for (int i = 0; i < nutrients.Length; ++i)
        {
            if (result.Nutrients == null)
            {
                result.Nutrients = new Dictionary<string, NutrientsPerDay>();
            }
            result.Nutrients.Add( nutrients[i].Name, new NutrientsPerDay { Value = nutrientsResult[i], MinValue = nutrients[i].Value });
            Console.WriteLine($"{nutrients[i].Name}: {nutrientsResult[i]:N2} (min {nutrients[i].Value})");
        }

        Console.WriteLine("\nAdvanced usage:");
        Console.WriteLine($"Problem solved in {solver.WallTime()} milliseconds");
        Console.WriteLine($"Problem solved in {solver.Iterations()} iterations");
        if (result.Advanced is null)
        {
            result.Advanced = new AdvancedUsage();
        }
        result.Advanced.SolvedIterations = solver.Iterations();
        result.Advanced.SolvedTimeMilliseconds = solver.WallTime();
        result.Advanced.Constraints = constraints.Count;
        result.Advanced.Variables = solver.NumVariables();
        return result;
    }
}