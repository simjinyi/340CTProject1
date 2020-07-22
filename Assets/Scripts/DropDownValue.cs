using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//[RequireComponent(typeof(DropdownAge))]
public class DropDownValue : MonoBehaviour
{
    public Dropdown DropdownAge;
    public Dropdown DropdownDifficulty;
    public Image imgAgeGrp;
    public Image imgDifficulty;
    public Sprite grp1;
    public Sprite grp2;
    public Sprite grp3;
    public Sprite easy;
    public Sprite medium;
    public Sprite hard;

    //string m_MyString;
    //int m_Index;
    //const string PrefName = "Age";

    void Awake()
    {
        imgAgeGrp = DropdownAge.GetComponent<Image>();
        imgDifficulty = DropdownDifficulty.GetComponent<Image>();
        DropdownAge.onValueChanged.AddListener(delegate
        {
            switch (DropdownAge.value)
            {
                case 0:
                    DataPersistence.Settings.SetAgeGroup(AgeGroup._6TO8);
                    imgAgeGrp.sprite = grp1;
                    break;
                case 1:
                    DataPersistence.Settings.SetAgeGroup(AgeGroup._9TO10);
                    imgAgeGrp.sprite = grp2;
                    break;
                case 2:
                    DataPersistence.Settings.SetAgeGroup(AgeGroup._11TO12);
                    imgAgeGrp.sprite = grp3;
                    break;
                default:
                    break;
            }
        });

        DropdownDifficulty.onValueChanged.AddListener(delegate
        {
            switch (DropdownDifficulty.value)
            {
                case 0:
                    DataPersistence.Settings.SetDifficulty(Difficulty.EASY);
                    imgDifficulty.sprite = easy;
                    break;
                case 1:
                    DataPersistence.Settings.SetDifficulty(Difficulty.MEDIUM);
                    imgDifficulty.sprite = medium;
                    break;
                case 2:
                    DataPersistence.Settings.SetDifficulty(Difficulty.HARD);
                    imgDifficulty.sprite = hard;
                    break;
                default:
                    break;
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        imgAgeGrp = DropdownAge.GetComponent<Image>();
        imgDifficulty = DropdownDifficulty.GetComponent<Image>();
        Debug.Log(DataPersistence.Settings.GetAgeGroup());
        
        if(DataPersistence.Settings.GetDifficulty() == Difficulty.EASY)
        {
            imgDifficulty.sprite = easy;
        }
        else if(DataPersistence.Settings.GetDifficulty() == Difficulty.MEDIUM)
        {
            imgDifficulty.sprite = medium;
        }
        else if (DataPersistence.Settings.GetDifficulty() == Difficulty.HARD)
        {
            imgDifficulty.sprite = hard;
        }

        if (DataPersistence.Settings.GetAgeGroup() == AgeGroup._6TO8)
        {
            imgAgeGrp.sprite = grp1;
        }
        else if (DataPersistence.Settings.GetAgeGroup() == AgeGroup._9TO10)
        {
            imgAgeGrp.sprite = grp2;
        }
        else if (DataPersistence.Settings.GetAgeGroup() == AgeGroup._11TO12)
        {
            imgAgeGrp.sprite = grp3;
        }

    }

    // Update is called once per frame
    void Update()
    {

        //DropdownDifficulty = transform.GetComponent<Dropdown>();
        //DropdownDifficulty.onValueChanged.AddListener(delegate
        //{
        //    switch (DropdownAge.value)
        //    {
        //        case 0:
        //            DataPersistence.Settings.SetDifficulty(Difficulty.EASY);
        //            break;
        //        case 1:
        //            DataPersistence.Settings.SetDifficulty(Difficulty.MEDIUM);
        //            break;
        //        case 2:
        //            DataPersistence.Settings.SetDifficulty(Difficulty.HARD);
        //            break;
        //        default:
        //            break;
        //    }
        //});
    }

    /*
    public static void onAgeGroupValueChanged()
    {
        
    }

    public static void onDifficultlyValueChanged()
    {
        
    }*/


    /*
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