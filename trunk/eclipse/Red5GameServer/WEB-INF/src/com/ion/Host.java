/**
 * 
 */
package com.ion;

import org.red5.server.api.IScope;

/**
 * @author Ezra
 *
 */
public class Host {

	public String hostname;
	public IScope scope;
	
	public Host(String hostname, IScope scope){
		//this.client = client;
		this.scope = scope;
		this.hostname = hostname;
	}
}
