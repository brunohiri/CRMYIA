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
        public static List<Chat> CarregaMensagem(long Para, long De, int Limit = 0, int NumeroDeMensagem = 0)
        {
            //List<Chat> ListEntityPara = null;
            //List<Chat> ListEntityDe = null;
            List<Chat> ListEntity = null;
            int ingnorar = CountConsulta(Para, De, Limit, NumeroDeMensagem) >= 4 ? CountConsulta(Para, De, Limit, NumeroDeMensagem) - 4 : CountConsulta(Para, De, Limit, NumeroDeMensagem);
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Chat
                        .Include(x => x.IdUsuarioDeNavigation)
                        .Include(x => x.IdUsuarioParaNavigation)
                        .AsNoTracking()
                        .Where(x => ((x.IdUsuarioPara == Para && x.IdUsuarioDe == De) || (x.IdUsuarioPara == De && x.IdUsuarioDe == Para)))
                        .OrderBy(x => x.DataCadastro)
                        .Skip(ingnorar)
                        .Take(Limit)
                        .ToList();
                }

                //using (YiaContext context = new YiaContext())
                //{
                //    ListEntityDe = context.Chat
                //        .Where(x => ((x.IdUsuarioPara == De && x.IdUsuarioDe == Para)))
                //        .Skip(NumeroDeMensagem)
                //        .Take(Limit)
                //        .OrderByDescending(x => x.DataCadastro)
                //        .ToList();
                //}
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
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

        //public static List<UsuarioChatViewModel> GetUsers(long IdUsuario)
        //{
        //    List<Usuario> ListUsuario = null;
        //    List<UsuarioChatViewModel> ListUsuarioChat = null;
        //    try
        //    {
        //        ListUsuario = ListarUsuarios();
        //        foreach (Usuario itemU in ListUsuario)
        //        {
        //            ListUsuarioChat.Add(new UsuarioChatViewModel()
        //            {
        //                Nome = itemU.Nome,
        //            });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return ListUsuarioChat;
        //}

        //public static List<UsuarioChatViewModel> GetUsers(long IdUsuario)
        //{
        //    List<UsuarioChatViewModel> ListUsuarioChatViewModel = null;
        //    UsuarioChatViewModel UsuarioChatViewModel;
        //    List<Usuario> ListUsuario = null;
        //    List<Chat> ListChat = null;
        //    DateTime? Data = null;
        //    string Msg = null;
        //    int i = 0;
        //    try
        //    {
        //        using (YiaContext context = new YiaContext())
        //        {

        //            ListUsuario = context.Usuario
        //                .Select(x => x).ToList();

        //            ListChat = context.Chat
        //                .Select(x => x).ToList();




        //            //ListEntity = context.Usuario
        //            //    .Include(x => x.ChatIdUsuarioDeNavigation)
        //            //    .Include(x => x.ChatIdUsuarioParaNavigation)
        //            //    .Select(x => new UsuarioChatViewModel() { 
        //            //        Nome = x.Nome,
        //            //        Imagem = x.CaminhoFoto + x.NomeFoto,
        //            //        Mensagem = (x.ChatIdUsuarioDeNavigation.Where(x => x.DataCadastro.Value != ).Count() > 0 ? x.:),
        //            //        DataMensagem = x.DataCadastro
        //            //    })
        //            //    .AsNoTracking()
        //            //    .ToList();
        //        }

        //        foreach (var itemU in ListUsuario)
        //        {
        //            foreach (var itemCh in ListChat)
        //            {
        //                if ((itemU.IdUsuario == itemCh.IdUsuarioDe || itemU.IdUsuario == itemCh.IdUsuarioPara) && itemCh.IdUsuarioPara == IdUsuario)
        //                {
        //                    Data = ListChat.Select(x => x.DataCadastro).Last();
        //                    Msg = ListChat.Select(x => x.Mensagem).Last();
        //                    i++;
        //                }
        //            }
        //            UsuarioChatViewModel = new UsuarioChatViewModel
        //            {
        //                Nome = itemU.Nome,
        //                Imagem = "dfgdgbdf",
        //                Mensagem = Msg,
        //                DataMensagem = Data
        //            };
        //            ListUsuarioChatViewModel.Add(UsuarioChatViewModel);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return ListUsuarioChatViewModel;
        //}


    }
}
