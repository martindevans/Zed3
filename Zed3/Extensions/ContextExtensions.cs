using System.Collections.Generic;
using System.Linq;
using Microsoft.Z3;
using Zed3.Expressions;

namespace Zed3.Extensions
{
    public static class ContextExtensions
    {
        public static IntExpression MkConstInt(this Context ctx, string name)
            => new((IntExpr)ctx.MkConst(name, ctx.IntSort), ctx);

        public static BoolExpression MkDistinct(this Context ctx, IEnumerable<IntExpression> expressions)
            => new(ctx.MkDistinct(expressions.Select(a => (Expr)a.Expr).ToArray()), ctx);

    }
}
