using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LearnWPF.Effects
{
    public class GrayScaleEffect : DuoTone
    {
        static GrayScaleEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Basic/GrayScale/GrayScale.ps");
        }

        public GrayScaleEffect()
        {
            this.PixelShader = _pixelShader;
            Initialize();
        }
    }
}
