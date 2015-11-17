using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class App_functions : MonoBehaviour 
{

	public GameObject field_number;
	public GameObject question_label;
	public GameObject title_label;
	public GameObject []screens;
	public int max_range=7;
	public int min_range=0;
	public int value=0;
	public List<Question> questions=new List<Question>();
	private int questions_size=0;
	private int questions_position=0;
	private string select_category;
	private int random_element;
	private string s_r_e;
	private int NegativeAnswer=0;
	//screens index
	private int how_many=0;
	private int yes_no=1;
	private int title=2;
	private int animation=6;
	private int commitment=3;
	//scene index
	private int configScene = 1;
	private int questions_scene=0;

	private OSCController osc;
	private string answers = ""; //data to send to vvvv

	void Start()
	{
		LoadConf.loadFromFile(delegate {
		if(LoadConf.tag==null){
				Application.LoadLevel(configScene);
		}
			//Fuerza variables para probar categoriass
			LoadConf.tag="co2";
			LoadConf.changeConf(LoadConf.tag);
			//------//
			//activa todos los elementos para evitar crasheo
			for (int i=0; i<screens.Length; i++) {
				screens[i].SetActive(true);
			}
			//genera num aleatorio para generar elemento que se lanza a mural
			random_element= UnityEngine.Random.Range(1,LoadConf.wall_elements);
			s_r_e=random_element.ToString();
			string sendtowall_text="instruccion_lanza";
			string congrats_text="texto_final";
			//if(random_element<10)
			s_r_e="0"+s_r_e;
			string s_r_e_init="01";
			if(LoadConf.tag=="residuos"){
				int random_draw=1;
				//si lanza flor
				if(random_element==1)
					random_draw=UnityEngine.Random.Range(1,5);
				if(random_element==2)
					random_draw=UnityEngine.Random.Range(1,3);
				if(random_element==3)
					random_draw=UnityEngine.Random.Range(1,8);

				s_r_e+="_"+random_draw.ToString();
				sendtowall_text+="_"+random_element.ToString();
				congrats_text+="_"+random_element.ToString();
				s_r_e_init+="_1";

			}
			/* Abre Sprites*/
			//inicio de app
			GameObject.Find("title/TouchButton/Title_text").GetComponent<Text>().color=LoadConf.color;
			GameObject.Find("title/TouchButton/TouchImage").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/boton_standby_off");
			GameObject.Find("title/TouchButton/avatar").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/avatar/"+s_r_e_init);
			//seccion de preguntas
			GameObject.Find("pregunta").GetComponent<Text>().color=LoadConf.color;
			//seccion commitment
			GameObject.Find("commitment/Button/avatar").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/avatar/"+s_r_e);
			//GameObject.Find("commitment/Button/text").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/compromiso_1_off");
			GameObject.Find("commitment/Button/frame").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/fondo_objetomural");
			//sendtowall
			GameObject.Find("sendtowall/Button/avatar").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/avatar/"+s_r_e);
			GameObject.Find("sendtowall/Button/frame").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/fondo_objetomural");
			GameObject.Find("sendtowall/Button/row").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/flecha_lanza");
			GameObject.Find("sendtowall/Button/inst").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/"+sendtowall_text);
			//ending
			GameObject.Find("ending/Button/congrats").GetComponent<Text>().color=LoadConf.color;
			GameObject.Find("ending/Button/ecoimg").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/imagen_ultima_pant");
			GameObject.Find("ending/Button/final_text").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/"+congrats_text);
			/*Termina de abrir Sprites */
			select_category = LoadConf.tag;
			Debug.Log(LoadConf.long_tag);
			osc = gameObject.AddComponent<OSCController>();
			osc.INIT("vvvv", LoadConf.ip, int.Parse(LoadConf.port),LoadConf.long_tag);
			questions = DecodeQuestions.LoadQuestions (select_category);
			questions_size = questions.Count;
			//para mandar sin el 01 02 etc la variable
			s_r_e=random_element.ToString();
			nextQuestion ();
		});

	}
	public void sendMessagetoWall(){
		osc.send(answers);
	}

	public void initScreen(int number){
		field_number.GetComponent<Text>().text="0";
		value = 0;
		for (int i=0; i<screens.Length; i++) {
			screens[i].SetActive(false);
		}
		screens[number].SetActive(true);
	}

	public void increaseValue(){
		if (value != max_range) {
			value++;
			field_number.GetComponent<Text>().text=value.ToString();
		}

	}
	public void restart(){
		Application.LoadLevel (questions_scene);
	}
	public void decreaseValue(){
		if (value != min_range) {
			value--;
			field_number.GetComponent<Text>().text=value.ToString();
		}

	}

	public void saveNegativeAnswer(){
		NegativeAnswer = questions_position;
	}

	public void addToAnswers(Text val){
		answers += "\""+val.text+"\",";
	}

	public void nextQuestion(){
		if (questions_size != questions_position) {
			question_label.SetActive(true);
			if (questions [questions_position].type == DecodeQuestions.TITLE) {
				title_label.GetComponent<Text>().text=questions[questions_position].question;
				initScreen (title);
			}
			if (questions [questions_position].type == DecodeQuestions.HOWMANY) {
				initScreen (how_many);
				question_label.GetComponent<Text>().text=questions[questions_position].question;
				Debug.Log(questions[questions_position].custom_yes+questions[questions_position].custom_no);
				if(questions[questions_position].custom_yes!=null)
					GameObject.Find("howmany/plus").GetComponent<Button>().image.sprite=Resources.Load<Sprite>(LoadConf.tag+"/botones/"+questions[questions_position].custom_yes);
				if(questions[questions_position].custom_no!=null)
					GameObject.Find("howmany/minus").GetComponent<Button>().image.sprite=Resources.Load<Sprite>(LoadConf.tag+"/botones/"+questions[questions_position].custom_no);

				GameObject.Find("howmany/draw").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/q_draws/"+questions_position.ToString()+"_1");
				field_number.GetComponent<Text> ().text = "0";
			}
			if (questions [questions_position].type == DecodeQuestions.YESNO) {
				initScreen (yes_no);
				question_label.GetComponent<Text>().text=questions[questions_position].question;
				Debug.Log(questions[questions_position].custom_yes+questions[questions_position].custom_no);
				if(questions[questions_position].custom_yes!=null)
					GameObject.Find("yes_no/si").GetComponent<Button>().image.sprite=Resources.Load<Sprite>(LoadConf.tag+"/botones/"+questions[questions_position].custom_yes);
				if(questions[questions_position].custom_no!=null)
					GameObject.Find("yes_no/no").GetComponent<Button>().image.sprite=Resources.Load<Sprite>(LoadConf.tag+"/botones/"+questions[questions_position].custom_no);

				GameObject.Find("yes_no/draw").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/q_draws/"+questions_position.ToString()+"_1");
			}

			questions_position++;

		} else {

			answers+="\""+LoadConf.tag+"\""+","+"\""+s_r_e+"\"";
			Debug.Log("Questionario Terminado: "+ answers);
			questions_position=0;
			answers="";
			if(NegativeAnswer==0)
				NegativeAnswer=UnityEngine.Random.Range(1,3);
			//inicia compromiso para setear imgenes relacionadas
			initScreen(commitment);
			GameObject.Find("commitment/Button/text").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/compromiso_"+NegativeAnswer.ToString()+"_off");
			GameObject.Find("commitment/Button/avatar").GetComponent<Image>().sprite=Resources.Load<Sprite>(LoadConf.tag+"/c_draws/"+NegativeAnswer.ToString());
			//empieza la animacion
			initScreen(animation);
			question_label.SetActive(false);




			//nextQuestion();
		}

	
	}

	public void loadConfig()
	{
		Application.LoadLevel(configScene);
	}
}
