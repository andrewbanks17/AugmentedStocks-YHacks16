using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.IO;
using System;
using System.Timers;

public class TrackableList : MonoBehaviour
{

    //global timer to update data
    private static System.Timers.Timer updateTimer = new System.Timers.Timer();

    protected class LogoInfo
    {
        private bool visible = false;
        string name;
        string data;
        string ticker;
        private Color color;

        public LogoInfo(string s, string t)
        {
            color = Color.white;
            data = "";
            name = s;
            ticker = t;
        }

        public Color getColor()
        {
            return color;
        }
        public void setColor(Color c)
        {
            color = c;
        }
        public string getTicker()
        {
            return ticker;
        }
        public bool isVisible()
        {
            return visible;

        }
        public void setVisible(bool b)
        {
            visible = b;
        }
        public string getData()
        {
            return data;

        }
        public void setData(String s)
        {
            data = s;
        }

    }


    //close, open, volume
    public Text symantecText;
    public Text dattoText;
    public Text nasdaqText;
    public Text corsairText;
    public Text comcastText;
    public Text vitechText;
    public Text finraText;
    public Text intuitText;

    Dictionary<string, LogoInfo> logoDictionary;

    // Use this for initialization
    void Start()
    {
        updateTimer.Interval = 6000;
        updateTimer.Elapsed += OnTimedEvent;

        // Have the timer fire repeated events (true is the default)
        updateTimer.AutoReset = true;
        updateTimer.Enabled = true;

        logoDictionary = new Dictionary<string, LogoInfo>();
        logoDictionary.Add("symantec", new LogoInfo("symantec", "SYMC"));        
        logoDictionary.Add("datto", new LogoInfo("datto", "private"));

        /*
        logoDictionary.Add("nasdaq", new LogoInfo("nasdaq", "NDAQ"));
        logoDictionary.Add("corsair", new LogoInfo("corsair", "private"));
        logoDictionary.Add("comcast", new LogoInfo("comcast", "CMCSA"));
        logoDictionary.Add("vitech", new LogoInfo("vitech", "private"));
        logoDictionary.Add("finra", new LogoInfo("finra", "private"));
        logoDictionary.Add("intuit", new LogoInfo("intuit", "INTU"));
        */
    }

    //updates the text of the companies. currently only symantec
    private void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        Debug.Log("Setting data!");
        logoDictionary["symantec"].setData(logoUpdate(logoDictionary["symantec"].getTicker(), "symantec"));
        logoDictionary["datto"].setData(logoUpdate(logoDictionary["datto"].getTicker(), "datto"));
        /*
        logoDictionary["nasdaq"].setData(logoUpdate(logoDictionary["nasdaq"].getTicker(), nasdaqText));
        logoDictionary["corsair"].setData(logoUpdate(logoDictionary["corsair"].getTicker(), corsairText));
        logoDictionary["comcast"].setData(logoUpdate(logoDictionary["comcast"].getTicker(), comcastText));
        logoDictionary["vitech"].setData(logoUpdate(logoDictionary["vitech"].getTicker(), vitechText));
        logoDictionary["finra"].setData(logoUpdate(logoDictionary["finra"].getTicker(), finraText));
        logoDictionary["intuit"].setData(logoUpdate(logoDictionary["intuit"].getTicker(), intuitText));
        */

        //  symantecText.text = logoDictionary["symantec"].getData();

