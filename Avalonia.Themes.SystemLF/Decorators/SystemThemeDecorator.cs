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
    public enum ControlType
	{
		Button,
		RadioButton,
		CheckBox,
        ListBox,
        ListBoxItem,
        TextBox,
        TextBoxReadOnly
	}
	
	public class SystemThemeDecorator : Control
    {
        public static readonly StyledProperty<bool> IsHoveredProperty = AvaloniaProperty.Register<SystemThemeDecorator, bool>(nameof(IsHovered), false);
        public bool IsHovered
        {
            get => GetValue(IsHoveredProperty);
            set => SetValue(IsHoveredProperty, value);
        }

        public static readonly StyledProperty<bool> IsPushedProperty = AvaloniaProperty.Register<SystemThemeDecorator, bool>(nameof(IsPushed), false);
        public bool IsPushed
        {
            get => GetValue(IsPushedProperty);
            set => SetValue(IsPushedProperty, value);
        }

        public static readonly StyledProperty<bool> IsTickedProperty = AvaloniaProperty.Register<SystemThemeDecorator, bool>(nameof(IsTicked), false);
        public bool IsTicked
        {
            get => GetValue(IsTickedProperty);
            set => SetValue(IsTickedProperty, value);
        }

        public static readonly StyledProperty<ControlType> ControlTypeProperty = AvaloniaProperty.Register<SystemThemeDecorator, ControlType>(nameof(ControlType), ControlType.Button);
        public ControlType ControlType
        {
            get => GetValue(ControlTypeProperty);
            set => SetValue(ControlTypeProperty, value);
        }

        static ISystemThemeDecoratorImpl DECORATOR_IMPL = null;
        static SystemThemeDecorator()
        {
            AffectsRender<SystemThemeDecorator>(IsHoveredProperty, IsPushedProperty, IsTickedProperty, ControlTypeProperty);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                DECORATOR_IMPL = new WindowsSystemThemeDecoratorImpl();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                DECORATOR_IMPL = new GtkThemeDecoratorImpl();
            else
                DECORATOR_IMPL = new NullThemeDecoratorImpl();
        }

        public override void Render(DrawingContext context)
        {
            if ((VisualRoot != null) && (VisualRoot is Window win))
                DECORATOR_IMPL.Render(context, Bounds, ControlType, IsHovered, IsPushed, IsTicked, IsEnabled, win);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size baseSize = base.MeasureOverride(availableSize);
            if (DECORATOR_IMPL.TryGetRequestedSize(ControlType, IsHovered, IsPushed, IsTicked, IsEnabled, out Size reqSize))
            {
                double outW = baseSize.Width;
                double outH = baseSize.Height;

                double reqW = reqSize.Width;
                double reqH = reqSize.Height;
                
                if (IsValidForDesiredSize(reqW))
                    outW = Math.Max(reqW, outW);
                
                if (IsValidForDesiredSize(reqH))
                    outH = Math.Max(reqH, outH);
                
                return new Size(outW, outH);
            }
            else
                return baseSize;
        }

        bool IsValidForDesiredSize(double test) => (!double.IsInfinity(test)) && (!double.IsNaN(test) && (test >= 0));
    }
}