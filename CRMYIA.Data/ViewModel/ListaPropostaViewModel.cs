using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class ListaPropostaViewModel
    {
        public long IdProposta { get; set; }
        public string DescricaoModalidade { get; set; }
        public string NomeCliente { get; set; }
        public string NomeCorretor { get; set; }
        public string DescricaoFaseProposta { get; set; }
        public decimal? ValorPrevisto { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime DataCadastro { get; set; }
        public string DescricaoStatusProposta { get; set; }
        public string NomeHistoricoProposta { get; set; }
        public bool? UsuarioMasterSlave { get; set; }

        /*
            <td>@(Item.IdModalidadeNavigation.Descricao.Length > 30 ? Item.IdModalidadeNavigation.Descricao.Substring(0,30) + "..." : Item.IdModalidadeNavigation.Descricao)</td>
            
            <td>@(Item.IdClienteNavigation.Nome.Length > 30 ? Item.IdClienteNavigation.Nome.Substring(0,30) + "..." : Item.IdClienteNavigation.Nome)</td>
            <td>@(Item.IdUsuarioCorretorNavigation == null ? "":Item.IdUsuarioCorretorNavigation.Nome.Length > 30 ? Item.IdUsuarioCorretorNavigation.Nome.Substring(0,30) + "..." : Item.IdUsuarioCorretorNavigation.Nome)</td>
            <td>@(Item.IdFasePropostaNavigation.Descricao.Length > 30 ? Item.IdFasePropostaNavigation.Descricao.Substring(0,30) + "..." : Item.IdFasePropostaNavigation.Descricao)</td>
            <td style="text-align:right;">@(string.Format("{0:c2}", Convert.ToDecimal(Item.ValorPrevisto)))</td>
            <td>@(Item.IdUsuarioNavigation.Nome.Length > 30 ? Item.IdUsuarioNavigation.Nome.Substring(0,30) + "..." : Item.IdUsuarioNavigation.Nome)</td>
            <td>@Item.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss")</td>
            <td>@(Item.IdStatusPropostaNavigation.Descricao.Length > 30 ? Item.IdStatusPropostaNavigation.Descricao.Substring(0,30) + "..." : Item.IdStatusPropostaNavigation.Descricao)</td>
            <td>@(Item.HistoricoProposta.Where(x => x.UsuarioMasterSlave == false && Item.IdProposta == x.IdProposta))</td>
            <td>
                <a asp-page="/NovaProposta" asp-route-id="@System.Web.HttpUtility.UrlEncode(Criptography.Encrypt(Item.IdProposta.ToString()))" title="Editar" class="text-info"><i class="icon fas fa-edit"></i></a>
            </td>
         */
    }
}
