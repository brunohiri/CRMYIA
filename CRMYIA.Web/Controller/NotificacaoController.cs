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

        public async Task Chat(string id, string url)
        {
            List<Notificacao> Entity = null;

            //Entity = Business.NotificacaoModel.GetTodasNotificacaoId(Convert.ToInt32(id));
            //bool status;
            //status = Entity.Count == 0 ? false : true;
            await Clients.All.SendAsync("ReceberChat", id, url);
        }

        public async Task NotificacaoHub(string id)
        {
            List<Notificacao> Entity = null;

            Entity = Business.NotificacaoModel.GetTodasNotificacaoId(Convert.ToInt32(id));
            bool status;
            status = Entity.Count == 0 ? false : true;
            await Clients.All.SendAsync("ReceberNotificacao", Entity, status, id);
        }

        public async Task DesativarNotificacao(string id, string url)
        {
            List<Notificacao> Entity = null;

            //Entity = Business.NotificacaoModel.GetTodasNotificacaoId(Convert.ToInt32(id));
            //bool status;
            //status = Entity.Count == 0 ? false : true;
            await Clients.All.SendAsync("Redirecionar", id , url);
        }
    }
}
