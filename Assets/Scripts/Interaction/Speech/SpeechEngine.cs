using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;


public class SpeechEngine : MonoBehaviour
{

    public ConfidenceLevel confidence = ConfidenceLevel.Low;
    public CustomEvents customEvents;

    protected GrammarRecognizer grammarRecognizer;
    //protected PhraseRecognizer keywordRecognizer;
    protected string word = "";

    private void Start()
    {
        PhraseRecognitionSystem.OnError += PhraseRecognitionSystem_OnError;

        if (grammarRecognizer != null && grammarRecognizer.IsRunning)
        {
            Debug.Log("grammarRecognizer already exists");
            grammarRecognizer.OnPhraseRecognized -= GrammarRecognizer_OnPhraseRecognized;
            grammarRecognizer.Stop();
            grammarRecognizer.Dispose();
        }
        
        string directory = Application.streamingAssetsPath;
        string fileName = "/GrammarFile.xml";
        if (Directory.Exists(directory))
        {

            if (File.Exists(directory + fileName))
            {
                grammarRecognizer = new GrammarRecognizer(directory + fileName, confidence);
                grammarRecognizer.OnPhraseRecognized += GrammarRecognizer_OnPhraseRecognized;
                grammarRecognizer.Start();

                if (grammarRecognizer.IsRunning)
                {
                    Debug.Log("Start - grammarRecognizer is running from file: " + grammarRecognizer.GrammarFilePath);
                }
            }
            else
            {
                Debug.Log("File not found.");
            }
        }
        else
        {
            Debug.Log("Directory not found.");
        }        
    }

    private void PhraseRecognitionSystem_OnError(SpeechError errorCode)
    {
        Debug.Log("errorCode =" + errorCode.ToString());
    }

    //private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    //{
    //    word = args.text;
    //    results.text = "You said: <b>" + word + "</b>";
    //}

    private void GrammarRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Phrase Recognised");
        word = args.text;
        
        SemanticMeaning[] meanings = args.semanticMeanings;
        foreach (SemanticMeaning item in meanings)
        {
            Debug.Log(item.ToString());
            Debug.Log(item.key);
            Debug.Log(item.values);
        }

        Debug.Log("GrammarRecognizer_OnPhraseRecognized: " + word);
        Debug.Log("GrammarRecognizer_OnPhraseRecognized - meanings: " + meanings.Length);

        Debug.Log(word);

        ProcessPhrase(word);
    }

    private void Update()
    {
        
    }

    void ProcessPhrase(string word)
    {
        Hand hand;
        Mode mode;
        // open left inventory

        if (word.Contains("left"))
        {
            hand = Hand.Left;
        }
        else if (word.Contains("right"))
        {
            hand = Hand.Right;
        }
        else if (word.Contains("both"))
        {
            hand = Hand.Both;
        }
        else
        {
            hand = Hand.None;
        }

        if (word.Contains("inventory"))
        {
            mode = Mode.Menu;
        }
        else if (word.Contains("thruster"))
        {
            mode = Mode.Thruster;
        }
        else if (word.Contains("laser"))
        {
            mode = Mode.Collection;
        }
        else
        {
            mode = Mode.Hand;
        }

        customEvents.switchMode.Invoke(mode, hand);
    }

    private void OnApplicationQuit()
    {
        //*********************************************
        //if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        //{
        //    keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
        //    keywordRecognizer.Stop();
        //}
        //*********************************************

        if (grammarRecognizer != null && grammarRecognizer.IsRunning)
        {
            grammarRecognizer.OnPhraseRecognized -= GrammarRecognizer_OnPhraseRecognized;
            grammarRecognizer.Stop();
        }
    }
}
