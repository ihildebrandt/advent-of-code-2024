
List<int> a = [];
List<int> b = [];
Dictionary<int, int> map = [];

string? line;

while ((line = Console.In.ReadLine()) != null)
{
    var items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
    a.Add(items[0]);
    b.Add(items[1]);
}

a.Sort();
b.Sort();

foreach (var number in b)
{
    if (!map.ContainsKey(number))
    {
        map.Add(number, 0);
    }
    map[number]++;
}

var acc = 0;
var sim = 0;
for (var i = 0; i < a.Count; i++)
{
    acc += Math.Abs(a[i] - b[i]);
    
    map.TryGetValue(a[i], out int occ);
    sim += a[i] * occ;
}

Console.WriteLine(acc);
Console.WriteLine(sim);