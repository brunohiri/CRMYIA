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
            Corretor = 4,
            Vendedor = 5,
            Marketing = 6
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

        #region Proposta
        public enum StatusProposta
        {
            EmAnalise = 1,
            Aceito = 2,
            Declinado = 3
        }

        public enum FaseProposta
        {
            Oportunidade = 1,
            CotacaoFiltros = 2,
            VisitaApresentacaoReuniao = 3,
            ImplantacaoDocumentacao = 4,
            Fechamento = 5
        }
        #endregion
    }
}
