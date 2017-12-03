using System;

namespace Lab3
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var port = Console.ReadLine();
      var server = new Server(string.Format("http://127.0.0.1:{0}/", port));
      server.Start();
    }
  }
}