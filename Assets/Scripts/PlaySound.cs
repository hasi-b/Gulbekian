using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void PlaySuccessSound(){

        SoundManager.Instance.PlaySuccessSound();
    }

        public void PlayFailureSound(){

        SoundManager.Instance.PlayFailureSound();
    }    public void PlayPhotoClickSound(){

        SoundManager.Instance.PlayPhotoClickSound();
    }    public void PlayPhotoSubmitSound(){

        SoundManager.Instance.PlayPhotoSubmitSound();
    }    public void PlayUIButtonSound(){

        SoundManager.Instance.PlayUIButtonSound();
    }
}
