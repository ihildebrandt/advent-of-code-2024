using Day06;

var input = Console.In.ReadToEnd();
var map = new Map(input);
var path = map.Walk();

Console.WriteLine(map.Count(Tile.Path));

Console.WriteLine("----------------");
Console.WriteLine("----------------");
Console.WriteLine("----------------");
Console.WriteLine("----------------");

var loopingPaths = new List<(int X, int Y)>();

for (var p = 0; p < path.Count; p++)
{
    if (loopingPaths.Any(lp => lp.X == path[p].X && lp.Y == path[p].Y))
    {
        continue;
    }

    map.Reset();
    map.PlaceObstacle((path[p].X, path[p].Y));
    map.Walk(out var looping);

    if (looping)
    {
        Console.WriteLine($"({path[p].X},{path[p].Y}): Looping");
        loopingPaths.Add(path[p]);
    }
}

Console.WriteLine(loopingPaths.Count);
