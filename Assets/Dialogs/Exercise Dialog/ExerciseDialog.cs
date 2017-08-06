using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ExerciseDialog : MonoBehaviour
{
	[SerializeField]
	GameObject viewport;

	[SerializeField]
	GameObject exerciseBrowserListItemPrefab;

	Text exerciseTitleText;
	Transform internetTab;
	ShapePanelContainer shapePanelContainer;
	ExercisePanelContainer exercisePanelContainer;
	
	UnityWebRequest refreshListRequest = null, loadExerciseRequest = null;
	bool fromInternet = false;

	void Start()
	{
		GameObject loadExerciseButtonGameObject = GameObject.Find("Load Exercise Button");
		exerciseTitleText = loadExerciseButtonGameObject.GetComponentInChildren<Text>();

		//internetTab = transform.Find("Panel").Find("Internet//Local Tab");
		//internetTab.gameObject.SetActive(true);
		
		GameObject shapePanelContainerGameObject = GameObject.Find("Shape Panel Container");
		shapePanelContainer = shapePanelContainerGameObject.GetComponent<ShapePanelContainer>();

		GameObject exercisePanelContainerGameObject = GameObject.Find("Exercise Panel Container");
		exercisePanelContainer = exercisePanelContainerGameObject.GetComponent<ExercisePanelContainer>();

		//RefreshList();

		// TODO: dirty hack but seems necessary for resolution-independent layout
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = new Vector2(1f, 1f);
	}

	void Update()
	{
		if (fromInternet)
		{
			if (WebRequestManager.CheckRequest(refreshListRequest))
			{
				HandleRefreshListResponse();
			}

			if (WebRequestManager.CheckRequest(loadExerciseRequest))
			{
				HandleLoadExerciseResponse();
			}
		}
	}

	void HandleRefreshListResponse()
	{
		ExerciseParser.ParseList(refreshListRequest.downloadHandler.text, exerciseBrowserListItemPrefab, viewport, this);
		refreshListRequest = null;
	}

	void HandleLoadExerciseResponse()
	{
		ExerciseParser.ParseExercise(loadExerciseRequest.downloadHandler.text, exerciseTitleText, exercisePanelContainer, shapePanelContainer);
		Destroy(gameObject);
		loadExerciseRequest = null;
	}

	public void RefreshList(bool fromInternet)
	{
		//internetTab.gameObject.SetActive(true);
		
		for (int i = 0; i < viewport.transform.childCount; i++)
		{
			Destroy(viewport.transform.GetChild(i).gameObject);
		}

		if (fromInternet)
		{
			refreshListRequest = UnityWebRequest.Get("http://public.hochschule-trier.de/~simonj/suremath/list.php");
			refreshListRequest.Send();
		}
		else
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
			FileInfo[] fileInfo = directoryInfo.GetFiles("*.xml", SearchOption.TopDirectoryOnly);

			string list = "<exercises>";

			int id = 0;
			foreach (FileInfo file in fileInfo)
			{
				list += "<exercise id = '" + id + "' title = '" + file.Name + "' author = '' faculty = ''/>";
				id++;
			}

			list += "</exercises>";

			ExerciseParser.ParseList(list, exerciseBrowserListItemPrefab, viewport, this);
		}

		this.fromInternet = fromInternet;
	}

	public void LoadExercise(int id)
	{
		for (int i = 0; i < viewport.transform.childCount; i++)
		{
			viewport.transform.GetChild(i).gameObject.SetActive(false);
		}

		if (fromInternet)
		{
			WWWForm postData = new WWWForm();
			postData.AddField("id", id);
			loadExerciseRequest = UnityWebRequest.Post("http://public.hochschule-trier.de/~simonj/suremath/load.php", postData);
			loadExerciseRequest.Send();
		}
		else
		{
			ExerciseBrowserPanel exerciseBrowserListItemController = viewport.transform.GetChild(id).GetComponent<ExerciseBrowserPanel>();
			Text titleText = exerciseBrowserListItemController.titleTextGameObject.GetComponent<Text>();
			string content = "<exercise>";
			content += File.ReadAllText(Application.persistentDataPath + "/" + titleText.text);
			content += "</exercise>";
			ExerciseParser.ParseExercise(content, exerciseTitleText, exercisePanelContainer, shapePanelContainer);
			Destroy(gameObject);
		}
	}

	public void BackButtonClicked()
	{
		Destroy(gameObject);
	}
}
