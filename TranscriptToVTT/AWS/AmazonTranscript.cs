namespace TranscriptToVTT.AWS
{
    /// <summary>
    /// The root class in which the JSON returned from AWS Transcribe will be deserialized into
    /// </summary>
    public class AmazonTranscript
    {
        public string JobName { get; set; }
        public string AccountId { get; set; }
        public TranscriptResult Results { get; set; }
        public string Status { get; set; }
    }
}
