using System.Collections.Generic;

namespace TranscriptToVTT.AWS
{
    public class TranscriptResultItem
    {
        public string Start_Time { get; set; }
        public string End_Time { get; set; }
        public List<TranscriptResultItemAlternatives> Alternatives { get; set; }
        /// <summary>
        /// Possible observed values are: "pronunciation" and "punctuation" (The reason this is not defined as an enum is in case Amazon added more values)
        /// </summary>
        public string Type { get; set; }
    }
}
