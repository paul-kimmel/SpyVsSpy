using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;

namespace SpyVsSpy
{

  public interface ICollision
  {
    bool ContainsCollisions(Matrix o);
    List<ICollision> FindCollisions(Matrix o);
    List<MatrixItem> Collisions { get; set; }
    [Description("Find a position (verices) where this item does not collide.")]
    MatrixItem HuntTheCalydonianBoar(MatrixItem item, Matrix o);
    string Name { get; }
  }


  public static class CrashTestDummy
  {
    private static readonly ICollision[] collisionTests = new ICollision[]{
    new DiagonalUpCollision(), new DiagonalDownCollision(),
    new VerticalUpCollision(), new VerticalDownCollision(),
    new LinearDownCollision(), new LinearUpCollision(),
    new HorizontalLeftCollision(), new HorizontalRightCollision()
  };

    public static List<ICollision> Find(Matrix o, params ICollision[] tests)
    {
      var list = new List<ICollision>();
      foreach (var test in tests)
      {
        list.AddRange(test.FindCollisions(o));
      }
      return list;
    }

    public static List<ICollision> FindAll(Matrix o)
    {
      var list = new List<ICollision>();
      foreach (var test in collisionTests)
      {
        list.AddRange(test.FindCollisions(o));
      }

      return list;
    }

    public static Tuple<List<ICollision>, int[]> CountCollisions(Matrix o, int column, int row)
    {
      var list = new List<ICollision>();
      foreach (var test in collisionTests)
      {
        list.AddRange(test.FindCollisions(o));
      }

      var columnCollisions = list.Where(x => x.Collisions.Any(y => y.Column == column && y.Row == row)).ToList();
      return Tuple.Create(columnCollisions, o.GetMarkers().ToArray());
    }

    public static bool HasCollisions(Matrix o)
    {
      return FindAll(o).Count == 0;
    }
  }

  public abstract class AbstractCollision : ICollision
  {
    public List<MatrixItem> Collisions { get; set; } = new List<MatrixItem>();

    public virtual bool ContainsCollisions(Matrix o)
    {
      return FindCollisions(o).Count > 0;
    }

    public abstract List<ICollision> FindCollisions(Matrix o);

    public virtual MatrixItem HuntTheCalydonianBoar(MatrixItem item, Matrix o)
    {
      return MatrixItem.Empty;
    }

    protected IEnumerable<MatrixItem> MarkedItems(Matrix o)
    {
      for (int column = o.LowerBound; column < o.UpperBound; column++)
      {
        for (int row = o.LowerBound; row < o.UpperBound; row++)
        {
          if (o[column, row].IsHit())
            yield return o[column, row];
        }
      }
    }

    public virtual string Name { get; protected set; } = "AbstractCollision";
  }

  public class DiagonalUpCollision : AbstractCollision
  {
    public DiagonalUpCollision() { }
    protected DiagonalUpCollision(MatrixItem collider1, MatrixItem collider2)
    {
      Collisions.Add(collider1);
      Collisions.Add(collider2);
    }

    public override List<ICollision> FindCollisions(Matrix o)
    {
      var results = new List<ICollision>();

      foreach (var item in MarkedItems(o))
      {
        int row = item.Row;

        for (int column = item.Column + 1; column < o.UpperBound; column++)
        {
          row--;
          if (row < o.LowerBound) break;
          if (o[column, row].IsHit())
          {
            results.Add(new DiagonalUpCollision(item, o[column, row]));
          }

        }
      }

      return results;
    }

  }

  public class DiagonalDownCollision : AbstractCollision
  {
    public DiagonalDownCollision() { }
    protected DiagonalDownCollision(MatrixItem collider1, MatrixItem collider2)
    {
      Collisions.Add(collider1);
      Collisions.Add(collider2);
    }

    public override List<ICollision> FindCollisions(Matrix o)
    {
      var results = new List<ICollision>();

      foreach (var item in MarkedItems(o))
      {
        int row = item.Row;

        for (int column = item.Column + 1; column < o.UpperBound; column++)
        {
          row++;
          if (row >= o.UpperBound) break;

          if (o[column, row].IsHit())
          {
            results.Add(new DiagonalDownCollision(item, o[column, row]));
          }

        }
      }

      return results;
    }
  }

