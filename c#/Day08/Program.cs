using System.Security.Cryptography.X509Certificates;
using Day08;


var tiles = new List<Tile>();

var lines = Console.In.ReadToEnd()
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim().ToCharArray())
                .ToArray();

var width = lines[0].Length;
var height = lines.Length;

var grid = new Tile[width, height];
var map = new Dictionary<char, List<(int X, int Y, Tile Tile)>>();

for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {
        var id = lines[y][x];
        if (id == '.')
        {
            grid[x,y] = new Tile(TileType.Empty, null);
        }
        else 
        {
            grid[x,y] = new Tile(TileType.Node, id);
            if (!map.ContainsKey(id))
            {
                map.Add(id, new List<(int X, int Y, Tile Tile)>());
            }
            map[id].Add((x, y, grid[x,y]));
        }
    }
}

/* standard */
foreach (var nodeList in map.Values)
{
    for (var i = 0; i < nodeList.Count; i++)
    {
        for (var j = i + 1; j < nodeList.Count; j++)
        {
            var nodeA = nodeList[i];
            var nodeB = nodeList[j];

            Console.WriteLine($"Considering: ({nodeA.X},{nodeA.Y}) and ({nodeB.X},{nodeB.Y})");

            var dx = Math.Abs(nodeA.X - nodeB.X);
            var dy = Math.Abs(nodeA.Y - nodeB.Y);

            if (nodeA.X == nodeB.X || nodeA.Y == nodeB.Y) throw new Exception("Deal with it!");

            int an1x, an2x, an1y, an2y;

            if (nodeA.X < nodeB.X) 
            {
                an1x = nodeA.X + dx * 2;
                an2x = nodeB.X - dx * 2;
            }
            else 
            {
                an2x = nodeB.X + dx * 2;
                an1x = nodeA.X - dx * 2;
            }
                
            if (nodeA.Y < nodeB.Y)
            {
                an1y = nodeA.Y + dy * 2;
                an2y = nodeB.Y - dy * 2;
            }
            else
            {
                an1y = nodeB.Y + dy * 2;
                an2y = nodeA.Y - dy * 2;
            }


            if (an1x >= 0 && an1x < width && an1y >= 0 && an1y < height)
            {
                Console.WriteLine($".. Adding Antinode ({an1x},{an1y})");
                grid[an1x,an1y].TileType |= TileType.Antinode;
            }

            if (an2x >= 0 && an2x < width && an2y >= 0 && an2y < height)
            {
                Console.WriteLine($".. Adding Antinode ({an2x},{an2y})");
                grid[an2x,an2y].TileType |= TileType.Antinode;
            }
        }
    }
}


/* resonant */
foreach (var nodeList in map.Values) 
{
    for (var i = 0; i < nodeList.Count; i++)
    {
        for (var j = i + 1; j < nodeList.Count; j++)
        {
            var nodeA = nodeList[i];
            var nodeB = nodeList[j];

            var dx = Math.Abs(nodeA.X - nodeB.X);
            var dy = Math.Abs(nodeA.Y - nodeB.Y);

            if (nodeA.X == nodeB.X || nodeA.Y == nodeB.Y) throw new Exception("Deal with it!");

            int an1x;
            int op = 1;
            
            var mx = Math.Min(nodeA.X, nodeB.X);
            var xsteps = width / dx;

            if (nodeA.X > nodeB.X)
            {
                an1x = mx + dx * (xsteps + 1);
                op = -1;
            }
            else
            {
                an1x = mx - dx * xsteps;
            }

            var my = Math.Min(nodeA.Y, nodeB.Y);
            var an1y = my - dy * xsteps;

            while (an1y < height)
            {
                an1x += dx * op;
                an1y += dy;

                if (an1x < 0 || an1x >= width || an1y < 0 || an1y >= height) continue;
                grid[an1x,an1y].TileType |= TileType.Antinode;
            }
        }
    }
}

var acc = 0;
for (var y = 0; y < grid.GetLength(1); y++)
{
    for (var x = 0; x < grid.GetLength(0); x++)
    {
        if ((grid[x,y].TileType & TileType.Antinode) == TileType.Antinode)
        {
            acc++;
            if ((grid[x,y].TileType & TileType.Node) == TileType.Node)
            {
                Console.Write(grid[x,y].NodeId);
            }
            else
            {
                Console.Write('#');
            }
        }
        else if ((grid[x,y].TileType & TileType.Node) == TileType.Node)
        {
            Console.Write(grid[x,y].NodeId);
        }
        else 
        {
            Console.Write('.');
        }
    }
    Console.WriteLine();
}
Console.WriteLine(acc);
