using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechEngine : MonoBehaviour
{
    public string[] keywords = new string[] { };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float speed = 1;
    public CustomEvents customEvents;

    protected PhraseRecognizer recognizer;
    protected string word = "right";

    private void Start()
    {
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        // results.text = "You said: <b>" + word + "</b>";


    }

    private void Update()
    {

        switch (word)
        {
            case "clear":
                customEvents.switchMode.Invoke(Mode.None, false);
                break;

            case "thrusters":
                customEvents.switchMode.Invoke(Mode.Thruster, false);
                break;

            case "hand":
                customEvents.switchMode.Invoke(Mode.Hand, false);
                break;

            case "weapon":
                customEvents.switchMode.Invoke(Mode.Weapon, false);
                break;
        }
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }

}