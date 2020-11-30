using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business
{
    public class NotificacaoModel
    {

        public static Notificacao Add( Notificacao Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Notificacao.Add(Entity);
                    context.SaveChanges();
                }
                return Entity;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public static List<Notificacao> GetTodasNotificacaoId(int IdUsuario)
        {
            List<Notificacao> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Notificacao
                        .Where(x => ((x.IdUsuarioVisualizar == IdUsuario)))
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void DesativarNotificacao(long IdNotificacao)
        {
            try
            {
                
                using (YiaContext context = new YiaContext())
                {
                    var notificacao = context.Notificacao
                   .Where(x => x.IdNotificacao == IdNotificacao).First();

                    notificacao.Ativo = false;
                    notificacao.Visualizado = true;
                    context.Notificacao.Attach(notificacao);
                    context.Entry(notificacao).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
