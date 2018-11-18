namespace TranscriptToVTT.AWS
{
    public class TranscriptResultItemAlternatives
    {
        public string Confidence { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// capitalize the small letter i when necessary (as in I'm good, I told you)
        /// </summary>
        public void ReplaceiWithI()
        {
            if (Content == "i")
                Content = "I";
            else if (Content != null && Content.StartsWith("i'"))
                Content = "I'" + Content.Substring(2);
        }
    }
}
