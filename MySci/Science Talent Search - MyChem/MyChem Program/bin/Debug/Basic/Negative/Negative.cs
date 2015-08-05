using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LearnWPF.Effects
{
    public class NegativeEffect : ShaderEffect
    {
        #region Constructors

        static NegativeEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Basic/Negative/Negative.ps");
        }

        public NegativeEffect()
        {
            this.PixelShader = _pixelShader;
            UpdateShaderValue(InputProperty);
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(NegativeEffect), 0);
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        #endregion

        #region Member Data

        private static PixelShader _pixelShader = new PixelShader();

        #endregion

    }
}
