using Capstone;
using System;

namespace capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // If Main contains more than 2 lines of executable code,
            // you're doing it wrong.
            ProgramCLI cli = new ProgramCLI();
            cli.RunCLI();
        }
    }
}
