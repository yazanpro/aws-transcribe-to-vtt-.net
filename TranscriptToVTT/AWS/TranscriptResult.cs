using System.Collections.Generic;

namespace TranscriptToVTT.AWS
{
    public class TranscriptResult
    {
        public List<TranscriptResultText> Transcripts { get; set; }
        public List<TranscriptResultItem> Items { get; set; }
    }
}
