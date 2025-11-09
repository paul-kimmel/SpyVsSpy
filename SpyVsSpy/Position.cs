using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SpyVsSpy
{
  public class Position
  {
    public PointF Location { get; protected set; } = PointF.Empty;
    public SizeF Size { get; protected set; } = SizeF.Empty;
    public RectangleF Rectangle { get; protected set; } = RectangleF.Empty;


    public static float DefaultHeight { get; set; } = 20f;
    public static float DefaultWidth { get; set; } = 60f;
    public static float DefaultPad { get; set; } = 10f;


    protected Position()
    {

    }

    public static Position GetNextPosition(int column, int row)
    {
      return new Position(GetNextLocation(column, row));
    }

    private static PointF GetNextLocation(int column, int row)
    {
      return new PointF((DefaultWidth + DefaultPad) * column, (DefaultHeight + DefaultPad) * row);
    }

    public Position(PointF location)
    {
      this.Location = location;
      this.Size = new SizeF(DefaultWidth, DefaultHeight);
      this.Rectangle = new RectangleF(this.Location, this.Size);
    }

    public Position(float X, float Y)
    {
      this.Location = new PointF(X, Y);
      this.Size = new SizeF(DefaultWidth, DefaultHeight);
      this.Rectangle = new RectangleF(this.Location, this.Size);
    }

    public Position(float X, float Y, float width, float height)
    {
      this.Location = new PointF(X, Y);
      this.Size = new SizeF(width, height);
      this.Rectangle = new RectangleF(this.Location, this.Size);
    }

    public Position(PointF location, SizeF size)
    {
      this.Location = location;
      this.Size = size;
      this.Rectangle = new RectangleF(this.Location, this.Size);
    }

    public PointF GetCenter(SizeF actualSize)
    {
      var x = actualSize.Width / 2;
      return new PointF(x, x);
    }

    public float GetRadius(SizeF actualSize)
    {
      return (float)(GetCircumference(actualSize) / (2 * Math.PI));
    }

    private float GetCircumference(SizeF actualSize)
    {
      var circumference = (float)(actualSize.Width * Math.PI);
      return circumference;
    }

    public PointF GetClockPoint(SizeF actualSize, float radian)
    {
      var center = GetCenter(actualSize);
      var radius = GetRadius(actualSize);

      float x = (float)(center.X + (radius * Math.Cos(radian)));
      float y = (float)(center.Y + (radius * Math.Sin(radian)));


      return new PointF(x, y);
    }

    public PointF GetClockPoint(SizeF actualSize, int angle)
    {
      if (angle < -360 || angle > 360)
        throw new ArgumentOutOfRangeException("angle", angle, $"Angle {angle} should be between -360 and 360");

      var center = GetCenter(actualSize);
      var radius = GetRadius(actualSize);
      var radian = angle * Math.PI / 180;

      float x = (float)(center.X + (radius * Math.Cos(radian)));
      float y = (float)(center.Y + (radius * Math.Sin(radian)));


      return new PointF(x, y);
    }

    public float X { get { return Location.X; } }
    public float Y { get { return Location.Y; } }

    public float Width { get { return Size.Width; } }
    public float Height { get { return Size.Height; } }

    private static readonly Position _empty = new Position();
    public static Position Empty
    {
      get { return _empty; }
    }
  }

}
