using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;

namespace iTMO.Help.Controller
{
    /// <summary>
    /// Used to send mails to developer, to me
    /// </summary>
    class CommunicateController
    {
        public static async void ReportStackTrace(string stackTrace, string error, string msg, params string[] opts)
        {

        }

        public static async void ContactDeveloper(string text, string group, string lastSearchGroup)
        {
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.To.Add(new EmailRecipient("goodforgod.dev@gmail.com"));

            string grp = group;
            if (string.IsNullOrWhiteSpace(grp))
                grp = lastSearchGroup;
            if (string.IsNullOrWhiteSpace(grp))
                grp = "";

            emailMessage.Subject = "iTMO.Help : Report";
            emailMessage.Body = text + "\n" + "From Student : " + grp + "\n";
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
    }
}
