using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using StandaloneFileBrowser;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class CanvasSampleSaveFileText : MonoBehaviour, IPointerDownHandler
{
    public Text output;

    // Sample text data
    private const string Data = "Example text created by StandaloneFileBrowser";

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    // Browser plugin should be called in OnPointerDown.
    public void OnPointerDown(PointerEventData eventData)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(Data);
        DownloadFile(gameObject.name, "OnFileDownload", "sample.txt", bytes, bytes.Length);
    }

    // Called from browser
    public void OnFileDownload()
    {
        output.text = "File Successfully Downloaded";
    }

#else

    //
    // Standalone platforms & editor
    //
    public void OnPointerDown(PointerEventData eventData) { }

    // Listen OnClick event in standalone builds
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        string path = FileBrowser.SaveFilePanel("Title", "", "sample", "txt");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, Data);
        }
    }

#endif
}