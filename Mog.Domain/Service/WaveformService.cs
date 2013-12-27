using MoG.Domain.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class WaveformService : IWaveformService
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        private int BORDER_WIDTH = 0;


        public Metadata Metadata { get; set; }
        private WaveStream reader = null;


        public WaveformService()
        {
            Width = 750;
            Height = 200;
            BackgroundColor = Color.WhiteSmoke;
            ForegroundColor = Color.DarkGray;
        }



        public Bitmap GetWaveform()
        {

            long sizeOfStream = reader.Length;
            byte[] stream = new byte[sizeOfStream];
            reader.Read(stream, 0, (int)sizeOfStream);

            List<float> data = new List<float>();
            for (int index = 0; index < sizeOfStream; index += 2)
            {
                short sample = (short)((stream[index + 1] << 8) |
                                        stream[index]);
                float sample32 = sample / 32768f;
                data.Add(sample32);
            }
            reader.Close();

            Bitmap image = DrawNormalizedAudio(data, this.ForegroundColor, this.BackgroundColor, new Size(this.Width, this.Height));
            return image;
        }



        private Bitmap DrawNormalizedAudio(List<float> data, Color foreColor, Color backColor, Size imageSize)
        {
            Bitmap bmp = new Bitmap(imageSize.Width, imageSize.Height);

            float width = bmp.Width - (2 * BORDER_WIDTH);
            float height = bmp.Height - (2 * BORDER_WIDTH);
            Color outsideColor = Color.FromArgb(foreColor.R / 2, foreColor.G / 2, foreColor.B / 2);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(backColor);
                Pen pen = new Pen(foreColor);
                Pen outsidePen = new Pen(outsideColor);
                float size = data.Count;
                for (float iPixel = 0; iPixel < width; iPixel += 1)
                {
                    // determine start and end points within WAV
                    int start = (int)(iPixel * (size / width));
                    int end = (int)((iPixel + 1) * (size / width));
                    if (end > data.Count)
                        end = data.Count;

                    float posAvg, negAvg;
                    averages(data, start, end, out posAvg, out negAvg);

                    float yMax = BORDER_WIDTH + height - ((posAvg + 1) * .5f * height);
                    float yMin = BORDER_WIDTH + height - ((negAvg + 1) * .5f * height);

                    Brush b = Brushes.Black;
                    g.FillRectangle(b, iPixel + BORDER_WIDTH, yMax - 1, 1, 1);
                    g.DrawLine(outsidePen, iPixel + BORDER_WIDTH, yMax, iPixel + BORDER_WIDTH, yMin);
                    g.FillRectangle(b, iPixel + BORDER_WIDTH, yMin, 1, 1);
                }
            }


            return bmp;
        }


        private void averages(List<float> data, int startIndex, int endIndex, out float posAvg, out float negAvg)
        {
            posAvg = 0.0f;
            negAvg = 0.0f;

            int posCount = 0, negCount = 0;

            for (int i = startIndex; i < endIndex; i++)
            {

                if (data[i] > 0)
                {
                    posCount++;
                    posAvg += data[i];
                }
                else
                {
                    negCount++;
                    negAvg += data[i];
                }


            }

            posAvg /= posCount;
            negAvg /= negCount;
        }







        public void Initialize(string filename)
        {
            string extension = System.IO.Path.GetExtension(filename);

            switch (extension.ToLower())
            {
                case ".mp3":
                    Mp3FileReader mp3Reader = new Mp3FileReader(filename);
                    extractMp3MetaData(mp3Reader);

                    this.reader = mp3Reader;
                    break;
                case ".wav":
                    this.reader = new WaveFileReader(filename);
                    break;
                default:
                    this.reader = new Mp3FileReader(filename);
                    break;
            }
        }
        private void extractMp3MetaData(Mp3FileReader mp3Reader)
        {
            Mp3Metadata mp3meta = new Mp3Metadata();
            var duration = mp3Reader.TotalTime;
            mp3meta.Duration = String.Format("{0:00}:{1:00}", duration.Minutes, duration.Seconds);
            this.Metadata = mp3meta;
        }



    }
    public interface IWaveformService
    {

        Bitmap GetWaveform();

        int Width { get; set; }
        int Height { get; set; }
        Metadata Metadata { get; set; }
        Color BackgroundColor { get; set; }

        Color ForegroundColor { get; set; }


        void Initialize(string p);


    }
}

