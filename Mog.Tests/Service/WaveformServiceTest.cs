using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG.Domain.Service;
using MoG.App_Start;
using Ninject;
using MoG.Domain.Models;
using System.IO;

namespace MoG.Test.Service
{
    [TestClass]
    public class WaveformServiceTest
    {
        private IWaveformService service;


        [TestInitialize]
        public void MyTestInitialize()
        {
            var kernel = NinjectWebCommon.CreatePublicKernel();
            service = kernel.Get<IWaveformService>();


        }

        [TestMethod]
        public void WaveformService_GetMetadata()
        {
            string filenameAndPath = "data/mabenz.mp3";
            FileStream stream = new FileStream(filenameAndPath, FileMode.Open);

            Mp3Metadata metadata = service.GetMetadata(filenameAndPath, stream) as Mp3Metadata;

            Assert.IsNotNull(metadata);
            Assert.IsTrue(!String.IsNullOrEmpty(metadata.Duration));
        }

        [TestMethod]
        public void WaveformService_GetWaveform()
        {
            string filenameAndPath ="data/mabenz.mp3";
            FileStream stream = new FileStream(filenameAndPath, FileMode.Open);

           // service.Initialize("data/mabenz.mp3");

            var waveform = service.GetWaveform(filenameAndPath, stream);
            waveform.Save("data/mabenz_waveform.png");
            Assert.IsNotNull(waveform);
            Assert.IsTrue(waveform.Size.Width > 0);
        }

    }
}
