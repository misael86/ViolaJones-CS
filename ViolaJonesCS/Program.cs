using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViolaJonesCS.Utilities;

namespace ViolaJonesCS
{
    class Program
    {
        static void Main(string[] args)
        {
           Matrix[] m = Data.getImageMatrixList(new string[1] { @"C:\Users\misael\Documents\Visual Studio 2010\Projects\ViolaJonesCS\ViolaJonesCS\Images\PFACES\face00001.bmp" });
        }
    }
}
