﻿using System;
using System.Collections;
using UnityEngine;
using System.IO;

public sealed class Logger : MonoBehaviour
{
    //Singleton approach for ensuring only one instance of the Logger class
    private static volatile Logger instance;
    private static object syncRoot = new System.Object();

    private int session_id = -1;
    private int user_id = -1;
    private const string FILEPATH = "./";
    private const string FILENAME = "loggingQueue.txt";

    private Logger() { }

    public static Logger Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        //instance = new Logger();
                        if (GameObject.FindGameObjectWithTag("Logger") == null) {
                            Debug.Log("Logger GameObject is null");
                        }
                        GameObject.Find("Logger").AddComponent<Logger>();
                        instance = GameObject.Find("Logger").GetComponent<Logger>();
                    }
                }
            }
            return instance;
        }
    }

    private void OfflineSaveLog(string latestUrl)
    {
        writeIntoFile(FILENAME,latestUrl);
        string[] urlQueue = File.ReadAllLines(@FILEPATH+"\\"+FILENAME);;
        for(int i=0; i<urlQueue.Length; i++)
        {
            StartCoroutine(TryUrl(urlQueue,i));
            if (urlQueue[i] == null)
            {
                rewriteUrlQueue(urlQueue);
                break;
            }
        }

    }

    private void rewriteUrlQueue(string[] urlQueue)
    {
        File.Delete(@FILEPATH + "\\" + FILENAME);
        foreach (String url in urlQueue)
        {
            if (url != null)
            {
                writeIntoFile(FILENAME,url);
            }
        }
    }

    private IEnumerator TryUrl(string[] urlQueue, int indexInQueue)
    {
        WWW db = new WWW(urlQueue[indexInQueue]);
        yield return db;

        PhpOutputHandler handler = new PhpOutputHandler(db);

        if (handler.Success())
        {
            urlQueue[indexInQueue] = null;
        }
    }

    public void StartSession()
    {
        if (user_id != -1)
            StartCoroutine(StartSessionWorker());
        else
            Debug.Log("user_id is not set");
    }

    private IEnumerator StartSessionWorker()
    {
        // Prepare url with ref to the session start script and the given parameters
        string url = RegisterScript.SERVER + "start_session.php?" + "user_id=" + user_id;
        Debug.Log(url);
        WWW db = new WWW(url);

        yield return db;

        Debug.Log("START SESSION HANDLER");
        PhpOutputHandler handler = new PhpOutputHandler(db,true);

        if (handler.Success() && handler.GetOutput().ContainsKey("SESSION_ID"))
        {
            session_id = Int32.Parse(handler.GetOutput()["SESSION_ID"]);
        }
        else
        {
            Debug.Log("Session Start could not be Logged although the connection to the database has been established.");
        }
    }

    public void EndSession()
    {
        Debug.Log("EndSession");
        StartCoroutine(EndSessionWorker());
    }

    private IEnumerator EndSessionWorker()
    {
        string url = RegisterScript.SERVER + "end_session.php?system_log_id=" + session_id;

        WWW db = new WWW(url);

        yield return db;

        Debug.Log("END SESSION HANDLER");
        PhpOutputHandler handler = new PhpOutputHandler(db, true);

        if (handler.Success())
        {
            Debug.Log("Session has been closed successfully");
        }
        else
        {
            Debug.Log("Session End could not be Logged although the connection to the database has been established.");
        }
    }

    private string getTimestamp()
    {
        return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
    }

    private void writeIntoFile(string fileName, string content)
    {
        if (!File.Exists(FILEPATH + fileName))
        {
            File.Create(FILEPATH + fileName).Close();
        }
        File.AppendAllText(FILEPATH + fileName, content+"\n");
    }

    public void Log(Action action)
    {
        string url;
        switch (action)
        {
            case Action.CHOOSE_GAME:
                url = "miau";               
                break;
            case Action.SHOW_STATISTICS:
                url = "http://localhost/unity_games/end_session.php?" + "system_action_log_id=" + 2;
                break;
            default: url = "";
                break;
        }
        OfflineSaveLog(url);
    }

    public void SetUserID(int user_id)
    {
        this.user_id = user_id;
    }

    public int GetUserID()
    {
        return this.user_id;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
