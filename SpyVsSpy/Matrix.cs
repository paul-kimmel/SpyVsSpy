using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SpyVsSpy
{
  public class Matrix
  {
    public int Size { get; protected set; }
    private int[] _Matrix;
    private static readonly int EMPTY = -1;
    public string Marker { get; protected set; } = "S";

    public Matrix(int size, string marker = "S")
    {
      Size = size + 1;
      Marker = marker;
      Initialize();
    }

    public Matrix(int size, int[] seeds, string marker = "S")
    {
      Size = size + 1;
      Marker = marker;
      Initialize(seeds);
    }

    private void Update()
    {
      var bitmap = MatrixRenderer.Draw(this);
      Broadcaster.Broadcast(bitmap);
    }

    public MatrixItem this[int column, int row]
    {
      get
      {
        return CreateMatrixItem(column, row);
      }
    }

    private MatrixItem CreateMatrixItem(int column, int row)
    {
      if (IsHit(column, row))
      {
        return new MatrixItem(column, row, Position.GetNextPosition(column, row), Marker);
      }
      else if (IsHeader(column, row))
      {
        return new MatrixHeaderItem(column, row, Position.GetNextPosition(column, row));
      }
      else
      {
        return new MatrixItem(column, row, Position.GetNextPosition(column, row));
      }
    }

    [Description("All swaps to a new row must be swaps with another column")]
    public void Swap(int from, int to)
    {
      if (from < LowerBound || from > UpperBound || to < LowerBound || from > UpperBound)
        throw new ArgumentException($"to {to} or from {from} is out of bounds [{LowerBound}, {UpperBound}]");

      var temp = _Matrix[to];
      _Matrix[to] = _Matrix[from];
      _Matrix[from] = temp;

      Debug.Assert(IsDistinct, $"Swap created an invalid state {Markers}");
    }

    private bool IsHeader(int column, int row)
    {
      return column == AbsoluteLowerBound || row == AbsoluteLowerBound;
    }

    public bool IsHit(int column, int row)
    {
      return _Matrix[column] == row;
    }

    private void Initialize(int[] seeds)
    {
      _Matrix = new int[Size];
      var range = seeds.ToList();
      range.Insert(0, EMPTY);
      range.CopyTo(_Matrix);
    }

    private void Initialize()
    {
      _Matrix = new int[Size];
      Initialize(Enumerable.Repeat(1, Size - 1).ToArray());
      
    }

    public void Reset()
    {
      Initialize();
    }

    public int AbsoluteLowerBound = 0;
    public int LowerBound => 1;
    public int UpperBound => Size;

    public int[] Markers
    {
      get { return GetMarkers().ToArray(); }
    }

    public bool IsDistinct
    {
      get { return Markers.Distinct().Count() == UpperBound - 1; }
    }

    public List<int> GetMarkers()
    {
      try
      {
        return _Matrix.Skip(1).ToList();
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return new List<int>(Size);
      }
    }



    private static readonly Random _shuffler = new Random(DateTime.Now.Millisecond);
    public void Shuffle()
    {
      try
      {
        int n = _Matrix.Length;
        while (n > 1)
        {
          int k = _shuffler.Next(n--);
          int temp = _Matrix[n];
          _Matrix[n] = _Matrix[k];
          _Matrix[k] = temp;
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        throw ex;
      }
    }


    public string GetColumnHeader(int column)
    {
      return column.ToString();
    }

    public string GetRowHeader(int row)
    {
      return row.ToString();
    }

  }

}
