using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using uPLibrary.Networking.M2Mqtt;

namespace SimXKeysInput
{
    public partial class Form1 : Form
    {
        Dictionary<string, Profile> profiles;
        MqttClient client;

        Dictionary<int, Key> currentKeys = new Dictionary<int, Key>();
        bool[] buttonStates = new bool[25];

        public Form1(string[] args)
        {
            InitializeComponent();
            xk24.SetGreenIndicator(0, 1);
            xk24.SetRedIndicator(0, 0);

            if (args.Length > 0)
                openConfig(args[0]);

            if (args.Length > 1 && profiles.ContainsKey(args[1]))
                selectProfile(args[1]);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "XML Files (.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            bool userClickedOK = (openFileDialog1.ShowDialog() == DialogResult.OK);

            // Process input if the user clicked OK.
            if(userClickedOK == true)
            {
                openConfig(openFileDialog1.FileName);
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            xk24.SetGreenIndicator(0, 0);
            xk24.SetRedIndicator(0, 1);
            xk24.SetAllBlue(0, false);
            xk24.SetAllRed(0, false);

            if (client != null)
            {
                client.Publish("zusi/status", Encoding.UTF8.GetBytes("SimXKeysInput shutting down"), 0, false);
                client.Disconnect();
            }
        }

        private void cmbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectProfile(cmbProfiles.Text);
        }

        private void HandleButtons(Xk24.XKeyEventArgs e)
        {
            //Gets the button number (CID) of the button that has changed state
            int button = (e.CID - 1000);
            String ButtonNum = (e.CID - 1000).ToString();
            if (e.PressState == true) //button press
                lblState.Text = ButtonNum + " DOWN";
            else
                lblState.Text = ButtonNum + " UP";

            if(currentKeys.ContainsKey(button))
            {
                Key key = currentKeys[button];

                if(e.PressState)
                    buttonStates[button] = !(buttonStates[button]);

                if ((key.type == KeyType.normal && e.PressState == true)
                    || (key.type == KeyType.toggle && e.PressState == true && buttonStates[button]==true))
                {
                    applyKeyPosition(button, key.on);
                    client.Publish(key.topic, Encoding.UTF8.GetBytes("1"), 0, false);
                }
                else if(key.type == KeyType.normal || (key.type == KeyType.toggle && e.PressState == true))
                {
                    applyKeyPosition(button, key.off);
                    client.Publish(key.topic, Encoding.UTF8.GetBytes("0"), 0, false);
                }
            }
        }

        private void openConfig(string filename)
        {
            profiles = new Dictionary<string, Profile>();

            ProfileList xmlProfiles;
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer mySerializer =
            new XmlSerializer(typeof(ProfileList));
            // To read the file, create a FileStream.
            FileStream myFileStream =
            new FileStream(filename, FileMode.Open, FileAccess.Read);
            // Call the Deserialize method and cast to the object type.
            xmlProfiles = (ProfileList)
            mySerializer.Deserialize(myFileStream);

            //Set up profile list
            cmbProfiles.Items.Clear();
            foreach (Profile p in xmlProfiles.profile)
            {
                profiles.Add(p.name, p);
                cmbProfiles.Items.Add(p.name);
            }

            //Set up MQTT
            client = new MqttClient(xmlProfiles.config.brokerAddress);

            string clientId = Guid.NewGuid().ToString();
            if (client.Connect(clientId) == 0)
                lblMQTT.Text = "Connected to " + xmlProfiles.config.brokerAddress;

            client.Publish("zusi/status", Encoding.UTF8.GetBytes("SimXKeysInput started"), 0, false);
        }

        private void selectProfile(string profileName)
        {
            cmbProfiles.Text = profileName;

            Profile newProfile = profiles[profileName];

            currentKeys = new Dictionary<int, Key>();
            if (newProfile.Mappings.XKeysInput != null)
                foreach (Key key in newProfile.Mappings.XKeysInput)
                {
                    applyKeyPosition(key.ID, key.off);
                    currentKeys.Add(key.ID, key);
                    buttonStates[key.ID] = false;
                }

            client.Publish("zusi/status", Encoding.UTF8.GetBytes("SimXKeysInput selected profile " + cmbProfiles.Text), 0, false);
        }

        private void applyKeyPosition(int key, KeyPosition pos)
        {
            applyLightState(key, 0, pos.blue);
            applyLightState(key, 1, pos.red);
        }

        private void applyLightState(int key, int bank, LightState state)
        {
            if (state == LightState.off)
                xk24.SetBacklightLED(0, key, bank, 0);
            else if (state == LightState.on)
                xk24.SetBacklightLED(0, key, bank, 1);
            else if (state == LightState.blink)
                xk24.SetBacklightLED(0, key, bank, 2);
        }
    }
}
