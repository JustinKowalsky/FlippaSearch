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
            driverGC = new ChromeDriver(@"Z:\Justin\Documents\Visual Studio 2015\chromedriver_win32");
            driverGC.Navigate().GoToUrl("https://flippa.com/websites/starter-sites?sitetype=blog&uniques_per_month_min=1000");
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<IWebElement> starterSites = new List<IWebElement>();
            List<String> myStarterSites = new List<string>();;
            List<String> mySiteSearch = new List<string>();
            var numPages = (driverGC.FindElement(By.XPath("//*[@id='searchBody']/div[1]/div[1]/h2/span")).Text);
            double numberPages = int.Parse(Regex.Match(numPages, @"\d+", RegexOptions.RightToLeft).Value);
            numberPages = Math.Ceiling(numberPages / 50);
            int j;
            for (int i = 1; i <= numberPages; i++)
            {
                var mySites = driverGC.FindElements(By.CssSelector(".ListingResults___listingResult"));
                int size = 1;
                for (j = 0; j < size; ++j)
                {
                    mySites = driverGC.FindElements(By.CssSelector(".ListingResults___listingResult"));
                    size = mySites.Count();
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
                        driverGC.FindElement(By.XPath("/html/body/div[3]/div[1]/div[1]/div[5]/div[1]/div/table[1]/tbody"));
                        for (int k = 1; k <= row_tr; k++)
                        {
                            for (int b = 1; b <= Column_td; b++)
                            {
                                CellValue = driverGC.FindElement(By.XPath("/html/body/div[3]/div[1]/div[1]/div[5]/div[1]/div/table[1]/tbody/tr[" + k + "]/td[" + b + "]")).Text.ToString();
                                if (CellValue == "Organic Search")
                                {
                                    String mySiteName = driverGC.FindElement(By.XPath("/html/body/div[3]/div[1]/div[1]/div[1]/div[1]/h1")).Text.ToString();
                                    newCellValue = driverGC.FindElement(By.XPath("/html/body/div[3]/div[1]/div[1]/div[5]/div[1]/div/table[1]/tbody/tr[" + k + "]/td[3]")).Text.ToString();
                                    cellValueChange = Regex.Replace(newCellValue, @"[%\s]", string.Empty);
                                    float organicSearch = float.Parse(cellValueChange);
                                    int searchPercentFinal = Convert.ToInt32(searchPercent.Text);
                                    if (organicSearch >= searchPercentFinal)
                                    {
                                        myStarterSites.Add(mySiteName);
                                        //myStarterSites.Add(CellValue);
                                        myStarterSites.Add(newCellValue);
                                        Console.WriteLine(mySiteName);
                                        //Console.WriteLine(CellValue);
                                        Console.WriteLine(newCellValue);
                                        using (StreamWriter writer = new StreamWriter(@"C:\Users\Justin\Desktop\newFile.txt"))
                                        {
                                            foreach (string s in myStarterSites)
                                            {
                                                writer.WriteLine(s); 
                                            }
                                        }
                                    }
                                       
                                }
                            }
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        continue;
                    }
                    //testing tables
                    driverGC.Navigate().Back();
                    driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    //write shit to file
                    siteLink = null;
                }
                mySites = null;
                try
                {
                    driverGC.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    //driverGC.FindElement(By.XPath("//*[@id='searchBody']/div[2]/div[2]/div/a[3]")).Click();
                    driverGC.FindElement(By.XPath("//*[@id='searchBody']/div[2]/div[2]/div/a[3]")).Click(); // go to next page
                    WebDriverWait wait = new WebDriverWait(driverGC, TimeSpan.FromSeconds(5));
                    wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector(".Pagination___pageLink  Pagination___prevLink"), (i + 1).ToString()));
                    //driverGC.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                    
                }
                catch (OpenQA.Selenium.WebDriverTimeoutException)
                {
                    continue;
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
