﻿namespace TranscriptToVTT.AWS
{
    public class TranscriptResultItemAlternative
    {
        public string Confidence { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// capitalize the small letter i when necessary (as in "I'm good" and "I told you")
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
