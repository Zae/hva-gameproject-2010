package com.ion
{
	import flash.display.Sprite;
	import flash.events.AsyncErrorEvent;
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.NetStatusEvent;
	import flash.events.SecurityErrorEvent;
	import flash.events.SyncEvent;
	import flash.net.NetConnection;
	import flash.net.ObjectEncoding;
	import flash.net.SharedObject;
	
	/**
	 * ...
	 * @author Ezra Pool
	 */
	public class Main extends Sprite 
	{
		private var nc:NetConnection;
		private var rso:SharedObject;
		
		public function Main():void 
		{
			if (stage) init();
			else addEventListener(Event.ADDED_TO_STAGE, init);
		}
		
		private function init(e:Event = null):void 
		{
			removeEventListener(Event.ADDED_TO_STAGE, init);
			// entry point
			
			this.nc = new NetConnection();
			nc.objectEncoding = ObjectEncoding.AMF3;
			nc.addEventListener(NetStatusEvent.NET_STATUS, onNetStatus);
			nc.addEventListener(SecurityErrorEvent.SECURITY_ERROR, onSecurityError);
			nc.addEventListener(AsyncErrorEvent.ASYNC_ERROR, onAsyncError);
			nc.addEventListener(IOErrorEvent.IO_ERROR, onIOErrorEvent);
			nc.connect("rtmp://red5.dooping.nl:1935/ion/room1", true);
		}
		private function onIOErrorEvent(event:IOErrorEvent):void 
		{
			trace(event);
		}
		private function onAsyncError(event:AsyncErrorEvent):void 
		{
			trace(event);
		}
		private function onSecurityError(event:SecurityErrorEvent):void 
		{
			trace(event);
		}
		private function onNetStatus(event:NetStatusEvent):void {
			trace(event);
			trace(nc.connected == true ? "true" : "false");
			rso = SharedObject.getRemote("Chat", nc.uri.toString());
			rso.addEventListener(SyncEvent.SYNC, OnSync);
			rso.connect(nc);
		}
		private function OnSync(event:SyncEvent):void 
		{
			trace(event);
			trace(rso.data.SystemMessage);
		}
	}
	
}