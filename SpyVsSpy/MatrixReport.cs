using System;
using System.Collections.Generic;
using System.Text;

namespace SpyVsSpy
{
  public class MatrixReport
  {
    public Matrix Matrix { get; protected set; }
    public MatrixReport(Matrix o)
    {
      this.Matrix = o;
    }

    public string WinningMove()
    {
      return string.Join(" ", GetWinningMoves());
    }

    public List<int> GetWinningMoves()
    {
      return Matrix.GetMarkers();
    }


  }

}
