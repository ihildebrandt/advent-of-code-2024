using System.Text;
using Microsoft.VisualBasic;

namespace Day06;

public class Map
{
    private readonly Tile[,] _tiles;
    private readonly (int X, int Y) _guardOrigin;
    private (int X, int Y, Direction Facing) _guard = (0, 0, Direction.North);
    private Dictionary<int, List<Direction>> _hitObstacle = new Dictionary<int, List<Direction>>();

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
                        _guardOrigin.X = _guard.X = x;
                        _guardOrigin.Y = _guard.Y = y;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }

    public void PlaceObstacle((int X, int Y) coords)
    {
        if (coords.X == _guard.X && coords.Y == _guard.Y) return;

        _tiles[coords.X, coords.Y] = Tile.TemporaryObstacle;
    }

    public void Reset()
    {
        _guard.X = _guardOrigin.X;
        _guard.Y = _guardOrigin.Y;
        _guard.Facing = Direction.North;
        
        _hitObstacle = new Dictionary<int, List<Direction>>();

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
            (_guard.X, _guard.Y)
        };

        (int X, int Y, Direction Facing) pointer = (_guard.X, _guard.Y, _guard.Facing);

        while (!looping)
        {
            var move = MovePointer(ref pointer, out looping);

            _guard.X = pointer.X;
            _guard.Y = pointer.Y;
            _guard.Facing = pointer.Facing;

            if (move == Tile.Bounds)
            {
                break;
            }

            _tiles[pointer.X, pointer.Y] = Tile.Path;
            path.Add((pointer.X, pointer.Y));
        }
        
        return path;
    }

    private Tile MovePointer(ref (int X, int Y, Direction Facing) pointer, out bool looping)
    {
        looping = false;

        var peek = Peek(pointer);
        if (peek == Tile.Obstacle || peek == Tile.TemporaryObstacle)
        {
            var coordIndex = pointer.Y * Width + pointer.X;

            if (_hitObstacle.TryGetValue(coordIndex, out var list))
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
                _hitObstacle.Add(coordIndex, new List<Direction> { pointer.Facing });
            }

            pointer.Facing = Turn(pointer.Facing);
            return MovePointer(ref pointer, out looping);
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