using UnityEngine;
using UnityEngine.UI;

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
    public Button SoundButton;

    private bool isMuted;

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
        isMuted = DataPersistence.GetMute();

        SoundButton.GetComponentInChildren<Text>().text = isMuted ? "Sound Off" : "Sound On";
        SoundButton.onClick.AddListener(() =>
        {
            if (isMuted)
            {
                DataPersistence.SetMute(false);
                SoundButton.GetComponentInChildren<Text>().text = "Sound On";
                isMuted = false;
            }
            else
            {
                DataPersistence.SetMute(true);
                SoundButton.GetComponentInChildren<Text>().text = "Sound Off";
                isMuted = true;
            }
        });

        imgAgeGrp = DropdownAge.GetComponent<Image>();
        imgDifficulty = DropdownDifficulty.GetComponent<Image>();
        Debug.Log(DataPersistence.Settings.GetAgeGroup());
        
        if(DataPersistence.Settings.GetDifficulty() == Difficulty.EASY)
        {
            DropdownDifficulty.value = 0;
            imgDifficulty.sprite = easy;
        }
        else if(DataPersistence.Settings.GetDifficulty() == Difficulty.MEDIUM)
        {
            DropdownDifficulty.value = 1;
            imgDifficulty.sprite = medium;
        }
        else if (DataPersistence.Settings.GetDifficulty() == Difficulty.HARD)
        {
            DropdownDifficulty.value = 2;
            imgDifficulty.sprite = hard;
        }

        if (DataPersistence.Settings.GetAgeGroup() == AgeGroup._6TO8)
        {
            DropdownAge.value = 0;
            imgAgeGrp.sprite = grp1;
        }
        else if (DataPersistence.Settings.GetAgeGroup() == AgeGroup._9TO10)
        {
            DropdownAge.value = 1;
            imgAgeGrp.sprite = grp2;
        }
        else if (DataPersistence.Settings.GetAgeGroup() == AgeGroup._11TO12)
        {
            DropdownAge.value = 2;
            imgAgeGrp.sprite = grp3;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

}