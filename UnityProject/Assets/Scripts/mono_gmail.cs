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
    [SerializeField] private InputField _inputField;
    public void SendMail()
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("voetkaart@gmail.com");
        mail.To.Add(_inputField.text);
        mail.Subject = "Test Mail";
        mail.Body = "Works";

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("voetkaart@gmail.com", "GamePr0t02") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");

    }
}