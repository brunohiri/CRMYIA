using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRMYIA.Business
{
    public class KPIGrupoUsuarioModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static KPIGrupoUsuario Get(long IdKPIGrupo)
        {
            KPIGrupoUsuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIGrupoUsuario
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdKPIGrupo == IdKPIGrupo)
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

        public static List<KPIGrupoUsuario> GetList()
        {
            List<KPIGrupoUsuario> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPIGrupoUsuario
                        .Where(x => x.Ativo)
                        .AsNoTracking()
                        .AsNoTracking()
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static void Add(KPIGrupoUsuario Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIGrupoUsuario.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(KPIGrupoUsuario Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIGrupoUsuario.Update(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
