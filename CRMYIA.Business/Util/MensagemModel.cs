using CRMYIA.Business.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.Model
{
    public partial class MensagemModel
    {
        #region Propriedades
        public EnumeradorModel.TipoMensagem TipoMensagem { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string CssClass { get; set; }
        public string IconClass { get; set; }
        #endregion

        #region Construtores
        public MensagemModel(EnumeradorModel.TipoMensagem _TipoMensagem, string _Mensagem, string _Titulo = null)
        {
            TipoMensagem = _TipoMensagem;
            Mensagem = _Mensagem;
            CssClass = ExtractCssClass(_TipoMensagem);
            IconClass = ExtractIconClass(_TipoMensagem);
            Titulo = _Titulo.IsNullOrEmpty() ? ExtractTitulo(_TipoMensagem) : _Titulo;
        }
        #endregion

        #region Métodos
        public static string ExtractCssClass(EnumeradorModel.TipoMensagem TipoMensagem)
        {
            string CssClass = string.Empty;
            switch (TipoMensagem)
            {
                case EnumeradorModel.TipoMensagem.Erro:
                    CssClass = "alert alert-danger";
                    break;
                case EnumeradorModel.TipoMensagem.Sucesso:
                    CssClass = "alert alert-success";
                    break;
                case EnumeradorModel.TipoMensagem.Aviso:
                    CssClass = "alert alert-warning";
                    break;
                case EnumeradorModel.TipoMensagem.Info:
                    CssClass = "alert alert-info";
                    break;
                default:
                    break;
            }
            return CssClass;
        }

        public static string ExtractIconClass(EnumeradorModel.TipoMensagem TipoMensagem)
        {
            string IconClass = string.Empty;
            switch (TipoMensagem)
            {
                case EnumeradorModel.TipoMensagem.Erro:
                    IconClass = "icon fas fa-ban";
                    break;
                case EnumeradorModel.TipoMensagem.Sucesso:
                    IconClass = "icon fas fa-check";
                    break;
                case EnumeradorModel.TipoMensagem.Aviso:
                    IconClass = "icon fas fa-info";
                    break;
                case EnumeradorModel.TipoMensagem.Info:
                    IconClass = "icon fas fa-exclamation-triangle";
                    break;
                default:
                    break;
            }
            return IconClass;
        }

        public static string ExtractTitulo(EnumeradorModel.TipoMensagem TipoMensagem)
        {
            string Titulo = string.Empty;
            switch (TipoMensagem)
            {
                case EnumeradorModel.TipoMensagem.Erro:
                    Titulo = "Erro";
                    break;
                case EnumeradorModel.TipoMensagem.Sucesso:
                    Titulo = "Sucesso";
                    break;
                case EnumeradorModel.TipoMensagem.Aviso:
                    Titulo = "Aviso";
                    break;
                case EnumeradorModel.TipoMensagem.Info:
                    Titulo = "Informação";
                    break;
                default:
                    break;
            }
            return Titulo;
        }

        public static string SetStatusChat(EnumeradorModel.StatusChat StatusChat)
        {
            string Status = string.Empty;
            switch (StatusChat)
            {
                case EnumeradorModel.StatusChat.Ativo:
                    Status = "success";
                    break;
                case EnumeradorModel.StatusChat.Ausente:
                    Status = "warning";
                    break;
                case EnumeradorModel.StatusChat.NaoIncomodar:
                    Status = "danger";
                    break;
                case EnumeradorModel.StatusChat.Invisivel:
                    Status = "light";
                    break;
                default:
                    break;
            }
            return Status;
        }
        #endregion
    }
}
