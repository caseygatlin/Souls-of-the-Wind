using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenuUI;
    public GameObject pausedText;
    public GameObject windSymbol;
    private Animator pausedTextFloat;
    private Animator windSymbolFlow;

    private const float EPSILON = 0.0001f;

	// Use this for initialization
	void Start ()
    {

        pausedTextFloat = pausedText.GetComponent<Animator>();
        pausedTextFloat.updateMode = AnimatorUpdateMode.UnscaledTime;

        windSymbolFlow = windSymbol.GetComponent<Animator>();
        windSymbolFlow.updateMode = AnimatorUpdateMode.UnscaledTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = System.Math.Abs(Time.timeScale - 1) < EPSILON ? 0f : 1f;
            pauseMenuUI.SetActive(!pauseMenuUI.activeInHierarchy);
        }
	}
}
