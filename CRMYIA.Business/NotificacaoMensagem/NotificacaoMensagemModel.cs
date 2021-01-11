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
    public class NotificacaoMensagemModel
    {

        public static NotificacaoMensagem Add(NotificacaoMensagem Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.NotificacaoMensagem.Add(Entity);
                    context.SaveChanges();
                }
                return Entity;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static bool QuantidadeNotificacaoMensagem(long De, long Para)
        {
            int quantidade = 0;
            bool TemNotificacao = false;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                   quantidade = context.NotificacaoMensagem
                        .Where(x => x.IdUsuarioDe == De && x.IdUsuarioPara == Para && x.Ativo == true)
                        .Count();
                    if(quantidade >= 1)
                    {
                        TemNotificacao = true;
                        return TemNotificacao;
                    }
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            return TemNotificacao;
        }

        public static void AtualizarNotificacaoMensagem(long De, long Para, string Mensagem)
        {
            try
            {

                using (YiaContext context = new YiaContext())
                {
                    var notificacaoMensagem = context.NotificacaoMensagem
                   .Where(x => x.IdUsuarioDe == De && x.IdUsuarioPara == Para && x.Ativo == true).First();

                    notificacaoMensagem.DataCadastro = DateTime.Now;
                    notificacaoMensagem.Mensagem = Mensagem;

                    context.NotificacaoMensagem.Attach(notificacaoMensagem);
                    context.Entry(notificacaoMensagem).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DesativarNotificacaoMensagem(long Para,long De)
        {
            try
            {

                using (YiaContext context = new YiaContext())
                {
                    var notificacaoMensagem = context.NotificacaoMensagem
                   .Where(x => x.IdUsuarioDe == Para && x.IdUsuarioPara == De && x.Ativo == true).First();

                    notificacaoMensagem.Visualizado = true;
                    notificacaoMensagem.Ativo = false;

                    context.NotificacaoMensagem.Attach(notificacaoMensagem);
                    context.Entry(notificacaoMensagem).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<NotificacaoMensagemViewModel> ObterTodos(long IdUsuario)
        {
            List<NotificacaoMensagemViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.NotificacaoMensagem
                        .Where(x => x.IdUsuarioPara == IdUsuario && x.Ativo == true)
                        .Select(x => new NotificacaoMensagemViewModel() { 
                            De = x.IdUsuarioDe.ToString(),
                            Para = x.IdUsuarioPara.ToString(),
                            Nome = x.IdUsuarioDeNavigation.Nome,
                            Mensagem = x.Mensagem,
                            Imagem = x.IdUsuarioDeNavigation.CaminhoFoto != null && x.IdUsuarioDeNavigation.NomeFoto != null? x.IdUsuarioDeNavigation.CaminhoFoto + x.IdUsuarioDeNavigation.NomeFoto : "img/fotoCadastro/foto-cadastro.jpeg",
                            DataCadastro = Util.Util.CalculaTempo(x.DataCadastro),
                            DataOrdem = x.DataCadastro,
                            Logado = x.IdUsuarioDeNavigation != null ? x.IdUsuarioDeNavigation.Logado  : x.IdUsuarioParaNavigation.Logado,
                        })
                        .OrderByDescending(x => x.DataOrdem)
                        .Take(3)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

    }
}
