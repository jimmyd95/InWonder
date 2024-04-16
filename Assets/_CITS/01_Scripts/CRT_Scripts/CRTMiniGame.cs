using TMPro;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System.Collections.Generic;


public class CRTMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    private List<string> words = new List<string>();
    // [SerializeField] private TextMeshProUGUI[] displayTextStorage = new TextMeshProUGUI[9];
    // private TextMeshPro[] displayTextStorage = new TextMeshPro[9];
    private int[] arbitraryTriple = new int[3];
    private List<string> checkIfCorrectWords = new List<string>();
    private string[] currentStorageText = new string[9];
    private bool _hasAquiredWords = false;
    private bool _hasDisplayedText = false;

    private void Start() {
        aquireWords();
        displayText();
    }

    private void Update() {

        if (_hasAquiredWords && _hasDisplayedText)
        {
            // check in every update if any of the nine buttons is selected, if so, check if the selected words are included in the array, if so, change to the next patch of words
            foreach (var item in buttons)
            {
                if(item.GetComponent<ButtonClass>().buttonSelected)
                {
                    var tempTMPtext = item.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshPro>(); // since this is embeded within the button, it has to go through all the child objects
                    // if the button is selected and it is not within the checkIfCorrectWords, add it to the list, and decrease the pressCount

                    if (!checkIfCorrectWords.Contains(tempTMPtext.text))
                    {
                        Debug.Log("Found list: " + tempTMPtext.text);
                        tempTMPtext.fontSize += 2;
                        checkIfCorrectWords.Add(tempTMPtext.text);
                        Debug.Log("This is the end of the line, check again if correct words: " + checkIfCorrectWords.Count + " " + checkIfCorrectWords.Last());
                    }

                    if (checkIfCorrectWords.Count == 3)
                    {
                        if (checkCorrectWords(checkIfCorrectWords))
                        {
                            displayText();
                            Debug.Log("The word is correct");
                        }
                        else
                        {
                            displayErrorText();
                            Debug.Log("The word is incorrect");
                        }
                        checkIfCorrectWords.Clear();
                    }
                    item.GetComponent<ButtonClass>().buttonSelected = false;
                }
            }
        }else if(!_hasAquiredWords)
        {
            aquireWords();
        }else if(!_hasDisplayedText)
        {
            displayText();
        }else
        {
            Debug.Log("Something went wrong");
        }
    }
    
    [Button("Start MiniGame")]
    public void StartMiniGame(){
        aquireWords();
        displayText();
    }

    // read the 'MiniGame-words.txt' file
    // export the words in each cell in respective 1D array
    private void aquireWords(){
        string path = "Assets/Resources/MiniGame_words.txt";

        string[] lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            // split the word with ',' and '\n' and add them to the list and exclude the empty entries
            words.AddRange(line.Split(new char[] {',', '\n'}, System.StringSplitOptions.RemoveEmptyEntries));
        }
        _hasAquiredWords = true;

        // foreach (var word in words)
        // {
        //     Debug.Log("Words: " + word);
        // }
        // Debug.Log("Words Count: " + words.Count + "\n\n\n");
    }

    // assign the words to the GameObjects, display them with text
    private void displayText(){
        selectRandomWords(); // call it so the arbitraryTriple is filled with random numbers
        // System.Array.Clear (displayTextStorage, 0, 9); // this is a slighly more readable way to clean out the array
        // ((IList)displayTextStorage).Clear(); // this is cleaner, but does the same thing

        // a lazy but effective way to do everything in a 1D arry
        currentStorageText = new string[9];
        for (int i = 0; i < 3; i++)
        {
            words.CopyTo(arbitraryTriple[i], currentStorageText, i * 3, 3);
        }

        var indices = randomizeNumbers(buttons);

        for (int i = 0; i < buttons.Length; i++) // this here should be 9, if not...
        {
            int tempTextPlacement = indices[i];
            // displayTextStorage[i].text = storageText[tempTextPlacement];
            buttons[i].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = currentStorageText[tempTextPlacement];
            // Debug.Log("The randomized text: " + currentStorageText[tempTextPlacement] + "\n\n");
        }
        _hasDisplayedText = true;
    }

    private void displayErrorText(){
        StartCoroutine(displayError());
    }

    IEnumerator displayError(){
        foreach (var button in buttons)
        {
            button.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<CRTTextChange>().ChangeTextToStrikethrough();
        }
        yield return new WaitForSecondsRealtime(0.8f);
        foreach (var button in buttons)
        {
            button.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<CRTTextChange>().ChangeTextToNormal();
        }
    }

    // find the correct comparison and check if the words are correct
    private bool checkCorrectWords(List<string> currentWords){

        // Enumerable.SequenceEqual(x.OrderBy(e => e), y.OrderBy(e => e))
        // comparing the sequence from words list and the currentWords list given that order does not matter
        for (int i = 0; i < currentWords.Count; i++)
        {
            // Debug.Log("Starting here --- Current Words: " + currentWords[i] + " current words lenght " + currentWords.Count + " | Arb words: " + words[arbitraryTriple[i]] + " | words length " + words.GetRange(arbitraryTriple[i], 3).Count);
            // Debug.Log("Current Words: " + currentWords[i] + " | Words: " + words[arbitraryTriple[i] + 1]);
            // Debug.Log("Current Words: " + currentWords[i] + " | Words: " + words[arbitraryTriple[i] + 2]);
            if (Enumerable.SequenceEqual(currentWords.OrderBy(e => e), words.GetRange(arbitraryTriple[i], 3).OrderBy(e => e))) return true;
        }

        return false;
    }

    private List<int> randomizeNumbers(GameObject[] buttons){
        // Generate a list of all possible indices
        List<int> indices = Enumerable.Range(0, buttons.Length).ToList();

        return randomizer(indices);
    }

    // randomly select a number between the length of words
    private void selectRandomWords(){
        var tempCounts = words.Count / 3;

        List<int> indices = Enumerable.Range(0, tempCounts).Select(i => i * 3).ToList();

        indices = randomizer(indices);

        arbitraryTriple[0] = indices[0];
        arbitraryTriple[1] = indices[1];
        arbitraryTriple[2] = indices[2];
    }

    private List<int> randomizer(List<int> indices){
        int n = indices.Count;

        while(n > 1){
            n--;
            int k = Random.Range(0, n + 1);
            int value = indices[k];
            indices[k] = indices[n];
            indices[n] = value;
        }

        return indices;
    }

}
