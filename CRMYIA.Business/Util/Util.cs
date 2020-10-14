using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CRMYIA.Business.Util
{
    public static class Util
    {
        #region Campos
        static readonly Regex MultipleSpacesRegex = new Regex(@"\s{2,}|(\r\n|\r\n\t|\r|\n|\t)|\&nbsp\;", RegexOptions.Compiled);
        #endregion

        #region Métodos

        public static string ReadResponse(this HttpWebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                var encoding = Encoding.UTF8;
                if (!string.IsNullOrEmpty(response.CharacterSet) && ((response.CharacterSet.Split('.')?.Length ?? 1) >= 1)) // Adicionada regra por conta do Sintegr CE (charSet=pt_BR.UTF-8)
                    if (response.CharacterSet.ToUpper() == "PT_BR.UTF-8")
                    { encoding = Encoding.GetEncoding("ISO-8859-1"); }
                    else
                    { encoding = Encoding.GetEncoding(response.CharacterSet.Replace("\"", "")); }

                using (var reader = new StreamReader(responseStream, encoding))
                { return reader.ReadToEnd(); }
            }
        }

        public static long GetTime()
        {
            long retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            retval = (long)(t.TotalMilliseconds + 0.5);
            return retval;
        }


        public static string TratarDocumento(string Documento)
        {
            return Documento.Replace("-", string.Empty).Replace(".", string.Empty).Replace("/", string.Empty).Trim();
        }

        public static string GerarUid()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 16);
        }

        /// <summary>
        /// Realiza a validação do CNPJ
        /// </summary>
        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        #region TratarNomeArquivo

        private const string CaracteresInvalidos = @"[\s\.\,\@\\\+\*\?\[\^\]\$\(\)\{\}\=\!\""\'\#\&\/\;\<\>\|\:-]";
        public static string TratarNomeArquivo(string FileName, int i)
        {
            return "SD_" + Regex.Replace(Path.GetFileNameWithoutExtension(FileName).Replace(".", string.Empty),
                    CaracteresInvalidos,
                    string.Empty, RegexOptions.Singleline) + "_"
                    + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + i.ToString()
                    + Path.GetExtension(FileName);
        }

        public static string TratarNomeArquivoSaida(string FileName, string Extensao)
        {
            return string.Format("{0}_PROCESSADO{1}", Path.GetFileNameWithoutExtension(FileName).Replace(".", string.Empty), Extensao);
        }

        public static string TratarNomeArquivoZip(string FileName)
        {
            return string.Format("{0}.zip", Path.GetFileNameWithoutExtension(FileName).Replace(".", string.Empty));
        }
        #endregion


        #region Força de Senha
        public static int CheckPassword(string senha)
        {
            if (senha == null) return 0;
            int pontosPorTamanho = GetPontoPorTamanho(senha);
            int pontosPorMinusculas = GetPontoPorMinusculas(senha);
            int pontosPorMaiusculas = GetPontoPorMaiusculas(senha);
            int pontosPorDigitos = GetPontoPorDigitos(senha);
            int pontosPorSimbolos = GetPontoPorSimbolos(senha);
            int pontosPorRepeticao = GetPontoPorRepeticao(senha);
            return pontosPorTamanho + pontosPorMinusculas + pontosPorMaiusculas + pontosPorDigitos + pontosPorSimbolos - pontosPorRepeticao;
        }

        private static int GetPontoPorTamanho(string senha)
        {
            return Math.Min(10, senha.Length) * 6;
        }

        private static int GetPontoPorMinusculas(string senha)
        {
            int rawplacar = senha.Length - Regex.Replace(senha, "[a-z]", "").Length;
            return Math.Min(2, rawplacar) * 5;
        }

        private static int GetPontoPorMaiusculas(string senha)
        {
            int rawplacar = senha.Length - Regex.Replace(senha, "[A-Z]", "").Length;
            return Math.Min(2, rawplacar) * 5;
        }

        private static int GetPontoPorDigitos(string senha)
        {
            int rawplacar = senha.Length - Regex.Replace(senha, "[0-9]", "").Length;
            return Math.Min(2, rawplacar) * 5;
        }

        private static int GetPontoPorSimbolos(string senha)
        {
            int rawplacar = Regex.Replace(senha, "[a-zA-Z0-9]", "").Length;
            return Math.Min(2, rawplacar) * 5;
        }

        private static int GetPontoPorRepeticao(string senha)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\w)*.*\1");
            bool repete = regex.IsMatch(senha);
            if (repete)
            {
                return 30;
            }
            else
            {
                return 0;
            }
        }

        public static EnumeradorModel.PasswordStrength GetPasswordStrength(string senha)
        {
            int placar = CheckPassword(senha);

            if (placar < 50)
                return EnumeradorModel.PasswordStrength.Inaceitavel;
            else if (placar < 60)
                return EnumeradorModel.PasswordStrength.Fraca;
            else if (placar < 80)
                return EnumeradorModel.PasswordStrength.Aceitavel;
            else if (placar < 100)
                return EnumeradorModel.PasswordStrength.Forte;
            else
                return EnumeradorModel.PasswordStrength.Segura;
        }
        #endregion

        #region TratarEndereco
        public static string RetornarEnderecoCompleto(string Endereco = null, string Numero = null, string Complemento = null, string Bairro = null, string Cidade = null, string UF = null, string CEP = null)
        {
            string completo = string.Empty;

            completo = Endereco;
            completo += ", nº " + Numero;
            if (!Complemento.IsNullOrEmpty())
                completo += " - " + Complemento;
            if (!Bairro.IsNullOrEmpty())
                completo += ", " + Bairro;
            if (!Cidade.IsNullOrEmpty())
                completo += " - " + Cidade;
            if (!UF.IsNullOrEmpty())
                completo += "-" + UF;
            if (!CEP.IsNullOrEmpty())
                completo += " - CEP: " + CEP;
            return completo;
        }
        #endregion

        #region Tratar Datas
        public static DateTime GetFirstDayOfMonth(int month)
        {
            return new DateTime(DateTime.Now.Year, month, 1);
        }
        public static DateTime GetLastDayOfMonth(int month)
        {
            return new DateTime(DateTime.Now.Year, month, DateTime.DaysInMonth(DateTime.Now.Year, month));
        }
        #endregion
        #endregion
    }
}

