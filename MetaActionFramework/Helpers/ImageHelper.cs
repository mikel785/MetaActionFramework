
using System.Drawing;
using System.Windows.Media;
using MAF.ExtensionMethods;
using MAF.Framework;

namespace MAF.Helpers
{
    using Image = System.Windows.Controls.Image;

    public static class ImageHelper
    {
        public static Image CreateImage(Bitmap source)
        {
            var result = new Image { Source = source.ToBitmapSource() };
            result.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);
            return result;
        }

        public static Image CreateImage(MetaAction source)
        {
            var result = new Image { Source = source.Icon.ToBitmapSource() };
            result.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);
            return result;
        }
    }
}
