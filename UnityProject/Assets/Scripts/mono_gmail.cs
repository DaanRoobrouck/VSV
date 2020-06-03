using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;

public class mono_gmail : MonoBehaviour
{
    [SerializeField] private InputField _inputFieldName;
    [SerializeField] private InputField _inputFieldEmail;

    public void SendMail()
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("voetkaart@gmail.com");
        mail.To.Add(_inputFieldEmail.text);
        mail.Subject = $"Resultaat Verkeersspel {_inputFieldName.text}" ;
        mail.Body = "Beste Mevrouw/Meneer,"
            + Environment.NewLine +
            $"{_inputFieldName.text} heeft zonet de TeVoetKaart game afgelegd en behaalde hierbij {ScoreManager._score.ToString()} punten!"
            + Environment.NewLine +
            $"Hij schatte zichzelf als volgt in: 'Ik denk dat ik een {Math.Round((ScoreLinkScript.SelfScore * 10), 1)} / 10 heb gehaald'"
            + Environment.NewLine +
            ""
            + Environment.NewLine +
            "Bedankt voor het spelen van ons spel en blijf veilig!"
            + Environment.NewLine +
            "   - Matisse, Daan, Briek, Ward en Maxim";



SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("voetkaart@gmail.com", "GamePr0t02") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);

    }
}