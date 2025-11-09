using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyVsSpy
{
  public class BestMove
  {
    public Matrix Matrix { get; protected set;  }

    public static void Run(Matrix matrix)
    {
      var o = new BestMove();
      o.Matrix = matrix;
      o.Run();
    }

    private void Run()
    {
      throw new NotImplementedException();
    }

    public static void RunWinOnce(Matrix matrix)
    {
      var o = new BestMove();
      o.Matrix = matrix;
      o.RunWinOnce();
    }

    public void RunWinOnce(int timeout = 100000)
    {
      MatrixRenderer.Draw(Matrix);

      var crashes = CrashTestDummy.FindAll(Matrix);
      var bitmap = MatrixRenderer.Connect(Matrix, crashes);

      Broadcaster.Broadcast(bitmap);

      if (crashes.Count == 0)
        Console.WriteLine($"You won with {new MatrixReport(Matrix).WinningMove()}");
      else
        Console.WriteLine($"You lost with {new MatrixReport(Matrix).WinningMove()}, Spy Killings: {crashes.Count}");
    }
    

  }
}