        // Debug.Log(ticker + " is the ticker");

    }

    //im assuming pass by reference I could make this void, but that is bad practice
    //this function refreshes the data stored in logoDictionary
    private string logoUpdate(string ticker, string name)
    {
        Debug.Log("Called with ticker:" + ticker);
        if (ticker.Equals("private"))
        {
            return "private";
        }

        //create a helper method and call it with each company name
        WebClient client = new WebClient();
        // System.Uri myUri = new System.Uri("sa4xs2xa.us-02.live-paas.net/?ticker=" + ticker + "&volume&open&close");
        //  string reply = client.DownloadString(myUri);

        string result = null;
        string url = "http://sa4xs2xa.us-02.live-paas.net/?ticker=" + ticker;
        WebResponse response = null;
        StreamReader reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
            result = reader.ReadToEnd();
        }
        catch (System.Exception ex)
        {
            // handle error
            //MessageBox.Show(ex.Message);
            Debug.Log("Youve been hit by the bug of Spo0k. like this error in 5 seconds or...\n" + ex.Message);

        }
        finally
        {
            if (reader != null)
                reader.Close();
            if (response != null)
                response.Close();
        }

        //split it before return

        String[] splitResults = result.Split(new Char[] { ' ' });
        String color = splitResults[0];
        if (color.Equals("G"))
        {
            logoDictionary["symantec"].setColor(Color.green);
        }
        else
        {
            logoDictionary["symantec"].setColor(Color.red);
        
        }
        String percentString = "Percent: " + splitResults[1]+"\n";
        String openString = "Open: " + splitResults[2]+"\n";
        String closeString = "Close: " + splitResults[3]+"\n";
        String volumeString = "Volume: " + splitResults[4] + "\n";
        
        return percentString + openString + closeString + volumeString;
        
    }

    //refreshes text visible on the screen so that it follows the object
    void Update()
    {
        // Get the Vuforia StateManager
        StateManager sm = TrackerManager.Instance.GetStateManager();

        // Query the StateManager to retrieve the list of
        // currently 'active' trackables 
        //(i.e. the ones currently being tracked by Vuforia)
        IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours();

        // Iterate through the list of active trackables
        //ayy lmao
        // Debug.Log("List of trackables currently active (tracked): ");

        HashSet<String> currentVisible = new HashSet<string>();
        foreach (TrackableBehaviour tb in activeTrackables)
        {


            logoDictionary[tb.TrackableName].setVisible(true);
            currentVisible.Add(tb.TrackableName);
            // if (logoDictionary.ContainsKey(tb.TrackableName))
            //   {

            //   }
            //   Debug.Log("Trackable: " + tb.TrackableName);
            //currentActive[counter] = tb.TrackableName;
            //   if (tb.TrackableName.Equals("symantec"))
            //  {
            //      symantecDetected = true;
            //  }
        }


        symantecText.text = getNewText("symantec", currentVisible);
        if (currentVisible.Contains("symantec"))
        {
            symantecText.color = logoDictionary["symantec"].getColor();

        }
        dattoText.text = getNewText("datto", currentVisible);
        if (currentVisible.Contains("datto"))
        {
            symantecText.color = logoDictionary["symantec"].getColor();

        }
        /*
        nasdaqText.text = getNewText("nasdaq", currentVisible);
        corsairText.text = getNewText("corsair", currentVisible);
        comcastText.text = getNewText("comcast", currentVisible);
        vitechText.text = getNewText("vitech", currentVisible);
        finraText.text = getNewText("finra", currentVisible);
        intuitText.text = getNewText("intuit", currentVisible);
        */

        //if (currentVisible.Contains("symantec"))
        //{
        //    symantecText.text = logoDictionary["symantec"].getData();
        //}
        //else
        //{
        //    symantecText.text = "";
        //}

        //        if (!logoDictionary["symantec"].isVisible())
        //       {
        //          symantecText.text = "";
        //     }

        //   if (symantecDetected)
        //     {
        //     symantecText.text = logoDictionary["symantec"].getData();

        //  Debug.Log(ticker + " is the ticker");

        //  Debug.Log(result);

        //response.Close();
        //    }
        //     else
        //     {
        //          symantecText.text = "";
        //    }

    }

    private string getNewText(String s, HashSet<String> currentVisible)
    {
        if (currentVisible.Contains(s))
        {
            return logoDictionary[s].getData();
        }
        else
        {
            return "";
        }
    }

}
