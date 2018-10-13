using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(new crud().GetemployeeListFirst().FirstOrDefault().displayname);
            Console.Read();
        }
    }
}
