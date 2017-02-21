using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Text.RegularExpressions;
using System.IO;

namespace FlippaSearch
{
    public partial class Form1 : Form
    {
        static IWebDriver driverGC;
        public Form1()
        {
            driverGC = new ChromeDriver(@"Z:\Justin\Documents\Visual Studio 2015\chromedriver_win32");
            driverGC.Navigate().GoToUrl("https://flippa.com/websites/starter-sites?sitetype=blog&uniques_per_month_min=1000");
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<IWebElement> starterSites = new List<IWebElement>();
            List<String> myStarterSites = new List<string>();
            var numPages = (driverGC.FindElement(By.XPath("//*[@id='searchBody']/div[1]/div[1]/h2/span")).Text);
            var mySites = driverGC.FindElements(By.CssSelector(".ListingResults___listingResult"));
            int size = 1;
            for (int i = 0; i < 2; i++)
            {

                for (int j = 0; j < size; j++)
                {
                    driverGC.Manage().Timeouts().PageLoad
                    mySites = driverGC.FindElements(By.CssSelector(".ListingResults___listingResult"));
                    size = mySites.Count();
                    var siteLink = " ";
                    
                    try
                    {
                        
                        siteLink = mySites[j].FindElement(By.CssSelector(".ListingResults___listingResultLink")).GetAttribute("href");
                        //MessageBox.Show(siteLink);
                        //driverGC.Navigate().GoToUrl(siteLink);
                        myStarterSites.Add(siteLink);
                    }
                    catch (OpenQA.Selenium.NoSuchElementException)
                    {
                        MessageBox.Show("Could not find anything");
                        break;
                    }
                    //IWebElement organicSearch = (driverGC.FindElement(By.XPath("/html/body/div[3]/div[1]/div[1]/div[5]/div[1]/div/table[1]/tbody/tr[1]/td[1]")));

                }
                try
                {
                    driverGC.FindElement(By.XPath("//*[@id='searchBody']/div[2]/div[2]/div/a[3]")).Click();
                }
                catch (OpenQA.Selenium.ElementNotVisibleException)
                {
                    Console.WriteLine("No more pages");
                    continue;
                }
                

            }
            using (StreamWriter writer = new StreamWriter(@"C:\Users\Justin\Desktop\newFile.txt"))
            {
                foreach (string s in myStarterSites)
                {
                    // writer.Write(s); // Writes in same line
                    writer.WriteLine(s);// Writes in next line
                }

            }
            MessageBox.Show("End");
            driverGC.Quit();
        }
    }
}
