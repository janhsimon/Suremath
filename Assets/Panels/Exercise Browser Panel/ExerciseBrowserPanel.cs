using UnityEngine;
using UnityEngine.UI;

public class ExerciseBrowserPanel : MonoBehaviour
{
	public GameObject titleTextGameObject;

	[SerializeField]
	GameObject authorTextGameObject;

	[SerializeField]
	GameObject facultyTextGameObject;

	ExerciseDialog exerciseDialog;
	int id;

	void Start()
	{
		// TODO: dirty hack but seems necessary for resolution-independent layout
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = new Vector2(1f, 1f);
	}

	public void SetExerciseDialog(ExerciseDialog exerciseDialog)
	{
		this.exerciseDialog = exerciseDialog;
	}

	public void SetInitialValues(string id, string title, string author, string faculty)
	{
		this.id = int.Parse(id);

		Text titleText = titleTextGameObject.GetComponent<Text>();
		titleText.text = title;

		Text authorText = authorTextGameObject.GetComponent<Text>();
		authorText.text = author;

		Text facultyText = facultyTextGameObject.GetComponent<Text>();
		facultyText.text = faculty;
	}

	public void LoadExercise()
	{
		exerciseDialog.LoadExercise(id);
	}
}
