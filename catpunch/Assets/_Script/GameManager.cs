using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [Header("Option")]
    public GameObject GO_OptionPanel; //�ɼ��г�
    public AudioMixer masterMixer; //����� �ͼ�
    public Slider BgmSlider; //�ɼ� Bgm �����̴�
    public Slider SfxSlider; //�ɼ� Sfx �����̴�
    private static float BgmValue; //Bgm �����̴� value�� �ӽ� ����
    private static float SfxValue; //Sfx �����̴� value�� �ӽ� ����

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OptionOpen() //�ɼ� ��ư Ŭ����
    {
        GO_OptionPanel.SetActive(true); //�ɼ��г� ����
        Time.timeScale = 0f; //�Ͻ�����
    }

    public void OptionClose() //�ɼ� �г� ���� ��ư Ŭ����
    {
        GO_OptionPanel.SetActive(false); //�ɼ��г� ����
        Time.timeScale = 1f; //���
    }

    public void BgmAudioControl() // Bgm �����̴� Volume ����
    {
        float sound = BgmSlider.value;
        BgmValue = BgmSlider.value; // �����̴� �� ����

        if (sound == -40f) masterMixer.SetFloat("BGM", -80); // value�� -40�� ��� -80���� ���� ���Ұ�
        if (sound == -40f) masterMixer.SetFloat("Title", -80); // value�� -40�� ��� -80���� ���� ���Ұ�
        else masterMixer.SetFloat("BGM", sound);
    }

    public void SfxAudioControl() // Effect �����̴� Volume ����
    {
        float sound = SfxSlider.value;
        SfxValue = SfxSlider.value;

        if (sound == -40f) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", sound);
    }
}
