using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class managerJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public static managerJoystick instance;
    [Header("Settings")]
    public Image imgJoystickBg;       // this is the BG
    public Image imgJoystick;         // this is the handler
    [Header("Settings")]
    public GameObject pauseMenuObject;
    private Button resumeButton;
    private Button mainMenu;
    private Button quitButton;
    [Header("Settings")]
    public bool isPaused;

    private Vector2 posInput;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        imgJoystickBg= GetComponent<Image>();
        imgJoystick = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)

                GameResumed();
            else
                GamePaused();
        }
    }

    public void GamePaused()
    {
        isPaused= true;
        Time.timeScale = 0f;
        pauseMenuObject.SetActive(true);
    }
    public void GameResumed()
    {
        isPaused= false;
        Time.timeScale = 1f;
        pauseMenuObject.SetActive(false);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(imgJoystickBg.rectTransform,
           eventData.position, eventData.pressEventCamera, out posInput))
        {
            posInput.x = posInput.x / imgJoystickBg.rectTransform.sizeDelta.x;
            posInput.y = posInput.y / imgJoystickBg.rectTransform.sizeDelta.y;
            Debug.Log(posInput.x.ToString() + "/" + posInput.y.ToString());

            if(posInput.magnitude > 1.0f)
            {
                posInput = posInput.normalized;
            }

            // move the handler here
            imgJoystick.rectTransform.anchoredPosition = new Vector2(posInput.x * (imgJoystickBg.rectTransform.sizeDelta.x / 2),
                                                                     posInput.y * (imgJoystickBg.rectTransform.sizeDelta.y / 2));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        Debug.Log("Pointer Down");        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        posInput = Vector2.zero;
        imgJoystick.rectTransform.anchoredPosition = Vector2.zero;

        Debug.Log("Pointer Up");
    }

    public float InputHorizontal()
    {
        if(posInput.x != 0)
        {
            return posInput.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float InputVertical()
    {
        if (posInput.y != 0)
        {
            return posInput.y;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}
