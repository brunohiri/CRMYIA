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

        public async Task NotificacaoHub(string id)
        {
            List<Notificacao> Entity = null;

            Entity = Business.NotificacaoModel.GetTodasNotificacaoId(id.ExtractLong());
            bool status;
            status = Entity.Count == 0 ? false : true;
            await Clients.All.SendAsync("ReceberNotificacao", Entity, status, id);
        }

        public async Task DesativarNotificacao(string id, string url)
        {
            List<Notificacao> Entity = null;

            
            await Clients.All.SendAsync("Redirecionar", id, url);
        }

        public async Task NotificarMensagem(string Nome, string Mensagem, string DataCadastro, string De, string Para, string Imagem)
        {
            List<NotificacaoMensagemViewModel> EntityNotificacaoMensagem = null;
            string IdUsuario = "";
            var identity = (ClaimsIdentity)Context.User.Identity;
            IEnumerable<Claim> Claims = identity.Claims;
            foreach (var t in Claims)
            {
                if (t.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid"))
                    IdUsuario = t.Value;
            }

            if (!NotificacaoMensagemModel.QuantidadeNotificacaoMensagem(De.ExtractLong(), Para.ExtractLong()))
            {
                NotificacaoMensagem EntityNotificacao = NotificacaoMensagemModel.Add(new NotificacaoMensagem()
                {
                    IdUsuarioDe = De.ExtractLong(),
                    IdUsuarioPara = Para.ExtractLong(),
                    Mensagem = Mensagem,
                    DataCadastro = DateTime.Now,
                    Visualizado = false,
                    Ativo = true
                });
            }
            else
            {
                NotificacaoMensagemModel.AtualizarNotificacaoMensagem(De.ExtractLong(), Para.ExtractLong(), Mensagem);
            }

           
            EntityNotificacaoMensagem = NotificacaoMensagemModel.ObterTodos(IdUsuario.ExtractLong());

            await Clients.All.SendAsync("ReceberNotificacaoMensagem", EntityNotificacaoMensagem);
        }

        public async Task NotificacaoMensagemHub(string Id)
        {
            List<NotificacaoMensagemViewModel> Entity = null;

            Entity = NotificacaoMensagemModel.ObterTodos(Id.ExtractLong());
            bool status;
            status = Entity.Count == 0 ? false : true;
            await Clients.All.SendAsync("ReceberNotificacaoMensagem", Entity, status, Id);
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
                    Imagem = Entity.CaminhoFoto + Entity.NomeFoto,
                };

                if (!_Connections.Any(u => u.Nome == IdentityName))
                {
                    _Connections.Add(UsuarioChatView);
                    _ConnectionsMap.Add(IdentityName, Context.ConnectionId);
                }
                var Imagem = "img/fotoCadastro/foto-cadastro.jpeg";
                if (Entity.CaminhoFoto != null && Entity.NomeFoto != null)
                {
                    Imagem = Entity.CaminhoFoto + Entity.NomeFoto;
                }

                Clients.Caller.SendAsync("getProfileInfo", Entity.Nome, Imagem);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }
            return base.OnConnectedAsync();
        }

        public List<UsuarioChatViewModel> CarregarUsuarios(string Nome = "")
        {
           
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
            string imagem = "";
            foreach (Usuario Item in ListUsuario)
            {
                if (Item.ChatIdUsuarioParaNavigation.Count() > 0 && Item.ChatIdUsuarioParaNavigation.Any(x => x.IdUsuarioDe == IdUsuario.ExtractLong())) {
                    EntityPara = Item.ChatIdUsuarioParaNavigation.Select(x => x).Where(x => x.IdUsuarioDe == IdUsuario.ExtractLong()).Last();
                    data = Item.DataCadastro.ToString("dd/MM/yyyy");
                    if(Item.CaminhoFoto == null && Item.CaminhoFoto == null)
                    {
                        imagem = "img/fotoCadastro/foto-cadastro.jpeg";
                    }
                    else
                    {
                        imagem = Item.CaminhoFoto + Item.NomeFoto;
                    }
                    UsuarioChatViewModel lista = new UsuarioChatViewModel()
                    {
                        IdUsuario = Item.IdUsuario,
                        Nome = Item.Nome,
                        Mensagem = EntityPara.Mensagem,
                        DataMensagem = data,
                        Imagem = imagem,
                    };
                    ListView.Add(lista);
                }
                else if (Item.ChatIdUsuarioDeNavigation.Count() > 0 && Item.ChatIdUsuarioDeNavigation.Any(x => x.IdUsuarioPara == IdUsuario.ExtractLong())){
                    EntityDe = Item.ChatIdUsuarioDeNavigation.Select(x => x).Where(x => x.IdUsuarioPara == IdUsuario.ExtractLong()).Last();
                    data = Item.DataCadastro.ToString("dd/MM/yyyy");
                    if (Item.CaminhoFoto == null && Item.CaminhoFoto == null)
                    {
                        imagem = "img/fotoCadastro/foto-cadastro.jpeg";
                    }
                    else
                    {
                        imagem = Item.CaminhoFoto + Item.NomeFoto;
                    }
                    UsuarioChatViewModel lista = new UsuarioChatViewModel()
                    {
                        IdUsuario = Item.IdUsuario,
                        Nome = Item.Nome,
                        Mensagem = EntityDe.Mensagem,
                        DataMensagem = data,
                        Imagem = imagem,
                    };
                    ListView.Add(lista);
                }
                else
                {
                    if (Item.CaminhoFoto == null && Item.CaminhoFoto == null)
                    {
                        imagem = "img/fotoCadastro/foto-cadastro.jpeg";
                    }
                    else
                    {
                        imagem = Item.CaminhoFoto + Item.NomeFoto;
                    }
                    UsuarioChatViewModel lista = new UsuarioChatViewModel()
                    {
                        IdUsuario = Item.IdUsuario,
                        Nome = Item.Nome,
                        Mensagem = null,
                        DataMensagem = null,
                        Imagem = imagem,
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
        public IEnumerable<ChatViewModel> GetMessageHistory(string Para, string De)
        {
            List<ChatViewModel> EntityChat = null;
            EntityChat = ChatModel.CarregaMensagem(Para.ExtractLong(), De.ExtractLong());
            
            return EntityChat;
        }
        public async Task EnviarPrivado(string De, string Para, string Nome, string Mensagem, string Imagem)
        {
            Chat Entity = null;
            ChatViewModel ChatView = null;
            if (_ConnectionsMap.TryGetValue(Nome, out string userId))
            {
                if (De != null && Para != null && Mensagem != null)
                {
                    
                    Entity = new Chat() {
                        IdUsuarioDe = De.ExtractLong(),
                        IdUsuarioPara = Para.ExtractLong(),
                        Mensagem = Mensagem,
                        DataCadastro = DateTime.Now,
                        Visualizado = false,
                        Ativo = true,
                    };
                    

                ChatModel.Add(Entity);

                    ChatView = new ChatViewModel() {
                        Nome = Nome,
                        Mensagem = Mensagem,
                        DataCadastro = Convert.ToString(DateTime.Now.ToString("dd MMM HH:mm tt")),
                        De = De,
                        Para = Para,
                        Imagem = Imagem,
                    };
                    
                    // Envia a Mensagem
                    //await Clients.Client(userId).SendAsync("novaMensagem", ChatView);
                    await Clients.All.SendAsync("novaMensagem", ChatView);
                    await Clients.Caller.SendAsync("NovaMensagemCaller", ChatView);

                }
            }
        }
        public async Task DesativarNotificacaoMensagem(string Para, string De)
        {
            bool status = false;
            List<NotificacaoMensagemViewModel> EntityNotificacaoMensagem = null;

            string IdUsuario = "";
            var identity = (ClaimsIdentity)Context.User.Identity;
            IEnumerable<Claim> Claims = identity.Claims;
            foreach (var t in Claims)
            {
                if (t.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid"))
                    IdUsuario = t.Value;
            }

            NotificacaoMensagemModel.DesativarNotificacaoMensagem(Para.ExtractLong(), De.ExtractLong());

            EntityNotificacaoMensagem = NotificacaoMensagemModel.ObterTodos(IdUsuario.ExtractLong());

            await Clients.All.SendAsync("ReceberNotificacaoMensagem", EntityNotificacaoMensagem);
        }
    }
}
