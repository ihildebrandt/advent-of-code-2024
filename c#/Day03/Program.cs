using Day03;

var input = Console.In.ReadToEnd();
Console.WriteLine(input);
var instructions = Parser.Read(input);

var acc = 0;
foreach (var instruction in instructions)
{
    acc += instruction.a * instruction.b;
}

Console.WriteLine(acc);