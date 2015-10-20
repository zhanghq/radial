using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Radial.Drawing
{
    /// <summary>
    /// Amount of random font warping to apply to rendered text
    /// </summary>
    public enum FontWarpFactor
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Low
        /// </summary>
        Low,
        /// <summary>
        /// Medium
        /// </summary>
        Medium,
        /// <summary>
        /// High
        /// </summary>
        High,
        /// <summary>
        /// Extreme
        /// </summary>
        Extreme
    }

    /// <summary>
    /// Amount of background noise to add to rendered image
    /// </summary>
    public enum BackgroundNoiseLevel
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Low
        /// </summary>
        Low,
        /// <summary>
        /// Medium
        /// </summary>
        Medium,
        /// <summary>
        /// High
        /// </summary>
        High,
        /// <summary>
        /// Extreme
        /// </summary>
        Extreme
    }

    /// <summary>
    /// Amount of curved line noise to add to rendered image
    /// </summary>
    public enum LineNoiseLevel
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Low
        /// </summary>
        Low,
        /// <summary>
        /// Medium
        /// </summary>
        Medium,
        /// <summary>
        /// High
        /// </summary>
        High,
        /// <summary>
        /// Extreme
        /// </summary>
        Extreme
    }

    /// <summary>
    /// CAPTCHA Image
    /// </summary>
    /// <seealso href="http://www.codinghorror.com">Original By Jeff Atwood</seealso>
    public class Captcha
    {
        #region Static

        /// <summary>
        /// RandomFontFamily
        /// </summary>
        private static readonly string[] RandomFontFamily = { "arial", "arial black", "comic sans ms", "courier new", "estrangelo edessa", "franklin gothic medium", "georgia", "lucida console", "lucida sans unicode", "mangal", "microsoft sans serif", "palatino linotype", "sylfaen", "tahoma", "times new roman", "trebuchet ms", "verdana" };

        /// <summary>
        /// RandomColor
        /// </summary>
        private static readonly Color[] RandomColor = { Color.Brown, Color.DodgerBlue, Color.DodgerBlue, Color.Black, Color.LightSeaGreen, Color.DarkOrange };

        /// <summary>
        /// Gets or sets a string of available text characters for the generator to use.
        /// </summary>
        /// <value>The text chars.</value>
        public static string TextChars { get; set; }

        /// <summary>
        /// Gets or sets the length of the text.
        /// </summary>
        /// <value>The length of the text.</value>
        public static int TextLength { get; set; }

        /// <summary>
        /// Gets and sets amount of random warping to apply to the <see cref="Captcha"/> instance.
        /// </summary>
        /// <value>The font warp.</value>
        public static FontWarpFactor FontWarp { get; set; }

        /// <summary>
        /// Gets and sets amount of background noise to apply to the <see cref="Captcha"/> instance.
        /// </summary>
        /// <value>The background noise.</value>
        public static BackgroundNoiseLevel BackgroundNoise { get; set; }

        /// <summary>
        /// Gets or sets amount of line noise to apply to the <see cref="Captcha"/> instance.
        /// </summary>
        /// <value>The line noise.</value>
        public static LineNoiseLevel LineNoise { get; set; }


        /// <summary>
        /// Gets or sets the canvas color.
        /// </summary>
        /// <value>
        /// The canvas color.
        /// </value>
        public static Color CanvasColor { get; set; }


        #endregion

        private int _height;
        private int _width;
        private Random _rand;

        #region Public Properties

        /// <summary>
        /// Returns the date and time this image was last rendered
        /// </summary>
        /// <value>The rendered at.</value>
        public DateTime RenderedAt { get; private set; }

        /// <summary>
        /// Gets the randomly generated Captcha text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Width of Captcha image to generate, in pixels
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get { return _width; }
            private set
            {
                if (value < 40)
                    throw new ArgumentOutOfRangeException("width", value, "width must be greater than or equal to 40.");

                _width = value;
            }
        }

        /// <summary>
        /// Height of Captcha image to generate, in pixels
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get { return _height; }
            private set
            {
                if (value < 20)
                    throw new ArgumentOutOfRangeException("height", value, "height must be greater than or equal to 20.");

                _height = value;
            }
        }

        #endregion

        /// <summary>
        /// Initializes the <see cref="Captcha"/> class.
        /// </summary>
        static Captcha()
        {
            FontWarp = FontWarpFactor.Medium;
            BackgroundNoise = BackgroundNoiseLevel.Low;
            LineNoise = LineNoiseLevel.Low;
            TextLength = 4;
            TextChars = "ACDEFGHJKLNPQRTUVXYZ2346789";
            CanvasColor = Color.White;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Captcha"/> class.
        /// </summary>
        /// <param name="width">The image width.</param>
        /// <param name="height">The image height.</param>
        public Captcha(int width, int height)
            : this(string.Empty, width, height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Captcha"/> class.
        /// </summary>
        /// <param name="text">The captcha text.</param>
        /// <param name="width">The image width.</param>
        /// <param name="height">The image height.</param>
        public Captcha(string text,int width,int height)
        {
            _rand = new Random();
            Width = width;
            Height = height;
            if (!string.IsNullOrWhiteSpace(text))
                Text = text.Trim();
            else
                Text = GenerateRandomText();
            RenderedAt = DateTime.Now;
        }

        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        /// <returns></returns>
        public Image RenderImage()
        {
            return GenerateImagePrivate();
        }


        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        /// <param name="format">The image format.</param>
        /// <returns></returns>
        public Stream RenderImage(ImageFormat format)
        {
            Checker.Parameter(format != null, "image format can not be null");
            Image img = RenderImage();

            Stream stream = new MemoryStream();

            img.Save(stream, format);

            return stream;
        }

        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        /// <param name="format">The image format.</param>
        /// <returns></returns>
        public byte[] RenderImageBytes(ImageFormat format)
        {
            List<byte> bytes = new List<byte>();
            Stream s=RenderImage(format);
            s.Position = 0;
            using (StreamReader sr = new StreamReader(s))
            {
                while (!sr.EndOfStream)
                {
                    bytes.Add((byte)sr.Read());
                }
            }
            return bytes.ToArray();
        }

        /// <summary>
        /// Returns a random font family from the font whitelist
        /// </summary>
        /// <returns></returns>
        private string GetRandomFontFamily()
        {
            return RandomFontFamily[_rand.Next(0, RandomFontFamily.Length)];
        }

        /// <summary>
        /// generate random text for the CAPTCHA
        /// </summary>
        /// <returns></returns>
        private string GenerateRandomText()
        {
            StringBuilder sb = new StringBuilder(TextLength);
            int maxLength = TextChars.Length;
            for (int n = 0; n <= TextLength - 1; n++)
                sb.Append(TextChars.Substring(_rand.Next(maxLength), 1));

            return sb.ToString();
        }

        /// <summary>
        /// Returns a random point within the specified x and y ranges
        /// </summary>
        /// <param name="xmin">The xmin.</param>
        /// <param name="xmax">The xmax.</param>
        /// <param name="ymin">The ymin.</param>
        /// <param name="ymax">The ymax.</param>
        /// <returns></returns>
        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(_rand.Next(xmin, xmax), _rand.Next(ymin, ymax));
        }

        /// <summary>
        /// Randoms the color.
        /// </summary>
        /// <returns></returns>
        private Color GetRandomColor()
        {
            return RandomColor[_rand.Next(0, RandomColor.Length)];
        }

        /// <summary>
        /// Returns a random point within the specified rectangle
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        /// <summary>
        /// Returns a GraphicsPath containing the specified string and font
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="f">The f.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        private GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            GraphicsPath gp = new GraphicsPath();
            gp.AddString(s, f.FontFamily, (int)f.Style, f.Size, r, sf);
            return gp;
        }

        /// <summary>
        /// Returns the CAPTCHA font in an appropriate size
        /// </summary>
        /// <returns></returns>
        private Font GetFont()
        {
            float fsize;
            string fname = GetRandomFontFamily();

            switch (FontWarp)
            {
                case FontWarpFactor.None:
                    goto default;
                case FontWarpFactor.Low:
                    fsize = Convert.ToInt32(_height * 0.8);
                    break;
                case FontWarpFactor.Medium:
                    fsize = Convert.ToInt32(_height * 0.85);
                    break;
                case FontWarpFactor.High:
                    fsize = Convert.ToInt32(_height * 0.9);
                    break;
                case FontWarpFactor.Extreme:
                    fsize = Convert.ToInt32(_height * 0.95);
                    break;
                default:
                    fsize = Convert.ToInt32(_height * 0.7);
                    break;
            }
            return new Font(fname, fsize, FontStyle.Bold);
        }

        /// <summary>
        /// Renders the CAPTCHA image
        /// </summary>
        /// <returns></returns>
        private Bitmap GenerateImagePrivate()
        {
            Bitmap bmp = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);

            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.Clear(CanvasColor);

                int charOffset = 0;
                double charWidth = _width / TextLength;
                Rectangle rectChar;
                foreach (char c in Text)
                {
                    // establish font and draw area
                    using (Font fnt = GetFont())
                    {
                        using (Brush fontBrush = new SolidBrush(GetRandomColor()))
                        {
                            rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), _height);

                            // warp the character
                            GraphicsPath gp = TextPath(c.ToString(), fnt, rectChar);
                            WarpText(gp, rectChar);

                            // draw the character
                            gr.FillPath(fontBrush, gp);

                            charOffset += 1;
                        }
                    }
                }

                Rectangle rect = new Rectangle(new Point(0, 0), bmp.Size);
                AddNoise(gr, rect);
                AddLine(gr, rect);
            }
            return bmp;
        }

        /// <summary>
        /// Warp the provided text GraphicsPath by a variable amount
        /// </summary>
        /// <param name="textPath">The text path.</param>
        /// <param name="rect">The rect.</param>
        private void WarpText(GraphicsPath textPath, Rectangle rect)
        {
            float WarpDivisor;
            float RangeModifier;

            switch (FontWarp)
            {
                case FontWarpFactor.None:
                    goto default;
                case FontWarpFactor.Low:
                    WarpDivisor = 6F;
                    RangeModifier = 1F;
                    break;
                case FontWarpFactor.Medium:
                    WarpDivisor = 5F;
                    RangeModifier = 1.3F;
                    break;
                case FontWarpFactor.High:
                    WarpDivisor = 4.5F;
                    RangeModifier = 1.4F;
                    break;
                case FontWarpFactor.Extreme:
                    WarpDivisor = 4F;
                    RangeModifier = 1.5F;
                    break;
                default:
                    return;
            }

            RectangleF rectF;
            rectF = new RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);

            int hrange = Convert.ToInt32(rect.Height / WarpDivisor);
            int wrange = Convert.ToInt32(rect.Width / WarpDivisor);
            int left = rect.Left - Convert.ToInt32(wrange * RangeModifier);
            int top = rect.Top - Convert.ToInt32(hrange * RangeModifier);
            int width = rect.Left + rect.Width + Convert.ToInt32(wrange * RangeModifier);
            int height = rect.Top + rect.Height + Convert.ToInt32(hrange * RangeModifier);

            if (left < 0)
                left = 0;
            if (top < 0)
                top = 0;
            if (width > this.Width)
                width = this.Width;
            if (height > this.Height)
                height = this.Height;

            PointF leftTop = RandomPoint(left, left + wrange, top, top + hrange);
            PointF rightTop = RandomPoint(width - wrange, width, top, top + hrange);
            PointF leftBottom = RandomPoint(left, left + wrange, height - hrange, height);
            PointF rightBottom = RandomPoint(width - wrange, width, height - hrange, height);

            PointF[] points = new PointF[] { leftTop, rightTop, leftBottom, rightBottom };
            Matrix m = new Matrix();
            m.Translate(0, 0);
            textPath.Warp(points, rectF, m, WarpMode.Perspective, 0);
        }


        /// <summary>
        /// Add a variable level of graphic noise to the image
        /// </summary>
        /// <param name="g">The graphics obj.</param>
        /// <param name="rect">The rect.</param>
        private void AddNoise(Graphics g, Rectangle rect)
        {
            int density;
            int size;

            switch (BackgroundNoise)
            {
                case BackgroundNoiseLevel.None:
                    goto default;
                case BackgroundNoiseLevel.Low:
                    density = 30;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.Medium:
                    density = 18;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.High:
                    density = 16;
                    size = 39;
                    break;
                case BackgroundNoiseLevel.Extreme:
                    density = 12;
                    size = 38;
                    break;
                default:
                    return;
            }

            SolidBrush br = new SolidBrush(GetRandomColor());
            int max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);

            for (int i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / density); i++)
                g.FillEllipse(br, _rand.Next(rect.Width), _rand.Next(rect.Height), _rand.Next(max), _rand.Next(max));

            br.Dispose();
        }

        /// <summary>
        /// Add variable level of curved lines to the image
        /// </summary>
        /// <param name="g">The graphics obj.</param>
        /// <param name="rect">The rect.</param>
        private void AddLine(Graphics g, Rectangle rect)
        {
            int length;
            float width;
            int linecount;

            switch (LineNoise)
            {
                case LineNoiseLevel.None:
                    goto default;
                case LineNoiseLevel.Low:
                    length = 4;
                    width = Convert.ToSingle(_height / 31.25);
                    linecount = 1;
                    break;
                case LineNoiseLevel.Medium:
                    length = 5;
                    width = Convert.ToSingle(_height / 27.7777);
                    linecount = 1;
                    break;
                case LineNoiseLevel.High:
                    length = 3;
                    width = Convert.ToSingle(_height / 25);
                    linecount = 2;
                    break;
                case LineNoiseLevel.Extreme:
                    length = 3;
                    width = Convert.ToSingle(_height / 22.7272);
                    linecount = 3;
                    break;
                default:
                    return;
            }

            PointF[] pf = new PointF[length + 1];
            using (Pen p = new Pen(GetRandomColor(), width))
            {
                for (int l = 1; l <= linecount; l++)
                {
                    for (int i = 0; i <= length; i++)
                        pf[i] = RandomPoint(rect);

                    g.DrawCurve(p, pf, 1.75F);
                }
            }
        }
    }
}
