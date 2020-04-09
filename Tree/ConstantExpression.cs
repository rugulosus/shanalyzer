using System;

namespace ShellAnalyze.Tree {
    public class ConstantExpression : Expression {
        public object Value { get; }

        public ConstantExpression(object value) {
            Value = value;
        }

        public override ValueType GetValueType() {
            return Value.GetType().ToString() switch {
                "System.Int32" => ValueType.Integer,
                "System.String" => ValueType.String,
                _ => ValueType.Undefined
            };
        }

        public override string ToString() {
            return string.Format("[Constant({0}): {1}]", Value.GetType().ToString(), Value.ToString());
        }
    }
}
