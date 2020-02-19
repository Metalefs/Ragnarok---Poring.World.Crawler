using Poring;
using System;
using System.Threading;

namespace Main
{
    class Program
    {
        private readonly PoringCrawler CarCrawler = new PoringCrawler();
        static void Main(string[] args)
        {
            Thread Crawler1 = new Thread(new ThreadStart(PoringCrawler.Start));
            Crawler1.Name = "First";
            Crawler1.Start();
            Console.Read();
        }
    }
}
