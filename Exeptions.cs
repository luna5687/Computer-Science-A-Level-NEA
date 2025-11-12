using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Science_A_Level_NEA
{
    public class DataBaseFullExeption : Exception
    {
        public DataBaseFullExeption() :base("The Database has reached the maximum size allowed") {}
        public DataBaseFullExeption(string message) : base(message) { }
    }

}
