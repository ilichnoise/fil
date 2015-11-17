using System;
using System.Net;
using System.Collections.Generic;

using UnityEngine;
using UnityOSC;

public class OSCController : MonoBehaviour 
{
	public Dictionary<string, ClientLog> _clients = new Dictionary<string, ClientLog>();
	//public Dictionary<string, ServerLog> _servers = new Dictionary<string, ServerLog>();
	private OSCMessage message;
	private string testaddress;
	private string clientId;
	void Start () 
	{
		//clientId = "VVVV";
		//CreateClient(IPAddress.Parse("127.0.0.1"), 9001);
		//CreateClient(IPAddress.Parse("192.168.15.4"), 9001);

	}

	public void INIT(string id, string IP, int port, string tag)
	{
		clientId = id;
		testaddress = tag;
		CreateClient(IPAddress.Parse(IP), port);
	}

	/************************************************************************/
	
	public void CreateClient(IPAddress destination, int port)
	{
		ClientLog clientitem = new ClientLog();
		clientitem.client = new OSCClient(destination, port);
		clientitem.log = new List<string>();
		clientitem.messages = new List<OSCMessage>();
		
		_clients.Add(clientId, clientitem);
		
		// Send test message
		//send ("ca");
	}
	public void send(string s)
	{
		message = new OSCMessage(testaddress,s);
		
		//_clients[clientId].log.Add(String.Concat(DateTime.UtcNow.ToString(),".",FormatMilliseconds(DateTime.Now.Millisecond), " : ",
		//                                         testaddress," ", DataToString(message.Data)));
		_clients[clientId].messages.Add(message);
		
		_clients[clientId].client.Send(message);
	}

	public Dictionary<string, ClientLog> Clients
	{
		get
		{
			return _clients;
		}
	}
	/*
	public Dictionary<string, ServerLog> Servers
	{
		get
		{
			return _servers;
		}
	}*/

	private string DataToString(List<object> data)
	{
		string buffer = "";
		
		for(int i = 0; i < data.Count; i++)
		{
			buffer += data[i].ToString() + " ";
		}
		
		buffer += "\n";
		
		return buffer;
	}
	private string FormatMilliseconds(int milliseconds)
	{	
		if(milliseconds < 100)
		{
			if(milliseconds < 10)
				return String.Concat("00",milliseconds.ToString());
			
			return String.Concat("0",milliseconds.ToString());
		}
		
		return milliseconds.ToString();
	}
}
