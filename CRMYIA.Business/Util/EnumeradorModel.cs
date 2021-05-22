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
            Marketing = 6,
            Todos = 7
        }
        #endregion

        #region Fornecedor
        public enum Fornecedor
        {
            ShiftData = 1
        }
        #endregion

        #region StatusVisita
        public enum StatusVisita
        {
            Pendente = 1,
            Agendada = 2,
            Realizada = 3,
            Adiada = 4,
            Cancelada = 5,
            FeriadoDataSazonal = 6
        }
        public enum Repete
        {
            Nunca = 1,
            TodosDias = 2,
            ACadaSemana = 3,
            ACada2Semanas = 4,
            ACadaMeses = 5,
            ACadaAno = 6,
            Personalizado = 7
        }
        public enum Frequencia
        {
            Diariamente = 1,
            Semanalmente  =2,
            Mensalmente = 3,
            Anualmente = 4
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

        #region Chat
        public enum StatusChat
        {
            Ativo,
            Ausente,
            NaoIncomodar,
            Invisivel
        }
        #endregion

        #region TipoSazonal
        public enum TipoSazonal
        {
            Feriado = 1,
            DataComemorativa = 2,
            Evento = 3
        }
        #endregion

        #region TipoSazonal
        public enum Visualizacao
        {
            Todos = 1,
            DataComemorativa = 2,
            Administrador = 3,
            Gerente = 4,
            Supervisor = 5,
            Corretor = 6,
            Vendedor = 7,
            Marketing = 8
        }
        #endregion
    }
}
