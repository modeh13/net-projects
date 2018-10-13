using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using WebViewTest.Interfaces;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebViewTest.ViewModel
{
    public class HtmlJsTestViewModel : BindableObject, INotifyPropertyChanged
    {
        private string UrlAssest;
        private ISaveAndLoad FileManager;

        #region Properties
        private HtmlWebViewSource htmlWebViewSource;

        public HtmlWebViewSource HtmlWebViewSource
        {
            get { return htmlWebViewSource; }
            set
            {
                htmlWebViewSource = value;
                OnPropertyChange();
            }
        }

        private UrlWebViewSource urlWebViewSource;

        public UrlWebViewSource UrlWebViewSource
        {
            get { return urlWebViewSource; }
            set
            {
                urlWebViewSource = value;
                OnPropertyChange();
            }
        }

        public Func<string, Task<string>> EvaluateJavascript { get; set; }
        public Action GoBack { get; set; }        

        private Action _refresh;
        public Action Refresh
        {
            get { return _refresh; }
            set { _refresh = value; }
        }

        private ICommand loadFormCommand;

        public ICommand LoadFormCommand
        {
            get { return loadFormCommand; }
            set { loadFormCommand = value; }
        }

        private ICommand loadFormWithResourcesCommand;

        public ICommand LoadFormWithResourcesCommand
        {
            get { return loadFormWithResourcesCommand; }
            set { loadFormWithResourcesCommand = value; }
        }

        private ICommand loadMintFormCommand;

        public ICommand LoadMintFormCommand
        {
            get { return loadMintFormCommand; }
            set { loadMintFormCommand = value; }
        }

        public ICommand GoBackCommand
        {
            get
            {
                return new Command(() => { GoBack(); });
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() => { Refresh(); });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion

        public HtmlJsTestViewModel()
        {
            LoadFormCommand = new Command(async () => await LoadForm());
            LoadFormWithResourcesCommand = new Command(async () => await LoadFormWithResources());
            LoadMintFormCommand = new Command(async () => await LoadMintForm());
            htmlWebViewSource = new HtmlWebViewSource();
            UrlWebViewSource = new UrlWebViewSource();
            FileManager = DependencyService.Get<ISaveAndLoad>();

            if (Device.RuntimePlatform == Device.iOS) {                
                UrlWebViewSource.Url = "";                
            }

            //UrlAssest = "https://doc-08-8o-docs.googleusercontent.com/docs/securesc/ha0ro937gcuc7l7deffksulhg5h7mbp1/806i5vtdk08ktk4ns9phuufun1mkr0j0/1521756000000/03041252391972702159/*/1S_uc70fZt5z5jbLz5Kc81Tww58AWVQCq?e=download";            
        }

        private string GetHtmlInternal()
        {
            StringBuilder html = new StringBuilder();

            html.Append("<html>");
            html.Append("<head>");
            html.Append("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            html.Append("<title>Html Js - Test</title>");
            html.Append("<script type='text/javascript' lang='javascript' src='index.js'></script>");
            html.Append("<script type='text/javascript'>");
            html.Append("function btnClick() { var text = document.getElementById('inpMessage').value; showMessage(text); }");
            html.Append("</script>");
            html.Append("</head>");
            html.Append("<body>");
            html.Append("<h1>Prueba de cargar Html and Js string from Internal Storage.</h1>");
            html.Append("<input id='inpMessage' type='text' value='' />");
            html.Append("<input type='button' value='JS' onclick='btnClick();' />");
            html.Append("@HTML");
            html.Append("</body>");
            html.Append("</html>");
            return html.ToString();
        }

        private async Task LoadForm()
        {
            string js = "function loadPage(){" +
                "var dv = document.createElement('div'); " +
                "dv.id='dvPrueba'; " +
                "dv.innerHTML = 'Esto es un DIV agregado por la función JS agregado a través de un String.'; " +
                "document.body.appendChild(dv); " +
                "alert('Se ha agregado el contenido correctament !!');" +
                "}";

            //fileManager.SaveText("js/prueba.js", js);

            StringBuilder html = new StringBuilder();
            html.Append("<html>");
            html.Append("<head>");
            html.Append("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            html.Append("<title>Html Js - Test</title>");
            html.Append("<link rel='stylesheet' href='Content/css/font-awesome.min.css' />");
            html.Append("<link rel='stylesheet' href='Content/css/bootstrap.min.css' />");
            html.Append("<script type='text/javascript' lang='javascript' src='js/prueba.js'></script>");
            //html.Append("<script type='text/javascript' lang='javascript' src='Content/js/functions.js'></script>");
            html.Append("<script type='text/javascript' lang='javascript' src='Content/js/jquery.min.js'></script>");
            html.Append("<script type='text/javascript' lang='javascript' src='Content/js/bootstrap.min.js'></script>");
            html.Append("<script type='text/javascript'>");
            //html.Append(js);
            html.Append("</script>");
            html.Append("</head>");
            html.Append("<body onload='loadPage()'>");
            html.Append("<h1>Prueba de cargar Html and Js string.</h1>");
            html.Append("<div class='container'>");
            html.Append("<div class='row'>");
            html.Append("<div class='col-6'>Left</div>");
            html.Append("<div class='col-6'>Right</div>");            
            html.Append("</div>");
            html.Append("</div>");
            html.Append("</body>");
            html.Append("</html>");
            //fileManager.SaveText("index.html", html.ToString());

            string htmlTemplate = FileManager.LoadText("cleanTemplate1.html", true);
            string htmlForm = FileManager.LoadText("example3.html", true);
            htmlTemplate = htmlTemplate.Replace("@HTML", htmlForm);

            //Assests
            //HtmlWebViewSource.BaseUrl = fileManager.GetBaseUrl();
            //htmlWebViewSource.Html = htmlTemplate.ToString();            

            //Internal Storage
            string templateHTML = GetHtmlInternal();
            string htmlTest = "<script type='text/javascript'>alert('Alert prueba.')</script>";
            templateHTML = templateHTML.Replace("@HTML", htmlTest);

            FileManager.SaveText("index.js", "function showMessage(text) { alert('Este es su mensaje:' + text); }");
            FileManager.SaveText("index.html", templateHTML);

            //To Load HTML from string.
            HtmlWebViewSource.BaseUrl = FileManager.GetAppBaseUrl();
            HtmlWebViewSource.Html = FileManager.LoadText("index.html");

            //To Load HTML from file.
            UrlWebViewSource.Url = FileManager.GetAppBaseUrl() + "index.html";

            //await EvaluateJavascript("alert('JS launched through Eval function.')");
            await EvaluateJavascript("console.log('JS launched through Eval function.')");
        }

        private async Task LoadFormWithResources()
        {            
            const string apiVersion = "11.0";

            //FileManager.DeleteFolder(apiVersion);

            if (!FileManager.ExistsFolder(apiVersion))
            {                
                await FileManager.DownloadFile(UrlAssest, apiVersion + ".zip"); //Get Resouces from URL
            }

            //Internal Storage
            //string templateHTML = GetHtmlInternal();
            //string htmlTest = "<script type='text/javascript'>alert('Alert prueba.')</script>";
            //templateHTML = templateHTML.Replace("@HTML", htmlTest);

            //FileManager.SaveText("index.js", "function showMessage(text) { alert('Este es su mensaje:' + text); }");
            //FileManager.SaveText("index.html", templateHTML);

            ////To Load HTML from string.
            //HtmlWebViewSource.BaseUrl = FileManager.GetAppBaseUrl();
            //HtmlWebViewSource.Html = FileManager.LoadText("index.html");
            //UrlWebViewSource.Url = Path.Combine(FileManager.GetAppBaseUrl(), "index.html");

            // - - - - - - - - - - - - - - - - - - - - - -
            //Read files with Resources
            string pathTemplate = Path.Combine(apiVersion, "form-offline", "template.html");
            string pathForm = Path.Combine(apiVersion, "form-offline", "index.html");

            string templateHTML = FileManager.LoadText(pathTemplate);
            templateHTML = templateHTML.Replace("##HTML-FORM##", "<div id='dvTest'>Probando !! ...</div><script type='text/javascript'>alert($('#dvTest').text())</script>");
            FileManager.SaveText(pathForm, templateHTML);

            //Read file from internal storage
            string baseUrl = string.Empty;
            string formHtml = FileManager.LoadText(pathForm);

            if (Device.RuntimePlatform == Device.Android)
            {
                //baseUrl = FileManager.GetAppBaseUrl(); // --> Path source = 11.0/js/... --- OK
                //baseUrl = Path.Combine(FileManager.GetAppBaseUrl(), apiVersion) + "/"; // --> Path source = css/                
                baseUrl = Path.Combine(FileManager.GetAppBaseUrl(), apiVersion, "form-offline") + "/"; // --> Path source = ../css/
            }
            else
            {
                //baseUrl = FileManager.GetAppBaseUrl(); // --> Path source = 11.0/js/... --- OK
                //baseUrl = Path.Combine(FileManager.GetAppBaseUrl(), apiVersion) + "/"; // --> Path source = css/                
                baseUrl = Path.Combine(FileManager.GetAppBaseUrl(), apiVersion, "form-offline") + "/"; // --> Path source = ../css/
            }

            //Using HTMLWebViewSource            
            HtmlWebViewSource.BaseUrl = baseUrl;
            HtmlWebViewSource.Html = formHtml;

            //Using URLWebViewSource
            //Create new instance "UrlWebViewSource" to refresh WebView if URL inicial = "".
            //UrlWebViewSource = new UrlWebViewSource();
            //UrlWebViewSource.Url = Path.Combine(FileManager.GetAppBaseUrl(), apiVersion, "form-offline", "index.html");
            // - - - - - - - - - - - - - - - - - - - - - -
            
            //await EvaluateJavascript("console.log('JS launched through Eval function.')");
        }

        private async Task LoadMintForm()
        {
            //OneDrive
            //UrlAssest = "https://mayasofting-my.sharepoint.com/personal/german_ramirez_mayasoft_com_co/_layouts/15/download.aspx?SourceUrl=%2Fpersonal%2Fgerman%5Framirez%5Fmayasoft%5Fcom%5Fco%2FDocuments%2FMint%20Grading%2Fform%5Fassets12%2E0%2Ezip";
            //string formUrl = "https://mayasofting-my.sharepoint.com/personal/german_ramirez_mayasoft_com_co/_layouts/15/download.aspx?SourceUrl=%2Fpersonal%2Fgerman%5Framirez%5Fmayasoft%5Fcom%5Fco%2FDocuments%2FMint%20Grading%2FHtmlForm%2Etxt";

            //GoogleDrive
            UrlAssest = "https://qa.mint-online.com/trunk/form-assets/form-assets.zip";
            string formUrl = "https://doc-0c-8o-docs.googleusercontent.com/docs/securesc/ha0ro937gcuc7l7deffksulhg5h7mbp1/0iioeec6d25ln64vs5tts9k2flrf59d9/1523023200000/03041252391972702159/*/1cZd-vjvQaCiMHU-xt-wV5Ktz1JsiHDMg?e=download";

            const string apiVersion = "12.1";
            const string templateName = "MintFormTemplateApp.html";
            const string formName = "index.html";
            const string formText = "HtmlForm.txt";

            string pathTemplate = Path.Combine(apiVersion, "template-form-app", templateName);
            string pathForm = Path.Combine(apiVersion, "template-form-app", formName);
            //string pathForm = Path.Combine(apiVersion, "template-form-app", "HtmlForm.txt");
            //string pathForm = Path.Combine(apiVersion, "template-form-app", "Json.txt");

            FileManager.DeleteFolder(apiVersion);
            if (!FileManager.ExistsFolder(apiVersion))
            {
                await FileManager.DownloadFile(UrlAssest, apiVersion + ".zip"); //Get Resouces from URL                
            }

            //await FileManager.DownloadFile(formUrl, pathForm);

            // - - - - - - - - - - - - - - - - - - - - - -
            //if (FileManager.ExistsFile(pathForm)) {
            //string jsonText = FileManager.LoadText(pathForm);
            //JObject json = JObject.Parse(jsonText);
            //MintRenderForm render = JsonConvert.DeserializeObject<MintRenderForm>(jsonText);
            //}

            //foreach (string directoryPath in FileManager.GetDirectories())
            //{
            //    System.Diagnostics.Debug.WriteLine("Directorio : " + directoryPath);
            //}

            // - - - - - - - - - - - - - - - - - - - - - -
            //Read files with Resources
            string templateHTML = FileManager.LoadText(pathTemplate);
            string formHTML = FileManager.LoadText(formText, true);
            //string formHTML = render.html;
            templateHTML = templateHTML.Replace("@-FORM-CONTENT-@", formHTML);
            FileManager.SaveText(pathForm, templateHTML);

            //Read file from internal storage
            //string baseUrl = string.Empty;
            //string formHtml = FileManager.LoadText(pathForm);

            //baseUrl = FileManager.GetAppBaseUrl(); // --> Path source = 11.0/js/... --- OK
            //baseUrl = Path.Combine(FileManager.GetAppBaseUrl(), apiVersion) + "/"; // --> Path source = css/                
            //baseUrl = Path.Combine(FileManager.GetAppBaseUrl(), apiVersion, "template-form-app") + "/"; // --> Path source = ../css/

            //Using HTMLWebViewSource            
            //HtmlWebViewSource.BaseUrl = baseUrl;
            //HtmlWebViewSource.Html = formHtml;

            //Using URLWebViewSource
            //Create new instance "UrlWebViewSource" to refresh WebView if URL inicial = "".
            UrlWebViewSource = new UrlWebViewSource();
            UrlWebViewSource.Url = Path.Combine(FileManager.GetAppBaseUrl(), apiVersion, "template-form-app", formName);
            // - - - - - - - - - - - - - - - - - - - - - -

            //await EvaluateJavascript("console.log('JS launched through Eval function.')");
        }
    }

    public class MintRenderForm {
        public string html { get; set; }
        public string[] validationError { get; set; }
        public string messageException { get; set; }
        public bool response { get; set; }
        public string nameException { get; set; }
    }
}