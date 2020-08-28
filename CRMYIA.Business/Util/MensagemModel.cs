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
        #endregion

        #region Construtores
        public MensagemModel(EnumeradorModel.TipoMensagem _TipoMensagem, string _Mensagem, string _Titulo = null)
        {
            TipoMensagem = _TipoMensagem;
            Mensagem = _Mensagem;
            CssClass = ExtractCssClass(_TipoMensagem);
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
        #endregion
    }
}
