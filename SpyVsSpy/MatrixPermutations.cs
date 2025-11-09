using System;
using System.Collections.Generic;
using System.Text;

namespace SpyVsSpy
{

  public class MatrixPermutations
  {
    public static List<Matrix> GetPermutations(Matrix o)
    {
      //todo: implement a permutation function based on matrix or size
      return new List<Matrix>();
    }

    public static List<Matrix> GetPermutations(int size)
    {
      return GetPermutations(new Matrix(size));
    }

  }

}
