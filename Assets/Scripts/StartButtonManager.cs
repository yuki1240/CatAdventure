﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonManager : MonoBehaviour
{
 
    public void startButtonClick()
    {
        SceneManager.LoadScene("Main");
    }
}
