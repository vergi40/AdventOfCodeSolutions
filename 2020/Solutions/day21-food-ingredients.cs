using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Solutions
{
    class Day21 : DayBase
    {
        public Day21() : base("21")
        {
        }

        public override long SolveA()
        {
            var foods = new List<Food>();
            foreach (var line in Content)
            {
                if (line.Contains('('))
                {
                    foods.Add(new Food(line));
                }
                else throw new ArgumentException();
            }
            
            // Which ingredients contain no allergens?
            
            //
            var allAllergens = foods.SelectMany(f => f.Allergens).Distinct();
            
            // All foods with certain allergen
            var foodsWithAllergen = new Dictionary<string, List<Food>>();
            foreach (var allergen in allAllergens)
            {
                var allergenFoods = foods.Where(f => f.Allergens.Contains(allergen)).ToList();
                foodsWithAllergen.Add(allergen, allergenFoods);
            }

            var knownAllergenicIngredients = new List<(string allergen, string ingredient)>();
            while(foodsWithAllergen.Count > 0)
            {
                // Common ingredients for one allergen
                var possibleAllergenIngredients = GetIngredientsForAllergen(foodsWithAllergen);

                // If definite allergen ingredients found, delete from each food
                foreach (var pair in possibleAllergenIngredients.ToList())
                {
                    if (pair.Value.Count == 1)
                    {
                        var knownIngredient = pair.Value.Single();
                        knownAllergenicIngredients.Add((pair.Key, knownIngredient));
                        foreach (var food in foods)
                        {
                            food.Ingredients.Remove(knownIngredient);
                        }

                        foodsWithAllergen.Remove(pair.Key);
                    }
                }
            }
            
            // Collect foods with no known allergens
            // One ingredient can be found many times!
            var result = foods.SelectMany(f => f.Ingredients).ToList();
            
            // Day 21b
            knownAllergenicIngredients.Sort();
            var dangerousList = knownAllergenicIngredients.Select(i => i.ingredient).ToList();
            var resultB = string.Join(',', dangerousList);
            

            return result.Count;
        }
        
        private Dictionary<string, List<string>> GetIngredientsForAllergen(Dictionary<string, List<Food>> allergenDict)
        {
            var possibleAllergenIngredients = new Dictionary<string, List<string>>();
            foreach (var pair in allergenDict)
            {
                var commonIngredient = pair.Value.First().Ingredients;
                foreach (var food in pair.Value)
                {
                    var joined = commonIngredient.Intersect(food.Ingredients);
                    commonIngredient = joined.ToList();
                }
                possibleAllergenIngredients.Add(pair.Key, commonIngredient);
            }

            return possibleAllergenIngredients;
        }

        class Food
        {
            public List<string> Ingredients { get; }
            public List<string> Allergens { get; }
            
            public Food(string line)
            {
                var split = line.Split('(');
                Ingredients = split[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                line = split[1].Trim(' ', ')');
                line = line.Replace("contains", "");
                Allergens = line.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            }
        }

        public override long SolveB()
        {
            return 0;
        }
    }
}
