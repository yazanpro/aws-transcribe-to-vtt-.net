# aws-transcribe-to-vtt-.net
Convert the JSON returned from AWS Transcribe to WebVTT format

Usage:
  
    TranscriptToVTT.TranscriptToVTTManager.GetVTTFromAWSTranscribeJson
    
  Parameters:
  
    json (string): The JSON content to be processed
    
    maxCharsPerCaption (int): The limit in which the length of the individual caption doesn't exceed (80 characters suggested)
    
    maxTimePerCaption (TimeSpan): The limit in which the duration of the individual caption doesn't exceed (5 seconds suggested)
    
    out transcript (string): The entire transcript in plain text
