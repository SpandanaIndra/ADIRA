using System.ComponentModel;
using System.Net.Mail;
using ADIRA.Shared.Utilities;
using ADIRA.Shared.Utilities.Enums;

namespace ADIRA.Shared.BusinessDataObjects
{
    public class EmailMessageData
    {
        public string MessageID { get; set; }
        public long UID { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public string FromDomain { get; set; }
        public DateTime? Received { get; set; }
        public string Folder { get; set; }
        public string AttachmentIcon { get; set; }
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        [DefaultValue(MailBodyType.Html)]
        public MailBodyType BodyType { get; set; }
        public Dictionary<string, object> ReplaceParameters { get; set; }
        public Dictionary<string, object> InputParameters { get; set; }
        public bool HighPriorityFlag { get; set; }
        public string ReplyToEmailID { get; set; }
        public List<DocumentData> Attachments { get; set; }

        public EmailMessageData()
        {
            ReplaceParameters = new Dictionary<string, object>();
            InputParameters = new Dictionary<string, object>();
            To = new List<string>();
            CC = new List<string>();
            BCC = new List<string>();
            HighPriorityFlag = false;
            Attachments = new List<DocumentData>();
        }

        public string GenerateRAW()
        {
            MailMessage systemMessage = new();

            foreach (var to in To)
            {
                if (!string.IsNullOrWhiteSpace(to))
                {
                    systemMessage.To.Add(new MailAddress(to.Trim()));
                }
            }

            systemMessage.From = new MailAddress(From);
            systemMessage.Subject = Subject;
            systemMessage.Body = Body;
            systemMessage.IsBodyHtml = true;

            if (!string.IsNullOrWhiteSpace(ReplyToEmailID))
            {
                systemMessage.ReplyToList.Add(new MailAddress(ReplyToEmailID));
            }

            if (HighPriorityFlag)
            {
                systemMessage.Priority = MailPriority.High;
            }

            foreach (var cc in CC ?? new List<string>())
            {
                if (!string.IsNullOrWhiteSpace(cc))
                {
                    systemMessage.CC.Add(new MailAddress(cc.Trim()));
                }
            }

            foreach (var bcc in BCC ?? new List<string>())
            {
                if (!string.IsNullOrWhiteSpace(bcc))
                {
                    systemMessage.Bcc.Add(new MailAddress(bcc.Trim()));
                }
            }

            foreach (var attachment in Attachments)
            {
                systemMessage.Attachments.Add(new Attachment(attachment.DocumentStream, attachment.DisplayName));
            }

            MimeKit.MimeMessage mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(systemMessage);
            mimeMessage.MessageId = MessageID;

            return Util.EncodeRAW(mimeMessage.ToString());
        }
    }
}
