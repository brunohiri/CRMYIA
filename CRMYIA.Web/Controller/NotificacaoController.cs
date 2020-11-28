using CRMYIA.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMYIA.Web.Controller
{
    public class NotificacaoController : Hub
    {
        public async Task NotificacaoHub(string id)
        {
            //if (Entity.IdNotificacao == null)
            //{
            //    await Clients.All.SendAsync("ReceberNotificacao", retorno);

            //}

            List<Notificacao> Entity = null;

            Entity = Business.NotificacaoModel.GetTodasNotificacaoId(Convert.ToInt32(id));
            bool status;
            status = Entity.Count == 0 ? false : true;
            //var retorno = new
            //{
            //    descricao = Entity.Descricao,
            //    url = Entity.Url,
            //    data = Entity.DataCadastro
            //};
            await Clients.All.SendAsync("ReceberNotificacao", Entity, status, id);
        }
    }
}
