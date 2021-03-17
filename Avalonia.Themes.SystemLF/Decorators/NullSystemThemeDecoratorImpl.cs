using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using DrawingContext = Avalonia.Media.DrawingContext;
using Avalonia.Markup.Xaml;
using AvBitmap = Avalonia.Media.Imaging.Bitmap;

namespace Avalonia.Themes.SystemLF
{
    public class NullThemeDecoratorImpl : ISystemThemeDecoratorImpl
    {
        public void Render(DrawingContext context, Rect bounds, ControlType ctrlType, bool isHovered, bool isPressed, bool isChecked, bool isEnabled, Window topLevel)
        { }

        public bool TryGetRequestedSize(ControlType type, bool isHovered, bool isPressed, bool isChecked, bool isEnabled, out Size size)
        {
            size = Size.Empty;
            return false;
        }
    }
}