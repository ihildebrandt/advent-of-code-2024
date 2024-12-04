
List<List<char>> matrix = [];

string? line;
while ((line = Console.In.ReadLine()) != null)
{
    matrix.Add(line.ToCharArray().ToList());
}

var findXmas = (int x, int y, int xStep, int yStep) => {
    if (xStep == 1 && yStep == 0 && x > matrix.Count - 4) return false;
    if (xStep == -1 && yStep == 0 && x < 3) return false;
    if (xStep == 0 && yStep == 1 && y > matrix[x].Count - 4) return false;
    if (xStep == 0 && yStep == -1 && y < 3) return false; 
    
    if (xStep == 1 && yStep == 1 && (x > matrix.Count - 4 || y > matrix[x].Count - 4)) return false;
    if (xStep == -1 && yStep == 1 && (x < 3 || y > matrix[x].Count - 4)) return false;
    if (xStep == -1 && yStep == -1 && (x < 3 || y < 3)) return false;
    if (xStep == 1 && yStep == -1 && (x > matrix.Count - 4 || y < 3)) return false;

    return
        matrix[x + xStep * 0][y + yStep * 0] == 'X' &&
        matrix[x + xStep * 1][y + yStep * 1] == 'M' &&
        matrix[x + xStep * 2][y + yStep * 2] == 'A' &&
        matrix[x + xStep * 3][y + yStep * 3] == 'S';
};

var findAllXmas = (int x, int y) => {
    if (matrix[x][y] != 'X') return 0;

    var count = 0;
    count += (findXmas(x, y, 1, 0) ? 1 : 0);
    count += (findXmas(x, y, -1, 0) ? 1 : 0);
    count += (findXmas(x, y, 0, 1) ? 1 : 0);
    count += (findXmas(x, y, 0, -1) ? 1 : 0);
    count += (findXmas(x, y, 1, 1) ? 1 : 0);
    count += (findXmas(x, y, -1, 1) ? 1 : 0);
    count += (findXmas(x, y, -1, -1) ? 1 : 0);
    count += (findXmas(x, y, 1, -1) ? 1 : 0);
    return count;
};

var findMas = (int x, int y) => {
    if (matrix[x][y] != 'A') return 0;

    if (x < 1 || x > matrix.Count - 2) return 0;
    if (y < 1 || y > matrix[x].Count - 2) return 0;

    if (
        // MAS exists in TL and BR
        ((matrix[x - 1][y - 1] == 'M' && matrix[x + 1][y + 1] == 'S') || (matrix[x + 1][y + 1] == 'M' && matrix[x - 1][y - 1] == 'S')) &&
        // MAS exists in TR and BL
        ((matrix[x + 1][y - 1] == 'M' && matrix[x - 1][y + 1] == 'S') || (matrix[x - 1][y + 1] == 'M' && matrix[x + 1][y - 1] == 'S'))
    ) 
    {
        return 1;
    }  

    return 0;
};

var accXmas = 0;
var accMas = 0;
for (var i = 0; i < matrix.Count; i++)
{
    for (var j = 0; j < matrix[i].Count; j++)
    {
        accXmas += findAllXmas(i, j);
        accMas += findMas(i, j);
    }
}

Console.WriteLine(accXmas);
Console.WriteLine(accMas);