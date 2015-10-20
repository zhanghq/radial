using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Radial.IO
{
    /// <summary>
    /// VideoMedia.
    /// </summary>
    public class VideoMedia
    {
        /// <summary>
        /// The FFMPEG_EXE_PATH.
        /// </summary>
        private static string FFMPEG_EXE_PATH = CheckRelativePath(@"ffmpeg.exe");
        /// <summary>
        /// The FFPROBE_EXE_PATH.
        /// </summary>
        private static string FFPROBE_EXE_PATH = CheckRelativePath(@"ffprobe.exe");

        #region static helpers

        /// <summary>
        /// Safely converts a string in format h:m:s.f to a TimeSpan using Regex allowing every part being as long as is
        /// </summary>
        private static TimeSpan ConvertFFmpegTimeSpan(string value)
        {
            Match m = _rexTimeSpan.Match(value);
            double v = 0.0;
            if (m == null || !m.Success) return new TimeSpan();

            if (!String.IsNullOrWhiteSpace(m.Groups["h"].Value))
            {
                int n = 0;
                int.TryParse(m.Groups["h"].Value, out n);
                v += n;
            }
            v *= 60.0;

            if (!String.IsNullOrWhiteSpace(m.Groups["m"].Value))
            {
                int n = 0;
                int.TryParse(m.Groups["m"].Value, out n);
                v += n;
            }
            v *= 60.0;

            if (!String.IsNullOrWhiteSpace(m.Groups["s"].Value))
            {
                int n = 0;
                int.TryParse(m.Groups["s"].Value, out n);
                v += n;
            }

            if (!String.IsNullOrWhiteSpace(m.Groups["f"].Value))
            {
                double n = 0;
                double.TryParse(String.Format("0{1}{0}", m.Groups["f"].Value,
                    CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out n);
                v += n;
            }

            return TimeSpan.FromSeconds(v);
        }
        private static readonly Regex _rexTimeSpan = new Regex(@"^(((?<h>\d+):)?(?<m>\d+):)?(?<s>\d+)([\.,](?<f>\d+))?$", RegexOptions.Compiled);

        /// <summary>
        /// Checks if the passed path is rooted and if not resolves it relative to the calling assembly
        /// </summary>
        private static string CheckRelativePath(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                string appDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().GetName().CodeBase);
                path = Path.Combine(appDir, path);
            }

            if (path.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
                path = path.Substring(6);

            return path;
        }

        /// <summary>
        /// Executes a process and passes its command-line output back after the process has exitted
        /// </summary>
        private static string Execute(string exePath, string parameters)
        {
            string result = String.Empty;

            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = exePath;
                p.StartInfo.Arguments = parameters;
                p.Start();
                p.WaitForExit();

                result = p.StandardOutput.ReadToEnd();
            }

            return result;
        }

        #endregion

        #region Properies


        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// Gets or sets the name of the format.
        /// </summary>
        /// <value>
        /// The name of the format.
        /// </value>
        public string FormatName { get; set; }
        /// <summary>
        /// Gets or sets the format name long.
        /// </summary>
        public string FormatNameLong { get; set; }
        /// <summary>
        /// Gets or sets the bit rate.
        /// </summary>
        public string BitRate { get; set; }

        /// <summary>
        /// Gets or sets the streams.
        /// </summary>
        public IList<VideoMediaStream> Streams { get; set; }
        /// <summary>
        /// Gets or sets the other values.
        /// </summary>
        public IDictionary<string, string> OtherValues { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height { get; set; }

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="VideoMedia"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public VideoMedia(string filePath)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(filePath), "file path can not be null or empty");

            Checker.Parameter(File.Exists(filePath), "can not found file: {0}", filePath);

            Streams = new List<VideoMediaStream>();
            OtherValues = new Dictionary<string, string>();

            this.FilePath = filePath;

            string cmdParams = String.Format("-hide_banner -show_format -show_streams -pretty {1}{0}{1}", filePath, filePath.Contains(' ') ? "\"" : "");
            string[] lines = Execute(FFPROBE_EXE_PATH, cmdParams).Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
            int curr = 0;
            VideoMediaStream stream = null;
            StringComparison sc = StringComparison.OrdinalIgnoreCase;
            string name;
            string value;
            foreach (string line in lines)
            {
                if (line.StartsWith("[/"))
                    curr = 0;
                else if (line.StartsWith("[FORMAT]"))
                    curr = 1;
                else if (line.StartsWith("[STREAM]"))
                {
                    stream = new VideoMediaStream();
                    this.Streams.Add(stream);
                    curr = 2;
                }
                else if (curr > 0)
                {
                    // Split name and value
                    int pos = line.IndexOf('=');
                    if (pos < 1) continue;
                    name = line.Substring(0, pos);
                    value = (pos == line.Length - 1) ? String.Empty : line.Substring(pos + 1);

                    if (curr == 1)
                    {
                        #region FFmpegMediaInfo fields
                        if (name.Equals("format_name", sc))
                            this.FormatName = value;
                        else if (name.Equals("format_long_name", sc))
                            this.FormatNameLong = value;
                        else if (name.Equals("duration", sc))
                            this.Duration = ConvertFFmpegTimeSpan(value);
                        else if (name.Equals("bit_rate", sc))
                            this.BitRate = value;
                        else
                            this.OtherValues.Add(name, value);
                        #endregion
                    }
                    else if (curr == 2)
                    {
                        #region FFmpegStreamInfo fields
                        if (name.Equals("index", sc))
                        {
                            int index = 0;
                            int.TryParse(value, out index);
                            stream.Index = index;
                        }
                        else if (name.Equals("codec_name", sc))
                            stream.CodecName = value;
                        else if (name.Equals("codec_long_name", sc))
                            stream.CodecNameLong = value;
                        else if (name.Equals("codec_type", sc))
                            stream.CodecType = value;
                        else if (name.Equals("codec_time_base", sc))
                            stream.CodecTimeBase = value;
                        else if (name.Equals("codec_tag_string", sc))
                            stream.CodecTagString = value;
                        else if (name.Equals("codec_tag", sc))
                            stream.CodecTag = value;
                        else if (name.Equals("width", sc))
                        {
                            int width = 0;
                            int.TryParse(value, out width);
                            stream.Width = width;
                        }
                        else if (name.Equals("height", sc))
                        {
                            int height = 0;
                            int.TryParse(value, out height);
                            stream.Height = height;
                        }
                        else if (name.Equals("sample_aspect_ratio", sc))
                            stream.SampleAspectRatio = value;
                        else if (name.Equals("display_aspect_ratio", sc))
                            stream.DisplayAspectRation = value;
                        else if (name.Equals("pix_fmt", sc))
                            stream.PixelFormat = value;
                        else if (name.Equals("r_frame_rate", sc))
                            stream.FrameRate = value;
                        else if (name.Equals("start_time", sc))
                            stream.StartTime = ConvertFFmpegTimeSpan(value);
                        else if (name.Equals("duration", sc))
                            stream.Duration = ConvertFFmpegTimeSpan(value);
                        else if (name.Equals("nb_frames", sc))
                        {
                            long frameCount = 0;
                            long.TryParse(value, out frameCount);
                            stream.FrameCount = frameCount;
                        }
                        else
                            stream.OtherValues.Add(name, value);
                        #endregion
                    }
                }
            }

            // Search the first video stream and copy video size to FFmpegMediaInfo for easier access
            VideoMediaStream video = this.Streams.FirstOrDefault(s => s.CodecType.Equals("video", sc));
            if (video != null)
            {
                this.Width = video.Width;
                this.Height = video.Height;
            }

        }


        /// <summary>
        /// Gets the snapshot.
        /// </summary>
        /// <param name="positioin">The positioin(in second, default to 1).</param>
        /// <returns></returns>
        public Image GetSnapshot(float positioin = 1)
        {
            return GetSnapshot(TimeSpan.FromSeconds(positioin));
        }

        /// <summary>
        /// Gets the snapshot.
        /// </summary>
        /// <param name="positioin">The positioin.</param>
        /// <returns></returns>
        public Image GetSnapshot(TimeSpan positioin)
        {
            if (positioin > Duration)
                positioin = Duration;

            string filename = this.FilePath;

            string tmpFileName = Path.Combine(Path.GetDirectoryName(filename), Path.GetRandomFileName());

            string cmdParams = String.Format("-hide_banner -ss {0} -i \"{1}\" -r 1 -t 1 -f image2 \"{2}\"", positioin, filename, tmpFileName);

            Bitmap result = null;

            Execute(FFMPEG_EXE_PATH, cmdParams);

            if (File.Exists(tmpFileName))
            {
                // Do not open the Bitmap directly from the file, because then the file is locked until the Bitmap is disposed!
                result = new Bitmap(new MemoryStream(File.ReadAllBytes(tmpFileName)));
                File.Delete(tmpFileName);
            }


            return result;
        }
    }

    /// <summary>
    /// VideoMediaStream.
    /// </summary>
    public class VideoMediaStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoMediaStream"/> class.
        /// </summary>
        public VideoMediaStream()
        {
            OtherValues = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Gets or sets the name of the codec.
        /// </summary>
        /// <value>
        /// The name of the codec.
        /// </value>
        public string CodecName { get; set; }
        /// <summary>
        /// Gets or sets the codec name long.
        /// </summary>
        public string CodecNameLong { get; set; }
        /// <summary>
        /// Gets or sets the type of the codec.
        /// </summary>
        /// <value>
        /// The type of the codec.
        /// </value>
        public string CodecType { get; set; }
        /// <summary>
        /// Gets or sets the codec time base.
        /// </summary>
        public string CodecTimeBase { get; set; }
        /// <summary>
        /// Gets or sets the codec tag string.
        /// </summary>
        public string CodecTagString { get; set; }
        /// <summary>
        /// Gets or sets the codec tag.
        /// </summary>
        public string CodecTag { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets the sample aspect ratio.
        /// </summary>
        public string SampleAspectRatio { get; set; }
        /// <summary>
        /// Gets or sets the display aspect ration.
        /// </summary>
        public string DisplayAspectRation { get; set; }
        /// <summary>
        /// Gets or sets the pixel format.
        /// </summary>
        public string PixelFormat { get; set; }
        /// <summary>
        /// Gets or sets the frame rate.
        /// </summary>
        public string FrameRate { get; set; }
        /// <summary>
        /// Gets or sets the frame count.
        /// </summary>
        public long FrameCount { get; set; }
        /// <summary>
        /// Gets or sets the sample rate.
        /// </summary>
        public string SampleRate { get; set; }
        /// <summary>
        /// Gets or sets the channels.
        /// </summary>
        public int Channels { get; set; }
        /// <summary>
        /// Gets or sets the channel layout.
        /// </summary>
        public string ChannelLayout { get; set; }
        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the other values.
        /// </summary>
        public IDictionary<string, string> OtherValues { get; set; }
    }
}
