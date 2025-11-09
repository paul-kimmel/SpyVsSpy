using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyVsSpy
{
  public class TestCases
  {
    public readonly static int[][] Seeds = new int[][]
    {
      new int[]{ 9, 3, 11, 13, 2, 12, 4, 7, 10, 5, 1, 8, 6 },  //3
      new int[]{ 9, 2, 10, 13, 3, 12, 4, 7, 11, 5, 1, 8, 6 },  //2
      new int[]{ 9, 2, 11, 13, 3, 10, 4, 7, 12, 5, 1, 8, 6},   //2
      new int[]{ 9, 2, 10, 5, 3, 12, 4, 7, 11, 13, 1, 8, 6},   // 2
      new int[]{9, 2, 11, 13, 3, 7, 4, 10, 12, 5, 1, 8, 6},    //1
      new int[]{9, 2, 8, 13, 3, 7, 4, 10, 12, 5, 1, 11, 6},    //1
      new int[]{9, 2, 8, 13, 1, 7, 4, 10, 12, 5, 3, 11, 6},    //1
      new int[]{8, 2, 9, 13, 3, 7, 4, 10, 12, 5, 1, 11, 6 },   //1 - Leads to win
      new int[]{9, 2, 8, 13, 3, 7, 11, 10, 12, 5, 1, 4, 6},    //1
      new int[]{ 8, 2, 9, 13, 1, 7, 4, 10, 12, 5, 3, 11, 6},   //0 Win!
      new int[]{ 1, 3, 12, 10, 7, 2, 11, 5, 8, 13, 9, 4, 6},   //0 Hackerrank Win!
      new int[]{ 5, 8, 13, 1, 10, 7, 2, 4, 9, 12, 3, 11, 6 },  //0 Candidate
      new int[]{ 6, 4, 1, 8, 11, 13, 3, 7, 2, 10, 12, 9, 5 }   //0 win

      };
  }
}
