using System.Text;
using Microsoft.VisualBasic;

namespace Day06;

public class Map
{
    private readonly Tile[,] _tiles;
    private readonly (int X, int Y) _guardOrigin;

    public int Width => _tiles.GetLength(0);

    public int Height => _tiles.GetLength(1);

    public Map(string map)
    {
        var rows = map.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        _tiles = new Tile[rows[0].Length, rows.Length];

        for (var y = 0; y < rows.Length; y++)
        {
            var row = rows[y];
            var cols = row.ToCharArray();

            for (var x = 0; x < cols.Length; x++)
            {
                switch (cols[x])
                {
                    case '.':
                        _tiles[x, y] = Tile.Open;
                        break;
                    case '#':
                        _tiles[x, y] = Tile.Obstacle;
                        break;
                    case '^':
                        _tiles[x, y] = Tile.Path;
                        _guardOrigin.X = x;
                        _guardOrigin.Y = y;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }

    public void PlaceObstacle((int X, int Y) coords)
    {
        _tiles[coords.X, coords.Y] = Tile.TemporaryObstacle;
    }

    public void Reset()
    {
        for (var y = 0; y < _tiles.GetLength(1); y++)
        {
            for (var x = 0; x < _tiles.GetLength(0); x++)
            {
                if (_tiles[x,y] == Tile.Path)
                {
                    _tiles[x,y] = Tile.Open;
                }
                else if (_tiles[x,y] == Tile.TemporaryObstacle)
                {
                    _tiles[x,y] = Tile.Open;
                }
            }
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = 0; y < _tiles.GetLength(1); y++)
        {
            for (var x = 0; x < _tiles.GetLength(0); x++)
            {
                switch (_tiles[x,y])
                {
                    case Tile.Open:
                        sb.Append('.');
                        break;
                    case Tile.Obstacle:
                        sb.Append('#');
                        break;
                    case Tile.Path:
                        sb.Append('X');
                        break;
                }
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public int Count(Tile tile)
    {
        var acc = 0;
        for (var y = 0; y < _tiles.GetLength(1); y++)
        {
            for (var x = 0; x < _tiles.GetLength(0); x++)
            {
                acc += _tiles[x,y] == tile ? 1 : 0;
            }
        }
        return acc;
    }

    public List<(int X, int Y)> Walk()
    {
        return Walk(out var _);
    }

    public List<(int X, int Y)> Walk(out bool looping)
    {
        looping = false;
        
        var path = new List<(int X, int Y)>
        {
            (_guardOrigin.X, _guardOrigin.Y)
        };

        var hits = new Dictionary<int, List<Direction>>();
        (int X, int Y, Direction Facing) pointer = (_guardOrigin.X, _guardOrigin.Y, Direction.North);

        while (!looping)
        {
            var move = MovePointer(hits, ref pointer, out looping);

            if (move == Tile.Bounds)
            {
                break;
            }

            _tiles[pointer.X, pointer.Y] = Tile.Path;
            path.Add((pointer.X, pointer.Y));
        }
        
        return path;
    }

    private Tile MovePointer(Dictionary<int, List<Direction>> hits, ref (int X, int Y, Direction Facing) pointer, out bool looping)
    {
        looping = false;

        var peek = Peek(pointer);
        if (peek == Tile.Obstacle || peek == Tile.TemporaryObstacle)
        {
            var coordIndex = pointer.Y * Width + pointer.X;

            if (hits.TryGetValue(coordIndex, out var list))
            {
                if (list.Contains(pointer.Facing))
                {
                    looping = true;
                    return Tile.Path;
                }
                list.Add(pointer.Facing);
            }
            else 
            {
                hits.Add(coordIndex, new List<Direction> { pointer.Facing });
            }

            pointer.Facing = Turn(pointer.Facing);
            return MovePointer(hits, ref pointer, out looping);
        }
        else 
        {
            var step = Step(ref pointer);
            if (step == Tile.Bounds)
            {
                return Tile.Bounds;
            }

            return step;
        }
    }

    private (int X, int Y) GetStep(Direction facing)
    {
        var y = (facing == Direction.North ? - 1 : 
                (facing == Direction.South ? 1 : 0));
        var x = (facing == Direction.West ? -1 : 
                (facing == Direction.East ? 1 : 0));
        return (x, y);
    }

    private bool OutOfBounds((int X, int Y) pointer)
    {
        if (pointer.X < 0 || pointer.X >= _tiles.GetLength(0) ||
            pointer.Y < 0 || pointer.Y >= _tiles.GetLength(1))
        {
            return true;
        }

        return false;
    }

    private Tile Peek((int X, int Y, Direction Facing) pointer)
    {
        var step = GetStep(pointer.Facing);

        var peekX = pointer.X + step.X;
        var peekY = pointer.Y + step.Y;

        if (OutOfBounds((peekX, peekY)))
        {
            return Tile.Bounds;
        }

        return _tiles[peekX, peekY];
    }

    private Direction Turn(Direction facing)
    {
        switch (facing)
        {
            case Direction.North:
                return Direction.East;
            case Direction.East:
                return Direction.South;
            case Direction.South:
                return Direction.West;
            case Direction.West:
                return Direction.North;
            default:
                throw new NotImplementedException();
        }
    }

    private Tile Step(ref (int X, int Y, Direction Facing) pointer)
    {
        var step = GetStep(pointer.Facing);
        pointer.X += step.X;
        pointer.Y += step.Y;

        if (OutOfBounds((pointer.X, pointer.Y)))
        {
            return Tile.Bounds;
        }

        return _tiles[pointer.X, pointer.Y];
    }
}