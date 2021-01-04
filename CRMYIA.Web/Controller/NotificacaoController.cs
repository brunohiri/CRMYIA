using CRMYIA.Business;
using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using CRMYIA.Data.Model;
using CRMYIA.Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Threading;

namespace CRMYIA.Web.Controller
{
    public class NotificacaoController : Hub
    {
        public readonly static List<UsuarioChatViewModel> _Connections = new List<UsuarioChatViewModel>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

        //private readonly YiaContext _context;
        //private readonly IMapper _mapper;
        //public NotificacaoController(YiaContext context, IMapper mapper)
        //{
        //    _context = context;
        //    _mapper = mapper;
        //}


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
            await Clients.All.SendAsync("Redirecionar", id, url);
        }

        //Chat
        public override Task OnConnectedAsync()
        {
            try
            {
                string IdUsuario = "";
                var identity = (ClaimsIdentity)Context.User.Identity;
                IEnumerable<Claim> Claims = identity.Claims;
                foreach (var t in Claims)
                {
                    if (t.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid"))
                        IdUsuario = t.Value;
                }
                Usuario Entity = null;
                Entity = ChatModel.GetUsuarioId(IdUsuario.ExtractLong());
                UsuarioChatViewModel UsuarioChatView = new UsuarioChatViewModel()
                {
                    IdUsuario = Entity.IdUsuario,
                    Nome = Entity.Nome,
                    Status = MensagemModel.SetStatusChat(Business.Util.EnumeradorModel.StatusChat.Ativo),
                    //EnumeradorModel.StatusChat.Ativo,
                    //Mensagem = Item.ChatIdUsuarioDeNavigation.Count() == 0 ? "" : Item.ChatIdUsuarioDeNavigation?.Select(x=> x.Mensagem).Last(),
                    //DataMensagem = DateTime.Now,
                    Imagem = Entity.CaminhoFoto + Entity.NomeFoto,
                };

                if (!_Connections.Any(u => u.Nome == IdentityName))
                {
                    _Connections.Add(UsuarioChatView);
                    _ConnectionsMap.Add(IdentityName, Context.ConnectionId);
                }

                Clients.Caller.SendAsync("getProfileInfo", Entity.Nome, Entity.CaminhoFoto + Entity.NomeFoto);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }
            return base.OnConnectedAsync();
        }

        public List<UsuarioChatViewModel> CarregarUsuarios(string Nome = "")
        {
            //return _Connections.Select(u => u).ToList();
            List<Usuario> ListUsuario = null;
            ListUsuario = ChatModel.GetUsers(Nome);
            List<UsuarioChatViewModel> ListView = new List<UsuarioChatViewModel>();
            string IdUsuario = "";
            var identity = (ClaimsIdentity)Context.User.Identity;
            IEnumerable<Claim> Claims = identity.Claims;
            foreach (var t in Claims)
            {
                if (t.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid"))
                    IdUsuario = t.Value;
            }

            Chat EntityDe = null;
            Chat EntityPara = null;
            string data = null;
            foreach (Usuario Item in ListUsuario)
            {
                if (Item.ChatIdUsuarioParaNavigation.Count() > 0 && Item.ChatIdUsuarioParaNavigation.Any(x => x.IdUsuarioDe == IdUsuario.ExtractLong())) {
                    EntityPara = Item.ChatIdUsuarioParaNavigation.Select(x => x).Where(x => x.IdUsuarioDe == IdUsuario.ExtractLong()).Last();
                    data = Item.DataCadastro.ToString("dd/MM/yyyy");
                    UsuarioChatViewModel lista = new UsuarioChatViewModel()
                    {
                        IdUsuario = Item.IdUsuario,
                        Nome = Item.Nome,
                        Mensagem = EntityPara.Mensagem,
                        DataMensagem = data,
                        Imagem = Item.CaminhoFoto + Item.NomeFoto,
                    };
                    ListView.Add(lista);
                }
                else if (Item.ChatIdUsuarioDeNavigation.Count() > 0 && Item.ChatIdUsuarioDeNavigation.Any(x => x.IdUsuarioPara == IdUsuario.ExtractLong())){
                    EntityDe = Item.ChatIdUsuarioDeNavigation.Select(x => x).Where(x => x.IdUsuarioPara == IdUsuario.ExtractLong()).Last();
                    data = Item.DataCadastro.ToString("dd/MM/yyyy");
                    UsuarioChatViewModel lista = new UsuarioChatViewModel()
                    {
                        IdUsuario = Item.IdUsuario,
                        Nome = Item.Nome,
                        Mensagem = EntityDe.Mensagem,
                        DataMensagem = data,
                        Imagem = Item.CaminhoFoto + Item.NomeFoto,
                    };
                    ListView.Add(lista);
                }
                else
                {
                    UsuarioChatViewModel lista = new UsuarioChatViewModel()
                    {
                        IdUsuario = Item.IdUsuario,
                        Nome = Item.Nome,
                        Mensagem = null,
                        DataMensagem = null,
                        Imagem = Item.CaminhoFoto + Item.NomeFoto,
                    };
                    ListView.Add(lista);
                }
            }
            return ListView;
        }
        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }
        public async Task SendPrivate(string receiverName, string message)
        {
            if (_ConnectionsMap.TryGetValue(receiverName, out string userId))
            {
                // Who is the sender;
                var sender = _Connections.Where(u => u.Nome == IdentityName).First();

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    // Build the message
                    var messageViewModel = new ChatViewModel()
                    {
                        Conteudo = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                        De = sender.Nome,
                        Imagem = sender.Imagem,
                        Para = "",
                        DataCadastro = DateTime.Now.ToLongTimeString()
                    };

                    // Send the message
                    await Clients.Client(userId).SendAsync("newMessage", messageViewModel);
                    await Clients.Caller.SendAsync("newMessage", messageViewModel);
                }
            }
        }
    }
}
