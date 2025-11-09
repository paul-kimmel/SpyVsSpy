using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SpyVsSpy
{
  public static class MatrixHelper
  {
    public static PointF Add(PointF source, PointF target)
    {
      return new PointF(source.X + target.X, source.Y + target.Y);
    }


    public static int GetAngle(PointF source, PointF target)
    {
      if (source.X < target.X) return 90;
      if (source.X == target.X) return 180;
      return 270;
    }
  }

}
