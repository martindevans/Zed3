using Microsoft.Z3;

namespace Zed3.Expressions
{
    public class BoolExpression
    {
        private readonly Context _ctx;

        public BoolExpr Expr { get; }

        public BoolExpression(BoolExpr expr, Context ctx)
        {
            _ctx = ctx;
            Expr = expr;
        }

        public override int GetHashCode()
        {
            return unchecked((int)Expr.Id);
        }

        public override bool Equals(object obj)
        {
            if (obj is BoolExpression ie)
                return ie.Expr.Equals(Expr);
            else
                return false;
        }

        #region not
        public static BoolExpression operator !(BoolExpression a) => new(a._ctx.MkNot(a.Expr), a._ctx);
        #endregion

        #region or
        public static BoolExpression operator |(BoolExpression a, BoolExpression b)
            => new(a._ctx.MkOr(a.Expr, b.Expr), a._ctx);

        public static BoolExpression operator |(BoolExpression a, bool b)
            => new(a._ctx.MkOr(a.Expr, a._ctx.MkBool(b)), a._ctx);
        #endregion

        #region and
        public static BoolExpression operator &(BoolExpression a, BoolExpression b)
            => new(a._ctx.MkAnd(a.Expr, b.Expr), a._ctx);

        public static BoolExpression operator &(BoolExpression a, bool b)
            => new(a._ctx.MkAnd(a.Expr, a._ctx.MkBool(b)), a._ctx);
        #endregion

        #region xor
        public static BoolExpression operator ^(BoolExpression a, BoolExpression b)
            => new(a._ctx.MkXor(a.Expr, b.Expr), a._ctx);

        public static BoolExpression operator ^(BoolExpression a, bool b)
            => new(a._ctx.MkXor(a.Expr, a._ctx.MkBool(b)), a._ctx);
        #endregion
    }
}
