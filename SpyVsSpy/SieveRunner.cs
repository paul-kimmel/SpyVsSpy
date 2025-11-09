using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SpyVsSpy
{
  public class SieveRunner
  {
    public Matrix Matrix { get; protected set; }

    protected SieveRunner() { }

    public static void Run(Matrix matrix)
    {
      var o = new SieveRunner() { Matrix = matrix };
      o.Run();
    }

    private void Run()
    {
      TryPhaseRunner();
    }

    public void TryPhaseRunner(int timeout = 100000)
    {
      try
      {
        List<int[]> seeds = GetThreeCollisionSet();
        seeds = GetTwoCollisionSet(seeds);
        seeds = GetOneCollisionSet(seeds);
        seeds = GetWinningSet(seeds);
      }
      catch (WinningMoveException ex)
      {
        Console.WriteLine(ex.Message);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    public List<int[]> GetThreeCollisionSet(int timeout = 100000)
    {
      TryOneRandomSwap();
      var results = GetCollisionSet(GetWinningMoves().ToArray(), 3, BestRandomMove);
      CheckFail(results, 3);
      return results;
    }

    private void TryOneRandomSwap()
    {
      MatrixRenderer.Draw(Matrix);
      var rows = Matrix.GetMarkers();

      for (int column = Matrix.LowerBound, rIndex = 0; column < Matrix.UpperBound; column++, rIndex++)
      {
        var row = rows[rIndex];
        RandomSwap(column, row);
      }
      var bitmap = MatrixRenderer.Draw(Matrix);
      Broadcaster.Broadcast(bitmap);
    }

    private static readonly Random random = new Random(DateTime.Now.Millisecond);
    private void RandomSwap(int column, int row)
    {
      try
      {
        Matrix.Swap(column, random.Next(Matrix.LowerBound, Matrix.UpperBound));
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }


    private static readonly Random eggs = new Random(DateTime.Now.Millisecond);
    public void BestRandomMove(int[] seeds)
    {
      var columns = eggs.Shuffle(Enumerable.Range(1, Matrix.UpperBound - 1).ToArray());

      for (var c = 0; c < columns.Count; c++)
      {
        var rows = Matrix.Markers;

        BestMove(columns[c], rows[columns[c] - 1]);
      }

      var bitmap = MatrixRenderer.Draw(Matrix);
      Broadcaster.Broadcast(bitmap);
    }

    private void BestMove(int column, int row)
    {
    
      Tuple<List<ICollision>, int[]> fewestCrashes = Tuple.Create(new List<ICollision>(), new int[] { });

      for (int c = Matrix.LowerBound; c < Matrix.UpperBound; c++)
      {
        Matrix.Swap(column, c);

        //var crashes = CrashTestDummy.CountCollisions(Matrix, c, row);
        var crashes = Tuple.Create(CrashTestDummy.FindAll(Matrix), Matrix.Markers);
        Broadcaster.Broadcast(MatrixRenderer.Connect(Matrix, crashes.Item1));
        if (crashes.Item1.Count() < fewestCrashes.Item1.Count() || fewestCrashes.Item1.Count == 0)
        {
          fewestCrashes = crashes;
        }
      }

      //var bestMove = new Matrix(Matrix.Size - 1, fewestCrashes.Item2);
      Matrix = new Matrix(Matrix.Size - 1, fewestCrashes.Item2);
      MatrixRenderer.Connect(Matrix, fewestCrashes.Item1);

      Broadcaster.Broadcast(MatrixRenderer.Draw(Matrix));
      
    }


    private string WinningMove()
    {
      return new MatrixReport(Matrix).WinningMove();
    }

    private List<int> GetWinningMoves()
    {
      return new MatrixReport(Matrix).GetWinningMoves();
    }


    public List<int[]> GetTwoCollisionSet(List<int[]> threes, int timeout = 100000)
    {
      return GetCollisionSet(threes, 2);
    }

    public List<int[]> GetOneCollisionSet(List<int[]> twos, int timeout = 100000)
    {
      return GetCollisionSet(twos, 1);
    }

    public List<int[]> GetWinningSet(List<int[]> ones, int timeout = 100000)
    {
      return GetCollisionSet(ones, 0);
    }

    private List<int[]> GetCollisionSet(List<int[]> sets, int countLimit, int timeout = 100000)
    {
      var result = new List<int[]>();
      foreach (var set in sets)
      {
        result.AddRange(GetCollisionSet(set, countLimit));
      }

      Console.WriteLine($"Found {result.Count} collisions for sets of {countLimit}");
      CheckFail(result, countLimit);
      return result;
    }


    private List<int[]> GetCollisionSet(int[] seeds, int countLimit, Action<int[]> swap = null, int timeout = 100000)
    {
      Action<int[]> report = (spores) =>
      {
        Console.WriteLine($"Checking {string.Join(" ", spores)} for sets of {countLimit}");
      };

      report(seeds);

      CheckIsWinner(seeds);

      using (var timer = RandomExtensions.Create(timeout))
      {
        var results = new List<int[]>();

        while (timer.Enabled)
        {
          if (swap == null)
            SeedSwapper(seeds);
          else
            swap(seeds);

          //report(Matrix.GetMarkers().ToArray());

          var crashes = CrashTestDummy.FindAll(Matrix);
          var bitmap = MatrixRenderer.Connect(Matrix, crashes);
          Broadcaster.Broadcast(bitmap);
          CheckIsWinner(crashes);

          if (crashes.Count <= countLimit)
          {
            int[] configuration = GetWinningMoves().ToArray();
            //if (results.IsDuplication(configuration) == false) // duplicates seem to help exhaustively check
            {
              results.Add(configuration);
              Console.WriteLine($"Results: {WinningMove()}, Collisions: {crashes.Count}");
            }

          }

          //Console.ReadLine();
          //Thread.Sleep(2500);
        }

        return results;
      }
    }

    private void CheckFail(List<int[]> seeds, int countLimit)
    {
      if (seeds != null && seeds.Count == 0)
        throw new FailedSieveException($"Fail: Found {seeds.Count} collisions for sets of {countLimit}");
    }

    private void CheckIsWinner(List<ICollision> crashes)
    {
      if (crashes != null && crashes.Count == 0)
      {
        throw new WinningMoveException($"Results: {WinningMove()}, Spy Killings: {crashes.Count}");
      }
    }

    private void CheckIsWinner(int[] seeds)
    {
      if (seeds == null || seeds.Length == 0) return;
      var temp = new Matrix(seeds.Length, seeds);
      CheckIsWinner(CrashTestDummy.FindAll(temp));
    }

    private void SeedSwap(int[] seeds)
    {
      MatrixRenderer.Draw(Matrix);

      for (int column = Matrix.LowerBound, i = 0; column < Matrix.UpperBound; column++, i++)
      {
        Matrix.Swap(column, seeds[column]);
        Broadcaster.Broadcast(MatrixRenderer.Draw(Matrix));
      }
    }

    private void SeedSwapper(int[] seeds)
    {
      SeedSwap(RandomSwapCopy(seeds));
    }

    private static readonly Random _swapRandom = new Random(DateTime.Now.Millisecond);
    private int[] RandomSwapCopy(int[] seeds)
    {
      var copy = new int[seeds.Length];

      seeds.CopyTo(copy, 0);

      var first = _swapRandom.Next(0, Matrix.UpperBound);
      var second = _swapRandom.Next(0, Matrix.UpperBound);
      var temp = copy[first];
      copy[first] = copy[second];
      copy[second] = temp;

#if !DEBUG
    CheckThrowCritical(copy);
    Console.WriteLine($"Randomized {string.Join(" ", copy)}");
#endif
      return copy;
    }


  }
}
