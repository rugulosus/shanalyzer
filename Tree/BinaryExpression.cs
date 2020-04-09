using System;

namespace ShellAnalyze.Tree {
    public class BinaryExpression : Expression {
        public Expression Left { get; }
        public Expression Right { get; }
        public Operation Operator { get; }

        public BinaryExpression(Operation op, Expression left, Expression right) {
            Operator = op;
            Left = left;
            Right = right;
        }

        public override ValueType GetValueType() {
            if (Left.GetValueType() == Right.GetValueType()) {
                return Left.GetValueType();
            } else {
                return ValueType.Undefined;
            }
        }

        public override string ToString() {
            return string.Format("[Binary: {0}, {1}, {2}]", Left, Operator, Right);
        }
    }
}
