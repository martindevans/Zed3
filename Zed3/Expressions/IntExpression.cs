using System;
using System.Numerics;
using Microsoft.Z3;

namespace Zed3.Expressions
{
    public class IntExpression
    {
        private readonly Context _ctx;

        public IntExpr Expr { get; }

        public IntExpression(IntExpr expr, Context ctx)
        {
            _ctx = ctx;
            Expr = expr;
        }

        public BigInteger Eval(Solver solver) => ((IntNum)solver.Model.Eval(Expr, true)).BigInteger;

        public override int GetHashCode()
        {
            return unchecked((int)Expr.Id);
        }

        public override bool Equals(object obj)
        {
            if (obj is IntExpression ie)
                return ie.Expr.Equals(Expr);
            else
                return false;
        }

        /// <summary>
        /// Check if the context of two expressions is the same. Throw an `ArgumentException` if not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>always returns null if it doesn't throw.</returns>
        private static T? CheckCtx<T>(IntExpression a, IntExpression b)
            where T : class
        {
            if (!a._ctx.Equals(b._ctx))
                throw new ArgumentException("Context for `a` and `b` are not the same", nameof(b));

            return null;
        }

        #region equality
        public static BoolExpression operator ==(IntExpression a, IntExpression b)
            => CheckCtx<BoolExpression>(a, b) ?? new(a._ctx.MkEq(a.Expr, b.Expr), a._ctx);

        public static BoolExpression operator ==(IntExpression a, int b)
            => new(a._ctx.MkEq(a.Expr, a._ctx.MkInt(b)), a._ctx);
        #endregion

        #region not equal
        public static BoolExpression operator !=(IntExpression a, IntExpression b)
            => !(a == b);

        public static BoolExpression operator !=(IntExpression a, int b)
            => !(a == b);
        #endregion

        #region gteq
        public static BoolExpression operator >=(IntExpression a, IntExpression b)
            => CheckCtx<BoolExpression>(a, b) ?? new(a._ctx.MkGe(a.Expr, b.Expr), a._ctx);

        public static BoolExpression operator >=(IntExpression a, int b)
            => new(a._ctx.MkGe(a.Expr, a._ctx.MkInt(b)), a._ctx);
        #endregion

        #region lteq
        public static BoolExpression operator <=(IntExpression a, IntExpression b)
            => CheckCtx<BoolExpression>(a, b) ?? new(a._ctx.MkLe(a.Expr, b.Expr), a._ctx);

        public static BoolExpression operator <=(IntExpression a, int b)
            => new(a._ctx.MkLe(a.Expr, a._ctx.MkInt(b)), a._ctx);
        #endregion

        #region gt
        public static BoolExpression operator >(IntExpression a, IntExpression b)
            => CheckCtx<BoolExpression>(a, b) ?? new(a._ctx.MkGt(a.Expr, b.Expr), a._ctx);

        public static BoolExpression operator >(IntExpression a, int b)
            => new(a._ctx.MkGt(a.Expr, a._ctx.MkInt(b)), a._ctx);
        #endregion

        #region lt
        public static BoolExpression operator <(IntExpression a, IntExpression b)
            => CheckCtx<BoolExpression>(a, b) ?? new(a._ctx.MkLt(a.Expr, b.Expr), a._ctx);

        public static BoolExpression operator <(IntExpression a, int b)
            => new(a._ctx.MkLt(a.Expr, a._ctx.MkInt(b)), a._ctx);
        #endregion

        #region multiply
        public static IntExpression operator *(IntExpression a, IntExpression b)
            => CheckCtx<IntExpression>(a, b) ?? new((IntExpr)a._ctx.MkMul(a.Expr, b.Expr), a._ctx);

        public static IntExpression operator *(IntExpression a, int b)
            => new((IntExpr)a._ctx.MkMul(a.Expr, a._ctx.MkInt(b)), a._ctx);
        #endregion

    }
}