  public class VerticalUpCollision : AbstractCollision
  {
    public VerticalUpCollision() { }
    protected VerticalUpCollision(MatrixItem collider1, MatrixItem collider2)
    {
      Collisions.Add(collider1);
      Collisions.Add(collider2);
    }

    public override List<ICollision> FindCollisions(Matrix o)
    {
      var results = new List<ICollision>();

      foreach (var item in MarkedItems(o))
      {
        for (int row = item.Row - 1; row >= o.LowerBound; row--)
        {
          if (o[item.Column, row].IsHit())
          {
            results.Add(new VerticalUpCollision(item, o[item.Column, row]));
          }
        }
      }

      return results;
    }
  }

  public class VerticalDownCollision : AbstractCollision
  {
    public VerticalDownCollision() { }
    protected VerticalDownCollision(MatrixItem collider1, MatrixItem collider2)
    {
      Collisions.Add(collider1);
      Collisions.Add(collider2);
    }

    public override List<ICollision> FindCollisions(Matrix o)
    {
      var results = new List<ICollision>();

      foreach (var item in MarkedItems(o))
      {
        for (int row = item.Row + 1; row < o.UpperBound; row++)
        {
          if (o[item.Column, row].IsHit())
          {
            results.Add(new VerticalDownCollision(item, o[item.Column, row]));
          }
        }
      }

      return results;
    }
  }

  public class HorizontalLeftCollision : AbstractCollision
  {
    public HorizontalLeftCollision() { }
    protected HorizontalLeftCollision(MatrixItem collider1, MatrixItem collider2)
    {
      Collisions.Add(collider1);
      Collisions.Add(collider2);
    }

    public override List<ICollision> FindCollisions(Matrix o)
    {
      var results = new List<ICollision>();

      foreach (var item in MarkedItems(o))
      {
        for (int column = item.Column - 1; column >= o.LowerBound; column--)
        {
          if (o[column, item.Row].IsHit())
          {
            results.Add(new HorizontalLeftCollision(item, o[column, item.Row]));
          }
        }
      }

      return results;
    }
  }

  public class HorizontalRightCollision : AbstractCollision
  {
    public HorizontalRightCollision() { }
    protected HorizontalRightCollision(MatrixItem collider1, MatrixItem collider2)
    {
      Collisions.Add(collider1);
      Collisions.Add(collider2);
    }

    public override List<ICollision> FindCollisions(Matrix o)
    {
      var results = new List<ICollision>();

      foreach (var item in MarkedItems(o))
      {
        for (int column = item.Column + 1; column < o.UpperBound; column++)
        {
          if (o[column, item.Row].IsHit())
          {
            results.Add(new HorizontalRightCollision(item, o[column, item.Row]));
          }
        }
      }

      return results;
    }
  }

  public class LinearUpCollision : AbstractCollision
  {
    public LinearUpCollision() { }
    protected LinearUpCollision(params MatrixItem[] args)
    {
      Collisions.AddRange(args);
    }

    public override List<ICollision> FindCollisions(Matrix o)
    {
      var results = new List<ICollision>();
      var hits = new List<MatrixItem>();

      foreach (var item in MarkedItems(o))
      {
        hits.Clear();
        hits.Add(item);

        int row = item.Row;
        for (int column = item.Column + 1; column < o.UpperBound; column += 1)
        {
          row -= 2;
          if (row >= o.LowerBound)
          {
            if (o[column, row].IsHit())
            {
              hits.Add(o[column, row]);
            }
          }
        }

        if (hits.Count > 2)
          results.Add(new LinearUpCollision(hits.ToArray()));

      }

      return results;
    }
  }

  public class LinearDownCollision : AbstractCollision
  {
    public LinearDownCollision() { }
    protected LinearDownCollision(params MatrixItem[] args)
    {
      Collisions.AddRange(args);
    }

    public override List<ICollision> FindCollisions(Matrix o)
    {
      var results = new List<ICollision>();
      var hits = new List<MatrixItem>();

      foreach (var item in MarkedItems(o))
      {
        hits.Clear();
        hits.Add(item);

        int row = item.Row;
        for (int column = item.Column + 1; column < o.UpperBound; column += 1)
        {
          row += 2;
          if (row < o.UpperBound)
          {
            if (o[column, row].IsHit())
            {
              hits.Add(o[column, row]);
            }
          }
        }

        if (hits.Count > 2)
          results.Add(new LinearDownCollision(hits.ToArray()));

      }

      return results;
    }
  }

}
