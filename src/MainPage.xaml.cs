using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ColorWheel
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void CanvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            var radius = Math.Min(sender.ActualWidth, sender.ActualHeight) / 2;
            var centerX = sender.ActualWidth / 2;
            var centerY = sender.ActualHeight / 2;

            args.DrawingSession.FillRectangle(0, 0, (float)sender.ActualWidth, (float)sender.ActualHeight, Colors.White);
            DrawColorWheel(args.DrawingSession, centerX, centerY, radius);
        }

        public static void DrawColorWheel(CanvasDrawingSession ds, double centerX, double centerY, double radius)
        {
            var list = new List<(double xRatio, double yRatio, Color color1, Color color2, Color color3, Color color4, Color color5, Color color6)>()
            {
                (1, 0.5, rgb(255, 0, 0), rgba(242, 13, 13, 0.8), rgba(230, 26, 26, 0.6), rgba(204, 51, 51, 0.4), rgba(166, 89, 89, 0.2), rgba(128, 128, 128, 0)),
                (0.853553, 0.853553, rgb(255, 191, 0), rgba(242, 185, 13, 0.8), rgba(230, 179, 26, 0.6), rgba(204, 166, 51, 0.4), rgba(166, 147, 89, 0.2), rgba(128, 128, 128, 0)),
                (0.5, 1, rgb(128, 255, 0), rgba(128, 242, 13, 0.8), rgba(128, 230, 26, 0.6), rgba(128, 204, 51, 0.4), rgba(128, 166, 89, 0.2), rgba(128, 128, 128, 0)),
                (0.146447, 0.853553, rgb(0, 255, 64), rgba(13, 242, 70, 0.8), rgba(26, 230, 77, 0.6), rgba(51, 204, 89, 0.4), rgba(89, 166, 108, 0.2), rgba(128, 128, 128, 0)),
                (0, 0.5, rgb(0, 255, 255), rgba(13, 242, 242, 0.8), rgba(26, 230, 230, 0.6), rgba(51, 204, 204, 0.4), rgba(89, 166, 166, 0.2), rgba(128, 128, 128, 0)),
                (0.146447, 0.146447, rgb(0, 64, 255), rgba(13, 70, 242, 0.8), rgba(26, 77, 230, 0.6), rgba(51, 89, 204, 0.4), rgba(89, 108, 166, 0.2), rgba(128, 128, 128, 0)),
                (0.5, 0, rgb(127, 0, 255), rgba(128, 13, 242, 0.8), rgba(128, 26, 230, 0.6), rgba(128, 51, 204, 0.4), rgba(128, 89, 166, 0.2), rgba(128, 128, 128, 0)),
                (0.853553, 0.146447, rgb(255, 0, 191), rgba(242, 13, 185, 0.8), rgba(230, 26, 179, 0.6), rgba(204, 51, 166, 0.4), rgba(166, 89, 147, 0.2), rgba(128, 128, 128, 0))

            };

            var bounds = new Rect(centerX - radius, centerY - radius, radius * 2, radius * 2);

            foreach ((double xRatio, double yRatio, Color color1, Color color2, Color color3, Color color4, Color color5, Color color6) in list)
            {
                var stops = new[]
                {
                    new CanvasGradientStop() { Color = color1, Position = 0 },
                    new CanvasGradientStop() { Color = color2, Position = 0.1f },
                    new CanvasGradientStop() { Color = color3, Position = 0.2f },
                    new CanvasGradientStop() { Color = color4, Position = 0.3f },
                    new CanvasGradientStop() { Color = color5, Position = 0.4f },
                    new CanvasGradientStop() { Color = color6, Position = 0.5f },
                };

                using (var brush = new CanvasRadialGradientBrush(ds, stops, CanvasEdgeBehavior.Clamp, CanvasAlphaMode.Premultiplied))
                {
                    brush.Center = new Vector2(
                        (float)(xRatio * bounds.Width + bounds.Left),
                        (float)(yRatio * bounds.Height + bounds.Top));

                    brush.RadiusX = (float)(radius * 2);
                    brush.RadiusY = (float)(radius * 2);

                    ds.FillCircle((float)centerX, (float)centerY, (float)radius, brush);
                }
            }


            Color rgba(byte r, byte g, byte b, double a)
            {
                return Color.FromArgb((byte)(a * 255), r, g, b);
            }

            Color rgb(byte r, byte g, byte b)
            {
                return Color.FromArgb(255, r, g, b);
            }

        }

        private void CanvasControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((CanvasControl)sender).Invalidate();
        }
    }
}
