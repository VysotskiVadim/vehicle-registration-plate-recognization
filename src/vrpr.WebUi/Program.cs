using System;
using Nancy.Hosting.Self;

namespace vrpr.WebUi
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var nancyHost = new NancyHost(new Uri("http://localhost:1234")))
            {
                nancyHost.Start();
                Console.WriteLine("Web server running... Press enter to stop it.");
                Console.ReadLine();
            }
            Console.WriteLine("Web server stopped... Press enter to exit.");
            Console.ReadLine();
        }
    }
}
