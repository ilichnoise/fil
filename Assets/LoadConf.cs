using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public class LoadConf{
	public static string ip = "";
	public static string port = "";
	public static string tag = "";
	public static string long_tag="";
	public static Color color;
	public static int wall_elements;
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
			long_tag = "/"+tag;
			string [] tag_data = tag.Split ('/');
			//checa que la ruta sea no mayor a dos

			if(tag_data.Length==2){
				tag = tag_data [1];
			}else{
				tag=null;
			}
			color.a=1.0f;
			//checa que la cadena tag sea acorde a secciones validas
			switch (tag){
				case DecodeQuestions.CO2:
					//toma color verde para tipografia
					color = new Color(69.0f/255.0f, 183.0f/255.0f, 114.0f/255.0f);
					//numero de maximo de elementos que existen para mandar al MURAL
					wall_elements=6;
				break;
				case DecodeQuestions.H2O:
					//toma color azul para tipografia
					color = new Color(15.0f/255.0f, 168.0f/255.0f, 209.0f/255.0f);
					//numero de maximo de elementos que existen para mandar al MURAL
					wall_elements=4;
				break;
				case DecodeQuestions.RESIDUOS:
					//toma color anaranjado para tipografia
					color = new Color(252.0f/255.0f, 151.0f/255.0f, 52.0f/255.0f);
					//numero de maximo de elementos que existen para mandar al MURAL
					wall_elements=3;
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
	public static void changeConf(string tag){
		switch (tag){
		case DecodeQuestions.CO2:
			//toma color verde para tipografia
			color = new Color(69.0f/255.0f, 183.0f/255.0f, 114.0f/255.0f);
			//numero de maximo de elementos que existen para mandar al MURAL
			wall_elements=6;
			break;
		case DecodeQuestions.H2O:
			//toma color azul para tipografia
			color = new Color(15.0f/255.0f, 168.0f/255.0f, 209.0f/255.0f);
			//numero de maximo de elementos que existen para mandar al MURAL
			wall_elements=4;
			break;
		case DecodeQuestions.RESIDUOS:
			//toma color anaranjado para tipografia
			color = new Color(252.0f/255.0f, 151.0f/255.0f, 52.0f/255.0f);
			//numero de maximo de elementos que existen para mandar al MURAL
			wall_elements=3;
			break;
		default:
			tag=null;
			break;
		}	
	}
}
