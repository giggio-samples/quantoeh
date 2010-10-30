using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuantoEh.Infra
{
    public abstract class DAOFila
    {
        protected byte[] Serializar(object objeto)
        {
            byte[] conteudo;
            using (var stream = new MemoryStream())
            {
                var bin = new BinaryFormatter();
                bin.Serialize(stream, objeto);
                conteudo = stream.GetBuffer();
            }
            return conteudo;
        }
    }
}