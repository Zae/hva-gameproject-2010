/**
 * 
 */
package com.ion;

import org.red5.io.amf3.IDataInput;
import org.red5.io.amf3.IDataOutput;
import org.red5.io.amf3.IExternalizable;
import org.red5.server.api.IScope;

/**
 * @author Ezra
 *
 */
public class Host implements IExternalizable {

	public String hostname;
	public IScope scope;
	
	public Host(String hostname, IScope scope){
		//this.client = client;
		this.scope = scope;
		this.hostname = hostname;
	}

	@Override
	public void readExternal(IDataInput arg0) {
		// TODO Auto-generated method stub
		this.hostname = (String)arg0.readUTF();
	}

	@Override
	public void writeExternal(IDataOutput arg0) {
		// TODO Auto-generated method stub
		arg0.writeUTF(this.hostname);
	}	
}
