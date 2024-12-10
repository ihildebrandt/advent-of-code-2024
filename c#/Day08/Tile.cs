namespace Day08;

[Flags]
public enum TileType
{
    Empty,
    Node,
    Antinode
}

public class Tile
{
    private readonly char? _nodeId;

    public char? NodeId => _nodeId;

    public TileType TileType { get; set; }

    public Tile(TileType type, char? nodeId)
    {
        TileType = type;
        _nodeId = nodeId;
    }
}