package com.ion;

import java.util.ArrayList;

import org.red5.server.adapter.MultiThreadedApplicationAdapter;
import org.red5.server.api.IClient;
import org.red5.server.api.IConnection;
import org.red5.server.api.IScope;
import org.red5.server.api.so.ISharedObject;

/**
 * 
 * @author Ezra
 *
 */
public class Application extends MultiThreadedApplicationAdapter
{
	private ArrayList<String> Hosts = new ArrayList<String>();

    public boolean appStart (IScope app )
    {
    	return true;
    }
    public void appStop (IScope app )
    {
    	
    }
    public boolean appConnect( IConnection conn, IScope scope, Object[] params )
    {
        return true;
    }
    public void appDisconnect( IConnection conn)
    {
       super.appDisconnect(conn);
    }
    public boolean roomJoin(IClient client, IScope room){
    	/* TODO Send a message to all the clients when
    	 * a new client joins the room.
    	 * 
    	 * There is also a possibility to check if there are
    	 * enough slots available for this new client.
    	 */
    	ISharedObject rso = getSharedObject(room, "Chat");
    	rso.setAttribute("SystemMessage", "User Joined the Room");
		return true;
    }
    public void roomLeave(IClient client, IScope scope){
    	/* TODO Send a message to all the clients that
    	 * a client has left the game.
    	 * 
    	 * Maybe check if there are still more than two
    	 * players in the room.
    	 */
    	ISharedObject rso = getSharedObject(scope, "Chat");
    	rso.setAttribute("SystemMessage", "User left the Room");
    }
    public boolean roomStart(IScope room){
    	synchronized(Hosts){
        	Hosts.add(room.getName());
    	}
		createSharedObject(room, "Chat", false);
    	return true;
    }
    public void roomStop(IScope room){
    	synchronized(Hosts){
	    	for(int i=0;i<Hosts.size();i++){
	    		String h = Hosts.get(i);
	    		if(h == room.getName()){
	        		Hosts.remove(i);
	    			break;
	    		}
	    	}
    	}
    }
    /**
     * getHosts method to get a list of the hosts on the server.
     * 
     * @return ArrayList<Host> Hosts A List of Hosts on the server.
     */
    public ArrayList<String> getHosts()
    {
    	return Hosts;
    }
}