using CRMYIA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private const string CaracteresInvalidosSeparadorPipe = @"[\s\.\@\\\+\*\?\[\^\]\$\(\)\{\}\=\!\""\'\&\/\;\<\>\|]";
        public static string TratarNomeArquivo(string FileName, int i)
        {
            return "YIA_" + Regex.Replace(Path.GetFileNameWithoutExtension(FileName).Replace(".", string.Empty),
                    CaracteresInvalidos,
                    string.Empty, RegexOptions.Singleline) + "_"
                    + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + i.ToString()
                    + Path.GetExtension(FileName);
        }

        public static string TratarNomeArquivoSeparadorPipe(string FileName, int i)
        {
            return "YIA_" + Regex.Replace(Path.GetFileNameWithoutExtension(FileName).Replace(".", string.Empty),
                    CaracteresInvalidosSeparadorPipe,
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
            return new DateTime(DateTime.Now.Year, month == 0 ? 1 : month, 1);
        }
        public static DateTime GetLastDayOfMonth(int month)
        {
            return new DateTime(DateTime.Now.Year, month == 0 ? 1 : month, DateTime.DaysInMonth(DateTime.Now.Year, month == 0 ? 1 : month));
        }

        public static string CalculaTempo(DateTime Data)
        {
            var now = DateTime.Now;

            TimeSpan tempo = now - Data;

            string retorno = "";

            int ano = 0;

            if((tempo.TotalMilliseconds / 1000) <= 60)
            {
                retorno = Convert.ToInt32((tempo.TotalMilliseconds / 1000)).ToString() + " Segundos atrás";
            }
            else if((tempo.TotalSeconds / 60) <= 60)
            {
                retorno = Convert.ToInt32((tempo.TotalSeconds / 60)).ToString() + " Minutos atrás";
            }
            else if ((tempo.TotalMinutes / 60) <= 24)
            {
                retorno = Convert.ToInt32((tempo.TotalMinutes / 60)).ToString() + " Horas atrás";
            }
            else if ( ((tempo.TotalHours / 24) / 30) <= 12)
            {
                retorno = Convert.ToInt32(((tempo.TotalHours / 24) / 30)).ToString() + " Mês atrás";
            }
            else
            {
                ano = now.Year - Data.Year;
                if (ano == 1)
                {
                    retorno = Convert.ToInt32(ano).ToString() + " Ano atrás";
                }
                else
                {
                    retorno = Convert.ToInt32(ano).ToString() + " Anos atrás";
                }
            }
           

            //retorno = tempo.TotalMilliseconds.ToString();

            return retorno;
        }
        public static string SetStatusChat(EnumeradorModel.StatusChat StatusChat)
        {
            string Status = string.Empty;
            switch (StatusChat)
            {
                case EnumeradorModel.StatusChat.Ativo:
                    Status = "success";
                    break;
                case EnumeradorModel.StatusChat.Ausente:
                    Status = "warning";
                    break;
                case EnumeradorModel.StatusChat.NaoIncomodar:
                    Status = "danger";
                    break;
                case EnumeradorModel.StatusChat.Invisivel:
                    Status = "light";
                    break;
                default:
                    break;
            }
            return Status;
        }

        public static string GetSlug(string title, bool remapToAscii = false, int maxlength = 300)
        {
            if (title == null)
            {
                return string.Empty;
            }

            int length = title.Length;
            bool prevdash = false;
            StringBuilder stringBuilder = new StringBuilder(length);
            char c;

            for (int i = 0; i < length; ++i)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    stringBuilder.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lower-case
                    stringBuilder.Append((char)(c | 32));
                    prevdash = false;
                }
                else if ((c == ' ') || (c == ',') || (c == '.') || (c == '/') ||
                  (c == '\\') || (c == '-') || (c == '_') || (c == '='))
                {
                    if (!prevdash && (stringBuilder.Length > 0))
                    {
                        stringBuilder.Append('-');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    int previousLength = stringBuilder.Length;

                    if (remapToAscii)
                    {
                        stringBuilder.Append(RemapInternationalCharToAscii(c));
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }

                    if (previousLength != stringBuilder.Length)
                    {
                        prevdash = false;
                    }
                }

                if (i == maxlength)
                {
                    break;
                }
            }

            if (prevdash)
            {
                return stringBuilder.ToString().Substring(0, (stringBuilder.Length - 1));
            }
            else
            {
                return stringBuilder.ToString();
            }
        }

        public static bool VerificaNomeArquivo(List<string> NomeArquivo)
        {
            bool achou = true;
            int maior = 0;
            foreach (string Nome in NomeArquivo) {
                string[] NomeVet = Nome.Split('-');

                if (NomeVet.Length == 3)
                {
                    maior++;
                }
                else
                {
                    achou = false;
                }
            }
            return achou;
        }

        public static bool VerificaNomeArquivoAssinaturaCartao(List<string> NomeArquivo)
        {
            bool achou = true;
            int maior = 0;
            foreach (string Nome in NomeArquivo)
            {
                string[] NomeVet = Nome.Split('-');

                if (NomeVet.Length == 2)
                {
                    maior++;
                }
                else
                {
                    achou = false;
                }
            }
            return achou;
        }

        /// <summary>
        /// Remaps the international character to their equivalent ASCII characters. See
        /// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
        /// </summary>
        /// <param name="character">The character to remap to its ASCII equivalent.</param>
        /// <returns>The remapped character</returns>
        private static string RemapInternationalCharToAscii(char character)
        {
            string s = character.ToString().ToLowerInvariant();
            if ("àåáâäãåąā".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøő".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (character == 'ř')
            {
                return "r";
            }
            else if (character == 'ł')
            {
                return "l";
            }
            else if ("đð".Contains(s))
            {
                return "d";
            }
            else if (character == 'ß')
            {
                return "ss";
            }
            else if (character == 'Þ')
            {
                return "th";
            }
            else if (character == 'ĥ')
            {
                return "h";
            }
            else if (character == 'ĵ')
            {
                return "j";
            }
            else
            {
                return string.Empty;
            }
        }

        public static List<CalendarioSazonal> CalcularFeriado(int ano, int QuantidadeAnos)
        {
            int i = 0;
            int antes = 5;
            int depois = 3;

            #region GuiIds
            Guid GuidIdCarnaval;
            Guid GuidIdQuarta;
            Guid GuidIdSexta;
            Guid GuidIdPascoa;
            Guid GuidIdCorpusChristi;
            Guid GuidIdSetembroAmarelo;
            Guid GuidIdOutubroRosa;
            Guid GuidIdNovembroAzul;

            Guid GuidIdConfraternizacao;
            Guid GuidIdAniversarioSp;
            Guid GuidIdDiaDaMulher;
            Guid GuidIdDiaDasMaes;
            Guid GuidIdDiaDosNamorados;
            Guid GuidIdDiaDosPais;
            Guid GuidIdDiaDasCrianca;
            Guid GuidIdDiaDoCorretorDeSeguro;
            Guid GuidIdDiaDoSecuritario;
            Guid GuidIdNatal;

            do
            {
                GuidIdCarnaval = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdCarnaval));
            do
            {
                GuidIdQuarta = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdQuarta));
            do
            {
                GuidIdSexta = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdSexta));
            do
            {
                GuidIdPascoa = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdPascoa));
            do
            {
                GuidIdCorpusChristi = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdCorpusChristi));
            do
            {
                GuidIdSetembroAmarelo = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdSetembroAmarelo));
            do
            {
                GuidIdOutubroRosa = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdOutubroRosa));
            do
            {
                GuidIdNovembroAzul = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdNovembroAzul));
            do
            {
                GuidIdConfraternizacao = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdConfraternizacao));
            do
            {
                GuidIdAniversarioSp = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdAniversarioSp));
            do
            {
                GuidIdDiaDaMulher = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdDiaDaMulher));
            do
            {
                GuidIdDiaDasMaes = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdDiaDasMaes));
            do
            {
                GuidIdDiaDosNamorados = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdDiaDosNamorados));
            do
            {
                GuidIdDiaDosPais = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdDiaDosPais));
            do
            {
                GuidIdDiaDasCrianca = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdDiaDasCrianca));
            do
            {
                GuidIdDiaDoCorretorDeSeguro = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdDiaDoCorretorDeSeguro));
            do
            {
                GuidIdDiaDoSecuritario = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdDiaDoSecuritario));
            do
            {
                GuidIdNatal = Guid.NewGuid();
            } while (Business.VisitaModel.VerificaGuidId(GuidIdNatal));
            #endregion

            List<CalendarioSazonal> ListCalendarioSazonal = new List<CalendarioSazonal>();
            while (i < QuantidadeAnos)
            {

                DateTime dtAtual = Convert.ToDateTime(ano + i + "-01-01 00:00:00.000".ToString());
                DateTime data = CalcularPascoa(ano + i);

                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Confraternização Universal (Ano Novo)", Cor = "#1B5E20", Tipo = (byte)EnumeradorModel.TipoSazonal.Feriado, GuidId = GuidIdConfraternizacao.ToString(), DataSazonal = new DateTime(dtAtual.Year, 1, 1), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 1, 1).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 1, 1).AddDays(depois), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Aniversário de São Paulo", Cor = "#1B5E20", Tipo = (byte)EnumeradorModel.TipoSazonal.Feriado, GuidId = GuidIdAniversarioSp.ToString(), DataSazonal = new DateTime(dtAtual.Year, 1, 25), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 1, 25).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 1, 25).AddDays(depois), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Carnaval", Cor = "#1B5E20", Tipo = (byte)EnumeradorModel.TipoSazonal.Feriado, GuidId = GuidIdCarnaval.ToString(), DataSazonal = data.AddDays(-47), DataCadastro = DateTime.Now, DataInicio = data.AddDays(-antes -47), DataFim = data.AddDays(depois - 47), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Dia Internacional da Mulher", Cor = "#E65100", Tipo = (byte)EnumeradorModel.TipoSazonal.DataComemorativa, GuidId = GuidIdDiaDaMulher.ToString(), DataSazonal = new DateTime(dtAtual.Year, 3, 8), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 3, 8).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 3, 8).AddDays(depois), ExisteCampanha = true, Ativo = true });
                //ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Quarta-feira de cinzas", Cor = "#1B5E20", Tipo = (byte)EnumeradorModel.TipoSazonal.Feriado, GuidId = GuidIdQuarta.ToString(), DataSazonal = data.AddDays(-46), DataCadastro = DateTime.Now, DataInicio = data.AddDays(-antes -46), DataFim = data.AddDays(depois - 46), ExisteCampanha = true, Ativo = true });
                //ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Sexta-feira Santa", Cor = "#1B5E20", Tipo = (byte)EnumeradorModel.TipoSazonal.Feriado, GuidId = GuidIdSexta.ToString(), DataSazonal = data.AddDays(-2), DataCadastro = DateTime.Now, DataInicio = data.AddDays(-antes -2), DataFim = data.AddDays(depois - 2), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Páscoa", Cor = "#1B5E20", Tipo = (byte)EnumeradorModel.TipoSazonal.Feriado, GuidId = GuidIdPascoa.ToString(), DataSazonal = data, DataCadastro = DateTime.Now, DataInicio = data.AddDays(-antes), DataFim = data.AddDays(depois), ExisteCampanha = true, Ativo = true });
                //ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Corpus Christi", Cor = "#1B5E20", Tipo = (byte)EnumeradorModel.TipoSazonal.Feriado, GuidId = GuidIdCorpusChristi.ToString(), DataSazonal = data.AddDays(60), DataCadastro = DateTime.Now, DataInicio = data.AddDays(60 - antes), DataFim = data.AddDays(60 + depois), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Dia dos Namorados", Cor = "#E65100", Tipo = (byte)EnumeradorModel.TipoSazonal.DataComemorativa, GuidId = GuidIdDiaDosNamorados.ToString(), DataSazonal = new DateTime(dtAtual.Year, 6, 12), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 6, 12).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 6, 12).AddDays(depois), ExisteCampanha = true, Ativo = true });
                //var mes = new DateTime(dtAtual.Year, 9, DateTime.DaysInMonth(dtAtual.Year, 9));
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Setembro Amarelo", Cor = "#E65100", Tipo = (byte)EnumeradorModel.TipoSazonal.DataComemorativa, GuidId = GuidIdSetembroAmarelo.ToString(), DataSazonal = new DateTime(dtAtual.Year, 9, 1), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 9, 1).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 9, DateTime.DaysInMonth(dtAtual.Year, 9)).AddDays(depois), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Outubro Rosa", Cor = "#E65100", Tipo = (byte)EnumeradorModel.TipoSazonal.DataComemorativa, GuidId = GuidIdOutubroRosa.ToString(), DataSazonal = new DateTime(dtAtual.Year, 10, 1), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 10, 1).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 10, DateTime.DaysInMonth(dtAtual.Year, 10)).AddDays(depois), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Dia das Crianças", Cor = "#1B5E20", Tipo = (byte)EnumeradorModel.TipoSazonal.Feriado, GuidId = GuidIdDiaDasCrianca.ToString(), DataSazonal = new DateTime(dtAtual.Year, 10, 12), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 10, 12).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 10, 12).AddDays(depois), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Dia do Corretor de Seguros", Cor = "#E65100", Tipo = (byte)EnumeradorModel.TipoSazonal.DataComemorativa, GuidId = GuidIdDiaDoCorretorDeSeguro.ToString(), DataSazonal = new DateTime(dtAtual.Year, 10, 12), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 10, 12).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 10, 12).AddDays(depois), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Dia do Securitário", Cor = "#E65100", Tipo = (byte)EnumeradorModel.TipoSazonal.DataComemorativa, GuidId = GuidIdDiaDoSecuritario.ToString(), DataSazonal = new DateTime(dtAtual.Year, 10, 18), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 10, 18).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 10, 18).AddDays(depois), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Novembro Azul", Cor = "#E65100", Tipo = (byte)EnumeradorModel.TipoSazonal.DataComemorativa, GuidId = GuidIdNovembroAzul.ToString(), DataSazonal = new DateTime(dtAtual.Year, 11, 1), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 11, 1).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 11, DateTime.DaysInMonth(dtAtual.Year, 11)).AddDays(depois), ExisteCampanha = true, Ativo = true });
                ListCalendarioSazonal.Add(new CalendarioSazonal() { Descricao = "Natal", Cor = "#1B5E20", Tipo = (byte)EnumeradorModel.TipoSazonal.Feriado, GuidId = GuidIdNatal.ToString(), DataSazonal = new DateTime(dtAtual.Year, 12, 25), DataCadastro = DateTime.Now, DataInicio = new DateTime(dtAtual.Year, 12, 25).AddDays(-antes), DataFim = new DateTime(dtAtual.Year, 12, 25).AddDays(depois), ExisteCampanha = true, Ativo = true });

                i++;
            }

            return ListCalendarioSazonal;
        }
        private static DateTime CalcularPascoa(int ano)
        {
            int r1 = ano % 19;
            int r2 = ano % 4;
            int r3 = ano % 7;
            int r4 = (19 * r1 + 24) % 30;
            int r5 = (6 * r4 + 4 * r3 + 2 * r2 + 5) % 7;
            DateTime dataPascoa = new DateTime(ano, 3, 22).AddDays(r4 + r5);
            int dia = dataPascoa.Day;
            switch (dia)
            {
                case 26:
                    dataPascoa = new DateTime(ano, 4, 19);
                    break;
                case 25:
                    if (r1 > 10)
                        dataPascoa = new DateTime(ano, 4, 18);
                    break;
            }
            return dataPascoa.Date;
        }

        public static int ObterNumeroDaSemana(DateTime Data)
        {
            CultureInfo CiCurr = CultureInfo.CurrentCulture;
            int NumeroDaSemana = CiCurr.Calendar.GetWeekOfYear(Data, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            return NumeroDaSemana;
        }
        public static int ObterClassificacao(DateTime Data)
        {
            //1- Qual é a semana do ano em da data desejada
            int semanaDoAnoDt = ObterNumeroDaSemana(Data);
            //2- Qual é o dia da semana da data desejada 
            int diaDaSemanaDt = (int)Data.DayOfWeek;

            DateTime primeiroDiaDoMes = new DateTime(Data.Year, Data.Month, 1);
            //3- Qual é a semana do ano do primeiro dia do mes da data desejada
            int semanaDoAnoPrimeiroDiaDoMes = ObterNumeroDaSemana(primeiroDiaDoMes);
            //4- Qual é a dia da semana do ano do primeiro dia do mes da data desejada
            int diaDaSemanaPrimeiroDiaDoMes = (int)primeiroDiaDoMes.DayOfWeek;
            //
            int fator = diaDaSemanaDt >= diaDaSemanaPrimeiroDiaDoMes ? 1 : 0;
            return (semanaDoAnoDt - semanaDoAnoPrimeiroDiaDoMes) + fator;
        }

        #endregion
        #endregion
    }
}

