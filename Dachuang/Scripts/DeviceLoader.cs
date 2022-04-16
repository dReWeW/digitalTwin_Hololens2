using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MRDeviceMonitor
{
    
    [System.Serializable]
    public class ElementData
    {
        //ycyNeedChange1
        public string name; //  英文名字
        public string EnvTemp;
        public string EnvHumidity;
        public string DustDensity;
        public string AtomosPressure;
        public string GasDensity;
        public string DeviceName;
        public string DeviceId;
        public string RoundSpeed;
        public string WorkTime;

        /// </summary>
        public string torqueLT;
        public string WorkTempLT;
        public string torqueLB;
        public string WorkTempLB;
        public string torqueRT;
        public string WorkTempRT;
        public string torqueRB;
        public string WorkTempRB;
        public string GimYaw;
        public string GimPit;
        public string GimRol;
        /// </summary>
        /// 
        public float xpos;
        public float ypos;
        public float zpos;
    }

    class DevicesData
    {
        public List<ElementData> devices;

        public static DevicesData FromJSON(string json)
        {
            
            return JsonUtility.FromJson<DevicesData>(json);
        }
    }
    public class DeviceLoader : MonoBehaviour
    {
        public Transform Parent;
        public GameObject PanelPrefab;
        Hashtable deviceMap;
        GameObject newDevice;
        //public void start()
        //{
        //    elementdata element = new elementdata();
        //    element.deviceid = "hhh";
        //    element.devicename = "device";
        //    string jsonstring = jsonutility.tojson(element);
        //    debug.log("jsonstring:" + jsonstring);
        //}

        public void Start()
        {
            deviceMap = new Hashtable();
        }
        public void ShowData(string deviceString)
        {
            List<ElementData> devices = DevicesData.FromJSON(deviceString).devices;
            foreach (ElementData device in devices)
            {
                string name = device.name;
                if (deviceMap.ContainsKey(name))
                {
                    GameObject udevice = (GameObject)deviceMap[name];
                    udevice.GetComponentInChildren<Element>().SetFromElementData(device);
                }
                else
                {
                    newDevice = Instantiate<GameObject>(PanelPrefab, Parent);
                    newDevice.GetComponentInChildren<Element>().SetFromElementData(device);
                    newDevice.transform.localPosition = new Vector3(device.xpos, device.ypos, device.zpos);
                    newDevice.transform.localRotation = Quaternion.identity;
                    deviceMap.Add(name, newDevice);

                }
            }
        }

    }
}
