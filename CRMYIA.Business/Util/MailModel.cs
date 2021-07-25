using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CRMYIA.Business.Util
{
    public class MailModel
    {
        #region Propriedades
        public static string Server { get; set; }
        public static string User { get; set; }
        public static string Pass { get; set; }
        public static string Port { get; set; }
        public static string EnableSSL { get; set; }

        #endregion

        #region Construtores
        public MailModel()
        {

        }
        #endregion

        #region Métodos

        public static string ReturnBodyTemplate()
        {
            string HtmlBody = string.Empty;
            HtmlBody = "<!DOCTYPE html>";
            HtmlBody += "<html lang=\"pt-br\">";
            HtmlBody += "<head>";
            HtmlBody += "    <meta charset=\"utf-8\" />";
            HtmlBody += "    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />";
            //Customizar Título
            HtmlBody += "    <title>EMPRESA</title>";
            HtmlBody += "    <meta content=\"\" name=\"keywords\">";
            HtmlBody += "    <meta content=\"\" name=\"description\">";
            HtmlBody += "</head>";
            HtmlBody += "<body>";
            HtmlBody += "    <!--==========================";
            HtmlBody += "      Header";
            HtmlBody += "    ============================-->";
            HtmlBody += "    <header id=\"header\" class=\"header-scrolled\">";
            HtmlBody += "        <div class=\"container-fluid\">";
            HtmlBody += "";
            HtmlBody += "            <div id=\"logo\" class=\"pull-left\">";
            HtmlBody += "                <h1><a href=\"/Index#intro\" class=\"scrollto\">";
            //Customizar Logo
            HtmlBody += "                <img src=\"\" alt=\"\" /></a></h1>";
            HtmlBody += "            </div>";
            HtmlBody += "";
            HtmlBody += "        </div>";
            HtmlBody += "    </header><!-- #header -->";
            HtmlBody += "";
            HtmlBody += "";
            HtmlBody += "    <main id=\"main\">";
            HtmlBody += "        <section id=\"cadastro\">";
            HtmlBody += "            <div class=\"container\">";
            HtmlBody += "";
            HtmlBody += "                <header class=\"section-header wow fadeInUp\">";
            HtmlBody += "                    <h3>{Titulo}</h3>";
            HtmlBody += "                    <p>{Subtitulo}</p>";
            HtmlBody += "                </header>";
            HtmlBody += "";
            HtmlBody += "                <div class=\"row\">";
            HtmlBody += "                    <div class=\"col-lg-12 col-md-12 box wow bounceInUp\">";
            HtmlBody += "                        <h4 class=\"title\"><a href=\"\">{Titulo2}</a></h4>";
            HtmlBody += "                        {Texto}";
            HtmlBody += "                    </div>";
            HtmlBody += "                </div>";
            HtmlBody += "";
            HtmlBody += "            </div>";
            HtmlBody += "        </section>";
            HtmlBody += "    </main>";
            HtmlBody += "";
            HtmlBody += "    <!--==========================";
            HtmlBody += "      Footer";
            HtmlBody += "    ============================-->";
            HtmlBody += "    <footer id=\"footer\">";
            HtmlBody += "        <div class=\"container\">";
            HtmlBody += "            <div class=\"copyright\">";
            //Customizar Footer
            HtmlBody += "                &copy; " + DateTime.Now.Year.ToString() + " - <strong>EMPRESA</strong>. Todos os Direitos Reservados.";
            HtmlBody += "            </div>";
            HtmlBody += "            <div class=\"credits\">";
            //Customizar Footer
            HtmlBody += "                Desenvolvido por <a href=\"\">EMPRESA</a>";
            HtmlBody += "            </div>";
            HtmlBody += "        </div>";
            HtmlBody += "    </footer><!-- #footer -->";
            HtmlBody += "</body>";
            HtmlBody += "</html>";
            return HtmlBody;
        }

        public static bool SendMail(string FromAddress, string ToAddress, string Subject, string HtmlBody)
        {
            try
            {
                SmtpClient client = new SmtpClient(Server);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(User, Pass);
                client.EnableSsl = Convert.ToBoolean(EnableSSL);
                client.Port = int.Parse(Port);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FromAddress);
                if (!ToAddress.Contains(";"))
                    mailMessage.To.Add(ToAddress);
                else
                {
                    ToAddress.Split(';').ToList().ForEach(delegate (string Item)
                    {
                        mailMessage.To.Add(Item);
                    });
                }
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = HtmlBody;
                mailMessage.Subject = Subject;
                client.Send(mailMessage);
            }
            catch (SmtpException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
        #endregion
    }
}

