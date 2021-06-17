using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business
{
    public class UsuarioClienteModel
    {

        public static void Add(UsuarioCliente Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.UsuarioCliente.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DesativarUltimoCorretor(long IdCliente, long IdUsuario)
        {
            List<UsuarioCliente> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.UsuarioCliente
                        .Where(x => x.IdCliente == IdCliente && x.IdUsuario != IdUsuario)
                        .ToList();

                    foreach (var Item in ListEntity)
                    {
                        Item.Ativo = false;
                        Update(Item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(UsuarioCliente Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.UsuarioCliente.Update(Entity);
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
