namespace Day03;

public static class Parser
{
    private enum ParserState
    {
        WaitingForOperation,
        WaitingForNumber,
        WaitingForClose
    }

    public static IEnumerable<(int a, int b)> Read(string input)
    {
        var state = ParserState.WaitingForOperation;
        var head = -1;

        var enabled = true;
        
        string? op = null;
        string? a = null;
        string? b = null;

        for (var i = 0; i < input.Length; i++)
        {
            var chr = input[i];
            // Console.Write(chr);

            switch (chr) {
                case 'm':
                case 'd':
                    if (state == ParserState.WaitingForOperation && head == -1)
                    {
                        // Console.WriteLine(" - setting start of operation");
                        head = i;
                    }
                    else 
                    {
                        // Console.WriteLine(" - unexpected - resetting");
                        state = ParserState.WaitingForOperation;
                        head = -1;
                    }
                    break;
                case 'u':
                case 'o':
                    if (state != ParserState.WaitingForOperation || i - head != 1)
                    {
                        // Console.WriteLine(" - unexpected - resetting");
                        state = ParserState.WaitingForOperation;
                        head = -1;
                    }
                    else 
                    {
                        // Console.WriteLine(" - expected - continuing");
                    }
                    break;
                case 'l':
                case 'n':
                    if (state != ParserState.WaitingForOperation || i - head != 2)
                    {
                        // Console.WriteLine(" - unexpected - resetting");
                        state = ParserState.WaitingForOperation;
                        head = -1;
                    }
                    else 
                    {
                        // Console.WriteLine(" - expected - continuing");
                    }
                    break;
                case '\'':
                    if (state != ParserState.WaitingForOperation || i - head != 3)
                    {
                        // Console.WriteLine(" - unexpected - resetting");
                        state = ParserState.WaitingForOperation;
                        head = -1;
                    }
                    else 
                    {
                        // Console.WriteLine(" - expected - continuing");
                    }
                    break;
                case 't':
                    if (state != ParserState.WaitingForOperation || i - head != 4)
                    {
                        // Console.WriteLine(" - unexpected - resetting");
                        state = ParserState.WaitingForOperation;
                        head = -1;
                    }
                    else 
                    {
                        // Console.WriteLine(" - expected - continuing");
                    }
                    break;
                case '(':
                    if (state != ParserState.WaitingForOperation || head == -1)
                    {
                        // Console.WriteLine(" - unexpected - resetting");
                        state = ParserState.WaitingForOperation;
                        head = -1;
                    }
                    else
                    {
                        op = input.Substring(head, i - head);
                        Console.WriteLine($"found op: {op}");

                        switch (op) {
                            case "mul":
                                state = ParserState.WaitingForNumber;
                                head = i + 1;
                                break;
                            case "do":
                            case "don't":
                                state = ParserState.WaitingForClose;
                                head = i + 1;
                                break;
                            default:
                                state = ParserState.WaitingForOperation;
                                head = -1;
                                break;

                        }

                        // Console.WriteLine(op);
                    }
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    if (state != ParserState.WaitingForNumber || i - head > 3)
                    {
                        // Console.WriteLine(" - unexpected - resetting");
                        state = ParserState.WaitingForOperation;
                        head = -1;
                    }
                    else 
                    {
                        // Console.WriteLine(" - expected - continuing");
                    }
                    break;
                case ',':
                    if (state != ParserState.WaitingForNumber)
                    {
                        // Console.WriteLine(" - unexpected - resetting");
                    }
                    else 
                    {
                        // Console.Write(" - expected - found ");
                        a = input.Substring(head, i - head);
                        // Console.WriteLine(a);
                        head = i + 1;
                    }
                    break;
                case ')':
                    if (state == ParserState.WaitingForNumber)
                    {
                        // Console.WriteLine(" - unexpected - resetting");
                        b = input.Substring(head, i - head);
                        Console.WriteLine($"{op}({a},{b}) " + (enabled ? "enabled" : "disabled"));
                        if (enabled)
                        {
                            yield return (int.Parse(a!), int.Parse(b));
                        }
                    }
                    else if (state == ParserState.WaitingForClose)
                    {
                        // Console.Write(" - expected - found ");
                        Console.WriteLine($"{op}()");
                        switch(op) {
                            case "do":
                                enabled = true;
                                break;
                            case "don't":
                                enabled = false;
                                break;
                        }
                    }
                    state = ParserState.WaitingForOperation;
                    head = -1;
                    break;
                default:
                    // Console.WriteLine(" - unexpected - resetting");
                    state = ParserState.WaitingForOperation;
                    head = -1;
                    break;
            }
        }
    }
}
