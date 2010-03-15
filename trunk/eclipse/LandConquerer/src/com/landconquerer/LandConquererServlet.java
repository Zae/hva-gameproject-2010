package com.landconquerer;

import java.io.IOException;
import java.util.List;

import javax.jdo.PersistenceManager;
import javax.servlet.http.*;

import com.landconquerer.Host;
import com.landconquerer.PMF;

/**
 * @version 0.1
 * @author Ezra
 */
@SuppressWarnings("serial")
public class LandConquererServlet extends HttpServlet {
	private enum GetMode { SHOWSERVERLIST, SHOWUSERS }
	private enum PostMode { HOST, JOIN }
	
	/**
	 * Deze functie zorgt voor alle logica voor de GET requests
	 * voor de applicatie.
	 * 
	 * @author Ezra
	 */
	public void doGet(HttpServletRequest req, HttpServletResponse resp) throws IOException 
	{
		resp.setContentType("text/plain");
		PersistenceManager pm = PMF.get().getPersistenceManager();
		try{
			if(req.getParameter("mode") == null)
				throw new IOException("mode not set");
			GetMode enumval = GetMode.valueOf(req.getParameter("mode"));
			switch (enumval) {
				case SHOWSERVERLIST:
					resp.getWriter().println("SHOWSERVERLIST");
					break;
				case SHOWUSERS:
				    String query = "select from " + Host.class.getName();
				    List<Host> hosts = (List<Host>) pm.newQuery(query).execute();
				    for(Host h : hosts){
				    	resp.getWriter().println(h.getTitle());
				    	resp.getWriter().println(h.getIP());
				    }
					break;
			}
		}catch(IOException e)
		{
			resp.getWriter().println("PLEASE SPECIFY A CORRECT MODE");
			resp.getWriter().println("AVAILABLE MODES ARE:");
			for(GetMode m : GetMode.values())
			{
				resp.getWriter().println(m);
			}
		}
		finally{
			pm.close();
		}
	}
	/**
	 * Deze functie zorgt voor alle logica voor de POST requests
	 * voor de applicatie.
	 * 
	 * @author Ezra
	 */
	public void doPost(HttpServletRequest req, HttpServletResponse resp) throws IOException
	{
		resp.setContentType("text/plain");
		PersistenceManager pm = PMF.get().getPersistenceManager();
		try{
			if(req.getParameter("title")== null || req.getParameter("ip") == null)
				throw new IOException("mode not set");
			Host host = new Host(req.getParameter("title"), req.getParameter("ip"));
			pm.makePersistent(host);
		}
		catch(IOException e){
			resp.getWriter().println("PLEASE SPECIFY A CORRECT MODE");
			resp.getWriter().println("AVAILABLE MODES ARE:");
			for(PostMode m : PostMode.values())
			{
				resp.getWriter().println(m);
			}
		}
		finally{
			pm.close();
		}
	}
}
