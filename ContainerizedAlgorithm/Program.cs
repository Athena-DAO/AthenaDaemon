using System;

namespace ContainerizedAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Command line arguements are: ");
            foreach(var arg in args)
            {
                Console.WriteLine(arg);
            }

            Console.WriteLine("Waiting for user input");
            string userInput = Console.ReadLine();
            Console.WriteLine("User input is " + userInput);

            Console.WriteLine("Bye, World!");
        }
    }
}
