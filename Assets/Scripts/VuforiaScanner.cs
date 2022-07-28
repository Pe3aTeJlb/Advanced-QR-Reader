using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Vuforia;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using UnityEngine.Networking;


[AddComponentMenu("System/VuforiaScanner")]
public class VuforiaScanner : MonoBehaviour
{
    private bool cameraInitialized;
    private QRCodeReader QRReader;
    public static string Data;

    public GameObject OBJ, OBJ2, OBJ3, ob1, text;
    public GameObject cam, target;
    public static bool TouchControl, MenuOpened;

    public static int fps;
    bool isFirstSwitch;
    public static bool scan_again;
    public bool IsModel;
    public int count;

    public string test;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Application.targetFrameRate = 30;
        DontDestroyOnLoad(cam);
        QRReader = new QRCodeReader();
        StartCoroutine(InitializeCamera());

        fps = PlayerPrefs.GetInt("Scans", 1);
        isFirstSwitch = true;
        scan_again = true;

        //  StartCoroutine(Chek_Internet_Connection());
       //   StartCoroutine(Get(test));
    }

    IEnumerator InitializeCamera()
    {
        yield return new WaitForSeconds(2);
        //  var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.GRAYSCALE, true);
        CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.GRAYSCALE, true);
        // var isAutoFocus = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
        cameraInitialized = true;
    }

    void Update()
    {
        count++;

        if (count <= fps)
        {
            IsModel = false;
            if (MenuOpened != true && scan_again == true)
            {
                if (cameraInitialized)
                {
                    try
                    {
                        var cameraFeed = CameraDevice.Instance.GetCameraImage(Image.PIXEL_FORMAT.GRAYSCALE);
                        if (cameraFeed == null)
                        {
                            return;
                        }

                        byte[] pixels = cameraFeed.Pixels;
                        int imageWidth = cameraFeed.BufferWidth;
                        int imageHeight = cameraFeed.BufferHeight;
                        int dstLeft, dstTop, dstWidth, dstHeight;

                        dstLeft = 0;
                        dstTop = 0;
                        dstWidth = imageWidth;
                        dstHeight = imageHeight;

                        PlanarYUVLuminanceSource source = new PlanarYUVLuminanceSource(pixels, imageWidth, imageHeight, dstLeft, dstTop, dstWidth,
                            dstHeight, true);

                        BinaryBitmap bitmap = new BinaryBitmap(
                             new HybridBinarizer(source));

                        var data = QRReader.decode(bitmap);

                        if (data != null)
                        {
                            scan_again = false;
                            StartCoroutine(QRSucces());
                            Data = data.Text;
                            //string[] DATATR = Data.Split(new Char[] { '\t', '\t', '\n' });
							string[] DATATR = Data.Split('!');

                            foreach (string ln in DATATR)
                            {

                                if (ln.Length > 0 && ln[0] != '#')
                                {
                                    string l = ln.Trim().Replace("  ", " ");
                                    string[] cmps = l.Split(' ');

                                    if (cmps[0] == "mtllib")
                                    {
                                        IsModel = true;
                                        break;
                                    }

                                }
                            }

                            if (IsModel == true && data.Text != Data)
                            {
								string dt = Data.Replace('!', '\n');
                                OBJ2 = OBJLoader.LoadOBJFile(dt);
                                OBJ2.layer = 4;
                                Mesh M = OBJ2.transform.GetComponentInChildren<MeshFilter>().mesh;
                                M.RecalculateBounds();
                                M.RecalculateNormals();
                                M.RecalculateTangents();

                                OBJ2.transform.SetParent(target.transform);
                                OBJ2.transform.localPosition = new Vector3(0, 0, 0);
                                OBJ2.transform.localRotation = Quaternion.Euler(0, 0, 0);
                                OBJ2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                                //   Debug.Log(mr.bounds.size);

                                if (M.bounds.size.y > 20)
                                {
                                    float z = M.bounds.size.y / 4;
                                    OBJ2.transform.localScale = new Vector3(OBJ2.transform.localScale.x / z, OBJ2.transform.localScale.y / z, OBJ2.transform.localScale.z / z);
                                }
                                UIController.bt_en = true;
                                Destroy(OBJ);

                            }

                            if (IsModel == false)
                            {
                                string[] DATATR2 = Data.Split('/');

                                if (DATATR2[0] == "http:" || DATATR2[0] == "https:")
                                {
									UIController.QRData = Data;
									UIController.Just_Hyperlink();
									UIController.bt_en = true;
								}else {
									string[] DATATR3 = Data.Split(' ');
									if (DATATR3[0] == "PplosStudioARMarker")
									{
										//&&  data.Text != Data
										StartCoroutine(ChekInternetConnection(Data));
										//  StartCoroutine(Get(Data));

									}
									else
									{
										UIController.QRData = Data;
										UIController.Just_Text();
										UIController.bt_en = true;
									}
										
                                }
                                
                            }
                        }
                        else
                        {
                            Debug.Log("No QR code detected !");
                        }
                    }

                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }

                }
            }
        }

        if (count == 30)
        {
            count = 0;
        }
    }

    IEnumerator QRSucces()
    {
        UIController.Status = "success";
        yield return new WaitForSeconds(2);
        UIController.Status = " ";
    }

    public void Flash()
    {

        if (isFirstSwitch == true)
        {
            CameraDevice.Instance.SetFlashTorchMode(true);
            isFirstSwitch = false;
        }
        else
        {
            CameraDevice.Instance.SetFlashTorchMode(false);
            isFirstSwitch = true;
        }

    }

    public IEnumerator Get(string data)
    {

        Destroy(OBJ2);
		string[] urlData = data.Split(' ');
		string url = "https://pplos-studio-db-default-rtdb.europe-west1.firebasedatabase.app/PplosStudio/models/";
		url += urlData [1].Split('=')[1].Trim() + ".json?password=" + urlData[2].Split('=')[1].Trim();
        Debug.Log(url);

        string json;
        string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
        Dictionary<string, string> ht = new Dictionary<string, string>();
        ht["User-Agent"] = userAgent;

		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			yield return www.Send();
			if(www.isNetworkError || www.isHttpError)
			{
				yield break;
			}
			else
			{	
				json = www.downloadHandler.text;
			}
		}

		Data = ModelData.createFromJSON(json).data.Replace('!', '\n');
		OBJ2 = OBJLoader.LoadOBJFile(Data);
        Mesh M = OBJ2.transform.GetComponentInChildren<MeshFilter>().mesh;

        M.RecalculateBounds();
        M.RecalculateNormals();
        M.RecalculateTangents();

        OBJ2.transform.SetParent(target.transform);
        OBJ2.transform.localPosition = new Vector3(0, 0, 0);
        OBJ2.transform.localRotation = Quaternion.Euler(0, 0, 0);
        OBJ2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        OBJ2.transform.GetComponentInChildren<MeshRenderer>().enabled = false;

        Debug.Log(M.bounds.size);

        bool some = false;

        if (M.bounds.size.y > 20 && some == false)
        {
            float z = 5 / M.bounds.size.y;
            OBJ2.transform.localScale = new Vector3(OBJ2.transform.localScale.x * z, OBJ2.transform.localScale.y * z, OBJ2.transform.localScale.z * z);
            some = true;
        }

        if (M.bounds.size.x > 2 && some == false)
        {
            float buff = 2 / M.bounds.size.x;
            OBJ2.transform.localScale = new Vector3(OBJ2.transform.localScale.x * buff, OBJ2.transform.localScale.y * buff, OBJ2.transform.localScale.z * buff);
            some = true;
        }

        if (M.bounds.size.z > 2 && some == false)
        {
            float buff = 2 / M.bounds.size.z;
            OBJ2.transform.localScale = new Vector3(OBJ2.transform.localScale.x * buff, OBJ2.transform.localScale.y * buff, OBJ2.transform.localScale.z * buff);
            some = true;
        }

        if (M.bounds.size.x < 1 && some == false)
        {
            float buff = 2 / M.bounds.size.x;
            OBJ2.transform.localScale = new Vector3(OBJ2.transform.localScale.x * buff, OBJ2.transform.localScale.y * buff, OBJ2.transform.localScale.z * buff);
            some = true;
        }

        if (M.bounds.size.z < 1 && some == false)
        {
            float buff = 2 / M.bounds.size.z;
            OBJ2.transform.localScale = new Vector3(OBJ2.transform.localScale.x * buff, OBJ2.transform.localScale.y * buff, OBJ2.transform.localScale.z * buff);
            some = true;
        }

        if (M.bounds.size.y < 1 && some == false)
        {
            float z = 5 / M.bounds.size.y;
            OBJ2.transform.localScale = new Vector3(OBJ2.transform.localScale.x * z, OBJ2.transform.localScale.y * z, OBJ2.transform.localScale.z * z);
            some = true;
        }

        UIController.bt_en = true;
        Destroy(OBJ);

    }

    public IEnumerator ChekInternetConnection(string Data)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://www.google.ru/"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                StartCoroutine(Error());
                scan_again = false;
                UIController.bt_en = true;
            }
            else
            {
                StartCoroutine(Get(Data));
                UIController.bt_en = true;
            }
        }

    }

    public IEnumerator Error()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(2);
        text.SetActive(false);
    }

    public static void Focus()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
    }

	[System.Serializable]
	public class ModelData
	{
		
		public string data;
		public string password;

		public static ModelData createFromJSON(string jsonString)
		{
			return JsonUtility.FromJson<ModelData>(jsonString);
		}

	}

}