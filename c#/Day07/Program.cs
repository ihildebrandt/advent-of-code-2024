

using Day07;

string? line;

List<Problem> problems = [];

while ((line = Console.In.ReadLine()) != null)
{
    var problem = Problem.Parse(line);
    // Console.WriteLine($"{line} => {problem.IsSolvable}");
    problems.Add(problem);
}

long acc = 0L;
foreach (var problem in problems)
{
    if (problem.IsSolvable)
    {
        acc += problem.Solution;
    }
}
Console.WriteLine(acc);

// Console.WriteLine(problems.Count(p => p.IsSolvable));