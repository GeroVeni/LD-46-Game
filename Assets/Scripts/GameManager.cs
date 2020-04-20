using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelTime;
    public Color workingColor;
    public Color cautionColor;
    public Color warningColor;
    public MainSwitch[] switches;

    public TextMeshProUGUI moduleCountText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI countdownText;

    public GameObject GameOver;
    public TextMeshProUGUI gameoverTimeText;

    bool isGameOver;
    int level;
    float timer;

    float breakTime;
    float breakTimer;

    float loseTime = 15;
    float loseTimer;
    float graceTime = 3;
    float[] graceTimers;

    int WORK_MIN_LIMIT = 2;

    // Start is called before the first frame update
    void Start()
    {
        graceTimers = new float[switches.Length];
        Init();
    }

    void Init()
    {
        for (int i = 0; i < graceTimers.Length; i++)
        {
            graceTimers[i] = graceTime;
        }

        isGameOver = false;
        level = 1;
        breakTime = GetRandomBreakTime();

        timer = 0;
        breakTimer = 0;
        loseTimer = 0;

        countdownText.enabled = false;
        GameOver.SetActive(false);
        AudioManager.Instance.Play("theme");
    }

    string GetTimeText(float time)
    {
        int seconds = (int)time;
        int minutes = seconds / 60;
        seconds %= 60;
        StringBuilder text = new StringBuilder();
        if (minutes < 10) { text.Append('0'); }
        text.Append(minutes);
        text.Append(':');
        if (seconds < 10) { text.Append('0'); }
        text.Append(seconds);
        return text.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            MainLoop();
        }
    }

    void MainLoop()
    {
        timer += Time.deltaTime;
        breakTimer += Time.deltaTime;
        for (int i = 0; i < graceTimers.Length; i++)
        {
            if (graceTimers[i] < graceTime) graceTimers[i] += Time.deltaTime;
        }

        if (timer >= level * levelTime)
        {
            level++;
            Debug.Log(level);
        }
        timeText.text = GetTimeText(timer);

        if (breakTimer >= breakTime)
        {
            int randomIndex = GetRandomSwitchIndex();
            if (randomIndex != -1)
            {
                breakTimer = 0;
                breakTime = GetRandomBreakTime();
                switches[randomIndex].controllingModule.Break();
                graceTimers[randomIndex] = 0;
            }
        }

        int operationalCount = OperationalCount();

        moduleCountText.text = operationalCount.ToString() + " / " + switches.Length;
        moduleCountText.color = GetTextColor(operationalCount);

        if (operationalCount < WORK_MIN_LIMIT)
        {
            loseTimer += Time.deltaTime;
            countdownText.enabled = true;
            float remTime = loseTime - loseTimer;
            if (remTime <= 0)
            {
                remTime = 0;
                // Game over
                isGameOver = true;
                GameOver.SetActive(true);
                gameoverTimeText.text = "You survived for " + GetTimeText(timer);
                AudioManager.Instance.StopAll();
                AudioManager.Instance.Play("explode");
            }
            countdownText.text = GetTimeText(remTime);
        }
        else
        {
            countdownText.enabled = false;
            // Reset lose timer
            loseTimer = 0;
        }
    }

    public void OnReplay()
    {
        foreach (MainSwitch s in switches)
        {
            s.controllingModule.ResetModule();
            s.State = ModuleState.WORKING;
        }

        Init();
        GameOver.SetActive(false);
    }

    Color GetTextColor(int operationalCount)
    {
        if (operationalCount == switches.Length) { return workingColor; }
        if (operationalCount >= WORK_MIN_LIMIT) { return cautionColor; }
        return warningColor;
    }

    int OperationalCount()
    {
        int count = 0;
        foreach (MainSwitch s in switches)
        {
            if (s.State == ModuleState.WORKING) { count++; }
        }
        return count;
    }

    float GetRandomBreakTime()
    {
        return 5 - (level - 1) * 0.2f;
    }

    int GetRandomSwitchIndex()
    {
        List<int> operatingModules = new List<int>();
        for (int i = 0; i < switches.Length; ++i)
        {
            if (switches[i].State != ModuleState.BROKEN &&
                graceTimers[i] >= graceTime) { operatingModules.Add(i); }
        }
        if (operatingModules.Count == 0) { return -1; }
        int index = Random.Range(0, operatingModules.Count);
        return operatingModules[index];
    }
}
