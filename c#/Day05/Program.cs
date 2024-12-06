string? line;
bool parsingRules = true;

var orderRules = new List<(int Before, int After)>();
var order = new List<int>();
var updates = new List<int[]>();

while ((line = Console.In.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        parsingRules = false;
        continue;
    }

    if (parsingRules)
    {
        var parts = line.Trim()
            .Split('|')
            .Select(int.Parse)
            .ToArray();

        orderRules.Add((parts[0], parts[1]));
    }
    else
    {
        var parts = line.Trim()
            .Split(',')
            .Select(int.Parse)
            .ToArray();
        
        updates.Add(parts);
    }    
}

var valid = (int[] update) => {
    Console.WriteLine($"Validating {string.Join(',', update)}");

    for (var p = 0; p < update.Length; p++)
    {
        var page = update[p];
        // Console.WriteLine($"Evaluating: {page}");

        for (var r = 0; r < orderRules.Count; r++)
        {
            var rule = orderRules[r];
            // Console.Write($"... Evaluating ({rule.Before}, {rule.After})");
            if (page != rule.Before && page != rule.After)
            {
                // Console.WriteLine($" ... Skipping.");
                continue;
            }

            for (var b = p - 1; b >= 0; b--)
            {
                var before = update[b];
                // Console.Write($" ... check {before} before {page}");
                if (before == rule.After)
                {
                    // Console.WriteLine($" ... ({before}, {page}) violates ({rule.Before}, {rule.After}).");
                    return false;
                }
            }

            for (var a = p + 1; a < update.Length; a++)
            {
                var after = update[a];
                // Console.Write($" .. check {after} after {page}");
                if (after == rule.Before)
                {
                    // Console.WriteLine($" ... ({page}, {after}) violates ({rule.Before}, {rule.After}).");
                    return false;
                }
            }

            // Console.WriteLine($" ... Valid.");
        }

        // Console.WriteLine($" ... Valid.");
    }

    // Console.WriteLine($"... Valid");
    return true;
};

List<int[]> validUpdates = [];
List<int[]> invalidUpdates = [];

foreach (var update in updates)
{
    if (valid(update))
    {
        validUpdates.Add(update);
    }
    else
    {
        invalidUpdates.Add(update);
    }
}

Console.WriteLine(validUpdates.Count());

var validAcc = 0;
foreach (var update in validUpdates)
{
    validAcc += update[update.Length / 2];
}

Console.WriteLine(validAcc);

var invalidAcc = 0;
foreach (var update in invalidUpdates)
{
    while(!valid(update))
    {
        Console.WriteLine($"Reordering {string.Join(',', update)}");

        for (var p = 0; p < update.Length; p++)
        {
            var page = update[p];
            
            for (var r = 0; r < orderRules.Count; r++)
            {
                var rule = orderRules[r];

                if (page == rule.Before)
                {
                    for (var b = p - 1; b > 0; b--)
                    {
                        var before = update[b];
                        if (before == rule.After)
                        {
                            // Console.WriteLine($"B! - Swapping {p}:{page} and {b}:{before} because of rule ({rule.Before},{rule.After})");
                            
                            var tmp = update[b];
                            update[b] = page;
                            update[p] = before;

                            b = 0;
                            p = update.Length;
                            r = orderRules.Count;
                        }
                    }
                }
                else if (page == rule.After)
                {
                    for (var a = p + 1; a < update.Length; a++)
                    {
                        var after = update[a];
                        if (after == rule.Before)
                        {
                            // Console.WriteLine($"A! - Swapping {p}:{page} and {a}:{after} because of rule ({rule.Before},{rule.After})");
                            
                            var tmp = update[a];
                            update[a] = page;
                            update[p] = after;

                            a = update.Length;
                            p = update.Length;
                            r = orderRules.Count;
                        }
                    }
                }
            }
        }
    }

    invalidAcc += update[update.Length / 2];
}

Console.WriteLine(invalidAcc);