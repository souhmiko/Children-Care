using Children_Care;
using Newtonsoft.Json;
using AForge.Imaging.Filters;
using Image = System.Drawing.Image;

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
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                _image = (Bitmap)Image.FromFile(Openfile.FileName);
                pictureBox1.Image = _image;

            }

            var file = _image;
            byte[] imBytes = FileReader.ImageToByteArray(_image);
            //convert _image(Bitmap) to byte array (byte[])
            //pass byte array to line below
            using (var content = new ByteArrayContent(imBytes))
            {
                // request API 
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                try
                {
                    var res = await client.PostAsync(predictionUrl, content);
                    var str = await res.Content.ReadAsStringAsync();
                    var myPredictionModel = JsonConvert.DeserializeObject<MyPredictionModel>(str);
                    var predictions = myPredictionModel.predictions;
                    int knife = 0;
                    int adult = 0;
                    int kid = 0;
                    foreach (var prediction in predictions)
                    {
                        if (prediction.probability > 0.7)
                        {
                            if (prediction.probability > 0.7 && prediction.tagName == "Knife")
                            {
                                
                                label2.Text = myPredictionModel.created.ToString();
                                knife += 1;
                                
                            }

                            else if (prediction.probability > 0.7 && prediction.tagName == "Adult")
                            {
                                
                                label2.Text = myPredictionModel.created.ToString();
                                adult++;

                            }

                            else if (prediction.probability > 0.7 && prediction.tagName == "Kid")
                            {
                                
                                label2.Text = myPredictionModel.created.ToString();
                                kid+= 1;
                            }

                        }
                        label1.Text = $"Knife: {knife}, Kid: {kid}, adult: {adult}";
                       

                        if (knife > 0 && kid > 0 && adult==0)
                        {
                            label3.Text = "Dangerous";
                        }
                        else
                        {
                            label3.Text = "Safe";
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
            public static byte[] ImageToByteArray(System.Drawing.Image image)
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    return ms.ToArray();
                }
            }
        }
    }
}
