using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    TMP_InputField obj;

    [SerializeField] TMP_InputField RR;
    [SerializeField] TMP_InputField TS;
    [SerializeField] TMP_InputField TH;
    [SerializeField] TMP_InputField S;
    [SerializeField] TMP_InputField MinO;
    [SerializeField] TMP_InputField MaxO;
    [SerializeField] TMP_InputField PA;
    [SerializeField] TMP_InputField PR;
    [SerializeField] TMP_InputField PO;

    void Start()
    {
        RR.text = PlayerPrefs.GetInt("renderRadius", 5).ToString();
        TS.text = PlayerPrefs.GetInt("terrainSize", 1).ToString();
        TH.text = PlayerPrefs.GetInt("terrainHeight", 5).ToString();
        S.text = PlayerPrefs.GetInt("seed", 5).ToString();
        MinO.text = PlayerPrefs.GetInt("minimunObjects", 1).ToString();
        MaxO.text = PlayerPrefs.GetInt("maximunObjects", 2).ToString();
        PA.text = PlayerPrefs.GetInt("percentTrees",65).ToString();
        PR.text = PlayerPrefs.GetInt("percentRocks",30).ToString();
        PO.text = PlayerPrefs.GetInt("percentOthers",5).ToString();
        //Debug.Log(RR.text);
    }


    public void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quitted");
    }

    void SetPreferences(string field, int number)
    {
        PlayerPrefs.SetInt(field, number);        
        ValidateValue(field, number, obj);
        Debug.Log(field + " is " + PlayerPrefs.GetInt(field, 5));
    }

    void ValidateValue(string field, int number, TMP_InputField holder)
    {
        // For Render Radius
        if(field == "renderRadius"){
            if(PlayerPrefs.GetInt(field, 5) < 5){
            PlayerPrefs.SetInt(field, 5);
            holder.text = "5";
            }
            return;
        }
        // For Terrain Size
        if(field == "terrainSize"){
            if(PlayerPrefs.GetInt(field, 1) < 1){
            PlayerPrefs.SetInt(field, 1);
            holder.text = "1";
            }
            return;
        }
        // For Terrain Height
        if(field == "terrainHeight"){
            if(PlayerPrefs.GetInt(field, 5) < 5){
            PlayerPrefs.SetInt(field, 5);
            holder.text = "5";
            }
            return;
        }
        // For Seed
        if(field == "seed"){
            if(PlayerPrefs.GetInt(field, 1) < 1){
            PlayerPrefs.SetInt(field, 1);
            holder.text = "1";
            }
            return;
        }
        // For Minimum Objects
        if(field == "minimunObjects"){
            if(PlayerPrefs.GetInt(field, 1) < 1){
            PlayerPrefs.SetInt(field, 1);
            holder.text = "1";
            }
            return;
        }
        // For Maximum Objects
        if(field == "maximunObjects"){
            if(PlayerPrefs.GetInt(field, 2) < 2){
            PlayerPrefs.SetInt(field, 2);
            holder.text = "2";
            }
            return;
        }
        // For Trees Percent
        if(field == "percentTrees"){
            if(PlayerPrefs.GetInt(field, 0) < 0){
            PlayerPrefs.SetInt(field, 0);
            holder.text = "0";
            }
            return;
        }
        // For Rocks Percent
        if(field == "percentRocks"){
            if(PlayerPrefs.GetInt(field, 0) < 0){
            PlayerPrefs.SetInt(field, 0);
            holder.text = "0";
            }
            return;
        }
        // For Others Percent
        if(field == "percentOthers"){
            if(PlayerPrefs.GetInt(field, 0) < 0){
            PlayerPrefs.SetInt(field, 0);
            holder.text = "0";
            }
            return;
        }        
    }

    public void RenderRadius()
    {
        GameObject currentObject = GameObject.Find("RR");
        TMP_InputField holder = currentObject.GetComponent<TMP_InputField>();
        //Debug.Log(holder);
        int holder2 =  int.Parse(holder.text);
        obj = holder;
        SetPreferences("renderRadius",holder2);
    }

    public void TerrainSize()
    {
        GameObject currentObject = GameObject.Find("TS");
        TMP_InputField holder = currentObject.GetComponent<TMP_InputField>();
        //Debug.Log(holder);
        int holder2 =  int.Parse(holder.text);
        obj = holder;
        SetPreferences("terrainSize",holder2);
    }

    public void TerrainHeight()
    {
        GameObject currentObject = GameObject.Find("TH");
        TMP_InputField holder = currentObject.GetComponent<TMP_InputField>();
        //Debug.Log(holder);
        int holder2 =  int.Parse(holder.text);
        obj = holder;
        SetPreferences("terrainHeight",holder2);
    }

    public void Seed()
    {
        GameObject currentObject = GameObject.Find("S");
        TMP_InputField holder = currentObject.GetComponent<TMP_InputField>();
        //Debug.Log(holder);
        int holder2 =  int.Parse(holder.text);
        obj = holder;
        SetPreferences("seed",holder2);
    }

    public void MinObjects()
    {
        GameObject currentObject = GameObject.Find("Min O");
        TMP_InputField holder = currentObject.GetComponent<TMP_InputField>();
        //Debug.Log(holder);
        int holder2 =  int.Parse(holder.text);
        obj = holder;
        SetPreferences("minimunObjects",holder2);
    }

    public void MaxObjects()
    {
        GameObject currentObject = GameObject.Find("Max O");
        TMP_InputField holder = currentObject.GetComponent<TMP_InputField>();
        //Debug.Log(holder);
        int holder2 =  int.Parse(holder.text);
        obj = holder;
        SetPreferences("maximunObjects",holder2);
    }

    public void PercentTrees()
    {
        GameObject currentObject = GameObject.Find("PA");
        TMP_InputField holder = currentObject.GetComponent<TMP_InputField>();
        //Debug.Log(holder);
        int holder2 =  int.Parse(holder.text);
        obj = holder;
        SetPreferences("percentTrees",holder2);
    }

    public void PercentRocks()
    {
        GameObject currentObject = GameObject.Find("PR");
        TMP_InputField holder = currentObject.GetComponent<TMP_InputField>();
        //Debug.Log(holder);
        int holder2 =  int.Parse(holder.text);
        obj = holder;
        SetPreferences("percentRocks",holder2);
    }

    public void PercentOthers()
    {
        GameObject currentObject = GameObject.Find("PO");
        TMP_InputField holder = currentObject.GetComponent<TMP_InputField>();
        //Debug.Log(holder);
        int holder2 =  int.Parse(holder.text);
        obj = holder;
        SetPreferences("percentOthers",holder2);
    }
}
