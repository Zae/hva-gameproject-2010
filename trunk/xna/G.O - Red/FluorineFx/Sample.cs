using System;
using System.Collections.Generic;
using System.Text;
using FluorineFx;

namespace FluorineFx
{
    /// <summary>
    /// Fluorine sample service.
    /// </summary>
    [RemotingService("Fluorine sample service")]
    public class Sample
    {
        public Sample()
        {
        }

        public string Echo(string text)
        {
            return "Gateway echo: " + text;
        }
    }
}
