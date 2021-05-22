using CRMYIA.Business.Util;
using CRMYIA.Data.Entities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Business.ShiftData
{
    public class ShiftDataEngine
    {
        #region Propriedades
        static string UrlDominio = "https://api.shiftdata.com.br/api";
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public string Login(out string Message)
        {
            string Token = string.Empty;
            string UrlMetodo = "/Login";
            Message = string.Empty;
            try
            {
                Fornecedor EntityFornecedor = FornecedorModel.Get((long)EnumeradorModel.Fornecedor.ShiftData);
                if (EntityFornecedor != null)
                {
                    RestClient client = new RestClient(UrlDominio);
                    RestRequest request = new RestRequest(UrlMetodo, Method.POST);
                    request.AddJsonBody("{\"accessKey\":\"" + EntityFornecedor.TokenAPI + "\"}");
                    var response = client.Execute(request);

                    if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ShiftDataResultLogin resultLogin = JsonConvert.DeserializeObject<ShiftDataResultLogin>(response.Content);

                        if (resultLogin.authenticated)
                            Token = resultLogin.accessToken;
                        else
                            Message = resultLogin.message;
                    }
                    else
                        Message = response.StatusCode.ToString();
                }
                else
                    Message = "Fornecedor não encontrado ou não está ativo!";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            return Token;
        }

        public ShiftDataResultPessoaFisica ExecutePessoaFisica(string AccessToken, string cpf, out string Message)
        {
            ShiftDataResultApiPessoaFisica EntityApi = null;
            ShiftDataResultPessoaFisica Entity = null;
            string UrlMetodo = "/PessoaFisica?cpf=";

            Message = string.Empty;
            try
            {
                RestClient client = new RestClient(UrlDominio);
                RestRequest request = new RestRequest(UrlMetodo + cpf, Method.GET);
                var response = client.Execute(request);

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    EntityApi = JsonConvert.DeserializeObject<ShiftDataResultApiPessoaFisica>(response.Content);

                    if (EntityApi.Code == (int)System.Net.HttpStatusCode.OK)
                        Entity = EntityApi.Result;
                    else
                        Message = EntityApi.CodeMessage + " - " + EntityApi.Message;
                }
                else
                    Message = response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Entity;
        }
        #endregion
    }
}
