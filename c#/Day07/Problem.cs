namespace Day07;

public class Problem
{
    public static Problem Parse(string problemText)
    {
        var parts = problemText.Split(':', StringSplitOptions.TrimEntries);
        var operands = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return new Problem(long.Parse(parts[0]), operands.Select(long.Parse).ToArray());
    }

    private static Func<long, long, long>[] Operators =>
    [
        (long a, long b) => a * b,
        (long a, long b) => a + b,
        (long a, long b) => long.Parse($"{a}{b}")
    ];

    private readonly long _solution;
    private readonly long[] _operands;

    private bool? _isSolvable;

    public bool IsSolvable 
    {
        get
        {
            if (!_isSolvable.HasValue)
            {
                _isSolvable = TryFindSolution();
            }
            return _isSolvable.Value;
        }
    }

    public long Solution => _solution;

    private Problem(long solution, long[] operands)
    {
        _solution = solution;
        _operands = operands;
    }

    private bool TryFindSolution()
    {
        var operatorPermutations = new List<List<Func<long, long, long>>>();

        var operatorCount = _operands.Length - 1;
        for (var p = 0; p < Math.Pow(Operators.Length, operatorCount); p++)
        {
            var permutation = new List<Func<long, long, long>>();
            for (var o = 0; o < _operands.Length - 1; o++)
            {
                var operatorIndex = p / (int)Math.Pow(Operators.Length, o) % Operators.Length;
                permutation.Add(Operators[operatorIndex]);
            }
            operatorPermutations.Add(permutation);
        }

        foreach (var permutation in operatorPermutations)
        {
            var acc = _operands[0];
            for (var o = 1; o < _operands.Length; o++)
            {
                acc = permutation[o-1].Invoke(acc, _operands[o]);
                if (acc > _solution)
                { 
                    break;
                }
            }

            if (acc == _solution)
            {
                return true;
            }
        }

        return false;
    }
}