using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System;
using System.IO;

public class InterfazController : MonoBehaviour 
{
	public InputField USER;
	public InputField PASS;
	public InputField IP;
	public InputField PORT;
	public InputField TAG;

	public GameObject pantallaLogin;
	public GameObject pantallaConf;

	public string user = "User";
	public string password = "Password";

	//public GameObject pantallaShot;

	private string _path;
	private OSCController cont;
	private IO io;

	void Start () 
	{
		_path = Application.persistentDataPath + "/initial.conf";
		Debug.Log (_path);
		io = new IO ();
		cont = gameObject.AddComponent<OSCController> ();
		//pantallaShot.SetActive (false);
		//Valores iniciales
		if(File.Exists(_path))
		{
			//Leemos el archivo. los datos estan separados por #
			string data = io.Read(Application.persistentDataPath + "/initial.conf");
			string [] datos = data.Split('#');
			Debug.Log("tamaño array: " + datos.Length);
			IP.text = datos[0];
			PORT.text = datos[1];
			TAG.text = datos[2];
		}
	}

	public void Login()
	{
		if(USER.text.Equals(user) && PASS.text.Equals(password))
		{
			Debug.Log("Login okay");
			pantallaLogin.SetActive(false);
		}

		else
		{
			StartCoroutine(wrongLogin());
		}
	}

	public void configButton()
	{
		int port;
		int.TryParse (PORT.text, out port);
		cont._clients.Clear ();
		cont.INIT ("vvvv", IP.text, port, TAG.text);
		io.Write(_path, IP.text+"#"+PORT.text+"#"+TAG.text);
		/*Aqui va la carga de la escena de la aplicacion, reemplazar la linea de abajo*/
		pantallaConf.SetActive (false);
	}
	/*********************************************************************************/

	IEnumerator wrongLogin()
	{
		PASS.image.color = Color.red;
		USER.image.color = Color.red;
		yield return new WaitForSeconds (.2f);
		PASS.image.color = Color.white;
		USER.image.color = Color.white;
	}
	public void regresar()
	{
		pantallaConf.SetActive (true);
		//pantallaShot.SetActive (false);
	}

	public void sendMessage()
	{
		cont.send ("1");
	}
	

}
