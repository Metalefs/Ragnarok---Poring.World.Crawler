using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Persistency.Database;
using Utilities;
using Utilities.Files;
using Domain;
using CrawlerBase;
using OpenQA.Selenium;
using System.IO;
using System.Reflection;

namespace Poring
{
    public class PoringCrawler
    {
        public static string PastaDoExecutavel => @"C:\ChromeDriver";
        private readonly static string URL_Base = Caminhos.URL_Poring_Crawler;

        public static CrawlerSelenium Crawler { get; private set; }

        public static void Setup()
        {
            Crawler = new CrawlerSelenium(PastaDoExecutavel
               , Caminhos.CaminhoLog
               , true);
        }

        public static void Start()
        {
            Setup();
            DBConnection MongoConnection = new DBConnection();
            try
            {
                List<Item> Items = GetItemDivsFromPage();
                MongoConnection.InserirVarios(Items, "Items");
                ManipuladorArquivoTexto.EscreverArquivo(string.Format("{0} registros inseridos no banco ás {1}", Items.Count, DateTime.Now.ToShortTimeString() + " - " + DateTime.Now.ToShortDateString()), Caminhos.CaminhoLog);
                //ManipuladorArquivoTexto.EscreverArquivo(string.Format("{0}" Items.ToString() ), Caminhos.CaminhoItens);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Crawler.Close();
        }

        private static List<Item> GetItemDivsFromPage()
        {
            List<Item> Items = new List<Item>();
            Crawler.Driver.Navigate().GoToUrl(Caminhos.URL_Poring_Crawler);
            Console.WriteLine("Esperando carregamento da página");
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("Buscando itens");
            List<IWebElement> itens = Crawler.Driver.FindElements(By.XPath("//div[@class='jss312 jss285']")).ToList();
            Console.WriteLine($"{itens.Count} itens encontrados");

            for(int i = 0; i < itens.Count; i++)
            {
                string Nome, Preco, Percent, Stock, Buyers, IsSnap, SnapEnd = string.Empty;
                Console.WriteLine(itens[i].Text);
                List<string> Values = itens[i].Text.Split("\n").ToList();
                try
                {
                    Nome = Values[0];
                    Percent = Values[1];
                    IsSnap = Values[2];
                    Preco = Values[4];
                    Stock = Values[6];
                    Buyers = Values[8];
                    SnapEnd = Values[10];
                    string[] NumerosPreco = Regex.Split(Preco, Patterns.NumberPattern);
                    string PrecoSemVirgula = string.Join("", NumerosPreco);
                    float PrecoFinal = float.Parse(PrecoSemVirgula);
                    Item Item = new Item
                    {
                        Model = Nome,
                        Price = PrecoFinal,
                        Percent = Percent,
                        Buyers = Buyers,
                        IsSnap = IsSnap,
                        SnapEnd = SnapEnd,
                        //ImageURL = itens[i].FindElement(By.TagName("img")).GetAttribute("src")
                    };
                    Items.Add(Item);
                }
                catch (Exception) { continue; }
            }
        
            return Items;
        }
       
    }

}
