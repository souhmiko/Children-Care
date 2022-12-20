// See https://aka.ms/new-console-template for more information
using Children_Care;
using Newtonsoft.Json;

namespace Children_Care
{
    public partial class Form1 : Form
    {
        static string predictionKey = "bc2d716dffc04ca6b78b27595fb85871";
        static string predictionUrl = "https://southeastasia.api.cognitive.microsoft.com/customvision/v3.0/Prediction/782c771b-12a0-4c7a-8f02-31d1435f34b0/detect/iterations/Iteration8/image";
        private Bitmap _image;

        public Form1()
        {
            InitializeComponent();
        }

        async void ChildrenCare()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);
            
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK
            {
                _image = (Bitmap)Image.FromFile(Openfile.FileName);
                pictureBox1.Image = _image;

            }
            //convert _image(Bitmap) to byte array (byte[])
            //pass byte array to line below
            using (var content = new ByteArrayContent(imgBytes))
            {
                // request API 
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                try
                {
                    var res = await client.PostAsync(predictionUrl, content);
                    var str = await res.Content.ReadAsStringAsync();
                    var myPredictionModel = JsonConvert.DeserializeObject<MyPredictionModel>(str);
                    var predictions = myPredictionModel.predictions;
                    foreach (var prediction in predictions)
                    {
                        if (prediction.probability > 0.7)
                        {
                            //100px by 100px
                            //prediction.boundingBox 0.75
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChildrenCare();

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        public class FileReader
        {
            public static byte[] ReadFully(Stream input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }
    }
}

