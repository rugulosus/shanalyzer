using System;

namespace ShellAnalyze.Tree {
    public class UnaryExpression : Expression {
        public Expression Factor { get; }
        public Operation Operator { get; }

        public UnaryExpression(Operation op, Expression factor) {
            Operator = op;
            Factor = factor;
        }

        public override ValueType GetValueType() {
            return Factor.GetValueType();
        }

        public override string ToString() {
            return string.Format("[Unary: {0}, {1}]", Operator, Factor);
        }
    }
}
