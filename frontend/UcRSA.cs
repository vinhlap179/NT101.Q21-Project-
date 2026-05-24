using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics; 

namespace frontend
{
    public partial class UcRSA : UserControl
    {
        public UcRSA()
        {
            InitializeComponent();
        }
        private bool IsPrime(BigInteger number)
        {
            if (number < 2) return false;
            if (number == 2 || number == 3) return true;
            if (number % 2 == 0) return false;

            for (BigInteger i = 3; i * i <= number; i += 2)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        private BigInteger GenerateE(BigInteger phiN)
        {
            BigInteger defaultE = 65537;
            if (defaultE < phiN && BigInteger.GreatestCommonDivisor(defaultE, phiN) == 1)
            {
                return defaultE;
            }

            for (BigInteger i = 3; i < phiN; i += 2)
            {
                if (BigInteger.GreatestCommonDivisor(i, phiN) == 1)
                {
                    return i;
                }
            }
            return 3;
        }

        private BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m, t, q;
            BigInteger x0 = 0, x1 = 1;

            if (m == 1) return 0;

            while (a > 1)
            {
                q = a / m;
                t = m;
                m = a % m;
                a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0) x1 += m0;
            return x1;
        }
        private void btnGenerateRSAKey_Click(object sender, EventArgs e)
        {
            try
            {
                BigInteger p = BigInteger.Parse(txtP.Text);
                BigInteger q = BigInteger.Parse(txtQ.Text);

                if (p == q)
                {
                    MessageBox.Show("Lỗi: Số p và q không được giống nhau!");
                    return;
                }
                if (!IsPrime(p) || !IsPrime(q))
                {
                    MessageBox.Show("Lỗi: p và q BẮT BUỘC phải là số nguyên tố!");
                    return;
                }

                BigInteger n = p * q;
                if (n < 128)
                {
                    MessageBox.Show("Lỗi: n = p×q phải lớn hơn 127!\n" +
                                    "Hãy chọn p và q lớn hơn (ví dụ: p=61, q=53)");
                    return;
                }
                txtN.Text = n.ToString();

                BigInteger phiN = (p - 1) * (q - 1);
                txtPhi.Text = phiN.ToString();

                BigInteger eKey = GenerateE(phiN);
                txtE.Text = eKey.ToString();

                BigInteger d = ModInverse(eKey, phiN);
                txtD.Text = d.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng chỉ nhập số nguyên vào ô p và q!");
            }
        }

        private void btnRSAEncrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtN.Text) || string.IsNullOrWhiteSpace(txtE.Text))
            {
                MessageBox.Show("Vui lòng tạo khóa (Generate Key) trước!");
                return;
            }

            BigInteger n = BigInteger.Parse(txtN.Text);
            BigInteger eKey = BigInteger.Parse(txtE.Text);
            string plainText = txtRSAInput.Text;

            StringBuilder cipherText = new StringBuilder();

            foreach (char c in plainText)
            {
                BigInteger m = (int)c;

                if (m >= n)
                {
                    MessageBox.Show($"Lỗi: Ký tự '{c}' (ASCII={m}) lớn hơn n={n}!\n" +
                                    "RSA không mã hóa được ký tự này.");
                    return;
                }

                BigInteger cResult = BigInteger.ModPow(m, eKey, n);

                cipherText.Append(cResult.ToString() + " ");
            }

            txtRSAOutput.Text = cipherText.ToString().Trim();
        }

        private void btnRSADecrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtN.Text) || string.IsNullOrWhiteSpace(txtD.Text))
            {
                MessageBox.Show("Vui lòng tạo khóa (Generate Key) hoặc nhập n, d trước!");
                return;
            }

            try
            {
                BigInteger n = BigInteger.Parse(txtN.Text);
                BigInteger d = BigInteger.Parse(txtD.Text);

                string[] cipherNumbers = txtRSAInput.Text.Trim().Split(' ');
                StringBuilder plainText = new StringBuilder();

                foreach (string numStr in cipherNumbers)
                {
                    if (!string.IsNullOrWhiteSpace(numStr))
                    {
                        BigInteger c = BigInteger.Parse(numStr);
                        BigInteger mResult = BigInteger.ModPow(c, d, n);

                        plainText.Append((char)(int)mResult);
                    }
                }

                txtRSAOutput.Text = plainText.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đầu vào không phải là bản mã hợp lệ!\nLỗi: " + ex.Message);
            }
        }

        private void btnRSAClear_Click(object sender, EventArgs e)
        {
            txtRSAInput.Clear();
            txtRSAOutput.Clear();
            txtP.Clear();
            txtQ.Clear();
            txtE.Clear();
            txtN.Clear();
            txtPhi.Clear();
            txtD.Clear();
        }

        private void btnRSACopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtRSAOutput.Text))
            {
                Clipboard.SetText(txtRSAOutput.Text);
                MessageBox.Show("Đã copy kết quả vào Clipboard!");
            }
        }
    }
}
