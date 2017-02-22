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
using System.Text.RegularExpressions;
using System.IO;
using OpenQA.Selenium.Support.UI;

namespace FlippaSearch
{
    public partial class Form1 : Form
    {
        static IWebDriver driverGC;
        public Form1()
        {
            driverGC = new ChromeDriver(@"C:\Users\Justin\Documents\Visual Studio 2015\chromedriver_win32");
            driverGC.Navigate().GoToUrl("https://flippa.com/websites/starter-sites?sitetype=blog&uniques_per_month_min=1000");
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<IWebElement> starterSites = new List<IWebElement>();
            List<String> myStarterSites = new List<string>();
            var numPages = (driverGC.FindElement(By.XPath("//*[@id='searchBody']/div[1]/div[1]/h2/span")).Text);
            double numberPages = int.Parse(Regex.Match(numPages, @"\d+", RegexOptions.RightToLeft).Value);
            WebDriverWait wait = new WebDriverWait(driverGC, TimeSpan.FromSeconds(10));
            wait.Until(d => driverGC.FindElements(By.CssSelector(".ListingResults___listingResult")));
            numberPages = Math.Ceiling(numberPages / 50);
            int j;
            for (int i = 1; i <= numberPages; i++)
            {
                //driverGC.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                var mySites = driverGC.FindElements(By.CssSelector(".ListingResults___listingResult"));
                int size = 1;
                for (j = 0; j < 3; ++j)
                {
                    //driverGC.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                    driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    mySites = driverGC.FindElements(By.CssSelector(".ListingResults___listingResult"));
                    size = mySites.Count();
                    //driverGC.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
                    driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    String siteLink = " ";
                    siteLink = mySites[j].FindElement(By.CssSelector(".ListingResults___listingResultLink")).GetAttribute("href");
                    driverGC.Navigate().GoToUrl(siteLink);
                    driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    //testing tables
                    int row_tr = 5;
                    int Column_td = 3;
                    String CellValue;
                    String newCellValue;
                    String cellValueChange;
                    try
                    {
                        driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                        driverGC.FindElement(By.XPath("/html/body/div[3]/div[1]/div[1]/div[5]/div[1]/div/table[1]/tbody"));
                        for (int k = 1; k <= row_tr; k++)
                        {
                            for (int b = 1; b <= Column_td; b++)
                            {
                                driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                                CellValue = driverGC.FindElement(By.XPath("/html/body/div[3]/div[1]/div[1]/div[5]/div[1]/div/table[1]/tbody/tr[" + k + "]/td[" + b + "]")).Text.ToString();
                                if (CellValue == "Organic Search")
                                {
                                    driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                                    String mySiteName = driverGC.FindElement(By.XPath("/html/body/div[3]/div[1]/div[1]/div[1]/div[1]/h1")).Text.ToString();
                                    newCellValue = driverGC.FindElement(By.XPath("/html/body/div[3]/div[1]/div[1]/div[5]/div[1]/div/table[1]/tbody/tr[" + k + "]/td[3]")).Text.ToString();
                                    cellValueChange = Regex.Replace(newCellValue, @"[%\s]", string.Empty);
                                    float organicSearch = float.Parse(cellValueChange);
                                    if (organicSearch >= 50)
                                    {
                                        myStarterSites.Add(mySiteName);
                                        myStarterSites.Add(CellValue);
                                        myStarterSites.Add(newCellValue);
                                        Console.WriteLine(mySiteName);
                                        Console.WriteLine(CellValue);
                                        Console.WriteLine(newCellValue);
                                        using (StreamWriter writer = new StreamWriter(@"C:\Users\Justin\Desktop\newFile.txt"))
                                        {
                                            foreach (string s in myStarterSites)
                                            {
                                                writer.WriteLine(s);// Writes in next line
                                                                    //writer.WriteLine(" ");
                                            }
                                            writer.WriteLine("");
                                        }
                                    }
                                       
                                }
                            }
                        }
                    }
                    catch (OpenQA.Selenium.NoSuchElementException)
                    {
                        
                    }
                    //testing tables
                    driverGC.Navigate().Back();
                    driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    //write shit to file
                    siteLink = "";
                }
                j = 0;
                //mySites = null;
                try
                {
                    driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    driverGC.FindElement(By.XPath("//*[@id='searchBody']/div[2]/div[2]/div/a[3]")).Click();
                    //driverGC.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                }
                catch (ElementNotVisibleException)
                {
                    Console.WriteLine("No more pages");
                }
            }
            
            //MessageBox.Show("End");
            driverGC.Quit();
            Application.Exit();    
        }
    }
}
