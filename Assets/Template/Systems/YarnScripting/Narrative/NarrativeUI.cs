/*

The MIT License (MIT)

Copyright (c) 2015-2017 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections.Generic;
using Yarn;

/// <summary>
/// Displays dialogue lines to the player, and sends user choices back
/// to the dialogue system.
/// </summary>
/// <remarks>
/// The DialogueUI component works closely with the <see
/// cref="NarrativeRunner"/> class. It receives <see cref="Line"/>s,
/// <see cref="OptionSet"/>s and <see cref="Command"/>s from the
/// DialogueRunner, and conveys them to the rest of the game. It is
/// also responsible for relaying input from the user to the
/// DialogueRunner, such as option selection or the signal to proceed
/// to the next line.
/// </remarks>
/// <seealso cref="NarrativeRunner"/>
public class NarrativeUI : DialogueUIBehaviour
{
    public SpriteLibrary SpriteLibrary;
    public GameObject Container;
    public GameObject LeftPortraitPanel;
    public GameObject NarratorPanel;
    public GameObject OptionsPanel;

    GameObject currentOptionsGO;

    /// <summary>
    /// The object that contains the dialogue and the options.
    /// </summary>
    /// <remarks>
    /// This object will be enabled when conversation starts, and
    /// disabled when it ends.
    /// </remarks>
    public GameObject dialogueContainer;

    /// <summary>
    /// How quickly to show the text, in seconds per character
    /// </summary>
    [Tooltip("How quickly to show the text, in seconds per character")]
    public float textSpeed = 0.025f;

    // When true, the user has indicated that they want to proceed to
    // the next line.
    private bool userRequestedNextLine = false;

    // The method that we should call when the user has chosen an
    // option. Externally provided by the DialogueRunner.
    private System.Action<int> currentOptionSelectionHandler;

    // When true, the DialogueRunner is waiting for the user to press
    // one of the option buttons.
    private bool waitingForOptionSelection = false;     

    /// <summary>
    /// A <see cref="UnityEngine.Events.UnityEvent"/> that is called
    /// when the dialogue starts.
    /// </summary>
    /// <remarks>
    /// Use this event to enable any dialogue-related UI and gameplay
    /// elements, and disable any non-dialogue UI and gameplay
    /// elements.
    /// </remarks>
    public UnityEngine.Events.UnityEvent onDialogueStart;

    /// <summary>
    /// A <see cref="UnityEngine.Events.UnityEvent"/> that is called
    /// when the dialogue ends.
    /// </summary>
    /// <remarks>
    /// Use this event to disable any dialogue-related UI and gameplay
    /// elements, and enable any non-dialogue UI and gameplay elements.
    /// </remarks>
    public UnityEngine.Events.UnityEvent onDialogueEnd;  

    /// <summary>
    /// A <see cref="UnityEngine.Events.UnityEvent"/> that is called
    /// when a <see cref="Line"/> has been delivered.
    /// </summary>
    /// <remarks>
    /// This method is called before <see cref="onLineUpdate"/> is
    /// called. Use this event to prepare the scene to deliver a line.
    /// </remarks>
    public UnityEngine.Events.UnityEvent onLineStart;

    /// <summary>
    /// A <see cref="UnityEngine.Events.UnityEvent"/> that is called
    /// when a line has finished being delivered.
    /// </summary>
    /// <remarks>
    /// This method is called after <see cref="onLineUpdate"/>. Use
    /// this method to display UI elements like a "continue" button.
    ///
    /// When this method has been called, the Dialogue UI will wait for
    /// the <see cref="MarkLineComplete"/> method to be called, which
    /// signals that the line should be dismissed.
    /// </remarks>
    /// <seealso cref="onLineUpdate"/>
    /// <seealso cref="MarkLineComplete"/>
    public UnityEngine.Events.UnityEvent onLineFinishDisplaying;

    /// <summary>
    /// A <see cref="NarrativeRunner.StringUnityEvent"/> that is called
    /// when the visible part of the line's localised text changes.
    /// </summary>
    /// <remarks>
    /// The <see cref="string"/> parameter that this event receives is
    /// the text that should be displayed to the user. Use this method
    /// to display line text to the user.
    ///
    /// The <see cref="NarrativeUI"/> class gradually reveals the
    /// localised text of the <see cref="Line"/>, at a rate of <see
    /// cref="textSpeed"/> seconds per character. <see
    /// cref="onLineUpdate"/> will be called multiple times, each time
    /// with more text; the final call to <see cref="onLineUpdate"/>
    /// will have the entire text of the line.
    ///
    /// If <see cref="MarkLineComplete"/> is called before the line has
    /// finished displaying, which indicates that the user has
    /// requested that the Dialogue UI skip to the end of the line,
    /// <see cref="onLineUpdate"/> will be called once more, to display
    /// the entire text.
    ///
    /// If <see cref="textSpeed"/> is `0`, <see cref="onLineUpdate"/>
    /// will be called just once, to display the entire text all at
    /// once.
    ///
    /// After the final call to <see cref="onLineUpdate"/>, <see
    /// cref="onLineFinishDisplaying"/> will be called to indicate that
    /// the line has finished appearing.
    /// </remarks>
    /// <seealso cref="textSpeed"/>
    /// <seealso cref="onLineFinishDisplaying"/>
    public NarrativeRunner.StringUnityEvent onLineUpdate;
        
    /// <summary>
    /// A <see cref="UnityEngine.Events.UnityEvent"/> that is called
    /// when a line has finished displaying, and should be removed from
    /// the screen.
    /// </summary>
    /// <remarks>
    /// This method is called after the <see cref="MarkLineComplete"/>
    /// has been called. Use this method to dismiss the line's UI
    /// elements.
    ///
    /// After this method is called, the next piece of dialogue content
    /// will be presented, or the dialogue will end.
    /// </remarks>
    public UnityEngine.Events.UnityEvent onLineEnd;

    /// <summary>
    /// A <see cref="UnityEngine.Events.UnityEvent"/> that is called
    /// when an <see cref="OptionSet"/> has been displayed to the user.
    /// 
    /// </summary>
    /// <remarks>
    /// Before this method is called, the <see cref="Button"/>s in <see
    /// cref="optionButtons"/> are enabled or disabled (depending on
    /// how many options there are), and the <see cref="Text"/> or <see
    /// cref="TMPro.TextMeshProUGUI"/> is updated with the correct
    /// text.
    ///
    /// Use this method to ensure that the active <see
    /// cref="optionButtons"/>s are visible, such as by enabling the
    /// object that they're contained in.
    /// </remarks>
    public UnityEngine.Events.UnityEvent onOptionsStart;
        
    /// <summary>
    /// A <see cref="UnityEngine.Events.UnityEvent"/> that is called
    /// when an option has been selected, and the <see
    /// cref="optionButtons"/> should be hidden.
    /// </summary>
    /// <remarks>
    /// This method is called after one of the <see
    /// cref="optionButtons"/> has been clicked, or the <see
    /// cref="SelectOption(int)"/> method has been called.
    ///
    /// Use this method to hide all of the <see cref="optionButtons"/>,
    /// such as by disabling the object they're contained in. (The
    /// DialogueUI won't hide them for you individually.)
    /// </remarks>
    public UnityEngine.Events.UnityEvent onOptionsEnd;

    /// <summary>
    /// A <see cref="NarrativeRunner.StringUnityEvent"/> that is called
    /// when a <see cref="Command"/> is received.
    /// </summary>
    /// <remarks>
    /// Use this method to dispatch a command to other parts of your game.
    /// 
    /// This method is only called if the <see cref="Command"/> has not
    /// been handled by a command handler that has been added to the
    /// <see cref="NarrativeRunner"/>, or by a method on a <see
    /// cref="MonoBehaviour"/> in the scene with the attribute <see
    /// cref="YarnCommandAttribute"/>.
    /// 
    /// {{|note|}}
    /// When a command is delivered in this way, the <see cref="NarrativeRunner"/> will not pause execution. If you want a command to make the DialogueRunner pause execution, see <see cref="NarrativeRunner.AddCommandHandler(string,
    /// NarrativeRunner.BlockingCommandHandler)"/>.
    /// {{|/note|}}
    ///
    /// This method receives the full text of the command, as it appears between
    /// the `<![CDATA[<<]]>` and `<![CDATA[>>]]>` markers.
    /// </remarks>
    /// <seealso cref="NarrativeRunner.AddCommandHandler(string,
    /// NarrativeRunner.CommandHandler)"/> 
    /// <seealso cref="NarrativeRunner.AddCommandHandler(string,
    /// NarrativeRunner.BlockingCommandHandler)"/> 
    /// <seealso cref="YarnCommandAttribute"/>
    public NarrativeRunner.StringUnityEvent onCommand;
        
    internal void Awake ()
    {

    }

    /// Runs a line.
    /// <inheritdoc/>
    public override Dialogue.HandlerExecutionType RunLine (Yarn.Line line, ILineLocalisationProvider localisationProvider, System.Action onLineComplete)
    {
        // Start displaying the line; it will call onComplete later
        // which will tell the dialogue to continue
        StartCoroutine(DoRunLine(line, localisationProvider, onLineComplete));
        return Dialogue.HandlerExecutionType.PauseExecution;
    }

    /// Show a line of dialogue, gradually        
    private IEnumerator DoRunLine(Yarn.Line line, ILineLocalisationProvider localisationProvider, System.Action onComplete)
    {
        onLineStart?.Invoke();

        userRequestedNextLine = false;

        // The final text we'll be showing for this line.
        string text = localisationProvider.GetLocalisedTextForLine(line);

        if (text == null)
        {
            Debug.LogWarning($"Line {line.ID} doesn't have any localised text.");
            text = line.ID;
        }

        // Pull out speaker info
        var splitText = text.Split(new string[] { "::" }, 2, StringSplitOptions.None);


        if (splitText.Length < 2)
        {
            var newGO = Instantiate(NarratorPanel, Container.transform);
            var newPanel = newGO.GetComponent<NarrativePanel>();
            newPanel.Set(null, "", text);
        }
        else
        {
            var speakerInfo = splitText[0];
            var splitSpeakerInfo = speakerInfo.Split(' ');
            var speakerName = splitSpeakerInfo[0];
            var speakerSpriteName = "portrait";

            if (splitSpeakerInfo.Length > 1)
            {
                speakerSpriteName += splitSpeakerInfo[1];
            }

            var speakerSprite = SpriteLibrary.GetSprite(speakerName, speakerSpriteName);
            var lineText = splitText[1];

            var newGO = Instantiate(LeftPortraitPanel, Container.transform);
            var newPanel = newGO.GetComponent<NarrativePanel>();
            newPanel.Set(speakerSprite, speakerName, lineText);
        }        

        // blocks until user hits continue.
        while (userRequestedNextLine == false)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        // Hide the text and prompt
        onLineEnd?.Invoke();

        onComplete();
    }

    /// Runs a set of options.
    /// <inheritdoc/>
    public override void RunOptions (Yarn.OptionSet optionSet, ILineLocalisationProvider localisationProvider, System.Action<int> onOptionSelected) 
    {
        StartCoroutine(DoRunOptions(optionSet, localisationProvider, onOptionSelected));
    }

    /// Show a list of options, and wait for the player to make a
    /// selection.
    private  IEnumerator DoRunOptions (Yarn.OptionSet optionsCollection, ILineLocalisationProvider localisationProvider, System.Action<int> selectOption)
    {
        currentOptionsGO = Instantiate(OptionsPanel, Container.transform);
        var newOptionsPanel = currentOptionsGO.GetComponent<NarrativeOptions>();
        var optionButtons = newOptionsPanel.Options;

        // Do a little bit of safety checking
        if (optionsCollection.Options.Length > optionButtons.Count) {
            Debug.LogWarning("There are more options to present than there are" +
                                "buttons to present them in. This will cause problems.");
        }

        // Display each option in a button, and make it visible
        int i = 0;

        waitingForOptionSelection = true;

        currentOptionSelectionHandler = selectOption;
            
        foreach (var optionString in optionsCollection.Options) {
            optionButtons [i].gameObject.SetActive (true);

            // When the button is selected, tell the dialogue about it
            var optionText = localisationProvider.GetLocalisedTextForLine(optionString.Line);
            optionButtons [i].onClick.RemoveAllListeners();
            optionButtons [i].onClick.AddListener(() => SelectOption(optionString.ID, optionText));



            if (optionText == null) {
                Debug.LogWarning($"Option {optionString.Line.ID} doesn't have any localised text");
                optionText = optionString.Line.ID;
            }

            var unityText = optionButtons [i].GetComponentInChildren<TMPro.TMP_Text> ();
            if (unityText != null) {
                unityText.text = optionText;
            }

            var optionsText = optionButtons [i].GetComponentInChildren<TMPro.TMP_Text> ();
            if (optionsText != null) {
                optionsText.text = optionText;
            }

            i++;
        }

        onOptionsStart?.Invoke();

        // Wait until the chooser has been used and then removed 
        while (waitingForOptionSelection) {
            yield return null;
        }

        onOptionsEnd?.Invoke();


    }

    /// Runs a command.
    /// <inheritdoc/>
    public override Dialogue.HandlerExecutionType RunCommand (Yarn.Command command, System.Action onCommandComplete) {
        // Dispatch this command via the 'On Command' handler.
        onCommand?.Invoke(command.Text);

        // Signal to the DialogueRunner that it should continue
        // executing. (This implementation of RunCommand always signals
        // that execution should continue, and never calls
        // onCommandComplete.)
        return Dialogue.HandlerExecutionType.ContinueExecution;
    }

    /// Called when the dialogue system has started running.
    /// <inheritdoc/>
    public override void DialogueStarted ()
    {
        dialogueContainer.SetActive(true);
        onDialogueStart?.Invoke();            
    }

    /// Called when the dialogue system has finished running.
    /// <inheritdoc/>
    public override void DialogueComplete ()
    {
        dialogueContainer.SetActive(false);
        onDialogueEnd?.Invoke();            
    }

    /// <summary>
    /// Signals that the user has finished with a line, or wishes to
    /// skip to the end of the current line.
    /// </summary>
    /// <remarks>
    /// This method is generally called by a "continue" button, and
    /// causes the DialogueUI to signal the <see
    /// cref="NarrativeRunner"/> to proceed to the next piece of
    /// content.
    ///
    /// If this method is called before the line has finished appearing
    /// (that is, before <see cref="onLineFinishDisplaying"/> is
    /// called), the DialogueUI immediately displays the entire line
    /// (via the <see cref="onLineUpdate"/> method), and then calls
    /// <see cref="onLineFinishDisplaying"/>.
    /// </remarks>
    public void MarkLineComplete() {
        userRequestedNextLine = true;
    }

    /// <summary>
    /// Signals that the user has selected an option.
    /// </summary>
    /// <remarks>
    /// This method is called by the <see cref="Button"/>s in the <see
    /// cref="optionButtons"/> list when clicked.
    ///
    /// If you prefer, you can also call this method directly.
    /// </remarks>
    /// <param name="optionID">The <see cref="OptionSet.Option.ID"/> of
    /// the <see cref="OptionSet.Option"/> that was selected.</param>
    public void SelectOption(int optionID, string text) {
        if (waitingForOptionSelection == false) {
            Debug.LogWarning("An option was selected, but the dialogue UI was not expecting it.");
            return;
        }
        waitingForOptionSelection = false;

        Destroy(currentOptionsGO);

        var newGO = Instantiate(NarratorPanel, Container.transform);
        var narratorPanel = newGO.GetComponent<NarrativePanel>();
        narratorPanel.Set(null, "", "Option Selected: " + text);

        currentOptionSelectionHandler?.Invoke(optionID);
    }

    public override void Clear()
    {
        while (Container.transform.childCount > 0)
        {
            var child = Container.transform.GetChild(0);
            child.transform.parent = null;
            Destroy(child.gameObject);
        }
    }
}