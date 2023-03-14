using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class DonwloadImages : MonoBehaviour
{
    public string ClipboardValue { get => GUIUtility.systemCopyBuffer; set => GUIUtility.systemCopyBuffer = value; }
    string currentSelection = "";
    [SerializeField] TextMeshProUGUI clipDownload;
    [SerializeField] int deepness = 2;


    [SerializeField] string title;
    [SerializeField] List<string> elements = new List<string>();
    [SerializeField] List<string> elementsAllIages = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Clipboard " + ClipboardValue);
    }

    // Update is called once per frame
    void Update()
    {
        currentSelection = ClipboardValue;
        clipDownload.text = ClipboardValue;
    }

    public void ScanCurrentPage()
    {
        Debug.Log("Start scan");
        StartCoroutine(GetText(currentSelection));
    }

    IEnumerator GetText(string _url)
    {
        int currentDeepness = 1;
        using (UnityWebRequest request = UnityWebRequest.Get(_url))
        {
            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("YESSS");
                title = "";
                elementsAllIages = new List<string>();
                elements = new List<string>();
                string pageURL = _url.Split('/')[1];
                Debug.Log(pageURL);
                var text = request.downloadHandler.text;
                using (var reader = new StringReader(text))
                {
                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    {
                        if (line.Contains("<title>"))
                        {
                            line = line.Replace("<title>", "");
                            line = line.Replace("</title>", "");
                            title = line;
                        }
                        if (line.Contains("href="))
                        {
                            string lastLine = line;
                            int value = line.IndexOf("href=");
                            line = line.Substring(value, line.Length - value);
                            if (line.Contains("\""))
                            {
                                var lines = line.Split('"');
                                if (!lines[1].Contains(pageURL) && !elements.Contains(lines[1]))
                                {
                                    
                                    elements.Add(pageURL + lines[1]);
                                }
                            }
                        }
                    }
                }
                Debug.Log("Text: " + text);
            }
            foreach (var link in elements)
            {
                if (currentDeepness <= deepness)
                {
                    yield return DeepScan(link, currentDeepness +1);
                }
            }
        }
    }

    IEnumerator DeepScan(string _url, int currentDeepness)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(_url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                elements = new List<string>();
                string pageURL = _url.Split('/')[0];
                var text = request.downloadHandler.text;
                using (var reader = new StringReader(text))
                {
                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    {
                        SearchCurrentLine(pageURL, line);

                        if (line.Contains("href="))
                        {
                            string lastLine = line;
                            int value = line.IndexOf("href=");
                            line = line.Substring(value, line.Length - value);
                            if (line.Contains("\""))
                            {
                                var lines = line.Split('"');
                                if (!lines[1].Contains(pageURL) && !elements.Contains(lines[1]))
                                {

                                    elements.Add(pageURL + lines[1]);
                                }
                            }
                        }
                    }
                }
                Debug.Log("Text: " + text);
            }
            foreach (var link in elements)
            {
                if (currentDeepness <= deepness)
                {
                    yield return DeepScan(link, currentDeepness + 1);
                }
            }
        }
    }

    private void SearchCurrentLine(string pageURL, string line)
    {
        string lastLine = line;

        if (line.Contains("data-src"))
        {
            int value = line.IndexOf("data-src");
            line = line.Substring(value, line.Length - value);
            if (line.Contains("\""))
            {
                var lines = line.Split('"');
                if (!lines[1].Contains(pageURL) && !elementsAllIages.Contains(lines[1]))
                {

                    elementsAllIages.Add(pageURL + lines[1]);
                }
            }
        }
    }
}
