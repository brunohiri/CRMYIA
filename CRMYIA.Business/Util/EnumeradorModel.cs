using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Business.Util
{
    public class EnumeradorModel
    {
        #region Configurações
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

        public enum PasswordStrength
        {
            Inaceitavel,
            Fraca,
            Aceitavel,
            Forte,
            Segura
        }
        #endregion

        #region Perfil
        public enum Perfil
        {
            Administrador = 1,
            Gerente = 2,
            Supervisor = 3,
            Corretor = 4
        }
        #endregion


        #region StatusVisita
        public enum StatusVisita
        {
            Pendente = 1,
            Agendada = 2,
            Realizada = 3,
            Adiada = 4,
            Cancelada = 5
        }
        #endregion
    }
}
