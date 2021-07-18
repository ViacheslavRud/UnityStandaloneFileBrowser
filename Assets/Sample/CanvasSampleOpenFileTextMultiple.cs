using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using StandaloneFileBrowser;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileTextMultiple : MonoBehaviour, IPointerDownHandler
{
    public Text output;

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnPointerDown(PointerEventData eventData)
    {
        UploadFile(gameObject.name, "OnFileUpload", ".txt", true);
    }

    // Called from browser
    public void OnFileUpload(string urls)
    {
        StartCoroutine(OutputRoutine(urls.Split(',')));
    }

#else
    //
    // Standalone platforms & editor
    //
    public void OnPointerDown(PointerEventData eventData) { }

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "txt", true);
        string[] paths = FileBrowser.OpenFilePanel("Open File", "", "", true);
        if (paths.Length <= 0)
            return;
        var urlArr = new List<string>(paths.Length);
        foreach (string t in paths)
        {
            urlArr.Add(new System.Uri(t).AbsoluteUri);
        }

        StartCoroutine(OutputRoutine(urlArr.ToArray()));
    }
#endif

    private IEnumerator OutputRoutine(string[] urlArr)
    {
        string outputText = "";
        foreach (string t in urlArr)
        {
            var loader = new WWW(t);
            yield return loader;
            outputText += loader.text;
        }

        output.text = outputText;
    }
}