package com.ion;

import java.util.ArrayList;
import org.red5.server.adapter.ApplicationAdapter;
import org.red5.server.api.IClient;
import org.red5.server.api.IConnection;
import org.red5.server.api.IScope;

/**
 * 
 * @author Ezra
 *
 */
public class Application extends ApplicationAdapter
{
	private ArrayList<Host> Hosts = new ArrayList<Host>();
	//private IConnection iConn;

    public boolean appStart (IScope app )
    {
    	return true;
    }
    public void appStop (IScope app )
    {
    	
    }
    public boolean appConnect( IConnection conn, IScope scope, Object[] params )
    {    	
    	//iConn = conn;
        return true;
    }
    public void appDisconnect( IConnection conn)
    {
    	/*for(int i=0;i<Hosts.size();i++){
    		Host h = Hosts.get(i);
    		if(h.client == conn.getClient()){
    			Hosts.remove(i);
    			break;
    		}    		
    	}*/
       super.appDisconnect(conn);
    }
    public boolean roomJoin(IClient client, IScope room){
    	/* TODO Send a message to all the clients when
    	 * a new client joins the room.
    	 * 
    	 * There is also a possibility to check if there are
    	 * enough slots available for this new client.
    	 */
		return false;
    }
    public void roomLeave(IClient client, IScope scope){
    	/* TODO Send a message to all the clients that
    	 * a client has left the game.
    	 * 
    	 * Maybe check if there are still more than two
    	 * players in the room.
    	 */
    	if(scope.getClients().size() < 1){
    		roomStop(scope);
    	}
    }
    public boolean roomStart(IScope room){
		Hosts.add(new Host(room.getName(), room));
    	return false;
    }
    public void roomStop(IScope room){
    	/* TODO Delete the room from the Hostlist
    	 * instead of using stopHostingGame.
    	 */
    	for(int i=0;i<Hosts.size();i++){
    		Host h = Hosts.get(i);
    		if(h.hostname == room.getName()){
    			Hosts.remove(i);
    			break;
    		}
    	}
    }
    /**
     * getHosts method to get a list of the hosts on the server.
     * 
     * @return ArrayList<Host> Hosts A List of Hosts on the server.
     */
    public ArrayList<Host> getHosts()
    {
    	return Hosts;
    }
}