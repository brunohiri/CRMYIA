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
    public class KPIGrupoModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static KPIGrupo Get(long IdUsuario)
        {
            KPIGrupo Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIGrupo
                        .AsNoTracking()
                        .Where(x => x.Ativo && x.IdUsuario == IdUsuario)
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
        public static KPIGrupo GetByKPIGrupo(long IdKPIGrupo)
        {
            KPIGrupo Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIGrupo
                        .Where(x => x.Ativo && x.IdKPIGrupo == IdKPIGrupo)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static List<KPIGrupo> GetList()
        {
            List<KPIGrupo> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.KPIGrupo
                        .Include(y => y.KPIMeta)
                        .Where(x => x.Ativo)
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

        public static void Add(KPIGrupo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIGrupo.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void Excluir(KPIGrupo Entity)
        {
            KPIGrupo bancoEntity = null;
            List<KPIGrupoUsuario> bancoEntityUsuarios = null;
            KPIMeta thisMeta = null;
            KPIMetaValor thisKPIMetaValor = null;
            KPIMetaVida thisKPIMetaVida = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    bancoEntity = context.KPIGrupo.Where(x => x.IdKPIGrupo == Entity.IdKPIGrupo && x.Ativo == true).FirstOrDefault();
                    thisMeta = context.KPIMeta.Where(x => x.IdKPIGrupoNavigation.IdKPIGrupo == bancoEntity.IdKPIGrupo && x.Ativo == true).FirstOrDefault();
                    bancoEntityUsuarios = context.KPIGrupoUsuario.Where(x => x.IdKPIGrupo == Entity.IdKPIGrupo && x.Ativo == true).ToList();
                    if (thisMeta != null)
                    {
                        thisKPIMetaValor = context.KPIMetaValor.Where(x => x.IdMeta == thisMeta.IdMeta && x.Ativo == true).FirstOrDefault();
                        thisKPIMetaVida = context.KPIMetaVida.Where(x => x.IdMeta == thisMeta.IdMeta && x.Ativo == true).FirstOrDefault();
                    }
                    if (bancoEntity != null)
                    {
                        if (bancoEntityUsuarios != null)
                            foreach (var item in bancoEntityUsuarios)
                            {
                                item.Motivo = "Exclusão grupo " + Entity.Nome.ToString();
                                item.Saida = DateTime.Now;
                                item.Ativo = false;
                                context.KPIGrupoUsuario.Update(item);
                            }

                        if (thisMeta != null)
                            thisMeta.Ativo = false;
                        if (thisKPIMetaValor != null)
                            thisKPIMetaValor.Ativo = false;
                        if (thisKPIMetaVida != null)
                            thisKPIMetaVida.Ativo = false;

                        context.KPIGrupo.Update(bancoEntity);
                        if (thisMeta != null)
                            context.KPIMeta.Update(thisMeta);
                        if (thisKPIMetaValor != null)
                            context.KPIMetaValor.Update(thisKPIMetaValor);
                        if (thisKPIMetaVida != null)
                            context.KPIMetaVida.Update(thisKPIMetaVida);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void Update(KPIGrupo Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.KPIGrupo.Update(Entity);
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
