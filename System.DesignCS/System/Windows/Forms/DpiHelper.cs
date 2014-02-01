namespace System.Windows.Forms
{
    using System;
    using System.Configuration;
    using System.Design;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;

    internal static class DpiHelper
    {
        private static double deviceDpiX = 96.0;
        private static double deviceDpiY = 96.0;
        private static bool enableHighDpi = false;
        private const string EnableHighDpiConfigurationValueName = "EnableWindowsFormsHighDpiAutoResizing";
        private static System.Drawing.Drawing2D.InterpolationMode interpolationMode = System.Drawing.Drawing2D.InterpolationMode.Invalid;
        private static bool isInitialized = false;
        private const double LogicalDpi = 96.0;
        private static double logicalToDeviceUnitsScalingFactorX = 0.0;
        private static double logicalToDeviceUnitsScalingFactorY = 0.0;

        public static Bitmap CreateResizedBitmap(Bitmap logicalImage, Size targetImageSize)
        {
            if (logicalImage == null)
            {
                return null;
            }
            return ScaleBitmapToSize(logicalImage, targetImageSize);
        }

        private static Bitmap CreateScaledBitmap(Bitmap logicalImage)
        {
            Size deviceImageSize = LogicalToDeviceUnits(logicalImage.Size);
            return ScaleBitmapToSize(logicalImage, deviceImageSize);
        }

        private static void Initialize()
        {
            if (!isInitialized)
            {
                try
                {
                    if (string.Equals(ConfigurationManager.AppSettings.Get("EnableWindowsFormsHighDpiAutoResizing"), "true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        enableHighDpi = true;
                    }
                }
                catch
                {
                }
                if (enableHighDpi)
                {
                    IntPtr dC = System.Design.UnsafeNativeMethods.GetDC(System.Design.NativeMethods.NullHandleRef);
                    if (dC != IntPtr.Zero)
                    {
                        deviceDpiX = System.Design.UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dC), 0x58);
                        deviceDpiY = System.Design.UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dC), 90);
                        System.Design.UnsafeNativeMethods.ReleaseDC(System.Design.NativeMethods.NullHandleRef, new HandleRef(null, dC));
                    }
                }
                isInitialized = true;
            }
        }

        public static Size LogicalToDeviceUnits(Size logicalSize)
        {
            return new Size(LogicalToDeviceUnitsX(logicalSize.Width), LogicalToDeviceUnitsY(logicalSize.Height));
        }

        public static int LogicalToDeviceUnitsX(int value)
        {
            return (int) Math.Round((double) (LogicalToDeviceUnitsScalingFactorX * value));
        }

        public static int LogicalToDeviceUnitsY(int value)
        {
            return (int) Math.Round((double) (LogicalToDeviceUnitsScalingFactorY * value));
        }

        public static void ScaleBitmapLogicalToDevice(ref Bitmap logicalBitmap)
        {
            if (logicalBitmap != null)
            {
                Bitmap bitmap = CreateScaledBitmap(logicalBitmap);
                if (bitmap != null)
                {
                    logicalBitmap.Dispose();
                    logicalBitmap = bitmap;
                }
            }
        }

        private static Bitmap ScaleBitmapToSize(Bitmap logicalImage, Size deviceImageSize)
        {
            Bitmap image = new Bitmap(deviceImageSize.Width, deviceImageSize.Height, logicalImage.PixelFormat);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.InterpolationMode = InterpolationMode;
                RectangleF srcRect = new RectangleF(0f, 0f, (float) logicalImage.Size.Width, (float) logicalImage.Size.Height);
                RectangleF destRect = new RectangleF(0f, 0f, (float) deviceImageSize.Width, (float) deviceImageSize.Height);
                srcRect.Offset(-0.5f, -0.5f);
                graphics.DrawImage(logicalImage, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return image;
        }

        public static void ScaleButtonImageLogicalToDevice(Button button)
        {
            if (button != null)
            {
                Bitmap image = button.Image as Bitmap;
                if (image != null)
                {
                    Bitmap bitmap2 = CreateScaledBitmap(image);
                    button.Image.Dispose();
                    button.Image = bitmap2;
                }
            }
        }

        private static System.Drawing.Drawing2D.InterpolationMode InterpolationMode
        {
            get
            {
                if (interpolationMode == System.Drawing.Drawing2D.InterpolationMode.Invalid)
                {
                    int num = (int) Math.Round((double) (LogicalToDeviceUnitsScalingFactorX * 100.0));
                    if ((num % 100) == 0)
                    {
                        interpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    }
                    else if (num < 100)
                    {
                        interpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    }
                    else
                    {
                        interpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    }
                }
                return interpolationMode;
            }
        }

        public static bool IsScalingRequired
        {
            get
            {
                Initialize();
                if (deviceDpiX == 96.0)
                {
                    return (deviceDpiY != 96.0);
                }
                return true;
            }
        }

        private static double LogicalToDeviceUnitsScalingFactorX
        {
            get
            {
                if (logicalToDeviceUnitsScalingFactorX == 0.0)
                {
                    Initialize();
                    logicalToDeviceUnitsScalingFactorX = deviceDpiX / 96.0;
                }
                return logicalToDeviceUnitsScalingFactorX;
            }
        }

        private static double LogicalToDeviceUnitsScalingFactorY
        {
            get
            {
                if (logicalToDeviceUnitsScalingFactorY == 0.0)
                {
                    Initialize();
                    logicalToDeviceUnitsScalingFactorY = deviceDpiY / 96.0;
                }
                return logicalToDeviceUnitsScalingFactorY;
            }
        }
    }
}

