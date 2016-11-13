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
        string real_name;
        string data;
        string ticker;
        private Color color;

        public LogoInfo(string s, string t, string r)
        {
            color = Color.white;
            data = "";
            name = s;
            ticker = t;
            real_name = r;
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
    public Text googleText;
    public Text microsoftText;
    public Text facebookText;
    public Text oneandoneText;
    public Text nasdaq2Text;

    Dictionary<string, LogoInfo> logoDictionary;

    // Use this for initialization
    void Start()
    {
        updateTimer.Interval = 6000;
        updateTimer.Elapsed += OnTimedEvent;

        // Have the timer fire repeated events (true is the default)
        updateTimer.AutoReset = true;
        updateTimer.Enabled = true;
        //GOOGL
        logoDictionary = new Dictionary<string, LogoInfo>();
        logoDictionary.Add("symantec", new LogoInfo("symantec", "SYMC", "Symantec Corp."));        
        logoDictionary.Add("datto", new LogoInfo("datto", "private", "Datto Inc."));
        logoDictionary.Add("nasdaq", new LogoInfo("nasdaq", "NDAQ", "Nasdaq Inc."));
        logoDictionary.Add("nasdaq2", new LogoInfo("nasdaq2", "NDAQ", "Nasdaq Inc."));
        logoDictionary.Add("corsair", new LogoInfo("corsair", "private", "Corsair Components Inc."));
        logoDictionary.Add("comcast", new LogoInfo("comcast", "CMCSA", "Comcast Corp."));
        logoDictionary.Add("finra", new LogoInfo("finra", "private", "Finra Inc."));
        logoDictionary.Add("intuit", new LogoInfo("intuit", "INTU", "Intuit Inc."));

        logoDictionary.Add("google", new LogoInfo("google", "GOOGL", "Alphabet Inc."));
        logoDictionary.Add("microsoft", new LogoInfo("microsoft", "MSFT", "Microsoft Inc."));
        logoDictionary.Add("facebook", new LogoInfo("facebook", "FB", "Facebook Inc."));
        logoDictionary.Add("1and1", new LogoInfo("1and1", "private", "1&1 Inc."));
    }
    
    //updates the text of the companies. currently only symantec
    private void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        //that hardcode lmao
        Debug.Log("Setting data!");
        logoDictionary["symantec"].setData(logoUpdate(logoDictionary["symantec"].getTicker(), "symantec", "Symantec Corp"));
        logoDictionary["datto"].setData(logoUpdate(logoDictionary["datto"].getTicker(), "datto", "Datto Inc"));
        logoDictionary["nasdaq"].setData(logoUpdate(logoDictionary["nasdaq"].getTicker(), "nasdaq", "Nasdaq Inc"));
        logoDictionary["nasdaq2"].setData(logoUpdate(logoDictionary["nasdaq2"].getTicker(), "nasdaq2", "Nasdaq Inc"));
        logoDictionary["corsair"].setData(logoUpdate(logoDictionary["corsair"].getTicker(), "corsair", "Corsair Components Inc"));
        logoDictionary["comcast"].setData(logoUpdate(logoDictionary["comcast"].getTicker(), "comcast", "Comcast Corp"));
        logoDictionary["finra"].setData(logoUpdate(logoDictionary["finra"].getTicker(), "finra", "Finra Inc"));
        logoDictionary["intuit"].setData(logoUpdate(logoDictionary["intuit"].getTicker(), "intuit", "Intuit Inc"));
        logoDictionary["google"].setData(logoUpdate(logoDictionary["google"].getTicker(), "google", "Alphabet Inc."));
        logoDictionary["microsoft"].setData(logoUpdate(logoDictionary["microsoft"].getTicker(), "microsoft", "Microsoft Inc."));
        logoDictionary["facebook"].setData(logoUpdate(logoDictionary["facebook"].getTicker(), "facebook", "Facebook Inc."));
        logoDictionary["1and1"].setData(logoUpdate(logoDictionary["1and1"].getTicker(), "1and1", "1&1 Inc."));

        //  symantecText.text = logoDictionary["symantec"].getData();

        // Debug.Log(ticker + " is the ticker");

    }
    
    //this function refreshes the data stored in logoDictionary
    private string logoUpdate(string ticker, string name, string real)
    {
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
        Debug.Log("Get request: " + result);
        //split it before return

        /*
        String[] splitResults = result.Split(new Char[] { ' ' });
        String color = splitResults[0];

        if (color.Equals("G"))
        {
            logoDictionary[name].setColor(Color.green);
        }
        else
        {
            logoDictionary[name].setColor(Color.red);
        
        }
        String percentString = real + ": " + splitResults[1]+"\n";
        String openString = "Open: " + splitResults[2]+"\n";
        String closeString = "Close: " + splitResults[3]+"\n";
        String volumeString = "Volume: " + splitResults[4] + "\n";
        */
        // return percentString + openString + closeString + volumeString;

        return "testing";
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
            Debug.Log("Trackable: " + tb.TrackableName);
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
            dattoText.color = logoDictionary["datto"].getColor();

        }

        nasdaqText.text = getNewText("nasdaq", currentVisible);
        if (currentVisible.Contains("nasdaq"))
        {
            nasdaqText.color = logoDictionary["nasdaq"].getColor();

        }
        nasdaq2Text.text = getNewText("nasdaq2", currentVisible);
        if (currentVisible.Contains("nasdaq2"))
        {
            nasdaq2Text.color = logoDictionary["nasdaq2"].getColor();

        }
        corsairText.text = getNewText("corsair", currentVisible);
        if (currentVisible.Contains("corsair"))
        {
            corsairText.color = logoDictionary["corsair"].getColor();

        }
        comcastText.text = getNewText("comcast", currentVisible);
        if (currentVisible.Contains("comcast"))
        {
            comcastText.color = logoDictionary["comcast"].getColor();

        }
        // vitechText.text = getNewText("vitech", currentVisible);
        finraText.text = getNewText("finra", currentVisible);
        if (currentVisible.Contains("finra"))
        {
            finraText.color = logoDictionary["finra"].getColor();

        }
        intuitText.text = getNewText("intuit", currentVisible);
        if (currentVisible.Contains("intuit"))
        {
            intuitText.color = logoDictionary["intuit"].getColor();

        }
        googleText.text = getNewText("google", currentVisible);
        if (currentVisible.Contains("google"))
        {
            googleText.color = logoDictionary["google"].getColor();

        }
        microsoftText.text = getNewText("microsoft", currentVisible);
        if (currentVisible.Contains("microsoft"))
        {
            microsoftText.color = logoDictionary["microsoft"].getColor();

        }
        facebookText.text = getNewText("facebook", currentVisible);
        if (currentVisible.Contains("facebook"))
        {
            facebookText.color = logoDictionary["facebook"].getColor();

        }
        oneandoneText.text = getNewText("1and1", currentVisible);
        if (currentVisible.Contains("1and1"))
        {
            oneandoneText.color = logoDictionary["1and1"].getColor();

        }
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
