using System;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Interpreter;

namespace QuantoEh.Dominio
{
    public class CalculadorDeExpressoes
    {
        public double Calcular(string expressao)
        {
            var interpreter = new InteractiveInterpreter {RememberLastValue = true};
            var contexto = interpreter.Parse(new StringInput("expressao", expressao));
            contexto.CompileUnit.Accept(new MethodInvocationFilter());
            var contexto2 = interpreter.EvalCompileUnit(contexto.CompileUnit);
            //var context = interpreter.Eval(expressao);
            if (contexto2.Errors.Count > 0)
            {
                throw new ErroDeCalculoException(contexto2.Errors[0].Message);
            }
            return Convert.ToDouble(interpreter.LastValue);
        }
        public class MethodInvocationFilter : DepthFirstVisitor
        {
            public override void LeaveMethodInvocationExpression(MethodInvocationExpression node)
            {
                throw new ArgumentException("Expressão inválida: " + node);
            }
        }
    }
}