using System;
using System.Collections.Generic;

public interface ITrade
{
    double Value { get; }
    string ClientSector { get; }
}

public class Trade : ITrade
{
    public double Value { get; private set; }
    public string ClientSector { get; private set; }

    public Trade(double value, string clientSector)
    {
        Value = value;
        ClientSector = clientSector;
    }
}

public interface ITradeCategoryStrategy
{
    string GetCategory(ITrade trade);
}

public class LowRisk : ITradeCategoryStrategy
{
    public string GetCategory(ITrade trade)
    {
        if (trade.Value < 1000000 && trade.Value >= 0 && trade.ClientSector == "Public")
        {
            return "LOWRISK";
        }
        return null;
    }
}

public class MediumRisk : ITradeCategoryStrategy
{
    public string GetCategory(ITrade trade)
    {
        if (trade.Value >= 1000000 && trade.ClientSector == "Public")
        {
            return "MEDIUMRISK";
        }
        return null;
    }
}

public class HighRisk : ITradeCategoryStrategy
{
    public string GetCategory(ITrade trade)
    {
        if (trade.Value >= 1000000 && trade.ClientSector == "Private")
        {
            return "HIGHRISK";
        }
        return null;
    }
}

public class TradeCategorizer
{
    private readonly List<ITradeCategoryStrategy> _strategies;

    public TradeCategorizer()
    {
        _strategies = new List<ITradeCategoryStrategy>
        {
            new LowRisk(),
            new MediumRisk(),
            new HighRisk()
        };
    }

    public List<string> Categorize(List<ITrade> portfolio)
    {
        var categories = new List<string>();

        foreach (var trade in portfolio)
        {
            bool categorized = false;
            foreach (var strategy in _strategies)
            {
                var category = strategy.GetCategory(trade);
                if (category != null)
                {
                    categories.Add(category);
                    categorized = true;
                    break;
                }
            }
            if (!categorized)
            {
                categories.Add(null);
            }
        }

        return categories;
    }
}


public class Program
{
    public static void Main()
    {
        var portfolio = new List<ITrade>
        {
            new Trade(2000000, "Private"),
            new Trade(400000, "Public"),
            new Trade(500000, "Public"),
            new Trade(3000000, "Public")
        };

        var categorizer = new TradeCategorizer();
        var categories = categorizer.Categorize(portfolio);

        Console.WriteLine(string.Join(", ", categories));
    }
}
