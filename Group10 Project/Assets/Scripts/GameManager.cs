using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string id;
    public string ID
    {
        get
        {
            return id;
        }
    }
    private int[] scores;
    public int[] Scores{
        get
        {
            return scores;
        }
    }
    private int[] correctAnswers;
    public int[] CorrectAnswers
    {
        get
        {
            return correctAnswers;
        }
    }
    private int[] wrongAnswers;
    public int[] WrongAnswers
    {
        get
        {
            return wrongAnswers;
        }
    }
    private int[] optFeedback;
    public int[] OptFeedback
    {
        get
        {
            return optFeedback;
        }
    }
    private int[] goodFeedback;
    public int[] GoodFeedback
    {
        get
        {
            return goodFeedback;
        }
    }
    private int[] badFeedback;
    public int[] BadFeedback
    {
        get
        {
            return badFeedback;
        }
    }
    private int[] videoFeedback;
    public int[] VideoFeedback
    {
        get
        {
            return videoFeedback;
        }
    }
    private int[] textFeedback;
    public int[] TextFeedback
    {
        get
        {
            return textFeedback;
        }
    }
    private float[] times;
    public float[] Times
    {
        get
        {
            return times;
        }
    }
    private int[] attempts;
    public int[] Attempts
    {
        get
        {
            return attempts;
        }
    }

    private bool[] levelsLockstates;
    public bool[] LevelsLockstates
    {
        get
        {
            return levelsLockstates;
        }
    }

    private PlayerData playerData;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("There's no game manager");
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        //LoadData
        playerData = SaveSystem.LoadData();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(playerData == null || playerData.numOfFields<12)
        {
            SetPlayerData();
        }
        else
        {
            id = playerData.id;
            scores = playerData.scores;
            times = playerData.times;
            attempts = playerData.attempts;
            levelsLockstates = playerData.levelsLockstates;
            correctAnswers = playerData.correctAnswers;
            wrongAnswers = playerData.wrongAnswers;
            optFeedback = playerData.optFeedback;
            videoFeedback = playerData.videoFeedback;
            textFeedback = playerData.textFeedback;
            goodFeedback = playerData.goodFeedback;
            badFeedback = playerData.badFeedback;
        }
        
    }

    public void SetPlayerData()
    {
        id = "" + System.Guid.NewGuid();
        scores = new int[9];
        times = new float[9];
        attempts = new int[9];
        levelsLockstates = new bool[9];
        correctAnswers = new int[9];
        wrongAnswers = new int[9];
        optFeedback = new int[9];
        videoFeedback = new int[9];
        textFeedback = new int [9];
        goodFeedback = new int[9];
        badFeedback = new int[9];
        InitScenesLocker();  
        playerData = new PlayerData(this);    
    }

    public int GetScore(int idx)
    {
        return scores[idx];
    }

    public void UpdateScore(int score)
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        if (score > scores[idx])
        {
            scores[idx] = score;
        }
    }

    public float GetTime(int idx)
    {
        return times[idx];
    }

    public void UpdateTime()
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        times[idx] += (float)Time.timeSinceLevelLoad;
    }

    public float GetTotalTime()
    {
        float totalTime = 0;
        foreach(float t in times)
        {
            totalTime += t;
        }

        return totalTime;
    }

    public int GetAttempts(int idx)
    {
        return attempts[idx];
    }

    public void UpdateAttempts()
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        attempts[idx]++;
        CallUnlockNextLevel(idx);
    }

    public int GetCorrectAnswers(int idx)
    {
        return correctAnswers[idx];
    }

    public void UpdateCorrectAnswers(int answers)
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        correctAnswers[idx] = answers;
    }

    public int GetWrongAnswers(int idx)
    {
        return wrongAnswers[idx];
    }

    public void UpdateWrongAnswers(int answers)
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        wrongAnswers[idx] = answers;
    }

    public int GetOptFeedback(int idx)
    {
        return optFeedback[idx];
    }

    public void UpdateOptFeedback (int num)
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        optFeedback[idx] += num;
    }

    public int GetTextFeedback(int idx)
    {
        return textFeedback[idx];
    }

    public void UpdateTextFeedback(int num)
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        textFeedback[idx] += num;
    }
    public int GetVideoFeedback(int idx)
    {
        return videoFeedback[idx];
    }

    public void UpdateVideoFeedback(int num)
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        videoFeedback[idx] += num;
    }

    public int GetGoodFeedback(int idx)
    {
        return goodFeedback[idx];
    }

    public void UpdateGoodFeedback(int num)
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        goodFeedback[idx] = num;
    }

     public int GetBadFeedback(int idx)
    {
        return badFeedback[idx];
    }

    public void UpdateBadFeedback(int num)
    {
        int idx = SceneManager.GetActiveScene().buildIndex - 1;
        badFeedback[idx] = num;
    }

    private void InitScenesLocker()
    {
        SetLockState(0, true);
        for (int i = 1; i < levelsLockstates.Length; i++)
        {
            SetLockState(i, false);
        }
    }

    public bool GetLockState(int idx)
    {
        return levelsLockstates[idx];
    }

    public void SetLockState(int idx, bool isUnlock)
    {
        levelsLockstates[idx] = isUnlock;
    }

    public void CallUnlockNextLevel(int idx) //same as idx in score[], for level 1, idx is 0
    {
        if (idx < levelsLockstates.Length - 1)//goal 3 level 3 does not need to unlock next level, so levelsLockstates[8] do nothing
        {
            SetLockState(idx + 1, true);
        }else{
            ServerTalker.Instance.SendData();
        }
        SaveSystem.SaveData();
    }

}
