using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static string Status,QRData;
    public Text t1,t3,t4,t5;
    public GameObject instruction, settings, about, menuSlider_image, scan_button, outputText_image, outputHyperlink_image, sun, im_target;
    public Toggle tg2,sun_tg;
    public static bool bt_en;

    public static GameObject s_im2, s_im3;
    public static bool someth_open = false;

    private void Start()
    {
        bt_en = false;
        int i= PlayerPrefs.GetInt("Slider",0);

        if (i == 1) {
            tg2.isOn = true;
        }
        if (i == 0)
        {
            tg2.isOn = false;
        }

        s_im2 = GameObject.Find("QRText");
        s_im3 = GameObject.Find("Hyperlink");

        outputHyperlink_image.SetActive(false);
        outputText_image.SetActive(false);
	
    }
    
    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Close();
        }

       // t1.text = "" + succ;
       // t2.text = "" + 1 / Time.deltaTime;
        t3.text = "" + VuforiaScanner.fps;
        t4.text = "" + QRData;
        t5.text = "" + QRData;

        if (tg2.isOn == true) {
            menuSlider_image.SetActive(false);
        }
        if (tg2.isOn == false) {
            menuSlider_image.SetActive(true);
        }
        if (sun_tg.isOn == true) {
            sun.transform.rotation = Quaternion.Euler(0, -im_target.transform.rotation.x, 0); 
        }
        if (sun_tg.isOn == false)
        {
           // sun.transform.rotation = Quaternion.Euler(50, -30, 0);
        }
        if (bt_en == true) {
            scan_button.SetActive(true);
        }
        if (bt_en == false)
        {
            scan_button.SetActive(false);
        }

    }

    public void Scan_Again() {
        VuforiaScanner.scan_again = true;
        bt_en = false;
    }

    public void Increase() {

        if (VuforiaScanner.fps < 30)
        {
            VuforiaScanner.fps += 1;
        }

    }

    public void Decrease() {

        if (VuforiaScanner.fps > 1)
        {
            VuforiaScanner.fps -= 1;
        }

    }
    
    public void Instruction() {
        instruction.SetActive(true);
        someth_open = true;
    }

    public void Settings() {
        settings.SetActive(true);
        someth_open = true;
    }

    public void About() {
        about.SetActive(true);
        someth_open = true;
    }

    public void Hyperlink()
    {
        Application.OpenURL(t5.text);
    }

    public static void Just_Text() {
        s_im2.SetActive(true);
        someth_open = true;
    }

    public static void Just_Hyperlink() {
        s_im3.SetActive(true);
        someth_open = true;
    }

    public void Close()
    {

        PlayerPrefs.SetInt("Scans", VuforiaScanner.fps);
        PlayerPrefs.SetInt("Slider",tg2.isOn? 1 : 0);
        instruction.SetActive(false);
        settings.SetActive(false);
        about.SetActive(false);
        outputHyperlink_image.SetActive(false);
        outputText_image.SetActive(false);
        QRData = null;
        MenuController.trig3 = true;
        someth_open = false;

    }

    public void Visit_Our_WtbSite() {
        Application.OpenURL("https://sites.google.com/view/pplosstudio");
    }

}
