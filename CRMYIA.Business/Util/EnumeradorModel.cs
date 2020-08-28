using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Business.Util
{
    public class EnumeradorModel
    {
        public enum TipoMensagem
        {
            Erro = 0,
            Sucesso = 1,
            Aviso = 2,
            Info = 3
        }

        public enum TipoClick
        {
            Anuncio = 1,
            Telefone = 2,
            Mensagem = 3
        }
    }
}
