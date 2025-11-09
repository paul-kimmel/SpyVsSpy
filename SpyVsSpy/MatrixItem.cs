using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SpyVsSpy
{
  public class MatrixItem
  {
    public string Value { get; set; } = "*";

    public int Column { get; protected set; } = -1;
    public int Row { get; protected set; } = -1;

    public Position Position { get; protected set; } = Position.Empty;
    public string Marker { get; protected set; } = "S";
    public static Font Font { get; protected set; } = new Font("Consolas", 10f, FontStyle.Regular);
    public static Font BoldFont { get; protected set; } = new Font("Consolas", 10f, FontStyle.Bold);
    public static Font SpyFont { get; protected set; } = new Font("Segoe UI Emoji", 6);

    public MatrixItem(int column, int row, Position position, string value = "*", string marker = "S")
    {
      this.Column = column;
      this.Row = row;
      this.Value = value;
      this.Position = position;
      this.Marker = marker;
    }

    protected MatrixItem()
    {
      this.Column = -1;
      this.Row = -1;
      this.Value = "Error";
    }

    public bool IsHit()
    {
      return Value == Marker;
    }



    public bool ContainsMarker()
    {
      return Value == Marker;
    }

    public Bitmap Circle(Bitmap bitmap, Pen pen = null)
    {
      using (Graphics graphics = Factory.Create(bitmap))
      {
        Pen mypen = pen == null ? Pens.Silver : pen;

        var size = graphics.MeasureString(this.Value, Font);
        if (Column != 0 && Row != 0)
        {
          graphics.DrawEllipse(mypen, Position.X, Position.Y, size.Width, size.Width);
#if DEBUG
          //bitmap = DrawConnector(bitmap, size, 90);
#endif
        }
        return bitmap = Draw(bitmap);
      }
    }

    public Bitmap Draw(Bitmap bitmap)
    {
      using (Graphics graphics = Factory.Create(bitmap))
      {
        if (Value == Marker)
        {
          graphics.DrawString("🕵", SpyFont, Brushes.Gray, Position.Rectangle.X, Position.Rectangle.Y);
          
        }
        else
        {
          graphics.DrawString(Value.ToString(), Font, Brushes.Black, Position.Rectangle.X + .25f, Position.Rectangle.Y + 1);
        }

#if DEBUG
        //graphics.DrawRectangle(Pens.Green, Position.Rectangle.X , Position.Rectangle.Y, Position.Rectangle.Width, Position.Rectangle.Height);
#endif
        return bitmap;
      }
    }

    public Bitmap DrawConnector(Bitmap bitmap, SizeF actualSize, float radian)
    {
      if (Column == 0 || Row == 0) return bitmap;

      using (Graphics graphics = Factory.Create(bitmap))
      {
        var point = GetClockPoint(actualSize, radian);
        //convert back to silver
        graphics.FillEllipse(Brushes.Black, Position.Rectangle.X + point.X - 2f, Position.Rectangle.Y + point.Y - 1.5f, 3, 3);

        return bitmap = Draw(bitmap);
      }
    }

    public Bitmap DrawConnector(Bitmap bitmap, SizeF actualSize, int angle)
    {
      if (Column == 0 || Row == 0) return bitmap;

      using (Graphics graphics = Factory.Create(bitmap))
      {
        var point = GetClockPoint(actualSize, angle);
        graphics.FillEllipse(Brushes.Black, Position.Rectangle.X + point.X - 2f, Position.Rectangle.Y + point.Y - 1.5f, 3, 3);

        return bitmap = Draw(bitmap);
      }
    }


    public PointF GetClockPoint(SizeF actualSize, int angle)
    {
      const int MOVE_ORIGIN_TO_TOP = 90;
      return Position.GetClockPoint(actualSize, angle - MOVE_ORIGIN_TO_TOP);
    }

    public PointF GetClockPoint(SizeF actualSize, float radian)
    {
      return Position.GetClockPoint(actualSize, radian);
    }

    private static readonly MatrixItem _empty = new MatrixItem();
    public static MatrixItem Empty { get { return _empty; } }

  }

  public class MatrixHeaderItem : MatrixItem
  {
    public MatrixHeaderItem(int column, int row, Position position) : base(column, row, position)
    {
      if (column != 0 && row != 0)
        throw new ArgumentException($"One of column {column} or Row {row} should be 0");

      if (column == 0)
        Value = row.ToString();
      else
        Value = column.ToString();
    }
  }

}
