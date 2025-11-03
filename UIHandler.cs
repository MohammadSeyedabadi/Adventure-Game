using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    /// <summary>
    // The float variable displayTime stores the length of time that the dialogue window will be displayed, initialized to 4 but made public so you can adjust this in the Inspector window.
    // The private VisualElement variable m_NonPlayerCharacterDialogue will store the reference to the VisualElement when you retrieve it.
    // The private float variable timerDisplay is set to the value of displayTime when the dialogue window is instantiated, and will count down the time until the window needs to be hidden.
    /// </summary>
    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue;
    private float m_TimerDisplay;
    //////////////////////////////////////////
    public static UIHandler instance { get; private set; }
    private VisualElement m_Healthbar;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 3. In the Start function, add an instruction to get the UI Document component that’s on the same UI Document GameObject as this script:
        UIDocument uiDocument = GetComponent<UIDocument>();
        // 4. Add a second instruction to get the root of the UI Hierarchy (currently the HealthBarBackground VisualElement) and call a function named “Q” on it:

        // Here’s an explanation of this code:
        // The root of the UI Hierarchy is accessed through rootVisualElement.
        // As you’ve encountered previously, the VisualElement is accessed using the dot operator.
        // The Q function is short for Query. You can use this function to find a particular VisualElement in the Hierarchy window that matches multiple search parameters — in this case you are using the element name.
        // Query is a generic function, because you can use it to query lots of different types. The specific type you are looking for is provided in angle brackets — in this case, it’s a VisualElement named “HealthBar”.
        // Note: GetComponent, which you’ve used previously, is also a generic function: you can use it to get any kind of component, but you need to specify which one you need when you use the function.
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        // style contains all the VisualElement properties that you can set with UI Builder, and it is the width that you are resizing with this instruction.
        // The special function Length defines a size in Percent (one of the various size types available for the property).
        // Note: If you provide a value to the Width property directly, the UI Toolkit system will interpret it as a pixel size rather than a percentage.
        SetHealthValue(1.0f);

        // The first instruction stores the reference to the NPCDialogue VisualElement in the m_NonPlayerDialogue variable.
        // The second instruction hides the VisualElement when the UIDocument GameObject is instantiated at the start of the game.
        // The third instruction sets the m_TimeDisplay countdown to -1. This value is just below zero, and so can be used as a default for not displaying the UI.
        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NpcTextContainer");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;
    }

    private void Update()
    {
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }

    public void DisplayDialogue()
    {
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        m_TimerDisplay = displayTime;
    }
    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }
}
