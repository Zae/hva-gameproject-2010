using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ION
{
    public class CheckedState : Serializable
    {
        private bool _Checked;
        private States _CurrentState;

        public enum States
        {
            NONE,
            CROSS,
            CIRCLE
        }

        public CheckedState()
        {
            this._CurrentState = States.NONE;
        }

        public bool Checked
        {
            get
            {
                return _Checked;
            }
        }
        public States CurrentState
        {
            get
            {
                return _CurrentState;
            }
            set
            {
                if ((_CurrentState = value) != States.NONE)
                {
                    _Checked = true;
                }
                else
                {
                    _Checked = false;
                }
            }
        }

        #region Serializable Members

        public MemoryStream Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write((int)this.CurrentState);

            return ms;
        }

        public void Deserialize(MemoryStream inData)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
