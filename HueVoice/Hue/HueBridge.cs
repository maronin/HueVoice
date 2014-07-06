using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HueVoice
{
    class HueBridge
    {
        string bridgeIP, username;
        WebClient client;

        public HueBridge(string ip, string username)
        {
            bridgeIP = ip;
            this.username = username;
            client = new WebClient();
        }

        public void getLight(int lightNum)
        {

        }

        public List<HueLight> getAllLights()
        {
            dynamic data = GET("http://" + bridgeIP + "/api/" + username + "/lights");
            int i = 1;
            var lights = new List<HueLight>();

            //get the data about all the lights
            while (data[i.ToString()] != null)
            {
                dynamic getLightJsonData = GET("http://" + bridgeIP + "/api/" + username + "/lights/" + i);
                HueLight light = new HueLight(getLightJsonData);
                lights.Add(light);
                i++;
            }
            return lights;
        }

        public void toggleLights(string group)
        {
            dynamic lightData = GET("http://" + bridgeIP + "/api/" + username + "/groups/" + group);
            bool toggle = lightData["action"].on;
            int transTime = 0;
            if (!toggle)
                transTime = 1;

            Object data = new
            {
                on = !toggle,
                bri = 254,
                transitiontime = transTime
            };
            PUT("http://" + bridgeIP + "/api/" + username + "/groups/" + group + "/action", data);
        }

        private dynamic GET(string address)
        {

            Uri uri = new Uri(address);
            client = new WebClient();
            var jsonData = client.DownloadString(uri); //how to GET from a URL
            dynamic data = JsonConvert.DeserializeObject(jsonData);
            return data;
        }

        private void PUT(string address, Object data)
        {
            var uri = new Uri(address);
            client = new WebClient();
            var jsonObj = JsonConvert.SerializeObject(data);
            client.UploadStringAsync(uri, "PUT", jsonObj);
        }


    }
}
