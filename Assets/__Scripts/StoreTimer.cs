using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine.UI;
using System.Threading;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class StoreTimer : MonoBehaviour
{
    public List<TimerData> nameTimePairs;
    private Regex namePattern = new Regex(@"^New Interval (\d+)$");
    private Regex nameTimerPattern = new Regex(@"^New Timer (\d+)$");
    public RadialTimer radialTimer;
    private string saveFilePath;
    public GameObject panelPrefab;
    public GameObject timerPrefab;
    public Transform page1Content;
    public Transform page2Content;
    public AddSequence page1;
    public AddSequence page2;
    public int sequenceIndex;
    public List<TimerSequence> sequences;
    public PanelManager panelManager;
    private Coroutine countdownCoroutine;
    public int currentPlayIndex;
    public Button skipButton;
    public Button repeatButton;
    public Text timerNameNext;
    public GameObject skipMask;
    public GameObject repeatMask;
    public Button editButton1;
    public Button editButton2;
    public Button playButton;
    public ColorPercentageBar bar;
    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        if (File.Exists(saveFilePath))
        {
            //File.Delete(saveFilePath);
        }
        LoadData();
        nameTimePairs = new List<TimerData>();
    }

    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
            sequences = saveData.sequences;
            if (sequences.Count == 0)
            {
                editButton1.enabled = false;
                editButton1.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                editButton1.enabled = true;
                editButton1.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            }
            for (int i = 0; i < sequences.Count; i++)
            {
                //AddToList(sequences[i]);
                GameObject panel = Instantiate(panelPrefab, page1Content);
                panel.GetComponent<SwipeToDelete>().SetIndex(i);
                panel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = sequences[i].sequenceName;
                panel.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = ConvertTime(sequences[i].TotalTime());
            }
            page1.currentIndex = sequences.Count;
        } 
    }

    public void SaveData()
    {
        SaveData saveData = new SaveData();
        saveData.sequences = sequences;
        if (!page1.gameObject.activeSelf)
        {
            sequences[sequenceIndex].AddTimers(nameTimePairs);
        }
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(saveFilePath, json);
    }

    void OnApplicationQuit()
    {
        SaveData();
    } 

    public void AddToList(string name, int hour, int min, int sec, int color)
    {
        int time = 60 * 60 * hour + 60 * min + sec;
        nameTimePairs.Add(new TimerData(name, time, color));
    }
    
    public void AddToList(string name)
    {
        sequences.Add(new TimerSequence(name, new List<TimerData>()));
    }

    public void InsertToList(int index, int mode)
    {
        if (mode == 0)
        {
            TimerSequence newSequence = new TimerSequence(sequences[index].sequenceName, new List<TimerData>());
            for (int i = 0; i < sequences[index].timers.Count; i++)
            {
                newSequence.timers.Add(sequences[index].timers[i]);
            }
            sequences.Insert(index + 1, newSequence);
        }
        else
        {
            TimerData newData = new TimerData(nameTimePairs[index].name, nameTimePairs[index].time, nameTimePairs[index].colorIndex);
            nameTimePairs.Insert(index + 1, newData);
        }
    }

    public void RemovePairAtIndex(int index)
    {
        if (index >= 0 && index < nameTimePairs.Count)
        {
            nameTimePairs.RemoveAt(index);
        }
    }

    public void RemoveNameAtIndex(int index)
    {
        if (index >= 0 && index < sequences.Count)
        {
            sequences.RemoveAt(index);
        }
    }
    
    public void StartTimer()
    {
        if (!panelManager.CheckReady())
        {
            return;
        }
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            radialTimer.Reset();
        }
        EditMode mode = page2Content.GetComponent<EditMode>();
        if (mode.isEditMode)
        {
            mode.ToggleEditMode();
        }
        countdownCoroutine = StartCoroutine(StartCountdownSequence());
    }

    IEnumerator StartCountdownSequence()
    {
        currentPlayIndex = 0;
        while (currentPlayIndex < nameTimePairs.Count)
        {
            radialTimer.isSkipped = false;
            radialTimer.isRepeated = false;
            if (currentPlayIndex == nameTimePairs.Count - 1)
            {
                skipButton.enabled = false;
                skipMask.SetActive(true);
                timerNameNext.text = "Next: ";
            }
            else
            {
                skipButton.enabled = true;
                skipMask.SetActive(false);
                timerNameNext.text = "Next: " + nameTimePairs[currentPlayIndex + 1].name;
            }
            if (currentPlayIndex == 0)
            {
                repeatButton.enabled = false;
                repeatMask.SetActive(true);
            }
            else
            {
                repeatButton.enabled = true;
                repeatMask.SetActive(false);
            }
            var timer = nameTimePairs[currentPlayIndex];
            radialTimer.StartCountdown(timer.name, timer.time);
            yield return new WaitUntil(() => radialTimer.IsCountdownFinished || radialTimer.isSkipped || radialTimer.isRepeated);

            if (radialTimer.isSkipped)
            {
                currentPlayIndex++;
            }
            else if (radialTimer.isRepeated)
            {
                if (currentPlayIndex > 0)
                {
                    currentPlayIndex--;
                }
            }
            else
            {
                currentPlayIndex++;
            }
            
        }
        /*foreach (var timer in nameTimePairs)
        {
            radialTimer.StartCountdown(timer.name, timer.time);
            yield return new WaitUntil(() => radialTimer.IsCountdownFinished);
        }*/
    }

    public void Return()
    {
        if (!panelManager.CheckReady())
        {
            return;
        }
        page1.gameObject.SetActive(true);
        EditMode mode = page2Content.GetComponent<EditMode>();
        if (mode.isEditMode)
        {
            mode.ToggleEditMode();
        }
        page2.gameObject.SetActive(false);
        sequences[sequenceIndex].AddTimers(nameTimePairs);
        page1Content.GetChild(sequenceIndex).GetChild(1).GetChild(1).GetComponent<Text>().text = ConvertTime(sequences[sequenceIndex].TotalTime());
        nameTimePairs.Clear();
        for (int i = page2Content.childCount - 1; i >= 0; i--)
        {
            Destroy(page2Content.GetChild(i).gameObject);
        }
    }

    public void LoadTimerData(int index)
    {
        if (sequences[index].timers.Count == 0)
        {
            editButton2.enabled = false;
            editButton2.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            playButton.enabled = false;
            playButton.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            editButton2.enabled = true;
            editButton2.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            playButton.enabled = true;
            playButton.gameObject.transform.GetChild(3).gameObject.SetActive(false);
        }
        for (int i = 0; i < sequences[index].timers.Count; i++)
        {
            TimerData timerData = sequences[index].timers[i];
            string name = timerData.name;
            int time = timerData.time;
            int colorIndex = timerData.colorIndex;
            nameTimePairs.Add(new TimerData(name, time, colorIndex));
            GameObject timer = Instantiate(timerPrefab, page2Content);
            timer.GetComponent<SwipeToDelete>().SetIndex(i);
            timer.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = name;
            timer.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = ConvertTime(time);
            timer.GetComponent<DragHandler>().SetColor(colorIndex);
        }
        page2.currentIndex = sequences[index].timers.Count;
        bar.UpdateColorPercentages();
    }
    
    private string ConvertTime(int duration)
    {
        string hour = (duration / 3600).ToString();
        string min = (duration % 3600 / 60).ToString();
        string sec = (duration % 60).ToString();
        
        if (hour == "0" && min == "0")
            return sec + "s ";
        if (hour == "0")
            return min + "m " + sec + "s ";
        return hour + "h " + min + "m " + sec + "s ";
    }

    public int GetSmallestAvailableNumber(int index)
    {
        HashSet<int> usedNumbers = new HashSet<int>();
        Regex pattern;
        if (index == 1)
        {
            pattern = nameTimerPattern;
            foreach (var sequence in sequences)
            {
                Match match = pattern.Match(sequence.sequenceName);
                if (match.Success)
                {
                    int number = int.Parse(match.Groups[1].Value);
                    usedNumbers.Add(number);
                }
            }
        }
        else
        {
            pattern = namePattern;
            foreach (var timer in nameTimePairs)
            {
                Match match = pattern.Match(timer.name);
                if (match.Success)
                {
                    int number = int.Parse(match.Groups[1].Value);
                    usedNumbers.Add(number);
                }
            }
        }
        int smallestAvailableNumber = 1;
        while (usedNumbers.Contains(smallestAvailableNumber))
        {
            smallestAvailableNumber++;
        }

        return smallestAvailableNumber;
    }

    public void SwitchIndex(int[] indices, int mode)
    {
        if (mode == 0)
        {
            List<TimerSequence> temp = new List<TimerSequence>(indices.Length);
            for (int i = 0; i < indices.Length; i++)
            {
                TimerSequence newSequence = new TimerSequence(sequences[indices[i]].sequenceName, new List<TimerData>());
                for (int j = 0; j < sequences[indices[i]].timers.Count; j++)
                {
                    newSequence.timers.Add(sequences[indices[i]].timers[j]);
                }
                temp.Add(newSequence);
            }
            sequences = temp;
        }
        else
        {
            List<TimerData> temp = new List<TimerData>(indices.Length);
            for (int i = 0; i < indices.Length; i++)
            {
                TimerData newData = new TimerData(nameTimePairs[indices[i]].name, nameTimePairs[indices[i]].time, nameTimePairs[indices[i]].colorIndex);
                temp.Add(newData);
            }
            nameTimePairs = temp;
        }
    }


}
