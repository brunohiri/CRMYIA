using CRMYIA.Business.Util;
using CRMYIA.Data.Context;
using CRMYIA.Data.Entities;
using CRMYIA.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CRMYIA.Business
{
    public class VideoModel
    {

        public static Video Get(long IdArquivo)
        {
            Video Entity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    Entity = context.Video
                        .Where(x => x.IdVideo == IdArquivo)
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Entity;
        }
        public static List<VideoViewModel> GetList()
        {
            List<VideoViewModel> ListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Video
                        .Select(x => new VideoViewModel
                        {
                            //Criptography.Decrypt(HttpUtility.UrlDecode(Id)).ExtractLong()
                            //System.Web.HttpUtility.UrlEncode(Criptography.Encrypt(Item.IdCampanhaArquivo.ToString()))
                            IdVideo = HttpUtility.UrlEncode(Criptography.Encrypt(x.IdVideo.ToString()).ToString()),
                            IdentificadorVideo = x.IdentificadorVideo.ToString(),
                            CaminhoArquivo = x.CaminhoArquivo,
                            NomeVideo = x.NomeVideo,
                            DataCadastro = x.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                            Ativo = x.Ativo
                        })
                        .Where(x => x.Ativo)
                        .OrderByDescending(o => o.NomeVideo).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ListEntity;
        }

        public static List<Video> GetListaVideos(long IdCampanha, byte IdGrupoCorretor)
        {
            List<Video> ListEntity = new List<Video>();
            List<Video> AuxListEntity = null;
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    ListEntity = context.Video
                    .Where(x => x.IdCampanha == IdCampanha)
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

        public static void Add(Video Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Video.Add(Entity);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(Video Entity)
        {
            try
            {
                using (YiaContext context = new YiaContext())
                {
                    context.Video.Update(Entity);
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
