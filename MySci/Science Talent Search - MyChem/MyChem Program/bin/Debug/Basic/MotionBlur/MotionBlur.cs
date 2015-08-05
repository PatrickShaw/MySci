using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LearnWPF.Effects
{
    public class MotionBlurEffect : ShaderEffect
    {
        private static PixelShader _pixelShader = new PixelShader();
        
        static MotionBlurEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Basic/MotionBlur/MotionBlur.ps");
        }

        public MotionBlurEffect()
        {
            this.PixelShader = _pixelShader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BlurAngleProperty);
            UpdateShaderValue(BlurMagnitudeProperty);
        }

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(MotionBlurEffect), 0);
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty BlurAngleProperty = DependencyProperty.Register("BlurAngle", typeof(double), typeof(MotionBlurEffect), new UIPropertyMetadata(0d, PixelShaderConstantCallback(0)));
        public double BlurAngle
        {
            get { return (double)GetValue(BlurAngleProperty); }
            set { SetValue(BlurAngleProperty, value); }
        }

        public static readonly DependencyProperty BlurMagnitudeProperty = DependencyProperty.Register("BlurMagnitude", typeof(double), typeof(MotionBlurEffect), new UIPropertyMetadata(0.001d, PixelShaderConstantCallback(1)));
        public double BlurMagnitude
        {
            get { return (double)GetValue(BlurMagnitudeProperty); }
            set { SetValue(BlurMagnitudeProperty, value); }
        }
    }
}
