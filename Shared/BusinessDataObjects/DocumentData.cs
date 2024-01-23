using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace ADIRA.Shared.BusinessDataObjects
{
    public class DocumentData
    {
        public Guid DocumentId { get; set; }
        public int ResourceTypeCode { get; set; }
        public Guid ResourceValue { get; set; }
        public string DisplayName { get; set; }
        public string FileLocation { get; set; }
        public string FileExtension { get; set; }
        public DateTime UploadedDateTime { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public int DocumentPurposeCode { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Guid LastUpdatedId { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        [JsonIgnore]
        public bool IsNewDocument { get; set; }
        public string ContentType { get; set; }
        [JsonIgnore]
        public bool IsSavedInDB { get; set; }
        public bool EncryptionFlag { get; set; }
        public Stream DocumentStream { get; set; }
        public byte[] DocumentBytes { get; set; }
        public byte[] SignatureImage { get; set; }
        [JsonIgnore]
        public IFormFile DocumentInfo { get; set; }
    }
}
