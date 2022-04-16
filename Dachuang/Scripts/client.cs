using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace MRDeviceMonitor {

    public class client : MonoBehaviour
    {
        public Socket m_socket;
        IPEndPoint m_endPoint;
        private SocketAsyncEventArgs m_connectSAEA;
        private SocketAsyncEventArgs m_sendSAEA;
        public string ip = "192.168.43.159";//需要连接到的服务器ip
        public int port = 8080;
        public string preMsg = " ";
        public string[] Msg;
        bool receive = false;
        bool needReconnect = false;
        private DeviceLoader deviceLoader;
        string str;

        // Start is called before the first frame update
        void Start()
        {
            deviceLoader = GetComponent<DeviceLoader>();
            Debug.Log("start client");
            Invoke("Client", 5f);
        }

        // Update is called once per frame
        void Update()
        {
            if (preMsg != " " && receive)
            {
                // 新设备
                Debug.Log("primitive message:" + preMsg);
                foreach (string s in Msg)
                    Debug.Log("processed message:" + s);
                deviceLoader.ShowData(str);
                receive = false;    
            }

            if (needReconnect) //处理断线重连
            {
                Invoke("Client", 5f);
                needReconnect = false;
            }
        }

        public void Client()
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress iPAddress = IPAddress.Parse(ip);
            m_endPoint = new IPEndPoint(iPAddress, port);

            Debug.Log("waiting for connecting with server");

            m_connectSAEA = new SocketAsyncEventArgs { RemoteEndPoint = m_endPoint };
            m_connectSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectedCompleted);
            m_socket.ConnectAsync(m_connectSAEA);
        }

        private void OnConnectedCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                needReconnect = true;
                Debug.Log("<OnConnectedCompleted> exit");
                return;
            }
            Socket socket = sender as Socket;
            string iPRemote = socket.RemoteEndPoint.ToString();
            Debug.Log("Client : 连接服务器 " + iPRemote + " 成功");

            SocketAsyncEventArgs receiveSAEA = new SocketAsyncEventArgs();
            byte[] receiveBuffer = new byte[1024];
            receiveSAEA.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);

            Debug.Log("waiting for messages from server");

            receiveSAEA.Completed += OnReceiveCompleted;
            receiveSAEA.RemoteEndPoint = m_endPoint;
            socket.ReceiveAsync(receiveSAEA);
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.OperationAborted)
            {
                Debug.Log("<OnReceiveCompleted> exit");
                return;
            }
            Socket socket = sender as Socket;
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                string ipAdress = socket.RemoteEndPoint.ToString();
                int lengthBuffer = e.BytesTransferred;
                byte[] receiveBuffer = e.Buffer;

                //读取指定位数的信息
                byte[] data = new byte[lengthBuffer];
                Array.Copy(receiveBuffer, 0, data, 0, lengthBuffer);
                str = System.Text.Encoding.Default.GetString(data);
                Debug.Log("have received messages from server");
                Debug.Log(str);
                preMsg = str;//这里直接赋值给debugText.text无法更新,通过update中检测的方式更新信息
                
                Msg = preMsg.Split(new char[1] { '_' });
                receive = true;
                //向服务器端发送消息
                //Send("接受信息成功");

                socket.ReceiveAsync(e);
            }
            else if (e.BytesTransferred == 0) //连接断开的处理
            {
                if (e.SocketError == SocketError.Success)
                {
                    Debug.Log("主动断开连接 ");
                    //DisConnect();
                }
                else
                {
                    Debug.Log("被动断开连接 ");
                }
                needReconnect = true;//通过update中检测的方式更新信息
            }
            else
            {
                return;
            }
        }

        #region 发送
        void Send(string msg)
        {
            byte[] sendBuffer = Encoding.Default.GetBytes(msg);
            if (m_sendSAEA == null)
            {
                m_sendSAEA = new SocketAsyncEventArgs();
                m_sendSAEA.Completed += OnSendCompleted;
            }
            m_sendSAEA.SetBuffer(sendBuffer, 0, sendBuffer.Length);
            if (m_socket != null)
            {
                m_socket.SendAsync(m_sendSAEA);
            }
        }

        void OnSendCompleted(object sender1, SocketAsyncEventArgs e1)
        {
            if (e1.SocketError != SocketError.Success)
            {
                Debug.Log("<OnSendCompleted> exit");
                return;
            }
            Socket socket1 = sender1 as Socket;
            byte[] sendBuffer = e1.Buffer;

            string sendMsg = Encoding.Default.GetString(sendBuffer);

            Debug.Log("Client : Send message" + sendMsg + "to Server" + socket1.RemoteEndPoint.ToString());
        }
        #endregion

        #region 断开连接
        void DisConnect()
        {
            Debug.Log("断开连接");
            if (m_socket != null)
            {
                try
                {
                    m_socket.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException except)
                {
                }
                finally
                {
                    m_socket.Close();
                }
            }
        }
        #endregion
    }
}