using System.Diagnostics;
using Common.MBcontracts;
using MassTransit;

namespace Render.Services;

public class RenderService(IBus bus, IPublishEndpoint publishEndpoint)
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
   // private readonly IRequestClient<UpdateVideoToPublicRequest> _UpdateVideoToPublicRequestClient= bus.CreateRequestClient<UpdateVideoToPublicRequest>();
   public async Task RenderVideo(Guid videoId, string videoPath)
    {
      
        string videoDirectory = videoPath.Replace("notRendered.unknown","");
       // Directory.CreateDirectory(videoDirectory); // Ensure the output directory exists

       
      //  string hlsSegmentFilename = Path.Combine(videoDirectory, "output_%03d.ts");
       
        string hlsPlaylistPath = Path.Combine(videoDirectory, "playlist.m3u8");

      
        string ffmpegArgs = $"-i \"{videoPath}\" -vf \"scale=-2:720\" -c:v libx264 -crf 23 -preset veryfast -c:a aac -b:a 128k -start_number 0 -hls_time 10 -hls_list_size 0 -f hls \"{hlsPlaylistPath}\"";

        // Start the FFmpeg process
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = ffmpegArgs,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using (Process process = new Process { StartInfo = startInfo })
        {
            process.Start();

            // Asynchronously read the standard output and standard error
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            process.WaitForExit();

          

            if (process.ExitCode == 0)
            {
                Console.WriteLine($"Video:{videoId} converted to HLS successfully.");

                _publishEndpoint.Publish<UpdateVideoToPublicRequest>(
                    new UpdateVideoToPublicRequest() { VideoId = videoId });
                //_UpdateVideoToPublicRequestClient.Create(new UpdateVideoToPublicRequest() { VideoId = videoId });
            }
            else
            {
                Console.WriteLine($"Failed to convert video:{videoId} to HLS.");
               //todo:retry with poly
                
                
                
            }
        }
    }
}