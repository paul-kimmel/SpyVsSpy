using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SpyVsSpy
{
  public class SpyRunner : IListener
  {
    public bool Listening => true;
#if LINQPAD
    protected DumpContainer Container { get; private set; } = ContainerCreate();
#endif
    protected Matrix Matrix { get; set; }

    protected SpyRunner(int size = 13)
    {
      var seeds = TestCases.Seeds[TestCases.Seeds.Length - 1];
      Matrix = new Matrix(seeds.Length, seeds);
    }

    public static void Run(int size = 13)
    {
      CheckMaxSizeLimit(size);

      var o = new SpyRunner(size);
      Broadcaster.Add(o);
      o.Run();
    }

    private void Run()
    {
      //BestMove.RunWinOnce(Matrix);
      SieveRunner.Run(Matrix);
    }

#if LINQPAD
    private static DumpContainer ContainerCreate()
    {
      var o = new DumpContainer();
      o.Dump();
      return o;
    }
#endif

    private static void CheckMaxSizeLimit(int size)
    {
      if (size > 999)
        throw new ArgumentOutOfRangeException("size", "Maximum size is 999");
    }

    public void Listen(object o, bool slow = false)
    {
      try
      {
#if LINQPAD
        Container.UpdateContent(o);
#endif
        Slowsky(slow);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    private void Slowsky(bool slow = false)
    {
#if DEBUG
      if (slow)
      {
        Thread.Sleep(25);
      }
#endif
    }


  }
}
