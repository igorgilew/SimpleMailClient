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

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
