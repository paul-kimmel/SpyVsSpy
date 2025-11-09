using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpyVsSpy
{
  public class MatrixEngine : AbstractValidator<Matrix>
  {
    public MatrixEngine()
    {
      RuleFor(o => o.Markers).NotEmpty().Must(HaveCorrectNumberOfElements).WithMessage("${PropertyName} has markers in the same row.");
    }

    private bool HaveCorrectNumberOfElements(Matrix o, int[] markers)
    {
      return markers.Length == o.Size - 1;
    }
  }
}
