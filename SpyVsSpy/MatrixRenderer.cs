using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;

namespace SpyVsSpy
{
  public class MatrixRenderer
  {
    public static Lazy<Bitmap> _Bitmap = new Lazy<Bitmap>(() => new Bitmap(1000, 450, PixelFormat.Format32bppPArgb));
    public Matrix Matrix { get; protected set; }

    public static Bitmap Draw(Matrix matrix)
    {
      var o = new MatrixRenderer();
      o.Matrix = matrix;
      o.Draw();
      return _Bitmap.Value;
    }

    public void Draw()
    {
      using (Graphics graphics = Factory.Create(_Bitmap.Value))
      {
        graphics.Clear(Color.White);
        for (int column = Matrix.AbsoluteLowerBound; column < Matrix.Size; column++)
          for (int row = Matrix.AbsoluteLowerBound; row < Matrix.Size; row++)
          {
            try
            {
              Matrix[column, row].Circle(_Bitmap.Value);
            }
            catch(Exception ex)
            {
              Debug.WriteLine(ex.Message);
            }
          }
      }
    }

    public static Bitmap Connect(Matrix matrix, List<ICollision> crashes)
    {
      var o = new MatrixRenderer();
      o.Matrix = matrix;
      o.Draw();
      o.Connect(crashes);
      return _Bitmap.Value;
    }

    public void Connect(MatrixItem source, MatrixItem target)
    {
      try
      {
        using (var graphics = Factory.Create(_Bitmap.Value))
        {
          var angle = MatrixHelper.GetAngle(source.Position.Location, target.Position.Location);

          var size = graphics.MeasureString(source.Value, MatrixItem.Font);
          source.DrawConnector(_Bitmap.Value, size, angle);
          var point1 = MatrixHelper.Add(source.GetClockPoint(size, angle), source.Position.Location);

          var x = source.Position.X - target.Position.X;
          var y = source.Position.Y - target.Position.Y;
          var radian = Math.Atan2(y, x);


          size = graphics.MeasureString(target.Value, MatrixItem.Font);
          target.DrawConnector(_Bitmap.Value, size, (float)radian);
          var point2 = MatrixHelper.Add(target.GetClockPoint(size, (float)radian), target.Position.Location);

          graphics.DrawLine(Pens.Gray, point1, point2);
        }
        Broadcaster.Broadcast(_Bitmap.Value);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    public void Connect(int column1, int row1, int column2, int row2)
    {
      try
      {
        Connect(Matrix[column1, row1], Matrix[column2, row2]);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    public void Connect(List<ICollision> crashes)
    {
      foreach (var crash in crashes)
      {
        Connect(crash.Collisions.First(), crash.Collisions.Last());
        //Thread.Sleep(100);
      }
    }
  }
}
