using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
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
                        .Where(x => x.IdUsuarioVisualizar == IdUsuario)
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
