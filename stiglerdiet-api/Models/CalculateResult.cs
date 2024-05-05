namespace stiglerdiet;

public class CalculateResult
{
    public List<FoodList> Foods { get; set; }
    public double OptimalAnnualPrice { get; set; }
    public Dictionary<string, NutrientsPerDay> Nutrients { get; set; }
    public AdvancedUsage? Advanced { get; set; }
}

public class AdvancedUsage
{
    public long SolvedTimeMilliseconds { get; set; }
    public long SolvedIterations { get; set; }
    public int Variables { get; set; }
    public int Constraints { get; set; }
}

public class AnnualFoods
{
    public string? Name { get; set; }
    public double Price { get; set; }
}
public class FoodList
{
    public string Name { get; set; }
    public double Price { get; set; }
}

public class NutrientsPerDay
{
    public double Value { get; set; }
    public double MinValue { get; set; }
}
/*
variables
constraints
Annual foods
   Food1
    Food2
Optimal annual price
Nutrients per day
    Calories value + min value
    Protein
    Carbohydrates
    Fat
    Fiber
Problem
    solved in x seconds
    solved in x iterations*/