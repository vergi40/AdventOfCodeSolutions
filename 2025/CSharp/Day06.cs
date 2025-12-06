namespace CSharp;

public class Day06
{
    [Test]
    public void A()
    {
        var input = Utils.GetInput("06").ToList();

        var expressions = ParseExpressions(input);

        long sum = 0;
        foreach (var expression in expressions)
        {
            if (expression.Operator == "+")
            {
                sum += expression.Sum();
            }
            else
            {
                sum += expression.Product();
            }
        }

        Assert.That(sum, Is.EqualTo(3525371263915));
    }

    [Test]
    public void B()
    {
        var input = Utils.GetInput("06").ToList();

        var exampleInput = new List<string>
        {
            "123 328  51 64 ",
            " 45 64  387 23 ",
            "  6 98  215 314",
            "*   +   *   +  "
        };

        var snippet = new List<string>
        {
            "84 72     7  8",
            "71 63  8279  6",
            "64 563 4166 74",
            "11 763 2574 54",
            "*  *   +    * "
        };

        var expressions = ParseExpressions(input);

        long sum = 0;
        foreach (var expression in expressions)
        {
            var transformed = CephalopodTransformation(expression.Original.Take(expression.Original.Count - 1).ToList());
            var expr2 = new Expression(expression.Operator, transformed, []);
            if (expr2.Operator == "+")
            {
                sum += expr2.Sum();
            }
            else
            {
                sum += expr2.Product();
            }
        }

        Assert.That(sum, Is.EqualTo(6846480843636));
    }

    private List<List<string>> ParseToColumns(List<string> input)
    {
        var startIndex = 0;
        var columns = new List<List<string>>();

        var line1 = input[0];
        var line2 = input[1];
        var line3 = input[2];
        var line4 = input[3];
        var line5 = input[4];

        for (int i = startIndex; i < input[0].Length; i++)
        {
            if (line1[i] == ' ' && line2[i] == ' ' && line3[i] == ' ' && line4[i] == ' ' && line5[i] == ' ')
            {
                // Found a column separator
                var column = new List<string>
                {
                    line1.Substring(startIndex, i - startIndex),
                    line2.Substring(startIndex, i - startIndex),
                    line3.Substring(startIndex, i - startIndex),
                    line4.Substring(startIndex, i - startIndex),
                    line5.Substring(startIndex, i - startIndex)
                };
                columns.Add(column);
                startIndex = i + 1;
            }
        }

        // Add last column
        var lastColumn = new List<string>
        {
            line1.Substring(startIndex),
            line2.Substring(startIndex),
            line3.Substring(startIndex),
            line4.Substring(startIndex),
            line5.Substring(startIndex)
        };
        columns.Add(lastColumn);
        return columns;
    }

    private List<Expression> ParseExpressions(List<string> input)
    {
        var columns = ParseToColumns(input);
        var expressions = new List<Expression>();
        foreach (var column in columns)
        {
            var expression = ParseExpression(column);
            expressions.Add(expression);
        }

        return expressions;
    }

    private Expression ParseExpression(List<string> column)
    {
        var values = new List<long>();
        foreach (var value in column.Take(column.Count - 1))
        {
            values.Add(int.Parse(value.Trim()));
        }

        return new Expression(column[^1].Trim(), values, column);
    }

    private record Expression(string Operator, List<long> Values, List<string> Original)
    {
        public long Sum() => Values.Sum();
        public long Product() => Values.Aggregate((acc, next) => acc * next);
    }

    private List<long> CephalopodTransformation(List<string> numbers)
    {
        // "123 328  51 64 ",
        // " 45 64  387 23 ",
        // "  6 98  215 314",

        // 123 45 6 -> 356 24 1
        var width = numbers[0].Length;
        var result = new List<long>();
        for (int i = width - 1; i >= 0; i--)
        {
            var columnString = "";
            foreach (var number in numbers)
            {
                if (number[i] != ' ')
                {
                    columnString += number[i];
                }
            }

            var parsed = int.Parse(columnString);
            if (parsed <= 0) throw new InvalidOperationException("Invalid column value");
            result.Add(parsed);
        }

        return result;
    }
}
