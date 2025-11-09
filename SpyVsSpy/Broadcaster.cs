using System;
using System.Collections.Generic;
using System.Text;

namespace SpyVsSpy
{
  public class Broadcaster
  {

    private static List<IListener> Listeners = new List<IListener>();

    public static void Broadcast(object o, bool slow = false)
    {
      foreach (var listener in Listeners)
      {
        if (listener.Listening)
          listener.Listen(o, slow);
      }
    }

    public static void Add(IListener listener)
    {
      if (Listeners.Contains(listener)) return;
      Listeners.Add(listener);
    }
  }


  public interface IListener
  {
    void Listen(object o, bool slow);
    bool Listening { get; }
  }
}
