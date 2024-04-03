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
            var readOutputTask = process.StandardOutput.ReadToEndAsync();
            var readErrorTask = process.StandardError.ReadToEndAsync();

            process.WaitForExit();

            string output = await readOutputTask;
            string error = await readErrorTask;

            if (process.ExitCode == 0)
            {
                Console.WriteLine($"Video:{videoId} converted to HLS successfully.");

                var videoLength = await GetVideoDurationInSeconds(hlsPlaylistPath);
                
                _publishEndpoint.Publish<UpdateVideoToPublicRequest>(
                    new UpdateVideoToPublicRequest() { VideoId = videoId,VideoLength = videoLength});
                //_UpdateVideoToPublicRequestClient.Create(new UpdateVideoToPublicRequest() { VideoId = videoId });
                
                //delete notRendered.unknown file
                File.Delete(videoPath);
                
            }
            else
            {
                Console.WriteLine($"Failed to convert video:{videoId} to HLS.");
               //todo:retry with poly
               
               
                
                
                
            }
        }
    }



    public async Task<decimal> GetVideoDurationInSeconds(string playlistPath)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "ffprobe",
            Arguments = $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{playlistPath}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        using (Process process = new Process { StartInfo = startInfo })
        {
            process.Start();

            var readOutputTask =  process.StandardOutput.ReadToEndAsync();
            process.WaitForExit();
                
            string output = await readOutputTask;
            // Trim the output to remove any leading/trailing whitespace and parse it to double
            if (double.TryParse(output.Trim(), out double duration))
            {
                return Convert.ToDecimal(duration);
            }
            else
            {
                throw new InvalidOperationException("Failed to parse video duration.");
            }
        }
    }
   
   
   
   
   
   
}