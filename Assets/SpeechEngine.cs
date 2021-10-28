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
            case "engage thrusters":
                customEvents.engageThrusters.Invoke();
                break;

            case "disengage thrusters":
                customEvents.disengageThrusters.Invoke();
                break;

            case "hand":
                customEvents.hand.Invoke();
                break;

            case "weapon":
                customEvents.weapon.Invoke();
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