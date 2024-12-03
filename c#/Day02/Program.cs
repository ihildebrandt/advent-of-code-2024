

string? line;
List<bool> reportSafety = [];

var isSafe = (int a, int b, bool increasing) => {
    return a != b && (increasing && a > b || !increasing && a < b) & Math.Abs(a - b) <= 3;
};

var checkLevels = (int[] levels) => {
    var increasing = levels[1] > levels[0];
    var unsafeLevels = new List<int>();
    
    for (int i = 1; i < levels.Count(); i++)
    {
        var a = levels[i];
        var b = levels[i - 1]; 

        if (!isSafe(levels[i], levels[i - 1], increasing))
        {
            return false;
        }
    }

    return true;
};

while ((line = Console.In.ReadLine()) != null)
{
    Console.Write(line);

    var levels = line.Trim()
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(l => int.Parse(l))
        .ToArray();

    var safe = true;
    
    if (!checkLevels(levels))
    {
        safe = false;

        for (var i = 0; i < levels.Length; i++)
        {
            var levelsAlt = levels.ToList();
            levelsAlt.RemoveAt(i);
            if (checkLevels(levelsAlt.ToArray()))
            {
                safe = true;
                break;
            }
        }
    }

    reportSafety.Add(safe);
    Console.WriteLine(" - " + (safe ? "safe" : "unsafe"));
}

Console.WriteLine(reportSafety.Count(s => s));