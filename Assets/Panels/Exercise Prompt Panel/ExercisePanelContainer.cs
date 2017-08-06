using UnityEngine;
using UnityEngine.UI;

public class ExercisePanelContainer : MonoBehaviour
{
	[SerializeField]
	GameObject textPromptPanelPrefab, valuePromptPanelPrefab, pointPromptPanelPrefab;

	public void AddTextPromptPanel(string promptText = "")
	{
		GameObject promptPanelGameObject = (GameObject)Instantiate(textPromptPanelPrefab, transform);

		if (promptText.Length > 0)
		{
			Text text = promptPanelGameObject.GetComponentInChildren<Text>();
			text.text = promptText;
		}
	}

	public void AddValuePromptPanel(string text, float solution)
	{
		GameObject valuePromptPanelGameObject = (GameObject)Instantiate(valuePromptPanelPrefab, transform);
		ValuePromptPanel valuePromptPanel = valuePromptPanelGameObject.GetComponent<ValuePromptPanel>();
		valuePromptPanel.SetInitialValues(text, solution);
	}

	public void AddPointPromptPanel(string text, Vector2 solution)
	{
		GameObject pointPromptPanelGameObject = (GameObject)Instantiate(pointPromptPanelPrefab, transform);
		PointPromptPanel pointPromptPanel = pointPromptPanelGameObject.GetComponent<PointPromptPanel>();
		pointPromptPanel.SetInitialValues(text, solution);
	}

	public void ClearPanels(bool leaveDefaultPrompt)
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			Destroy(transform.GetChild(i).gameObject);
		}

		if (leaveDefaultPrompt)
		{
			AddTextPromptPanel();
		}
	}

	public void ValidatePanels()
	{
		ValuePromptPanel[] valueInputPanels = GetComponentsInChildren<ValuePromptPanel>();
		foreach (ValuePromptPanel valueInputPanel in valueInputPanels)
		{
			valueInputPanel.Validate();
		}

		PointPromptPanel[] pointInputPanels = GetComponentsInChildren<PointPromptPanel>();
		foreach (PointPromptPanel pointInputPanel in pointInputPanels)
		{
			pointInputPanel.Validate();
		}
	}
}
