using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LearnWPF.Effects
{
    public class SepiaEffect : DuoTone
    {
        static SepiaEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Basic/Sepia/Sepia.ps");
        }

        public SepiaEffect()
        {
            this.PixelShader = _pixelShader;
            Initialize();
            UpdateShaderValue(LightColorProperty);
            UpdateShaderValue(DarkColorProperty);
        }

        public static readonly DependencyProperty TonedProperty = DependencyProperty.Register("Toned", typeof(double), typeof(SepiaEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(1), CoerceTone));
        public double Toned
        {
            get { return (double)GetValue(DesaturationFactorProperty); }
            set { SetValue(DesaturationFactorProperty, value); }
        }

        public static readonly DependencyProperty LightColorProperty = DependencyProperty.Register("LightColor", typeof(Color), typeof(SepiaEffect), new UIPropertyMetadata(Colors.GhostWhite, PixelShaderConstantCallback(2)));

        public Color LightColor
        {
            get { return (Color)GetValue(LightColorProperty); }
            set { SetValue(LightColorProperty, value); }
        }

        public static readonly DependencyProperty DarkColorProperty = DependencyProperty.Register("DarkColor", typeof(Color), typeof(SepiaEffect), new UIPropertyMetadata(Colors.Chocolate, PixelShaderConstantCallback(3)));

        public Color DarkColor
        {
            get { return (Color)GetValue(DarkColorProperty); }
            set { SetValue(DarkColorProperty, value); }
        }

        protected static object CoerceTone(DependencyObject d, object value)
        {
            SepiaEffect effect = (SepiaEffect)d;
            double newTone = (double)value;

            if (newTone < 0.0 || newTone > 1.0)
            {
                return effect.Toned;
            }

            return newTone;
        }


    }
}
