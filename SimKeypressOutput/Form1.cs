using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SimKeypressOutput
{
    public partial class Form1 : Form
    {
        delegate void SetTextCallback(string text);

        Dictionary<string, Profile> profiles;
        MqttClient client;


        Dictionary<string, KeyPressFunction> keys;
        Dictionary<string, int> seqCounts;                                                          

        public Form1(string[] args)
        {
            InitializeComponent();

            keys = new Dictionary<string, KeyPressFunction>();
            seqCounts = new Dictionary<string, int>();

            if(args.Length > 0)
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
            if (userClickedOK == true)
            {
                openConfig(openFileDialog1.FileName);
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (client != null)
            {
                client.Publish("zusi/status", Encoding.UTF8.GetBytes("SimKeypressOutput shutting down"), 0, false);
                client.Disconnect();
            }
        }

        private void cmbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectProfile(cmbProfiles.Text);
        }

        private void SetMQTTText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lblMQTT.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetMQTTText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lblMQTT.Text = text;
            }
        }

        private void SetInputText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lblState.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetInputText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lblState.Text = text;
            }
        }


        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if(Encoding.UTF8.GetString(e.Message) == "1")
            {
                SetMQTTText(e.Topic + " ON");

                if (keys.ContainsKey(e.Topic))
                {
                    KeyPressFunction kp = keys[e.Topic];
                    if (kp.Items[0].GetType() == typeof(KeyPressToggle))
                    {
                        KeyPressToggle toggle = (KeyPressToggle)kp.Items[0];
                        runKeyDown(toggle.activate);
                        runKeyUp(toggle.activate);
                    }
                    else if (kp.Items[0].GetType() == typeof(KeyPressAction))
                    {
                        KeyPressAction action = (KeyPressAction)kp.Items[0];
                        runKeyDown(action);
                    }
                    else if (kp.Items[0].GetType() == typeof(KeyPressSequence))
                    {
                        KeyPressSequence seq = (KeyPressSequence)kp.Items[0];
                        int count = seqCounts[e.Topic] + 1;
                        if (count >= seq.step.Count())
                            count = 0;
                        seqCounts[e.Topic] = count;
                        runKeyDown(seq.step[count]);
                        runKeyUp(seq.step[count]);
                    }
                }

            }
            else if (Encoding.UTF8.GetString(e.Message) == "0")
            {
                SetMQTTText(e.Topic + " OFF");
                if (keys.ContainsKey(e.Topic))
                {
                    KeyPressFunction kp = keys[e.Topic];
                    if (kp.Items[0].GetType() == typeof(KeyPressToggle))
                    {
                        KeyPressToggle toggle = (KeyPressToggle)kp.Items[0];
                        runKeyDown(toggle.deactivate);
                        runKeyUp(toggle.deactivate);
                    }
                    else if (kp.Items[0].GetType() == typeof(KeyPressAction))
                    {
                        KeyPressAction action = (KeyPressAction)kp.Items[0];
                        runKeyUp(action);
                    }
                }
            }
            else
            {
                SetMQTTText(e.Topic + " UNKNOWN " + e.Message.ToString());
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

            client.Publish("zusi/status", Encoding.UTF8.GetBytes("SimKeypressOutput started"), 0, false);

            client.Subscribe(new string[] { "zusi/control/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        }

        private void selectProfile(string profileName)
        {
            cmbProfiles.Text = profileName;

            Profile newProfile = profiles[profileName];

            keys.Clear();
            if (newProfile.Mappings.keypressOutput != null)
            {
                foreach (KeyPressFunction key in newProfile.Mappings.keypressOutput)
                {
                    keys[key.topic] = key;
                    seqCounts[key.topic] = 0;
                }
            }

            client.Publish("zusi/status", Encoding.UTF8.GetBytes("SimKeypressOutput selected profile " + cmbProfiles.Text), 0, false);
        }

        void runKeyDown(KeyPressAction action)
        {
            if (action.ctrl)
                ScanKeyInput.KeyDown(ScanCodeShort.LCONTROL);
            if (action.alt)
                ScanKeyInput.KeyDown(ScanCodeShort.LMENU);
            if (action.shift)
                ScanKeyInput.KeyDown(ScanCodeShort.LSHIFT);

            ScanKeyInput.KeyDown((ScanCodeShort)Enum.Parse(typeof(ScanCodeShort), action.Value, true));
            SetInputText(action.Value + " DOWN");
        }

        void runKeyUp(KeyPressAction action)
        {
            ScanKeyInput.KeyUp((ScanCodeShort)Enum.Parse(typeof(ScanCodeShort), action.Value, true));
            SetInputText(action.Value + " UP");

            if (action.shift)
                ScanKeyInput.KeyUp(ScanCodeShort.LSHIFT);
            if (action.alt)
                ScanKeyInput.KeyUp(ScanCodeShort.LMENU);
            if (action.ctrl)
                ScanKeyInput.KeyUp(ScanCodeShort.LCONTROL);
        }
    }
}

