using System;
using System.Collections.Generic;
using System.Linq;

namespace Mastermind
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestAll();
            //return;

            Console.WriteLine("What are the symbols?");
            var solver = new MastermindSolver(ReadStringLength(6).ToCharArray());

            while (true)
            {
                var guesses = solver.Guess();
                Console.WriteLine($"Guess: {guesses.C0}{guesses.C1}{guesses.C2}");

                Console.WriteLine("What was the response?");
                var response = ReadChallengeResponse();

                solver.Constrain(guesses, response);
            }
        }

        private static void TestAll()
        {
            ChallengeResponse ScoreSingle(int index, char guess, IReadOnlyList<char> code)
            {
                if (guess == code[index])
                {
                    // ReSharper disable once PossibleInvalidOperationException
                    return ChallengeResponse.TryParse('!').Value;
                }

                if (code.Contains(guess))
                {
                    // ReSharper disable once PossibleInvalidOperationException
                    return ChallengeResponse.TryParse('?').Value;
                }

                // ReSharper disable once PossibleInvalidOperationException
                return ChallengeResponse.TryParse(' ').Value;
            }

            (ChallengeResponse, ChallengeResponse, ChallengeResponse) Score(MastermindSolver.Guesses guess, IReadOnlyList<char> code)
            {
                return (ScoreSingle(0, guess.C0, code), ScoreSingle(1, guess.C1, code), ScoreSingle(2, guess.C2, code));
            }

            var counts = new List<long>();
            for (var i = 0; i < 1000; i++)
                counts.Add(0);

            var chars = new[] { '0', '1', '2', '3', '4', '5' };

            var codes = from a in chars
                        from b in chars
                        from c in chars
                        where a != b && a != c && b != c
                        select "" + a + b + c;

            foreach (var c in codes)
            {
                var code = c.ToArray();

                var solver = new MastermindSolver(chars);

                int counter = 0;
                do
                {
                    counter++;
                    var guess = solver.Guess();
                    if (guess.C0 == code[0] && guess.C1 == code[1] && guess.C2 == code[2])
                        break;
                    solver.Constrain(guess, Score(guess, code));

                } while (true);

                counts[counter]++;
                Console.WriteLine(counter);
            }

            var x = from item in counts.Select((c, i) => (c, i))
                    let i = item.i
                    let c = item.c
                    where c != 0
                    select (i, c);
            foreach (var (i, c) in x)
                Console.WriteLine($"{c} puzzles took {i}");

        }

        private static (ChallengeResponse, ChallengeResponse, ChallengeResponse) ReadChallengeResponse()
        {
            while (true)
            {
                var input = ReadString();
                if (input.Length < 3)
                    input = input.PadRight(3, ' ');

                var parsed = input.Select(ChallengeResponse.TryParse).ToArray();
                if (parsed.Any(a => a == null))
                {
                    Console.WriteLine("Not a valid input (incorrect chars, must be '!', '?' or ' ')");
                    continue;
                }

                return (parsed[0]!.Value, parsed[1]!.Value, parsed[2]!.Value);
            }
        }

        private static string ReadString()
        {
            while (true)
            {
                var r = Console.ReadLine();
                if (r == null)
                {
                    Console.WriteLine($"Not a valid input (expected non-null string)");
                    continue;
                }

                return r;
            }
        }

        private static string ReadStringLength(int length)
        {
            while (true)
            {
                var r = Console.ReadLine();
                if (r?.Length != length)
                {
                    Console.WriteLine($"Not a valid input (expected length {length}, got length {r?.Length})");
                    continue;
                }

                return r;
            }
        }
    }
}
