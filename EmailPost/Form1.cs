using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System.IO;

namespace EmailPost
{
    public partial class EmailPost : Form
    {
        public EmailPost()
        {
            InitializeComponent();
        }
        string fromAdress = "yatiia@yandex.ru";
        private void SendMessageBySMTP()
        {
            var message = CreateMailMessage();
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("yatiia@yandex.ru", "qwerty11");
            smtp.EnableSsl = true;
            smtp.Send(message);
        }
        
        private MailMessage CreateMailMessage()
        {          
            // создаем объект сообщения
            return new MailMessage(fromAdress, tbAdress.Text, "TestMessaging", rtbContent.Text);                        
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessageBySMTP();
        }
        private void ReadHtmlFromMIME(MimeMessage message)
        {
            var tmpDir = Path.Combine(Path.GetTempPath(), message.MessageId);
            var visitor = new HtmlPreviewVisitor(tmpDir);

            Directory.CreateDirectory(tmpDir);

            message.Accept(visitor);
            wb.DocumentText = "0";
            wb.Document.OpenNew(true);
            wb.Document.Write(visitor.HtmlBody);
            wb.Refresh();           
            
        }
        private void GetMail()
        {
            using (ImapClient client = new ImapClient())
            {
                //тут указывается почтовый сервер
                client.Connect("imap.yandex.ru", 993, true);
                //почта и пароль
                client.Authenticate("yatiia@yandex.ru", "qwerty11");
                IMailFolder inbox = client.Inbox;               
                inbox.Open(FolderAccess.ReadOnly);
                var uids = client.Inbox.Search(SearchQuery.All);

                foreach (var uid in uids)
                {
                    var message = client.Inbox.GetMessage(uid);
                    ReadHtmlFromMIME(message);
                    // write the message to a file
                    //message.WriteTo(string.Format("{0}.eml", uid));
                }

                client.Disconnect(true);
               // MessageBox.Show(string.Format("Входящие {0}", inbox.Count)); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetMail();
        }
    }
}
