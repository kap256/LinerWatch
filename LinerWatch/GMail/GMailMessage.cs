using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

namespace LinerWatch.GMail
{
    class GMailMessage
    {
        static readonly Encoding Encode=Encoding.UTF8;
        readonly Message NativeMessage;
        public string Subject { get; private set; } = "無題";
        public string Body { get; private set; } = "";

        public GMailMessage(Message native)
        {
            NativeMessage = native;
            foreach(var header in native.Payload.Headers) {
                if(header.Name == "Subject") {
                    Subject = header.Value;
                }
            }

            var base64url = NativeMessage.Payload.Body.Data;
            var base64 = base64url.Replace("-", "+").Replace("_", "/");
            var bytes = Convert.FromBase64String(base64);
            Body = Encode.GetString(bytes);
        }

        public override string ToString()
        {
            return Subject;
        }
    }
}
