using UnityEngine;
using System.Collections;
using System.IO;

public class IO
{
	public string Read(string _path)
	{
		string data = "";
		StreamReader file = new StreamReader(_path);
		
		while(!file.EndOfStream)
		{
			data += file.ReadLine();
		}
		file.Close();
		return data;
	}

	public void Write(string _path, string datos)
	{
		System.IO.File.WriteAllText(_path, datos);
	}
}
