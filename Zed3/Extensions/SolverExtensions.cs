using Microsoft.Z3;
using Zed3.Expressions;

namespace Zed3.Extensions
{
    public static class SolverExtensions
    {
        public static void Assert(this Solver s, BoolExpression expr)
        {
            s.Assert(expr.Expr);
        }
    }
}
