using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
  [Serializable]
  public class Input
  {
    public int K { get; set; }
    public decimal[] Sums { get; set; }
    public int[] Muls { get; set; }
  }
}