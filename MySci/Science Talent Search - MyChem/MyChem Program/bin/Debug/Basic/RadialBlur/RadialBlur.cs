using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LearnWPF.Effects
{
    public class RadialBlurEffect : ShaderEffect
    {
        private static PixelShader _pixelShader = new PixelShader();
        
        static RadialBlurEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Basic/RadialBlur/RadialBlur.ps");
        }

        public RadialBlurEffect()
        {
            this.PixelShader = _pixelShader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BlurCentreProperty);
            UpdateShaderValue(BlurMagnitudeProperty);
        }

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(RadialBlurEffect), 0);
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty BlurCentreProperty = DependencyProperty.Register("BlurCentre", typeof(Point), typeof(RadialBlurEffect), new UIPropertyMetadata(new Point(0.5d, 0.5d), PixelShaderConstantCallback(0)));
        public Point BlurCentre
        {
            get { return (Point)GetValue(BlurCentreProperty); }
            set { SetValue(BlurCentreProperty, value); }
        }

        public static readonly DependencyProperty BlurMagnitudeProperty = DependencyProperty.Register("BlurMagnitude", typeof(double), typeof(RadialBlurEffect), new UIPropertyMetadata(0.001d, PixelShaderConstantCallback(1)));
        public double BlurMagnitude
        {
            get { return (double)GetValue(BlurMagnitudeProperty); }
            set { SetValue(BlurMagnitudeProperty, value); }
        }
    }
}
