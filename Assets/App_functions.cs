using UnityEngine;
using UnityEngine.UI;
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
	private int how_many=0;
	private int yes_no=1;
	private int title=2;
	private int configScene = 1;

	private OSCController osc;
	private string answers = ""; //data to send to vvvv

	void Start()
	{
		LoadConf.loadFromFile(delegate {
		if(LoadConf.tag==null){
				Application.LoadLevel(configScene);
		}
			//LoadConf.tag="residuos";
			/* Abre Sprites*/


			GameObject.Find("title").GetComponent<Image>().overrideSprite=Resources.Load<Sprite>(LoadConf.tag+"/fondo_standby");
			GameObject.Find("fondo_app").GetComponent<Image>().overrideSprite=Resources.Load<Sprite>(LoadConf.tag+"/fondo_todo");
			GameObject.Find("yes_no/si").GetComponent<Button>().image.sprite=Resources.Load<Sprite>(LoadConf.tag+"/botones/si_off");
			GameObject.Find("yes_no/no").GetComponent<Button>().image.sprite=Resources.Load<Sprite>(LoadConf.tag+"/botones/no_off");
			//GameObject.Find("howmany/plus").GetComponent<Button>().image.sprite=Resources.Load<Sprite>(LoadConf.tag+"/mas_off");
			//GameObject.Find("howmany/minus").GetComponent<Image>().overrideSprite=Resources.Load<Sprite>(LoadConf.tag+"/menos_off");
			/*Termina de abrir Sprites */
			select_category = LoadConf.tag;
			Debug.Log(LoadConf.long_tag);
			osc = gameObject.AddComponent<OSCController>();
			osc.INIT("vvvv", LoadConf.ip, int.Parse(LoadConf.port),LoadConf.long_tag);
			questions = DecodeQuestions.LoadQuestions (select_category);
			questions_size = questions.Count;
			nextQuestion ();
		});

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

	public void decreaseValue(){
		if (value != min_range) {
			value--;
			field_number.GetComponent<Text>().text=value.ToString();
		}

	}

	public void addToAnswers(Text val){
		answers += "\""+val.text+"\",";
	}

	public void nextQuestion(){
		if (questions_size != questions_position) {
			question_label.GetComponent<Text>().text=questions[questions_position].question;
			if (questions [questions_position].type == DecodeQuestions.TITLE) {
				title_label.GetComponent<Text>().text=questions[questions_position].question;
				initScreen (title);
			}
			if (questions [questions_position].type == DecodeQuestions.HOWMANY) {
				initScreen (how_many);
				field_number.GetComponent<Text> ().text = "0";
			}
			if (questions [questions_position].type == DecodeQuestions.YESNO) {
				initScreen (yes_no);
			}

			questions_position++;

		} else {
			answers+="\""+LoadConf.tag+"\"";
			Debug.Log("Questionario Terminado: "+ answers);
			osc.send(answers);
			questions_position=0;
			answers="";
			nextQuestion();
		}

	
	}

	public void loadConfig()
	{
		Application.LoadLevel(configScene);
	}
}
