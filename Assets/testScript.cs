using CI.WSANative.Pickers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testScript : MonoBehaviour {

    //Make sure to attach these Buttons in the Inspector
    public Button m_YourFirstButton;

    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        m_YourFirstButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");
        WSANativeFilePicker.PickSingleFile("Select", WSAPickerViewMode.Thumbnail, WSAPickerLocationId.PicturesLibrary, new[] { ".png", ".jpg" }, result =>
        {
            if (result != null)
            {
                byte[] fileBytes = result.ReadBytes();
                string fileString = result.ReadText();
            }
        });
    }
}
