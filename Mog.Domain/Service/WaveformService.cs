using MoG.Domain.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class WaveformService : IWaveformService
    {
        #region Fields
        public int Width { get; set; }
        public int Height { get; set; }

        public Color BackgroundColor { get; set; }

        public Color ForegroundColor { get; set; }

        private int BORDER_WIDTH = 0;


        private ILogService serviceLog = null;

        #endregion fields

        #region Constructors

        public WaveformService(ILogService logservice)
        {
            this.serviceLog = logservice;
            Width = 750;
            Height = 200;
            BackgroundColor = Color.WhiteSmoke;
            ForegroundColor = Color.DarkGray;
        }

        #endregion Constructors

        #region private  utils
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
                try
                {
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
                catch (Exception exc)
                {
                    this.serviceLog.LogError("waveformService::DrawNormalizedAudio", exc);

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

        private WaveStream getReader(string filename, Stream inputStream)
        {
           
            WaveStream result = null;
            string extension = System.IO.Path.GetExtension(filename);
            inputStream.Position = 0;

            switch (extension.ToLower())
            {
                case ".mp3":
                    Mp3FileReader mp3Reader = new NAudio.Wave.Mp3FileReader(inputStream
                        , new Mp3FileReader.FrameDecompressorBuilder
                            (waveFormat =>
                                new NLayer.NAudioSupport.Mp3FrameDecompressor
                                    (waveFormat)));


                    result = mp3Reader;
                    break;
                case ".wav":
                    result = new WaveFileReader(inputStream);
                    break;
            }
            return result;
        }


        #endregion 

        #region GetWaveForm
        private List<float> getWavWaveForm(WaveFileReader reader)
        {
            List<float> data = new List<float>();
            float[] buffer;

            while ((buffer = reader.ReadNextSampleFrame()) != null)
            {
                data.Add(buffer[0]);
            }
            return data;
        }

        private List<float> getMp3WaveForm(byte[] stream, long sizeOfStream)
        {
            List<float> data = new List<float>();
            for (int index = 0; index < sizeOfStream; index += 4)
            {
                byte[] myData = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    myData[i] = stream[index + i];
                }

                //Array.Reverse(myData);  // Deal with Endian issue?
                Single myvalue = BitConverter.ToSingle(myData, 0);
                data.Add((float)myvalue);
                //short sample = (short)((stream[index + 1] << 8) |
                //                        stream[index]);
                //float sample32 = sample / 32768f;
                //data.Add(sample32);
            }
            return data;
        }



        public Bitmap GetWaveform(string filename, Stream inputStream)
        {
            WaveStream reader = getReader(filename, inputStream);

            List<float> data = null;


            if (reader is Mp3FileReader)
            {
                long sizeOfStream = reader.Length;
                byte[] stream = new byte[sizeOfStream];
                reader.Read(stream, 0, (int)sizeOfStream);
                data = getMp3WaveForm(stream, sizeOfStream);
                reader.Close();
            }
            else
            {// we have a wav file reader
                data = getWavWaveForm(reader as WaveFileReader);
            }



            Bitmap image = DrawNormalizedAudio(data, this.ForegroundColor, this.BackgroundColor, new Size(this.Width, this.Height));
            return image;
        }

        #endregion getwaveform

        #region GetMetadata
        public Metadata GetMetadata(string filename, Stream inputStream)
        {
            WaveStream reader = getReader(filename, inputStream);
            Mp3Metadata mp3meta = new Mp3Metadata();
            var duration = reader.TotalTime;
            mp3meta.Duration = String.Format("{0:00}:{1:00}", duration.Minutes, duration.Seconds);
            return mp3meta;
        }
        #endregion 
    }
    public interface IWaveformService
    {

        Bitmap GetWaveform(string filename, Stream inputStream);
        Metadata GetMetadata(string filename, Stream inputStream);

        int Width { get; set; }
        int Height { get; set; }
        Color BackgroundColor { get; set; }

        Color ForegroundColor { get; set; }

    }
}

