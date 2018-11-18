using Newtonsoft.Json;
using System;
using System.Text;
using TranscriptToVTT.AWS;

namespace TranscriptToVTT
{
    public static class TranscriptToVTTManager
    {
        /// <summary>
        /// Processes the JSON returned from AWS Transcribe and converts it to WebVTT format
        /// </summary>
        /// <param name="json">The JSON content to be processed</param>
        /// <param name="maxCharsPerCaption">The limit in which the length of the individual caption doesn't exceed
        /// (the algorithm might tolerate this value slightly for best result)</param>
        /// <param name="maxTimePerCaption">The limit in which the duration of the individual caption doesn't exceed
        /// (The algorithm might tolerate this value slightly for best result)</param>
        /// <param name="transcript">The entire transcript in plain text</param>
        /// <returns>The WebVTT content</returns>
        public static string GetVTTFromAWSTranscribeJson(string json, int maxCharsPerCaption, TimeSpan maxTimePerCaption, out string transcript)
        {
            // set the out parameter
            transcript = null;
            if (string.IsNullOrWhiteSpace(json))
                return json;

            // Deserialize the JSON into AmazonTranscript object
            var amazonTranscript = JsonConvert.DeserializeObject<AmazonTranscript>(json);

            var vtt = new StringBuilder();
            if (amazonTranscript?.Results?.Items != null)
            {
                if (amazonTranscript.Results.Transcripts?.Count > 0)
                    transcript = amazonTranscript.Results.Transcripts[0].Transcript; // Set the entire transcript

                var runningStartTime = default(TimeSpan);
                var runningEndTime = default(TimeSpan);
                bool isStarted = false;
                var runningContent = new StringBuilder();

                for (int i = 0; i < amazonTranscript.Results.Items.Count; i++)
                {
                    var item = amazonTranscript.Results.Items[i];

                    if (item.Alternatives?.Count > 0)
                    {
                        // If there are more than 1 alternative, we only use the first once. Hopefully alternatives are natively sorted by confidence
                        var alternative = item.Alternatives[0];

                        // Possible observed values are: "pronunciation" and "punctuation"
                        if (item.Type == "punctuation")
                        {
                            // the punctuation doesn't have a space before it
                            if (runningContent.Length > 0)
                                runningContent.Length--;

                            // Append the punctuation to the running content right away
                            runningContent.Append(alternative.Content).Append(" ");
                        }
                        else if (item.Start_Time != null && item.Start_Time != null)
                        {
                            string[] startSecMilPair = item.Start_Time.Split('.');
                            string[] endSecMilPair = item.End_Time.Split('.');
                            if (startSecMilPair.Length >= 2 && endSecMilPair.Length >= 2
                                && int.TryParse(startSecMilPair[0], out int startSec) && int.TryParse(startSecMilPair[1], out int startMil)
                                && int.TryParse(endSecMilPair[0], out int endSec) && int.TryParse(endSecMilPair[1], out int endMil))
                            {
                                // capitalize the small letter i when necessary
                                alternative.ReplaceiWithI();

                                // Amazon uses seconds and milliseconds only
                                var startTime = new TimeSpan(0, 0, 0, startSec, startMil);
                                var endTime = new TimeSpan(0, 0, 0, endSec, endMil);

                                // Handle the first iteration
                                if (!isStarted)
                                {
                                    runningStartTime = startTime;
                                    isStarted = true;
                                }

                                // threshold check
                                if (endTime - runningStartTime > maxTimePerCaption || runningContent.Length > maxCharsPerCaption)
                                {
                                    // If the caption has just started then we don't append the running content just yet
                                    if (runningContent.Length > 0)
                                    {
                                        // Get rid of the last space
                                        runningContent.Length--;
                                        AppendToVTT(vtt, runningStartTime, runningEndTime, runningContent.ToString());
                                        runningContent.Clear();
                                    }
                                    // Save the running start and end times
                                    runningStartTime = startTime;
                                    runningEndTime = endTime;
                                }
                                // Append the alternative (a word) to the running value
                                runningContent.Append(alternative.Content).Append(" ");
                                runningEndTime = endTime;
                            }
                        }
                    }
                }
                // Append leftover running content if any
                if (runningContent.Length > 0)
                {
                    runningContent.Length--;
                    AppendToVTT(vtt, runningStartTime, runningEndTime, runningContent.ToString());
                    runningContent.Clear();
                }
            }
            
            return "WEBVTT" + Environment.NewLine + Environment.NewLine + vtt.ToString();
        }

        private static void AppendToVTT(StringBuilder vtt, TimeSpan startTime, TimeSpan endTime, string content)
        {
            if (vtt.Length > 0)
            {
                vtt.Append(Environment.NewLine);
                vtt.Append(Environment.NewLine);
            }
            vtt.Append(startTime.Hours.ToString("D2"));
            vtt.Append(":");
            vtt.Append(startTime.Minutes.ToString("D2"));
            vtt.Append(":");
            vtt.Append(startTime.Seconds.ToString("D2"));
            vtt.Append(".");
            vtt.Append(startTime.Milliseconds.ToString("D3"));
            vtt.Append(" --> ");

            vtt.Append(endTime.Hours.ToString("D2"));
            vtt.Append(":");
            vtt.Append(endTime.Minutes.ToString("D2"));
            vtt.Append(":");
            vtt.Append(endTime.Seconds.ToString("D2"));
            vtt.Append(".");
            vtt.Append(endTime.Milliseconds.ToString("D3"));

            vtt.Append(Environment.NewLine);

            vtt.Append(content);
        }
    }
}
