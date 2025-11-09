using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyVsSpy
{
  public static class RandomExtensions
  {

    /// <summary>
    /// .NET * Defines a shuffle tha returns void. If you use .NET rename this to Shuffle2 or something.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="array"></param>
    /// <returns></returns>
    public static List<int> Shuffle(this Random random, int[] array)
    {
      int n = array.Length;
      while (n > 1)
      {
        int k = random.Next(n--);
        int temp = array[n];
        array[n] = array[k];
        array[k] = temp;
      }

      return new List<int>(array);
    }

    public static System.Timers.Timer Create(int timeout = 10000)
    {
      var timer = new System.Timers.Timer(timeout);

      timer.Elapsed += (sender, args) =>
      {
        (sender as System.Timers.Timer).Stop();
      };

      timer.Start();
      return timer;
    }

  }
}
