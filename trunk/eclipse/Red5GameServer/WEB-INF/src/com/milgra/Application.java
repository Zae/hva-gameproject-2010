package com.milgra;

import java.util.ArrayList;

import org.red5.server.adapter.ApplicationAdapter;
import org.red5.server.api.IConnection;
import org.red5.server.api.IScope;

public class Application extends ApplicationAdapter
{
	private ArrayList<String> Hosts;

    public Boolean appStart ( )
    {
    	Hosts = new ArrayList<String>();
    	return true;
    }
    public void appStop ( )
    {

    }
    public boolean appConnect( IConnection conn, IScope scope, Object[] params )
    {
    	//boolean accept = (Boolean)params[0];

        //if ( !accept ) rejectClient( "you passed false..." );

    	//createSharedObject(scope, "Player1", false);
    	
        return true;
    }

    public void appDisconnect( IConnection conn)
    {
       super.appDisconnect(conn);
    }
    public ArrayList<String> getHosts()
    {
    	return Hosts;
    }
    public Boolean HostGame(String name)
    {
    	Hosts.add("name");
    	return true;
    }
}