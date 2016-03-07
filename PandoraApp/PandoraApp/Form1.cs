using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Windows.Forms;

namespace PandoraApp
{
    public partial class Pandora : Form
    {
        string userName, password;
        bool playing, onPandora;
        int currentStation = 0;
        int originalSpot;
        IWebDriver driver;
        public Pandora()
        {
            InitializeComponent();
            playing = true;
            onPandora = false;
            originalSpot = label5.Left;
        }
        private void setInformation()
        {
            try
            {
                if (driver.FindElement(By.ClassName("songTitle")).Displayed)
                {

                    label5.Text = driver.FindElement(By.ClassName("songTitle")).Text;
                    label6.Text = driver.FindElements(By.ClassName("stationNameText"))[currentStation].Text;
                }
            }
            catch
            {
                Console.WriteLine("Error: Unable to display song title at this time");
            }
      
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            userName =  textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            userName = "madscience9@gmail.com";
            password = "taranto";
            if (!onPandora)
            {
                ChromeOptions options = new ChromeOptions();
                //options.AddExtension("/addBlock.crx");
                //options.AddArgument("--no-startup-window");
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                driver = new ChromeDriver(service, options);

                driver.Navigate().GoToUrl("http://www.pandora.com/account/sign-in");
                if (userName != null && password != null)
                {
                    driver.FindElement(By.Name("email")).SendKeys(userName);
                    driver.FindElement(By.Name("password")).SendKeys(password);
                    driver.FindElement(By.ClassName("loginButton")).Click();
                }
                using (StreamWriter file = new StreamWriter("login.txt"))
                {
                    file.WriteLine(userName);
                    file.WriteLine(password);
                }
                onPandora = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try {
                if (playing)
                {
                    driver.FindElement(By.ClassName("pauseButton")).Click();
                    playing = false;
                }
                else
                {
                    driver.FindElement(By.ClassName("playButton")).Click();
                    playing = true;
                }
                if (playing) button2.Text = "Pause";
                else button2.Text = "Play";
            }
            catch
            {
                Console.WriteLine("Error: No play option");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try {
                driver.FindElement(By.ClassName("skipButton")).Click();
            }
            catch
            {
                Console.WriteLine("Error: No skip at this time");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try {
                driver.FindElement(By.ClassName("thumbUpButton")).Click();
            }
            catch
            {
                Console.WriteLine("Error: No liking at this time");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try {
                driver.FindElement(By.ClassName("thumbDownButton")).Click();
            }
            catch
            {
                Console.WriteLine("Error: No liking at this time");
            }
        }

        private void Pandora_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try {
                var stationList = driver.FindElements(By.ClassName("stationNameText"));
                currentStation += 1;
                if (stationList.Count == currentStation)
                {
                    currentStation = 1;
                }
                stationList[currentStation].Click();
                System.Console.WriteLine(stationList[currentStation].Text);
            }
            catch
            {
                Console.WriteLine("Error: No station skipping at this time");
            }
        }

        private void Pandora_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try {
                StreamReader file = new StreamReader("login.txt");
                string name = file.ReadLine();
                string pass = file.ReadLine();
                file.Close();
                if (name != null && pass != null)
                {
                    driver.FindElement(By.Name("email")).SendKeys(name);
                    driver.FindElement(By.Name("password")).SendKeys(pass);
                    driver.FindElement(By.ClassName("loginButton")).Click();
                }
            }
            catch
            {
                Console.WriteLine("no saved stuff");
            }

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (onPandora)
            {
                setInformation();
                if(label5.Left+label5.Width < splitContainer4.Panel2.Width)
                {
                    label5.Left = originalSpot;
                }
                if (label5.Width > splitContainer4.Panel2.Width)
                {
                    label5.Left = label5.Left - 4;
                }

                if (label6.Left+label6.Width < splitContainer4.Panel2.Width)
                {
                    label6.Left = originalSpot;
                }
                if (label6.Width > splitContainer4.Panel2.Width)
                {
                    label6.Left = label6.Left - 4;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            password = textBox2.Text;
        }
    }
}
