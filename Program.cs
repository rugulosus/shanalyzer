using System;
using Sprache;

namespace ShellAnalyze {
    class Program {
        static void Main(string[] args) {
            var result = ExpressionParser.ExprParser.Parse("1+1*-2-3 == $var1");
            Console.WriteLine(result);
        }
    }
}
