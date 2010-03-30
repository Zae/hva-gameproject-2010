using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx.AMF3;

namespace ION
{
    class Host : IExternalizable
    {
        public String hostname;

        public void ReadExternal(IDataInput arg0)
        {
            this.hostname = (String)arg0.ReadUTF();
        }
        public void WriteExternal(IDataOutput arg0)
        {
            arg0.WriteUTF(this.hostname);
        }
    }
}
