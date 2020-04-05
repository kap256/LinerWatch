using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinerWatch.GMail
{
    class GMailControl:IDisposable
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/gmail-dotnet-quickstart.json
        static readonly string[] Scopes = { GmailService.Scope.GmailReadonly };
        static readonly string ApplicationName = "LinerWatch";

        GmailService Service;

        public GMailControl()
        {
            UserCredential credential;

            using(var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read)) {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.

                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            Service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public void Dispose()
        {
            Service.Dispose();
        }

        public List<GMailMessage> GetKeioMails()
        {
            var list_req = Service.Users.Messages.List("me");
            list_req.Q = $"from:{Consts.KEIO_ADDRESS}";
            list_req.MaxResults = Consts.LIST_NO;

            // List labels.
            IList<Message> messages = list_req.Execute().Messages;
            if(messages != null && messages.Count > 0) {
                var ret = new List<GMailMessage>( messages.Count);
                foreach(var mes in messages) {
                    var get_req = Service.Users.Messages.Get("me", mes.Id);
                    var detail = get_req.Execute();
                    ret.Add(new GMailMessage(detail));
                }
                return ret;
            } else {
                return null;
            }
        }
    }
}
