using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class App_functions : MonoBehaviour {

	public GameObject field_number;
	public GameObject question_label;
	public GameObject []screens;
	public int max_range=7;
	public int min_range=0;
	public int value=0;
	public List<Question> questions=new List<Question>();
	private int questions_size=0;
	private int questions_position=0;

	private int how_many=0;
	private int yes_no=1;

	void Start(){
		questions=DecodeQuestions.LoadQuestions (DecodeQuestions.CO2);
		questions_size = questions.Count;
		nextQuestion ();
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

	public void increseTotalValue(int val){

	}

	public void nextQuestion(){
		if (questions_size != questions_position) {
			question_label.GetComponent<Text>().text=questions[questions_position].question;
			if (questions [questions_position].type == DecodeQuestions.HOWMANY) {
				initScreen (how_many);
				field_number.GetComponent<Text> ().text = "0";
			}
			if (questions [questions_position].type == DecodeQuestions.YESNO) {
				initScreen (yes_no);

			}
			questions_position++;

		} else {
			Debug.Log("Questionario Terminado");
			questions_position=0;
			nextQuestion();
		}

	
	}
}
