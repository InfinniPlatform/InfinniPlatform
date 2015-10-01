using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Api;
using Newtonsoft.Json.Linq;

namespace TestAppVersion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var signInApi = new InfinniSignInApi("localhost", "9900","1");
            signInApi.SignInInternal("Admin", "Admin", false);

            memoEdit1.Text = "Зарегистрировались под Admin";

            var customServiceApi = new InfinniCustomServiceApi("localhost", "9900", "1");
            customServiceApi.CookieContainer = signInApi.CookieContainer;
            var result = customServiceApi.ExecuteAction("TestConfig", "common", "Helloworld", new { });

            memoEdit1.Text = memoEdit1.Text + Environment.NewLine + result.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var signInApi = new InfinniSignInApi("localhost", "9900", "1");
            signInApi.SignInInternal("Admin", "Admin", false);

            memoEdit1.Text = "Зарегистрировались под Admin";

            var customServiceApi = new InfinniCustomServiceApi("localhost", "9900","1");
            customServiceApi.CookieContainer = signInApi.CookieContainer;
            var result = customServiceApi.ExecuteAction("systemconfig", "metadata", "getirrelevantversions", new
                {
                    UserName = "Admin"
                });

            foreach (var data in result)
            {
                memoEdit1.Text = memoEdit1.Text + Environment.NewLine + data.ToString();    
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Укажите версию в формате x.y.z.i");
                return;
            }

            var signInApi = new InfinniSignInApi("localhost", "9900", "1");
            signInApi.SignInInternal("Admin", "Admin", false);

            memoEdit1.Text = "Зарегистрировались под Admin";

            var version =
                new
                {
                    ConfigurationId = "TestConfig",
                    Version = textBox1.Text
                };

            var versionApi = new InfinniVersionApi("localhost", "9900","1");
            dynamic result = versionApi.SetRelevantVersion("Admin", version);

            memoEdit1.Text = memoEdit1.Text + Environment.NewLine + result.ToString();
            
        }
    }
}
