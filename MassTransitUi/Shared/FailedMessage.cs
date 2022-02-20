using System.ComponentModel.DataAnnotations;

namespace MassTransitUi.Shared
{
    public class FailedMessage
    {
        public long Id { get; set; }
        public string MessageId { get; set; }
        public string Queue { get; set; }
        public DateTime RecievedTsUtc { get; set; }
        public byte[] Content { get; set; }

        [Required]
        public List<FailedMessageHeader> Headers { get; set; }
        public string ErrorMessage { get; set; }
        public string Properties { get; set; }
    }

    public class FailedMessageHeader
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        //public FailedMessage FailedMessage { get; set; }
    }
}
