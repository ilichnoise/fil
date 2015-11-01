using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class DecodeQuestions{
	public const string CO2 = "co2";
	public const string H2O = "h2o";
	public const string RESIDUOS = "residuos";
	public const string TITLE = "title";

	public const string HOWMANY="howmany";
	public const string YESNO="yes/no";


	public static List<Question> LoadQuestions(string name_category){
		TextAsset data = Resources.Load (name_category) as TextAsset;
		string json_content = data.text;

		List<Question> l_questions=new List<Question>();
		var questions = JSON.Parse(json_content);
		for (int i=0; i<questions.Count; i++) {
			Question aux=new Question();
			aux.question=questions[i]["question"];
			aux.type=questions[i]["type_question"];
			l_questions.Add(aux);
		}

		return l_questions;
	}

}
