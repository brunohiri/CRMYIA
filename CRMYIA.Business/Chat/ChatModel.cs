using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business
{
    public class ChatModel
    {

        public static Chat Add(Chat Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Chat.Add(Entity);
                    context.SaveChanges();
                }
                return Entity;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static int CountConsulta(long Para, long De, int Limit = 0, int NumeroDeMensagem = 0)
        {
            int retorno;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    retorno = context.Chat.Count(x => ((x.IdUsuarioPara == Para && x.IdUsuarioDe == De) || (x.IdUsuarioPara == De && x.IdUsuarioDe == Para)));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return retorno;
        }
        public static List<ChatViewModel> CarregaMensagem(long Para, long De/*, int Limit = 0, int NumeroDeMensagem = 0*/)
        {
            List<ChatViewModel> ListChatEntity = null;
           try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListChatEntity = context.Chat
                       .Include(x => x.IdUsuarioDeNavigation)
                       .Include(x => x.IdUsuarioParaNavigation)
                       .Where(x => (x.IdUsuarioPara == Para && x.IdUsuarioDe == De) || (x.IdUsuarioPara == De && x.IdUsuarioDe == Para))
                       .OrderBy(x => x.DataCadastro)
                       .Select(x => new ChatViewModel() {
                           Nome = x.IdUsuarioDeNavigation != null? x.IdUsuarioDeNavigation.Nome: x.IdUsuarioParaNavigation.Nome,
                           Mensagem = x.Mensagem,
                           De = Convert.ToString(x.IdUsuarioDe),
                           Imagem = x.IdUsuarioDeNavigation != null?
                           x.IdUsuarioDeNavigation.CaminhoFoto != null && x.IdUsuarioDeNavigation.NomeFoto != null? x.IdUsuarioDeNavigation.CaminhoFoto + x.IdUsuarioDeNavigation.NomeFoto: "img/fotoCadastro/foto-cadastro.jpeg":
                           x.IdUsuarioParaNavigation.CaminhoFoto != null && x.IdUsuarioParaNavigation.NomeFoto != null ? x.IdUsuarioParaNavigation.CaminhoFoto + x.IdUsuarioParaNavigation.NomeFoto : "img/fotoCadastro/foto-cadastro.jpeg",
                           Para = Convert.ToString(x.IdUsuarioPara),
                           DataCadastro = Convert.ToString(x.DataCadastro.ToString("dd MMM HH:mm tt")),//23 Jan 2:00 pm
                       })
                       //.Skip(ingnorar)
                       //.Take(Limit)
                       .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListChatEntity;
        }

        public static List<Usuario> GetUsers(string Nome = "")
        {
            List<Usuario> ListUsuario = null;
           
            try
            {
                using (YiaContext context = new YiaContext())
                {
                   
                  
                    ListUsuario =
                    context.Usuario
                    .Where(x => x.Nome.Contains(Nome))
                    //.OrderBy(x => x.Nome)
                    .Include(i => i.ChatIdUsuarioDeNavigation)
                    .Include(i => i.ChatIdUsuarioParaNavigation)
                    //.Skip(NumeroDeLinha)
                    .Take(500)
                    .ToList();
                 
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListUsuario;
        }

        public static Usuario GetUsuarioId(long IdUsuario)
        {
            Usuario Entity = null;

            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity =
                    context.Usuario
                    .Where(x => x.IdUsuario == IdUsuario)
                    .AsNoTracking()
                    .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
    }
}
