using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;

namespace SpyVsSpy
{
  public class Factory
  {
    public static Graphics Create(Bitmap bitmap)
    {
      var o = Graphics.FromImage(bitmap);
      o.CompositingMode = CompositingMode.SourceOver;
      o.CompositingQuality = CompositingQuality.HighSpeed;
      o.InterpolationMode = InterpolationMode.High;
      o.PixelOffsetMode = PixelOffsetMode.HighSpeed;
      o.SmoothingMode = SmoothingMode.AntiAlias;
      o.TextRenderingHint = TextRenderingHint.AntiAlias;
      return o;
    }
  }
}
