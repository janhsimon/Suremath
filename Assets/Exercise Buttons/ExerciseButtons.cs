using UnityEngine;
using UnityEngine.UI;

public class ExerciseButtons : MonoBehaviour
{
	[SerializeField]
	GameObject exerciseDialogPrefab;

	[SerializeField]
	GameObject dialogContainer;

	[SerializeField]
	ShapePanelContainer shapePanelContainer;

	[SerializeField]
	ExercisePanelContainer exercisePanelContainer;

	[SerializeField]
	Text exerciseTitleText;

	public void UnloadExerciseButtonClicked()
	{
		shapePanelContainer.ClearAllPanels();
		exerciseTitleText.text = "Aufgabe laden...";
		exercisePanelContainer.ClearPanels(true);
	}

	public void LoadExerciseButtonClicked()
	{
		GameObject exerciseDialog = (GameObject)Instantiate(exerciseDialogPrefab, dialogContainer.transform);

		RectTransform exerciseDialogRectTransform = exerciseDialog.GetComponent<RectTransform>();
		exerciseDialogRectTransform.offsetMin = new Vector2(0f, 0f);
		exerciseDialogRectTransform.offsetMax = new Vector2(0f, 0f);
	}

	public void SolveExerciseButtonClicked()
	{
		exercisePanelContainer.ValidatePanels();
	}
}