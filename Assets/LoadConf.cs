using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public class LoadConf{
	public static string ip = "";
	public static string port = "";
	public static string tag = "";
	public delegate void Delegate1();
	private static IO io;

	public static void loadFromFile(Delegate1 action){
		string _path = Application.persistentDataPath + "/initial.conf";
		Debug.Log (_path);
		io = new IO ();
		if (File.Exists (_path)) {
			//Leemos el archivo. los datos estan separados por #
			string data = io.Read (Application.persistentDataPath + "/initial.conf");
			string [] datos = data.Split ('#');
			ip = datos [0];
			//expresion regular ip
			Match match = Regex.Match(ip, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
			if (!match.Success)
			{
				tag=null;
			}
			//checa si es numerico el puerto
			port = datos [1];
			int n;
			if(!int.TryParse(port, out n)){
				tag=null;
			}
			tag = datos [2];
			string [] tag_data = tag.Split ('/');
			//checa que la ruta sea no mayor a dos
			if(tag_data.Length==2){
				tag = tag_data [1];
			}else{
				tag=null;
			}
			//checa que la cadena tag sea acorde a secciones validas
			switch (tag){
				case DecodeQuestions.CO2:
				break;
				case DecodeQuestions.H2O:
				break;
				case DecodeQuestions.RESIDUOS:
				break;
				default:
					tag=null;
				break;
			}
			
			action ();
		} else {
			tag=null;
			action();
		}

	}
}
