using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [Header("Option")]
    public GameObject GO_OptionPanel; //옵션패널
    public AudioMixer masterMixer; //오디오 믹서
    public Slider BgmSlider; //옵션 Bgm 슬라이더
    public Slider SfxSlider; //옵션 Sfx 슬라이더
    private static float BgmValue; //Bgm 슬라이더 value값 임시 저장
    private static float SfxValue; //Sfx 슬라이더 value값 임시 저장

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OptionOpen() //옵션 버튼 클릭시
    {
        GO_OptionPanel.SetActive(true); //옵션패널 켜짐
        Time.timeScale = 0f; //일시정지
    }

    public void OptionClose() //옵션 패널 종료 버튼 클릭시
    {
        GO_OptionPanel.SetActive(false); //옵션패널 꺼짐
        Time.timeScale = 1f; //재생
    }

    public void BgmAudioControl() // Bgm 슬라이더 Volume 조절
    {
        float sound = BgmSlider.value;
        BgmValue = BgmSlider.value; // 슬라이더 값 저장

        if (sound == -40f) masterMixer.SetFloat("BGM", -80); // value가 -40일 경우 -80으로 만들어서 음소거
        if (sound == -40f) masterMixer.SetFloat("Title", -80); // value가 -40일 경우 -80으로 만들어서 음소거
        else masterMixer.SetFloat("BGM", sound);
    }

    public void SfxAudioControl() // Effect 슬라이더 Volume 조절
    {
        float sound = SfxSlider.value;
        SfxValue = SfxSlider.value;

        if (sound == -40f) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", sound);
    }
}
