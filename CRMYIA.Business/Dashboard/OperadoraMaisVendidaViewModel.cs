using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMYIA.Business.Dashboard
{
    public class OperadoraMaisVendidaViewModel
    {
        #region Propriedades
        public long IdOperadora { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static List<OperadoraMaisVendidaViewModel> GetListOperadorasMaisVendidas(byte TakeQtd)
        {
            List<OperadoraMaisVendidaViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Proposta
                        .Include(y => y.IdCategoriaNavigation)
                            .ThenInclude(z => z.IdLinhaNavigation)
                                .ThenInclude(k => k.IdProdutoNavigation)
                                    .ThenInclude(p => p.IdOperadoraNavigation)
                        .AsNoTracking()
                        .Where(x => x.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation.Ativo
                            && x.Ativo && x.IdStatusProposta == (byte?)(EnumeradorModel.StatusProposta.Aceito))
                        .GroupBy(g => g.IdCategoriaNavigation.IdLinhaNavigation.IdProdutoNavigation.IdOperadoraNavigation)
                        .Select(y => new { Operadora = y.Key, Quantidade = y.Count() })
                        .OrderByDescending(o => o.Quantidade)
                        .Take(TakeQtd)
                        .Select(r => new OperadoraMaisVendidaViewModel()
                        {
                            IdOperadora = r.Operadora.IdOperadora,
                            Descricao = r.Operadora.Descricao,
                            Valor = r.Quantidade.ExtractIntMilharFormat()
                        }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }
        #endregion
    }
}
