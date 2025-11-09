using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyVsSpy
{
  public class WinningMoveException : Exception
  {
    public WinningMoveException(string message) : base(message) { }
  }

  public class FailedSieveException : Exception
  {
    public FailedSieveException(string message) : base(message) { }
  }

}
