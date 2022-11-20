using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Timer = System.Timers.Timer;
using Quartz;
using Quartz.Impl;
using Newtonsoft.Json;
using RestSharp;
using KarelSeviceFormApp.Model;
using KarelSeviceFormApp.VM_Model;
using KarelSeviceFormApp;

namespace KarelSeviceFormApp
{
    //[RunInstaller(true)]

    public partial class Form1 : Form
    {
        private Timer timer;
        public Form1()
        {
            InitializeComponent();
          
            anketDataList.ColumnCount = 5;

            anketDataList.Columns[0].Name = "CampID";
            anketDataList.Columns[1].Name = "CustName";
            anketDataList.Columns[2].Name = "CustPhone";
            anketDataList.Columns[3].Name = "CustData1";
            anketDataList.Columns[4].Name = "DurumType";


            //forEq with

            anketDataList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            anketDataList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            anketDataList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            anketDataList.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            anketDataList.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //try
            //{

       
            //string _path = "C:\\Users\\tronu\\OneDrive\\Masaüstü\\windows form app\\KarelSeviceFormApp\\KarelSeviceFormApp\\JsonDataDB\\ApiAddToCampaign.json";
            //var jsonData = System.IO.File.ReadAllText(_path);
            //var jsonDataDB = JsonConvert.DeserializeObject<List<ApiAddToCampJson>>(jsonData)
            //      ?? new List<ApiAddToCampJson>();
            //foreach (var item in jsonDataDB)
            //{
            //    anketDataList.Rows.Add(new ApiAddToCampJson
            //    {
            //        CampID = item.CampID,
            //        CustName = item.CustName,
            //        CustPhone = item.CustPhone,
            //        CustData1 = item.CustData1,
            //        DurumType = item.DurumType
            //    });
            //}
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //private void addRow(string CampId,string CustName,string CustPhone,string CustData1, bool DurumType)
        //{
        //    //String[] row = [CampId, CustName, CustPhone, CustData1, DurumType];

        //    anketDataList.Rows.Add(CampId, CustName, CustPhone, CustData1, DurumType);
        //}
        private void ApiAddToCampaign(string CampId, string CustId, string CustName, string CustPhone, string CustGsm, string CustData1, string DialStart, string DialEnd)
        {
            
            List<ApiAddToCampJson> errorApiDial = new List<ApiAddToCampJson>();
            string _path = "C:\\Users\\tronu\\OneDrive\\Masaüstü\\windows form app\\KarelSeviceFormApp\\KarelSeviceFormApp\\JsonDataDB\\ApiAddToCampaign.json";
            var jsonData = System.IO.File.ReadAllText(_path);
            errorApiDial = JsonConvert.DeserializeObject<List<ApiAddToCampJson>>(jsonData)
                  ?? new List<ApiAddToCampJson>();





            if ((errorApiDial.Count == 0) || (errorApiDial.Count > 0 && !errorApiDial.Select(x => x.CustPhone).Contains(CustPhone)) || (errorApiDial.Find(x => x.CustPhone == CustPhone).DurumType == false))
            {

                var client = new RestClient("http://10.19.12.12/ccm/wsdl/ccmapi.php");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml;charset=UTF-8");

                var body =
                    "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:ccmapiwsdl\">" +
                    " <soapenv:Header/>" +
                    "<soapenv:Body>" +
                    " <urn:ApiAddToCampaign soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                    "<CampId xsi:type=\"xsd:string\">" + CampId + "</CampId>" +
                    "<CustId xsi:type=\"xsd:string\">" + CustId + "</CustId>" +
                    " <CustName xsi:type=\"xsd:string\">" + CustName + "</CustName>" +
                    "<CustPhone xsi:type=\"xsd:string\">" + CustPhone + "</CustPhone>" +
                    " <CustPhone2 xsi:type=\"xsd:string\">?</CustPhone2>" +
                    "<CustGsm xsi:type=\"xsd:string\">" + CustGsm + "</CustGsm>" +
                    " <CustFax xsi:type=\"xsd:string\">?</CustFax>" +
                    "<CustPerson xsi:type=\"xsd:string\">?</CustPerson>" +
                    "<CustData1 xsi:type=\"xsd:string\">" + CustData1 + "</CustData1>" +
                    "<CustData2 xsi:type=\"xsd:string\">?</CustData2>" +
                    "<CustData3 xsi:type=\"xsd:string\">?</CustData3>" +
                    "<CustData4 xsi:type=\"xsd:string\">?</CustData4>" +
                    " <DialStart xsi:type=\"xsd:string\">" + DialStart + "</DialStart>" +
                    "<DialEnd xsi:type=\"xsd:string\">" + DialEnd + "</DialEnd>" +
                    "<CheckNumberExist xsi:type=\"xsd:string\">?</CheckNumberExist>" +
                    "</urn:ApiAddToCampaign>" +
                    "</soapenv:Body>" +
                    "</soapenv:Envelope>";

                request.AddParameter("text/xml;charset=UTF-8", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);
                XmlNodeList elemList = doc.GetElementsByTagName("return");


                foreach (XmlNode chldNode in elemList)
                {

                    var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(errorApiDial);

                    if (chldNode.ChildNodes[0].InnerText.StartsWith("ERR"))
                    {

                        try
                        {

                            //string text = File.ReadAllText(_path);
                            //var persons = JsonSerializer.Deserialize<ErrorApiDial>(text);


                            if (!errorApiDial.Select(x => x.CustPhone).Contains(CustPhone))
                            {
                                errorApiDial.GroupBy(x => x.CustPhone).Select(x => x.First()).ToList();


                                errorApiDial.Add(new ApiAddToCampJson
                                {
                                    CampID = CampId,
                                    CustName = CustName,
                                    CustPhone = CustPhone,
                                    CustData1 = CustData1,
                                    DurumType = false,

                                });

                                //DataGridView dataGridView = new DataGridView();

                                anketDataList.Rows.Add(new ApiAddToCampJson
                                {
                                    CampID = CampId,
                                    CustName = CustName,
                                    CustPhone = CustPhone,
                                    CustData1 = CustData1,
                                    DurumType = false
                                });



                                if (!File.Exists(_path))
                                {
                                    // Create a file to write to.
                                    File.WriteAllText(_path, JsonConvert.SerializeObject(errorApiDial));
                                }
                                File.WriteAllText(_path, JsonConvert.SerializeObject(errorApiDial));
                            }




                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    else if (chldNode.ChildNodes[0].InnerText.StartsWith("OK"))
                    {
                        try
                        {



                            if (errorApiDial.Count > 0)
                            {
                                if (errorApiDial.Select(x => x.CustPhone).Contains(CustPhone))
                                {

                                    errorApiDial.Find(x => x.CustPhone == CustPhone).DurumType = true;

                                    File.WriteAllText(_path, JsonConvert.SerializeObject(errorApiDial));

                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else if (errorApiDial.Count > 0)
                            {
                                if (!errorApiDial.Select(x => x.CustPhone).Contains(CustPhone))
                                {
                                    errorApiDial.Add(new ApiAddToCampJson
                                    {
                                        CampID = CampId,
                                        CustName = CustName,
                                        CustPhone = CustPhone,
                                        CustData1 = CustData1,
                                        DurumType = true
                                    });
                                }
                            }
                            else if (errorApiDial.Count == 0)
                            {
                                errorApiDial.Add(new ApiAddToCampJson
                                {
                                    CampID = CampId,
                                    CustName = CustName,
                                    CustPhone = CustPhone,
                                    CustData1 = CustData1,
                                    DurumType = true
                                });
                            }


                        }
                        catch (Exception ex)
                        {


                        }
                    }
                }

            }
        }

        
        private void EneryaGazComdata(string TARIH)
        {
            try
            {

                var client = new RestClient("http://egpoq00.enr.local:50000/XISOAPAdapter/MessageServlet?senderParty=&senderService=COMDATA_IVR&receiverParty=&receiverService=&interface=SI_CCAnketArama_Sync_OB&interfaceNamespace=http://energaz.com/ComdataCallCenterAnketArama");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("SOAPAction", "http://sap.com/xi/WebService/soap1.1");
                request.AddHeader("Content-Type", "text/xml;charset=UTF-8");
                request.AddHeader("Authorization", "Basic UkZDVVNFUjphQTEyMzQ1Ng==");
                request.AddHeader("Cookie", "JSESSIONID=0K6u_4yjCDCZngpMxM-7V8_FMukThAESj4YA_SAPRhn5SsC-YaW0xTJMTNpYNpvH; JSESSIONMARKID=NMn5CQu3X5Apk_e955bHYrOL9NQRTLGwYzGhKPhgA; saplb_*=(J2EE8818420)8818450");

                var body = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:rfc:functions\">" +
                    "<soapenv:Header/>" +
                    " <soapenv:Body>" +
                    "<urn:ZMD_WS_USER_PRS_BULK>" +
                    "<!--You may enter the following 4 items in any order-->" +
                    "<!--Optional:-->" +
                    "<BUKRS>3000</BUKRS>" +
                    "<!--Optional:-->" +
                    "<FLG></FLG>" +
                    "<!--Optional:-->" +
                    "<TARIH>" + TARIH + "</TARIH>" +
                    " <!--Optional:-->" +
                    "<GT_DATA>" +
                    "<!--Zero or more repetitions:-->" +
                    "<item>" +
                    "<!--Optional:-->" +
                    "<MANDT></MANDT>" +
                    "<!--Optional:-->" +
                    "<TARIH></TARIH>" +
                    "<!--Optional:-->" +
                    "<SOZHESAP></SOZHESAP>" +
                    "<!--Optional:-->" +
                    "<SURECADI></SURECADI>" +
                    "<!--Optional:-->" +
                    "<PERADSOYAD></PERADSOYAD>" +
                    "<!--Optional:-->" +
                    "<MUSADSOYAD></MUSADSOYAD>" +
                    "<!--Optional:-->" +
                    "<CEPTEL></CEPTEL>" +
                    "<!--Optional:-->" +
                    "<USER></USER>" +
                    "<!--Optional:-->" +
                    "<LOKASYON></LOKASYON>" +
                    "<!--Optional:-->" +
                    "<SEHIR></SEHIR>" +
                    "</item>" +
                    "</GT_DATA>" +
                    "</urn:ZMD_WS_USER_PRS_BULK>" +
                    "</soapenv:Body>" +
                    "</soapenv:Envelope>";


                request.AddParameter("text/xml;charset=UTF-8", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                List<Enerya_Anket_Arama> anketAramaObList = new List<Enerya_Anket_Arama>();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);
                XmlNodeList elemList = doc.GetElementsByTagName("item");



                foreach (XmlNode chldNode in elemList)
                {
                    anketAramaObList.Add(new Enerya_Anket_Arama
                    {
                        //Id = int.Parse(chldNode.InnerText),
                        MANDT = chldNode.ChildNodes[0].InnerText,
                        TARIH = DateTime.Parse(chldNode.ChildNodes[1].InnerText),
                        SOZHESAP = chldNode.ChildNodes[2].InnerText,
                        SURECADI = chldNode.ChildNodes[3].InnerText,
                        PERADSOYAD = chldNode.ChildNodes[4].InnerText,
                        MUSADSOYAD = chldNode.ChildNodes[5].InnerText,
                        CEPTEL = chldNode.ChildNodes[6].InnerText,
                        USER = chldNode.ChildNodes[7].InnerText,
                        LOKASYON = chldNode.ChildNodes[8].InnerText,
                        SEHIR = chldNode.ChildNodes[9].InnerText,


                    });
                }

                #region Zaman Ayar
                FullTime fullTime = new FullTime();

                DateTime startTime = Convert.ToDateTime(fullTime.startTime);
                DateTime endTime = Convert.ToDateTime(fullTime.endTime);
                DateTime startTime2 = Convert.ToDateTime(fullTime.startTime2);
                DateTime endTime2 = Convert.ToDateTime(fullTime.endTime2);
                #endregion


                var anketListDinstinc = anketAramaObList.GroupBy(x => x.CEPTEL).Select(x => x.First()).ToList();


                //string _path = "C:\\Program Files (x86)\\AHL Karel otomatik numara servisi\\JsonFile\\errorJsonFile.json";
                string _path = "C:\\Users\\tronu\\OneDrive\\Masaüstü\\windows form app\\KarelSeviceFormApp\\KarelSeviceFormApp\\JsonDataDB\\ApiAddToCampaign.json";
                foreach (var item in anketListDinstinc)
                {

                    var jsonData = System.IO.File.ReadAllText(_path);
                    var errorApiDial = JsonConvert.DeserializeObject<List<ApiAddToCampJson>>(jsonData)
                          ?? new List<ApiAddToCampJson>();


                    if (item.CEPTEL.StartsWith("+9"))
                    {

                        if (errorApiDial.Select(x => x.CustPhone).Contains(item.CEPTEL.Substring(2)))
                        {

                            if (errorApiDial.Find(x => x.CustPhone == item.CEPTEL.Substring(2)).DurumType != true)
                            {
                                if ((DateTime.Now > startTime && DateTime.Now < endTime) || (DateTime.Now > startTime2 && DateTime.Now < endTime2))
                                {
                                    ApiAddToCampaign("", "", item.MUSADSOYAD, item.CEPTEL.Substring(2), null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime2.ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                            }
                        }

                        else
                        {
                            if ((DateTime.Now > startTime && DateTime.Now < endTime) || (DateTime.Now > startTime2 && DateTime.Now < endTime2))
                            {
                                ApiAddToCampaign("", "", item.MUSADSOYAD, item.CEPTEL.Substring(2), null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime2.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                    }
                    else
                    {

                        if (errorApiDial.Select(x => x.CustPhone).Contains(item.CEPTEL))
                        {

                            if (errorApiDial.Find(x => x.CustPhone == item.CEPTEL).DurumType != true)
                            {
                                if ((DateTime.Now > startTime && DateTime.Now < endTime) || (DateTime.Now > startTime2 && DateTime.Now < endTime2))
                                {
                                    ApiAddToCampaign("", "", item.MUSADSOYAD, item.CEPTEL, null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime2.ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                            }
                        }

                        else
                        {
                            if ((DateTime.Now > startTime && DateTime.Now < endTime) || (DateTime.Now > startTime2 && DateTime.Now < endTime2))
                            {
                                ApiAddToCampaign("", "", item.MUSADSOYAD, item.CEPTEL, null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime2.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                    }


                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void PersonelBazliCagri(string TARIH)
        {
            try
            {


                var client = new RestClient("http://egpoq00.enr.local:50000/XISOAPAdapter/MessageServlet?senderParty=&senderService=COMDATA&receiverParty=&receiverService=&interface=SI_PersonelBazliCagri_OB&interfaceNamespace=http://energaz.com/CRM/PersonelBazliCagri");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("SOAPAction", "http://sap.com/xi/WebService/soap1.1");
                request.AddHeader("Content-Type", "text/xml;charset=UTF-8");
                request.AddHeader("Authorization", "Basic UkZDVVNFUjphQTEyMzQ1Ng==");
                request.AddHeader("Cookie", "JSESSIONID=Ho6zU3lUfMEVkmNdPqCcQJbc5btVhAESj4YA_SAPSA-DTO07AwjnEQGUTxrBQ-No; JSESSIONMARKID=JROs9gednb9dv2CR50zdcRUOSYFMiK5kOXHBKPhgA; saplb_*=(J2EE8818420)8818450");
                var body =
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:rfc:functions\">" +
                "<soapenv:Header/>" +
                "<soapenv:Body>" +
                "<urn:ZCRM_PERSONEL_BAZLI_CAGRI_WS>" +
                "<!--You may enter the following 2 items in any order-->" +
                "<!--Optional:-->" +
                "<IV_KANAL></IV_KANAL>" +
                "<IV_TARIH>" + TARIH + "</IV_TARIH>" +
                "</urn:ZCRM_PERSONEL_BAZLI_CAGRI_WS>" +
                "</soapenv:Body>" +
                "</soapenv:Envelope>";

                request.AddParameter("text/xml;charset=UTF-8", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                List<PersonelBazliCagriAnketArama> personelBazliobList = new List<PersonelBazliCagriAnketArama>();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);
                XmlNodeList elemList = doc.GetElementsByTagName("item");





                foreach (XmlNode chldNode in elemList)
                {
                    personelBazliobList.Add(new PersonelBazliCagriAnketArama
                    {
                        //Id = int.Parse(chldNode.InnerText),
                        ISLEM_NO = chldNode.ChildNodes[0].InnerText,
                        TARIH = DateTime.Parse(chldNode.ChildNodes[1].InnerText),
                        SAAT = chldNode.ChildNodes[2].InnerText,
                        GRUP = chldNode.ChildNodes[3].InnerText,
                        AGRUP = chldNode.ChildNodes[4].InnerText,
                        ORGANIZASYON = chldNode.ChildNodes[5].InnerText,
                        KULLANICI = chldNode.ChildNodes[6].InnerText,
                        MUHATAP = chldNode.ChildNodes[7].InnerText,
                        AD_SOYAD = chldNode.ChildNodes[8].InnerText,
                        TELEFON = chldNode.ChildNodes[9].InnerText,
                        CAGRI_TEL = chldNode.ChildNodes[10].InnerText,


                    });
                }

                #region Zaman Ayar
                FullTime fullTime = new FullTime();

                DateTime startTime = Convert.ToDateTime(fullTime.startTime);
                DateTime endTime = Convert.ToDateTime(fullTime.endTime);
                DateTime startTime2 = Convert.ToDateTime(fullTime.startTime2);
                DateTime endTime2 = Convert.ToDateTime(fullTime.endTime2);
                #endregion


                var personelBazListDinstinc = personelBazliobList.GroupBy(x => x.TELEFON).Select(x => x.First()).ToList();


                //string _path = "C:\\Program Files (x86)\\AHL Karel otomatik numara servisi\\JsonFile\\errorJsonFile.json";
                string _path = "C:\\Users\\tronu\\OneDrive\\Masaüstü\\windows form app\\KarelSeviceFormApp\\KarelSeviceFormApp\\JsonDataDB\\ApiAddToCampaign.json";

                foreach (var item in personelBazListDinstinc)
                {
                    var jsonData = System.IO.File.ReadAllText(_path);
                    var errorApiDial = JsonConvert.DeserializeObject<List<ApiAddToCampJson>>(jsonData)
                          ?? new List<ApiAddToCampJson>();


                    if (item.TELEFON.StartsWith("+9"))
                    {

                        if (errorApiDial.Select(x => x.CustPhone).Contains(item.TELEFON.Substring(2)))
                        {
                            if (errorApiDial.Find(x => x.CustPhone == item.TELEFON.Substring(2)).DurumType != true)
                            {
                                if ((DateTime.Now > startTime && DateTime.Now < endTime))
                                {
                                    ApiAddToCampaign("", "", item.AD_SOYAD, item.TELEFON.Substring(2), null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                else if ((DateTime.Now > startTime2 && DateTime.Now < endTime2))
                                {
                                    ApiAddToCampaign("", "", item.AD_SOYAD, item.TELEFON.Substring(2), null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime2.ToString("yyyy-MM-dd HH:mm:ss"));
                                }

                            }
                        }
                        else
                        {
                            if ((DateTime.Now > startTime && DateTime.Now < endTime))
                            {
                                ApiAddToCampaign("", "", item.AD_SOYAD, item.TELEFON.Substring(2), null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            else if ((DateTime.Now > startTime2 && DateTime.Now < endTime2))
                            {
                                ApiAddToCampaign("", "", item.AD_SOYAD, item.TELEFON.Substring(2), null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime2.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                    }
                    else
                    {

                        if (errorApiDial.Select(x => x.CustPhone).Contains(item.TELEFON))
                        {
                            if (errorApiDial.Find(x => x.CustPhone == item.TELEFON).DurumType != true)
                            {
                                if ((DateTime.Now > startTime && DateTime.Now < endTime))
                                {
                                    ApiAddToCampaign("", "", item.AD_SOYAD, item.TELEFON, null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

                                }
                                else if ((DateTime.Now > startTime2 && DateTime.Now < endTime2))
                                {
                                    ApiAddToCampaign("", "", item.AD_SOYAD, item.TELEFON, null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime2.ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                            }

                        }
                        else
                        {
                            if ((DateTime.Now > startTime && DateTime.Now < endTime))
                            {
                                ApiAddToCampaign("", "", item.AD_SOYAD, item.TELEFON, null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

                            }
                            else if ((DateTime.Now > startTime2 && DateTime.Now < endTime2))
                            {
                                ApiAddToCampaign("", "", item.AD_SOYAD, item.TELEFON, null, "POLLOB-5", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), endTime2.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                    }


                }


            }
            catch (Exception ex)
            {

            }
        }




       private void main()
        {
            DateTime nowTime = DateTime.Now;

            FullTime fullTime = new FullTime();

            DateTime startTime = Convert.ToDateTime(fullTime.startTime);
            DateTime endTime = Convert.ToDateTime(fullTime.endTime);
            DateTime startTime2 = Convert.ToDateTime(fullTime.startTime2);
            DateTime endTime2 = Convert.ToDateTime(fullTime.endTime2);




            Thread.Sleep(1000);
            try
            {
                if ((DateTime.Now > startTime && DateTime.Now < endTime))
                {
                    var TARIH = DateTime.Now.ToString("yyyy-MM-dd");
                    EneryaGazComdata("2021-09-09");
                    PersonelBazliCagri("2021-08-01");
                }
                else if ((DateTime.Now > startTime2 && DateTime.Now < endTime2))
                {
                    var TARIH = DateTime.Now.ToString("yyyy-MM-dd");
                    EneryaGazComdata("2021-09-09");
                    PersonelBazliCagri("2021-08-01");

                }
            }
            catch (Exception ex)
            {
            }

        }


        static public void clearFileJson()
        {
            DateTime nowTime = DateTime.Now;

            FullTime fullTime = new FullTime();


            DateTime startClearTime = Convert.ToDateTime(fullTime.startClearTime);
            DateTime endClearTime = Convert.ToDateTime(fullTime.endClearTime);



            Thread.Sleep(1000);
            try
            {


                if (DateTime.Now > startClearTime && DateTime.Now < endClearTime)
                {

                    string _path = "C:\\Users\\tronu\\OneDrive\\Masaüstü\\windows form app\\KarelSeviceFormApp\\KarelSeviceFormApp\\JsonDataDB\\ApiAddToCampaign.json";
                    var jsonData = System.IO.File.ReadAllText(_path);
                    var errorApiDial = JsonConvert.DeserializeObject<List<ApiAddToCampJson>>(jsonData)
                          ?? new List<ApiAddToCampJson>();

                    File.Delete(_path);
                    Thread.Sleep(2000);
                    File.Create(_path);
                }
          


            }
            catch (Exception ex)
            {
            }



        }


        private IScheduler Baslat()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler sched = schedFact.GetScheduler().Result;
            if (!sched.IsStarted)
            {
                sched.Start();
            }
            return sched;
        }


        private void startWithQuartz(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            IScheduler sched = Baslat();
            //IJobDetail OgledenOnce = JobBuilder.Create<OgledenOnce>().WithIdentity("OgledenOnce", null).Build();
            //IJobDetail OgledenSonra = JobBuilder.Create<OgledenSonra>().WithIdentity("OgledenSonra", null).Build();
            //IJobDetail ClearJsonFile = JobBuilder.Create<ClearJsonFile>().WithIdentity("ClearJsonFile", null).Build();

            FullTime fullTime = new FullTime();

            DateTime startTime = Convert.ToDateTime(fullTime.startTime);
            DateTime endTime = Convert.ToDateTime(fullTime.endTime);
            DateTime startTime2 = Convert.ToDateTime(fullTime.startTime2);
            DateTime endTime2 = Convert.ToDateTime(fullTime.endTime2);


            DateTime startClearTime = Convert.ToDateTime(fullTime.startClearTime);
            DateTime endClearTime = Convert.ToDateTime(fullTime.endClearTime);
            try
            {
                if (DateTime.Now > startTime && DateTime.Now < endTime)
                {
                    //ISimpleTrigger TriggerGorev = (ISimpleTrigger)TriggerBuilder.Create().WithIdentity("OgledenOnce").WithSimpleSchedule(x => x.WithIntervalInMinutes(30).RepeatForever()).EndAt(endTime).Build();
                    //await sched.ScheduleJob(OgledenOnce, TriggerGorev);
                    main();

                }
                else if (DateTime.Now > startTime2 && DateTime.Now < endTime2)
                {
                    //ISimpleTrigger TriggerGorev2 = (ISimpleTrigger)TriggerBuilder.Create().WithIdentity("OgledenSonra").WithSimpleSchedule(x => x.WithIntervalInMinutes(30).RepeatForever()).EndAt(endTime2).Build();
                    //await sched.ScheduleJob(OgledenSonra, TriggerGorev2);
                    main();

                }
                else if (DateTime.Now > startClearTime && DateTime.Now < endClearTime)
                {
                    //ISimpleTrigger TriggerGorev3 = (ISimpleTrigger)TriggerBuilder.Create().WithIdentity("ClearJsonFile").StartAt(startClearTime).WithSimpleSchedule(x => x.WithIntervalInMinutes(30).RepeatForever()).EndAt(endClearTime).Build();
                    //await sched.ScheduleJob(ClearJsonFile, TriggerGorev3);
                    clearFileJson();
                }
            }
            catch (Exception)
            {
            }
            timer.Start();
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            timer = new Timer
            {
                Interval = 3000,
                AutoReset = true,

            };
            timer.Elapsed += startWithQuartz;
            timer.Start();

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Uygulamayı sonlandırmak istiyormusun ?", "Exit", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
           
                Application.Exit();
                
            }
            
        }

        private void anketDataList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }
}






//class OgledenOnce : IJob
//{
//    public Task Execute(IJobExecutionContext context)
//    {
//        try
//        {
//            Form1.main();
//        }
//        catch (Exception)
//        {

//        }
//        return Task.CompletedTask;
//    }
//}

//class OgledenSonra : IJob
//{
//    public Task Execute(IJobExecutionContext context)
//    {
//        try
//        {
//            Form1.main();
//        }
//        catch (Exception)
//        {

//        }
//        return Task.CompletedTask;
//    }
//}

//class ClearJsonFile : IJob
//{
//    public Task Execute(IJobExecutionContext context)
//    {
//        try
//        {
//            Form1.clearFileJson();
//        }
//        catch (Exception)
//        {

//        }
//        return Task.CompletedTask;
//    }
//}
