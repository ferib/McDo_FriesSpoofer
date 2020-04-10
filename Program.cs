using System;
using RestSharp;

namespace FriesNetworkSpoofer
{
    class Program
    {
        static void Main(string[] args)
        {
            FriesHit fries = new FriesHit();
            fries.start(); // ("eyJzdGFnZSI6MTQsInNjb3JlIjoxNjksImxpdmVzIjoxLCJzdGFnZVNjb3JlIjoxMSwic3RhcnRUaW1lIjoxNTgzMjQ5MDY1MjU1fQ==", "MTU4MzI0OTA2NTI1NQ==", "MTU4MzI0OTA2ODQ3OQ==");
            Console.ReadKey();
        }
    }
}
