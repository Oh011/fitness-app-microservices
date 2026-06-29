namespace NutritionService.Domain.Entities;

public class NutritionFact
{
    public int Id { get; set; }
    public int Calories { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fats { get; set; }
    public double Fiber { get; set; }
}
