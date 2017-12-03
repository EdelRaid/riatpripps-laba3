using System;
using System.Linq;

namespace Lab3
{
  [Serializable]
  public class Output
  {
    public decimal SumResult { get; set; }
    public int MulResult { get; set; }
    public decimal[] SortedInputs { get; set; }

    public Output(Input input)
    {
      this.SumResult = input.K * input.Sums.Sum(s => s);
      this.MulResult = input.Muls.Aggregate((p, x) => p *= x);
      this.SortedInputs = input.Sums.Union(input.Muls.Select(m => Convert.ToDecimal(m))).OrderBy(e => e).ToArray();
    }

    public Output() { }
  }
}