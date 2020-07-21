using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SaveDropDownValue : MonoBehaviour
{
    

    Dropdown DropdownAge;
    string m_MyString;
    int m_Index;

    const string PrefName = "Age";

    void Awake()
    {
        DropdownAge = transform.GetComponent<Dropdown>();
        DropdownAge.onValueChanged.AddListener(delegate
        {
            PlayerPrefs.SetInt(PrefName, DropdownAge.value);
            PlayerPrefs.Save();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        DropdownAge.onValueChanged.AddListener(delegate
        {
            PlayerPrefs.SetInt(PrefName, DropdownAge.value);
            PlayerPrefs.Save();
        });


        DropdownAge.value = PlayerPrefs.GetInt(PrefName, 0);

    }

    // Update is called once per frame
    void Update()
    {

    }
    /*
     //Fetch the Dropdown GameObject the script is attached to
        DropdownAge = transform.GetComponent<Dropdown>();
        //Clear the old options of the Dropdown menu
        DropdownAge.ClearOptions();



     //Dropdown.OptionData m_NewData, m_NewData2, m_NewData3;
    //List<Dropdown.OptionData> m_Messages = new List<Dropdown.OptionData>();
     
            
        m_NewData = new Dropdown.OptionData();
        m_NewData.text = "6 - 8";
        m_Messages.Add(m_NewData);

        //Create a new option for the Dropdown menu which reads "Option 2" and add to messages List
        m_NewData2 = new Dropdown.OptionData();
        m_NewData2.text = "9 - 10";
        m_Messages.Add(m_NewData2);

        //Create a new option for the Dropdown menu which reads "Option 3" and add to messages List
        m_NewData3 = new Dropdown.OptionData();
        m_NewData3.text = "11 - 12";
        m_Messages.Add(m_NewData2);
        

        //Take each entry in the message List
        foreach (Dropdown.OptionData message in m_Messages)
        {
            //Add each entry to the Dropdown
            DropdownAge.options.Add(message);
            //Make the index equal to the total number of entries
            m_Index = m_Messages.Count - 1;
        }
     
     */
}