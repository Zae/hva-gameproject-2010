using System;
using FluorineFx;
using FluorineFx.Net;

namespace SharedObjectTest
{
    public class UsersRSO : RemoteSharedObject
    {
        public void msgFromSrvr(string msg)
        {
            System.Console.Write(msg);
        }
    }
}
