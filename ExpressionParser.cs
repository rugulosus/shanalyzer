using System;
using Sprache;
using ShellAnalyze.Tree;

namespace ShellAnalyze {
    public static class ExpressionParser {
        private static readonly Parser<Operation> Add = Parse.Char('+').Token().Return(Operation.Add);
        private static readonly Parser<Operation> Subtract = Parse.Char('-').Token().Return(Operation.Subtract);
        private static readonly Parser<Operation> Multiply = Parse.Char('*').Token().Return(Operation.Multiply);
        private static readonly Parser<Operation> Divide = Parse.Char('/').Token().Return(Operation.Divide);
        private static readonly Parser<Operation> GreaterThan = Parse.Char('>').Token().Return(Operation.GreaterThan);
        private static readonly Parser<Operation> GreaterThanOrEqual = Parse.String(">=").Token().Return(Operation.GreaterThanOrEqual);
        private static readonly Parser<Operation> LessThan = Parse.Char('<').Token().Return(Operation.LessThan);
        private static readonly Parser<Operation> LessThanOrEqual = Parse.String("<=").Token().Return(Operation.LessThanOrEqual);
        private static readonly Parser<Operation> Equal = Parse.String("==").Token().Return(Operation.Equal);
        private static readonly Parser<Operation> NotEqual = Parse.String("!=").Token().Return(Operation.NotEqual);
        private static readonly Parser<Operation> Negate = Parse.Char('-').Token().Return(Operation.Negate);

        private static readonly Parser<Expression> VariableParser =
            from symbol in Parse.Char('$')
            from name in Parse.Identifier(Parse.Letter.Or(Parse.Chars(new[] {'#', '?'})), Parse.LetterOrDigit).Token()
            select new VariableExpression(symbol.ToString() + name);
        
        private static readonly Parser<Expression> IntegerConstantParser =
            from value in Parse.Decimal
            select new ConstantExpression(int.Parse(value));

        private static readonly Parser<Expression> StringConstantParser = 
            from value in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit)
            select new ConstantExpression(value);

        private static readonly Parser<Expression> QuotedStringConstantParser =
            from quote in Parse.Chars(new[] {'"', '\''})
            from value in Parse.CharExcept(quote).Or(
                from escape in Parse.Char('\\')
                from q in Parse.Char(quote)
                select q
            ).Many().Text()
            from end in Parse.Char(quote)
            select new ConstantExpression(quote.ToString() + value + quote.ToString());

        private static readonly Parser<Expression> FactorParser = VariableParser.XOr(IntegerConstantParser).Or(StringConstantParser);

        private static Parser<Expression> OperatorParser(Parser<Operation> operatorParser, Parser<Expression> operandParser) {
            return (
                from left in operandParser
                from op in operatorParser
                from right in OperatorParser(operatorParser, operandParser)
                select new BinaryExpression(op, left, right)
            ).Or(operandParser);
        }

        private static readonly Parser<Expression> OperatorPriority2 = (
            from sign in Negate.Optional()
            from factor in FactorParser
            select sign.IsDefined ? new UnaryExpression(sign.Get(), factor) : factor
            ).Token();

        private static readonly Parser<Expression> OperatorPriority3 = OperatorParser(Multiply.Or(Divide), OperatorPriority2);
        private static readonly Parser<Expression> OperatorPriority4 = OperatorParser(Add.Or(Subtract), OperatorPriority3);
        private static readonly Parser<Expression> OperatorPriority6 = OperatorParser(GreaterThanOrEqual.Or(LessThanOrEqual).Or(GreaterThan).Or(LessThan), OperatorPriority4);
        private static readonly Parser<Expression> OperatorPriority7 = OperatorParser(NotEqual.Or(Equal), OperatorPriority6);
        public static readonly Parser<Expression> ExprParser = OperatorPriority7;
    }
}
