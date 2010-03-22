package com.milgra;

import org.red5.server.adapter.ApplicationAdapter;
import org.red5.server.api.IConnection;

public class Application extends ApplicationAdapter
{
    public Boolean appStart ( )
    {
    	return true;
    }
    public void appStop ( )
    {

    }
    public boolean appConnect( IConnection conn , Object[] params )
    {
    	//boolean accept = (Boolean)params[0];

        //if ( !accept ) rejectClient( "you passed false..." );

        return true;
    }

    public void appDisconnect( IConnection conn)
    {
       super.appDisconnect(conn);
    }
}