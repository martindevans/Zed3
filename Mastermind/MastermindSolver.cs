using System;
using System.Collections.Generic;
using Microsoft.Z3;
using Zed3.Expressions;
using Zed3.Extensions;
using System.Linq;

namespace Mastermind
{
    class MastermindSolver
        : IDisposable
    {
        private readonly IReadOnlyList<char> _chars;

        private readonly IntExpression[] _secrets;

        private readonly Context _ctx;
        private readonly Solver _solver;

        public MastermindSolver(IReadOnlyList<char> chars)
        {
            _chars = chars;

            _ctx = new Context();
            _solver = _ctx.MkSolver();

            // Create one variable for each character in the solution
            _secrets = new[] {
                _ctx.MkConstInt("Character 0"),
                _ctx.MkConstInt("Character 1"),
                _ctx.MkConstInt("Character 2")
            };

            // Each character is distinct in solution
            _solver.Assert(_ctx.MkDistinct(_secrets));

            // Restrict characters to the correct range
            foreach (var secret in _secrets)
            {
                _solver.Assert(secret >= 0);
                _solver.Assert(secret < _chars.Count);
            }
        }

        public Guesses Guess()
        {
            var sat = _solver.Check();
            if (sat != Status.SATISFIABLE)
                throw new InvalidOperationException("Unsatisfiable");

            var r0 = (int)_secrets[0].Eval(_solver);
            var r1 = (int)_secrets[1].Eval(_solver);
            var r2 = (int)_secrets[2].Eval(_solver);

            return new Guesses(_chars, r0, r1, r2);
        }

        public void Constrain(Guesses guesses, (ChallengeResponse, ChallengeResponse, ChallengeResponse) results)
        {
            // Constrain solution to say it definitely wasn't the previous guess
            if (results.ToEnumerable().Any(a => a.Response != CharResponse.RightCharRightPos))
                _solver.Assert(!(_secrets[0] == guesses[0] & _secrets[1] == guesses[1] & _secrets[2] == guesses[2]));

            Func<int, BoolExpression> MapChallengeResponse(ChallengeResponse cr)
            {
                return cr.Response switch {
                    CharResponse.RightCharWrongPos => OneEquals,
                    CharResponse.RightCharRightPos => Correct,
                    CharResponse.WrongChar => NoneEquals,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            _solver.Assert(Accumulate(
                MapChallengeResponse(results.Item1),
                MapChallengeResponse(results.Item2),
                MapChallengeResponse(results.Item3)
            ));

            // Accumulate all possible permutations
            BoolExpression Accumulate(Func<int, BoolExpression> a, Func<int, BoolExpression> b, Func<int, BoolExpression> c)
            {
                var acc = (a(0) & b(1) & c(2))
                        | (a(0) & b(2) & c(1))
                        | (a(1) & b(2) & c(0))
                        | (a(1) & b(0) & c(2))
                        | (a(2) & b(0) & c(1))
                        | (a(2) & b(1) & c(0));
                return acc;
            }

            // Helpers to constrain based on a single response
            BoolExpression OneEquals(int index) => (_secrets[0] == guesses[index] ^ _secrets[1] == guesses[index] ^ _secrets[2] == guesses[index]) & (_secrets[index] != guesses[index]);
            BoolExpression NoneEquals(int index) => _secrets[0] != guesses[index] & _secrets[1] != guesses[index] & _secrets[2] != guesses[index];
            BoolExpression Correct(int index) => _secrets[index] == guesses[index];
        }

        public void Dispose()
        {
            _solver?.Dispose();
            _ctx?.Dispose();
        }

        public readonly struct Guesses
        {
            private readonly IReadOnlyList<char> _chars;
            public char C0 => _chars[I0];
            public char C1 => _chars[I1];
            public char C2 => _chars[I2];

            internal readonly int I0;
            internal readonly int I1;
            internal readonly int I2;

            internal int this[int index] => index switch {
                0 => I0,
                1 => I1,
                2 => I2,
                _ => throw new IndexOutOfRangeException("Index must be >= 0 and < 3")
            };

            public Guesses(IReadOnlyList<char> chars, int i0, int i1, int i2)
            {
                _chars = chars;
                I0 = i0;
                I1 = i1;
                I2 = i2;
            }
        }
    }
}
