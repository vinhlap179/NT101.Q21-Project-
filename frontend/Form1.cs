using Guna.UI2.WinForms;

namespace frontend
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            // Gắn sự kiện bằng code để chắc chắn nút bấm hoạt động
            this.Load += FrmMain_Load;

            btnPlayFair.Click += btnPlayFair_Click;
            btnRSA.Click += btnRSA_Click;
            btnAbout.Click += btnAbout_Click;

        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            ShowPlayFairPage();
        }

        private void LoadUserControl(UserControl uc)
        {
            pnlContent.Controls.Clear();

            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);

            uc.BringToFront();
            pnlContent.BringToFront();
        }

        private void SetActiveButton(Guna2Button activeButton)
        {
            btnPlayFair.FillColor = Color.FromArgb(31, 41, 55);
            btnRSA.FillColor = Color.FromArgb(31, 41, 55);
            btnAbout.FillColor = Color.FromArgb(31, 41, 55);

            activeButton.FillColor = Color.FromArgb(37, 99, 235);
        }

        private void ShowPlayFairPage()
        {
            lblPageTitle.Text = "PlayFair Cipher";
            lblPageDesc.Text = "Encrypt and decrypt text using PlayFair algorithm";

            LoadUserControl(new UcPlayFair());
            SetActiveButton(btnPlayFair);
        }

        private void ShowRSAPage()
        {
            lblPageTitle.Text = "RSA Cipher";
            lblPageDesc.Text = "Generate keys, encrypt and decrypt text using RSA algorithm";

            LoadUserControl(new UcRSA());
            SetActiveButton(btnRSA);
        }

        private void ShowAboutPage()
        {
            lblPageTitle.Text = "About Project";
            lblPageDesc.Text = "Information about this project";

            LoadUserControl(new UcAbout());
            SetActiveButton(btnAbout);
        }

        private void btnPlayFair_Click(object sender, EventArgs e)
        {
            ShowPlayFairPage();
        }

        private void btnRSA_Click(object sender, EventArgs e)
        {
            ShowRSAPage();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            ShowAboutPage();
        }

        private void lblPageDesc_Click(object sender, EventArgs e)
        {
            // Để trống, tránh lỗi do lỡ double click label
        }
    }
}
