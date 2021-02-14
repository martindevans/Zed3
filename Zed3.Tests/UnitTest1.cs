using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Z3;
using Zed3.Expressions;
using Zed3.Extensions;

namespace Zed3.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MkConstraint_EqualsParameter2()
        {
            using var ctx = new Context();

            var a = ctx.MkConstInt("A");
            var s = ctx.MkSolver();

            s.Assert(a * a == 9);

            Assert.AreEqual(Status.SATISFIABLE, s.Check());

            var result = (IntNum)s.Model.Eval(a.Expr, true);
            Assert.AreEqual(3, result.Int64);
        }

        [TestMethod]
        public void MethodName()
        {
            using var ctx = new Context();

            var a = ctx.MkConstInt("A");
            var s = ctx.MkSolver();

            var arr = ctx.MkArrayConst("Array", ctx.IntSort, ctx.IntSort);
            var a0 = new IntExpression((IntExpr)ctx.MkSelect(arr, ctx.MkInt(0)), ctx);
            s.Assert(a0 == 3);

            Assert.AreEqual(Status.SATISFIABLE, s.Check());

            var result = (ArrayExpr)s.Model.Eval(arr);
            var i0 = (IntNum)result.Arg(0);

            Assert.AreEqual(3, i0.Int64);
        }
    }
}
