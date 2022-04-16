using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace MRDeviceMonitor
{

    public class Element : MonoBehaviour
    {
        // 存储被激活的设备列表
        //ycyNeedChange3
        public static List<Element> Elements;
        public MeshRenderer body;
        public ElementData data;

        public Text EnvTemp;
        public Text EnvHumidity;
        public Text DustDensity;
        public Text AtomosPressure;
        public Text GasDensity;

        public Text DeviceName;
        public Text DeviceId;
        public Text RoundSpeed;
        public Text WorkTime;

        /// </summary>
        public Text torqueLT;
        public Text WorkTempLT;
        public Text torqueLB;
        public Text WorkTempLB;
        public Text torqueRT;
        public Text WorkTempRT;
        public Text torqueRB;
        public Text WorkTempRB;
        public Text GimYaw;
        public Text GimPit;
        public Text GimRol;
        /// </summary>



        private bool active = false;

        private bool hasAutoActive = false;

        private PresentToPlayer present;

        private void Awake()
        {
            if (Elements == null)
            {
                Elements = new List<Element>();
            }
        }
        public void AddActiveElement()
        {
            if (!active)
            {
                Debug.Log("ADD ACT");
                active = true;
                Element element = gameObject.GetComponent<Element>();
                Elements.Add(element);
            }
        }
        public void RemoveActiveElement()
        {
            if (active)
            {
                Elements.Remove(this);
                present = GetComponent<PresentToPlayer>();
                active = false;
            }
        }
        public void Start()
        {
            present = GetComponent<PresentToPlayer>();
            // 初始化时关闭动画机，需要用到时再开启
            GetComponent<Animator>().enabled = false;
        }

        public void Open()
        {
            // if (present.Presenting)
            // {
            //     return;
            // }
            StartCoroutine(UpdateActive());

        }

        private async void Update()
        {
            if (body.isVisible && !hasAutoActive)
            {
                Debug.Log("visible");
                AddActiveElement();
                Open();
                hasAutoActive=true;
            }
        }
        public IEnumerator UpdateActive()
        {
            // present.Present();
            // while(!present.InPosition){
            //     // 如果Player还未走到合适的距离，则先不呈现信息
            // }
            Debug.Log("coro");
            Animator animator = gameObject.GetComponent<Animator>();
            animator.enabled = true;
            animator.SetBool("Opened", true);

            while (Elements.Contains(this))
            {
                // 如果当前元素正在被激活，则返回
                yield return null;
            }
            animator.SetBool("Opened", false);
            yield return new WaitForSeconds(0.66f);

            // present.Return();
        }
        //ycyNeedChange2
        public void SetFromElementData(ElementData data)
        {
            this.data = data;
          
            EnvTemp.text = data.EnvTemp;
            EnvHumidity.text = data.EnvHumidity;
            DustDensity.text = data.DustDensity;
            AtomosPressure.text = data.AtomosPressure;
            GasDensity.text = data.GasDensity;

            DeviceName.text = data.DeviceName;
            DeviceId.text = data.DeviceId;
            RoundSpeed.text = data.RoundSpeed;
            WorkTime.text = data.WorkTime;

            /// </summary>
            torqueLT.text = data.torqueLT;
            WorkTempLT.text = data.WorkTempLT;
            torqueLB.text = data.torqueLB;
            WorkTempLB.text = data.WorkTempLB;
            torqueRT.text = data.torqueRT;
            WorkTempRT.text = data.WorkTempRT;
            torqueRB.text = data.torqueRB;
            WorkTempRB.text = data.WorkTempRB;
            GimYaw.text = data.GimYaw;
            GimPit.text = data.GimPit;
            GimRol.text = data.GimRol; 
            /// </summary>
            /// 
            transform.parent.name = data.name;
        }
    }

}
