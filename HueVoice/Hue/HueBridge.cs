using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            //make a class out of this or overload something. Just make it easier to create data. 
            string data = "{\"on\":" + (!toggle).ToString().ToLower() +
                          ",\"bri\":" + 254 +
                          ",\"transitiontime\":" + transTime +
                          "}";
            PUT("http://" + bridgeIP + "/api/" + username + "/groups/" + group + "/action", data);
        }

        public void changeLightColor(Color color, string group)
        {    
            List<Double> xy = getRGBtoXY(color);
            Double x = xy[0], y = xy[1];
            string data = "{\"xy\":[" + x + "," + y + "]," + "\"on\":true}";
            PUT("http://" + bridgeIP + "/api/" + username + "/groups/" + group + "/action", data);
        }

        public void dimLights(string p, string group)
        {
            double percentage = double.Parse(p.TrimEnd(new[] { '%' })) / 100;
            //dynamic lightData = GET("http://" + bridgeIP + "/api/" + username + "/groups/" + group);
            //int brightness = lightData["action"].bri;

            string data = "{\"bri\":" + (int)(254*percentage) + ", \"on\": true}";
            PUT("http://" + bridgeIP + "/api/" + username + "/groups/" + group + "/action", data);

        }

        public void turnLightsOn(string group)
        {
            client = new WebClient();
            
            string data = "{\"on\":true}";
            PUT("http://" + bridgeIP + "/api/" + username + "/groups/" + group + "/action", data);

            
        }

        public void turnLightsOff(string group)
        {
            client = new WebClient();

            string data = "{\"on\":false}";
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

        private void PUT(string address, string data)
        {
            var uri = new Uri(address);
            client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UploadStringCallback);
            var jsonObj = data;

            client.UploadStringAsync(uri, "PUT", jsonObj);
        }

        void UploadStringCallback(object sender, UploadStringCompletedEventArgs e)
        {
            //MessageBox.Show(e.Result + System.Environment.NewLine);

        }

        public static List<Double> getRGBtoXY(Color c)
        {
            // For the hue bulb the corners of the triangle are:
            // -Red: 0.675, 0.322
            // -Green: 0.4091, 0.518
            // -Blue: 0.167, 0.04
            double[] normalizedToOne = new double[3];
            float cred, cgreen, cblue;
            cred = c.R;
            cgreen = c.G;
            cblue = c.B;
            normalizedToOne[0] = (cred / 255);
            normalizedToOne[1] = (cgreen / 255);
            normalizedToOne[2] = (cblue / 255);
            float red, green, blue;

            // Make red more vivid
            if (normalizedToOne[0] > 0.04045)
            {
                red = (float)Math.Pow(
                        (normalizedToOne[0] + 0.055) / (1.0 + 0.055), 2.4);
            }
            else
            {
                red = (float)(normalizedToOne[0] / 12.92);
            }

            // Make green more vivid
            if (normalizedToOne[1] > 0.04045)
            {
                green = (float)Math.Pow((normalizedToOne[1] + 0.055)
                        / (1.0 + 0.055), 2.4);
            }
            else
            {
                green = (float)(normalizedToOne[1] / 12.92);
            }

            // Make blue more vivid
            if (normalizedToOne[2] > 0.04045)
            {
                blue = (float)Math.Pow((normalizedToOne[2] + 0.055)
                        / (1.0 + 0.055), 2.4);
            }
            else
            {
                blue = (float)(normalizedToOne[2] / 12.92);
            }

            float X = (float)(red * 0.649926 + green * 0.103455 + blue * 0.197109);
            float Y = (float)(red * 0.234327 + green * 0.743075 + blue * 0.022598);
            float Z = (float)(red * 0.0000000 + green * 0.053077 + blue * 1.035763);

            float x = X / (X + Y + Z);
            float y = Y / (X + Y + Z);

            double[] xy = new double[2];
            xy[0] = x;
            xy[1] = y;
            List<Double> xyAsList = new List<Double> { x, y };
            return xyAsList;
        }

    }
}
