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
    protected string word = "";

    protected Dictionary<string, Mode> modeDict = new Dictionary<string, Mode>();
    protected Dictionary<string, Hand> handDict = new Dictionary<string, Hand>();

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
        
        SetUpDictionaries();
    }

    private void PhraseRecognitionSystem_OnError(SpeechError errorCode)
    {
        Debug.Log("errorCode =" + errorCode.ToString());
    }

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

    void ProcessPhrase(string word)
    {
        Hand hand = Hand.Both;
        Mode mode = Mode.Hand;
        bool closing = false;

        // Debug to Pause the editor
        if (word.Contains("break"))
        {
           // Debug.Break();
        }
        else if (word.Contains("quit game"))
        {
            Application.Quit();
        }
        else
        {
            string[] words = word.Split(' ');
            foreach (var w in words)
            {
                Mode testMode;
                Hand testHand;

                if (modeDict.TryGetValue(w, out testMode) && !closing)
                {
                    mode = testMode;
                    // If closing a general mode then this should keep the mode in question
                    // from overwritting it
                    closing = mode == Mode.Hand;
                }

                if (handDict.TryGetValue(w, out testHand))
                {
                    hand = testHand;

                }
            }


            customEvents.switchMode.Invoke(mode, hand);

            Debug.LogError("Switched To: " + mode.ToString() + " On The: " + hand.ToString() + " Hand");
        }
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
    
    private void SetUpDictionaries()
    {
        string hand = "normal hand hands";
        string thruster = "thrusters thruster engine engines booster boosters";
        string collection = "laser lasers digging collection";
        string market = "inventory market";
        
        SetupModeDictionary(hand, Mode.Hand);
        SetupModeDictionary(thruster, Mode.Thruster);
        SetupModeDictionary(collection, Mode.Collection);
        SetupModeDictionary(market, Mode.Menu);
        
        // These are contextual. Opening toggles that mode open and closing resets the mode on that
        // hand to a neutral mode. Therefore it doesn't matter what opening action word is 
        // used in the demo, but closing should revert the active hand's mode back to 
        // normal
        string close = "close off deactivate disengage";
        SetupModeDictionary(close, Mode.Hand);


        string left = "left buy";
        string right = "right sell";
        string both = "both all";
        SetupHandDictionary(left, Hand.Left);
        SetupHandDictionary(right, Hand.Right);
        SetupHandDictionary(both, Hand.Both);
        
    }

    private void SetupModeDictionary(string input, Mode inMode)
    {
        foreach (string variable in input.Split(' '))
        {
            modeDict.Add(variable, inMode);
        }
    }
    private void SetupHandDictionary(string input, Hand inHand)
    {
        foreach (string variable in input.Split(' '))
        {
            if (!handDict.ContainsKey(variable))
            {
                handDict.Add(variable, inHand);
            }
        }
    }
}
