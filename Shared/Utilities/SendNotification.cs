using ADIRA.Shared.BusinessDataObjects;
using ADIRA.Shared.Utilities.Enums;
using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ADIRA.Shared.Utilities
{
    public class SendNotification
    {
        public async Task<string> SendEmail(EmailMessageData messageData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(messageData.From)) messageData.From = "adira@techwishgroup.com";

                string accessCode = await GetGoogleAccessTokenFromRefreshToken();
                string messageRaw = messageData.GenerateRAW();
                string gmailAPI = "https://www.googleapis.com/gmail/v1/users/me/messages/send?alt=json";

                HttpWebRequest request = Util.CreateHttpWebRequest(gmailAPI, Enums.HttpMethod.POST, accessCode, "application/json", "Accept=text/html,application/xhtml+xml,application/json;q=0.9,*/*;q=0.8", "{\"raw\":\"" + messageRaw + "\" }");

                return await Util.GetResponseBody(request);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> SendSMTPEmail(EmailMessageData messageData)
        {
            try
            {
                string response = string.Empty;
                var email = new MimeMessage
                {
                    MessageId = messageData.MessageID
                };

                email.From.Add(new MailboxAddress(messageData.FromName ?? "ADIRA Group", messageData.From ?? "adira@techwishgroup.com"));
                messageData.To.ForEach(x => email.To.Add(new MailboxAddress(x, x)));
                messageData.CC?.ForEach(x => email.Cc.Add(new MailboxAddress(x, x)));
                messageData.BCC?.ForEach(x => email.Bcc.Add(new MailboxAddress(x, x)));
                email.Subject = messageData.Subject;

                var builder = new BodyBuilder();
                messageData.Attachments?.ForEach(attachment => {
                    builder.Attachments.Add(attachment.FileName, attachment.DocumentInfo.OpenReadStream());
                });

                if (messageData.BodyType.Equals(MailBodyType.Html))
                {
                    builder.HtmlBody = messageData.Body;
                }
                else
                {
                    builder.TextBody = messageData.Body;
                }

                if (messageData.HighPriorityFlag)
                {
                    email.Priority = MessagePriority.Urgent;
                    email.Importance = MessageImportance.High;
                }

                email.Body = builder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    smtp.Authenticate("adira@techwishgroup.com", "TwA%D@R#25");

                    response = smtp.Send(email);
                    smtp.Disconnect(true);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> GetGoogleAccessTokenFromRefreshToken()
        {
            try
            {
                string access_token = "";
                string refresh_token = "1//056w0U8ZyrKt1CgYIARAAGAUSNwF-L9IrJVjxTmjkvCI9Rg7G55aOxcEYhvc6rcXdPawiffqu8JxOUWXYaBs-8hZpRlyIqf7INso";
                string client_id = "986361033310-btcfepbe68i3ap97pmv43t4ftifo8hoj.apps.googleusercontent.com";
                string client_secret = "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";
                string tokenRequestURI = "https://www.googleapis.com/oauth2/v3/token";
                string tokenRequestBody = string.Format("refresh_token={0}&client_id={1}&client_secret={2}&grant_type=refresh_token", Uri.EscapeDataString(refresh_token), client_id, client_secret);

                HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(tokenRequestURI);
                tokenRequest.Method = "POST";
                tokenRequest.ContentType = "application/x-www-form-urlencoded";
                tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                byte[] _byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody);
                tokenRequest.ContentLength = _byteVersion.Length;
                Stream stream = tokenRequest.GetRequestStream();
                await stream.WriteAsync(_byteVersion);
                stream.Close();

                WebResponse tokenResponse = await tokenRequest.GetResponseAsync();
                using (StreamReader reader = new(tokenResponse.GetResponseStream()))
                {
                    string responseText = await reader.ReadToEndAsync();
                    Dictionary<string, string> tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                    access_token = tokenEndpointDecoded["access_token"];
                }

                return access_token;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
