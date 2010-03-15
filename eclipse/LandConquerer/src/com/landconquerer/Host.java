package com.landconquerer;

import com.google.appengine.api.datastore.Key;

import javax.jdo.annotations.IdGeneratorStrategy;
import javax.jdo.annotations.PersistenceCapable;
import javax.jdo.annotations.Persistent;
import javax.jdo.annotations.PrimaryKey;

@PersistenceCapable
public class Host
{
	@PrimaryKey
	@Persistent(valueStrategy = IdGeneratorStrategy.IDENTITY)
	private Key key;
	
	@Persistent
	private String title;
	
	@Persistent
	private String IP;
	
	public Host(String title, String IP)
	{
		this.title = title;
		this.IP = IP;
	}
	public Key getKey()
	{
		return this.key;
	}
	public String getTitle()
	{
		return this.title;
	}
	public String getIP()
	{
		return this.IP;
	}
}
