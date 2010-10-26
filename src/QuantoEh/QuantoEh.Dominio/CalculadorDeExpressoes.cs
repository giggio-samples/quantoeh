using System;
using Boo.Lang.Interpreter;

namespace QuantoEh.Dominio
{
    public class CalculadorDeExpressoes
    {
        public double Calcular(string expressao)
        {
            var interpreter = new InteractiveInterpreter2 {RememberLastValue = true};
            var context = interpreter.Eval(expressao);
            if (context.Errors.Count > 0)
            {
                throw new ErroDeCalculoException(context.Errors[0].Message);
            }
            return Convert.ToDouble(interpreter.LastValue);
        }
    }
}