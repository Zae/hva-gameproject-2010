using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx.AMF3;

namespace ION
{
    class Host : IExternalizable
    {
        String hostname;

        public override void readExternal(IDataInput arg0)
        {
            this.hostname = (String)arg0.ReadUTF();
        }
        public override void writeExternal(IDataOutput arg0)
        {
            arg0.WriteUTF(this.hostname);
        }
    }
}
