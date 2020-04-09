using System;

namespace ShellAnalyze.Tree {
    public class VariableExpression : Expression {
        public string Name { get; }

        public VariableExpression(string name) {
            Name = name;
        }

        public override ValueType GetValueType() {
            return ValueType.Undefined;
        }

        public override string ToString() {
            return string.Format("[Variable: {0}]", Name);
        }
    }
}
