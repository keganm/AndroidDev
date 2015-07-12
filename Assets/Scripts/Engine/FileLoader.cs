using UnityEngine;
using System.Collections;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("HighScoreCollection")]
public class HighScoreContainer
{
	[XmlArray("HighScore"), XmlArrayItem("HighScore")]
	public int[] HighScore;

	public void Save(string path)
	{
		XmlSerializer serializer = new XmlSerializer (typeof(HighScoreContainer));
		using( FileStream stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, this);
		}
	}

	public static HighScoreContainer Load(string path)
	{
		XmlSerializer serializer = new XmlSerializer (typeof(HighScoreContainer));
		using (FileStream stream = new FileStream(path, FileMode.Open)) 
		{
			return serializer.Deserialize(stream) as HighScoreContainer;
		}
	}

	public static HighScoreContainer LoadFromText(string text)
	{
		XmlSerializer serializer = new XmlSerializer (typeof(HighScoreContainer));
		return serializer.Deserialize (new StringReader (text)) as HighScoreContainer;
	}
}

public class FileLoader : MonoBehaviour {

	void Start()
	{
		HighScoreContainer highScores = new HighScoreContainer ();
		highScores.HighScore = new int[10];
		Debug.Log ("Input");
		for (int i = 0; i < highScores.HighScore.Length; i++) {
			highScores.HighScore[i] = Random.Range(0,1000);
			Debug.Log(i.ToString() + " :: " + highScores.HighScore[i].ToString());
		}
		highScores.Save (Path.Combine (Application.persistentDataPath, "HighScores.xml"));

		Debug.Log ("Clear");
		highScores = new HighScoreContainer ();
		highScores.HighScore = new int[10];
		for(int i = 0; i < highScores.HighScore.Length; i++)
		{
			Debug.Log(i.ToString() + " :: " + highScores.HighScore[i].ToString());
		}
		Debug.Log ("Loaded");
		highScores = HighScoreContainer.Load(Path.Combine(Application.persistentDataPath, "HighScores.xml"));
		for(int i = 0; i < highScores.HighScore.Length; i++)
		{
			Debug.Log(i.ToString() + " :: " + highScores.HighScore[i].ToString());
		}
	}
}
