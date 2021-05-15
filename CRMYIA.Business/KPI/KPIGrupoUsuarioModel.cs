﻿using System;
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
        public static KPIGrupoUsuario Get(long IdUsuario)
        {
            KPIGrupoUsuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIGrupoUsuario
                        .Where(x => x.Ativo && x.IdUsuario == IdUsuario)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static KPIGrupoUsuario GetByKPIGrupoUsuario(long IdKPIGrupoUsuario)
        {
            KPIGrupoUsuario Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.KPIGrupoUsuario
                        .Where(x => x.Ativo && x.IdKPIGrupoUsuario == IdKPIGrupoUsuario)
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
                        .Include(y => y.IdMetaNavigation).ThenInclude(z => z.KPIMetaValor)
                        .Include(y => y.IdMetaNavigation).ThenInclude(z => z.KPIMetaVida)
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
        public static void Excluir(KPIGrupoUsuario Entity)
        {
            KPIGrupoUsuario bancoEntity = null;
            KPIMeta thisMeta = null;
            KPIMetaValor thisKPIMetaValor = null;
            KPIMetaVida thisKPIMetaVida = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    bancoEntity = context.KPIGrupoUsuario.Where(x => x.IdKPIGrupoUsuario == Entity.IdKPIGrupoUsuario && x.Ativo == true).FirstOrDefault();
                    thisMeta = context.KPIMeta.Where(x => x.IdKPIGrupoNavigation.IdKPIGrupo == bancoEntity.IdKPIGrupoUsuario && x.Ativo == true).FirstOrDefault();
                    if (thisMeta != null)
                    {
                        thisKPIMetaValor = context.KPIMetaValor.Where(x => x.IdMeta == thisMeta.IdMeta && x.Ativo == true).FirstOrDefault();
                        thisKPIMetaVida = context.KPIMetaVida.Where(x => x.IdMeta == thisMeta.IdMeta && x.Ativo == true).FirstOrDefault();
                    }
                    if (bancoEntity != null)
                    {
                        bancoEntity.Motivo = Entity.Motivo;
                        bancoEntity.Saida = DateTime.Now;
                        bancoEntity.Ativo = false;
                        if (thisMeta != null)
                            thisMeta.Ativo = false;
                        if (thisKPIMetaValor != null)
                            thisKPIMetaValor.Ativo = false;
                        if (thisKPIMetaVida != null)
                            thisKPIMetaVida.Ativo = false;

                        context.KPIGrupoUsuario.Update(bancoEntity);
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
