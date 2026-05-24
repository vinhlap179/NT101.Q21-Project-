using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace frontend
{
    public partial class UcPlayFair : UserControl
    {
        public UcPlayFair()
        {
            InitializeComponent();
        }

        private void dgvPlayfairMatrix_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private string GeneratePlayFairMatrixString(string key)
        {
            string cleanedKey = key.ToUpper().Replace("J", "I");
            cleanedKey = new string(cleanedKey.Where(char.IsLetter).ToArray());

            string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";

            string matrixString = new string((cleanedKey + alphabet).Distinct().ToArray());

            return matrixString;
        }

        private string FormatPlainText(string input)
        {
            input = input.ToUpper().Replace("J", "I");
            input = new string(input.Where(char.IsLetter).ToArray());

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                sb.Append(input[i]);

                if (i == input.Length - 1)
                {
                    sb.Append("X");
                    break;
                }

                if (input[i] == input[i + 1])
                {
                    sb.Append("X");
                }
                else
                {
                    sb.Append(input[i + 1]);
                    i++;
                }
            }
            return sb.ToString();
        }

        private string ProcessPlayfair(string text, string key, bool isEncrypt)
        {
            string matrix = GeneratePlayFairMatrixString(key);

            string processedText = isEncrypt
                ? FormatPlainText(text)
                : new string(text.ToUpper().Replace("J", "I").Where(char.IsLetter).ToArray());

            if (!isEncrypt && processedText.Length % 2 != 0)
            {
                processedText += "X";
            }

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < processedText.Length; i += 2)
            {
                char a = processedText[i];
                char b = processedText[i + 1];

                int posA = matrix.IndexOf(a);
                int posB = matrix.IndexOf(b);


                //cal row and col of a and b
                int rowA = posA / 5, colA = posA % 5;
                int rowB = posB / 5, colB = posB % 5;

                if (rowA == rowB)
                {
                    int shift = isEncrypt ? 1 : 4;
                    colA = (colA + shift) % 5;
                    colB = (colB + shift) % 5;
                }
                else if (colA == colB)
                {
                    int shift = isEncrypt ? 1 : 4;
                    rowA = (rowA + shift) % 5;
                    rowB = (rowB + shift) % 5;
                }
                else
                {
                    int tempCol = colA;
                    colA = colB;
                    colB = tempCol;
                }

                //append the resulting characters from the matrix
                result.Append(matrix[rowA * 5 + colA]);
                result.Append(matrix[rowB * 5 + colB]);
            }
            return result.ToString();
        }
        private void txtPlayfairKey_TextChanged(object sender, EventArgs e)
        {
            string matrixChars = GeneratePlayFairMatrixString(txtPlayfairKey.Text);

            dgvPlayfairMatrix.ColumnCount = 5;
            dgvPlayfairMatrix.RowCount = 5;

            int charIndex = 0;
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    dgvPlayfairMatrix.Rows[i].Cells[j].Value = matrixChars[charIndex++].ToString();
                }
            }
        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void btnPlayfairClear_Click(object sender, EventArgs e)
        {
            if (txtPlayfairInput.Text != "" || txtPlayfairOutput.Text != "" || txtPlayfairKey.Text != "" )
            {
                txtPlayfairInput.Clear();
                txtPlayfairKey.Clear();
                txtPlayfairOutput.Clear();
            }
        }

        private void btnPlayfairEncrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlayfairInput.Text) || string.IsNullOrWhiteSpace(txtPlayfairKey.Text))
            {
                MessageBox.Show("Vui lòng nhập Key và nội dung cần mã hóa!");
                return;
            }

            txtPlayfairOutput.Text = ProcessPlayfair(txtPlayfairInput.Text, txtPlayfairKey.Text, true);
        }

        private void btnPlayfairDecrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlayfairKey.Text) || string.IsNullOrWhiteSpace(txtPlayfairInput.Text))
            {
                MessageBox.Show("Vui lòng nhập Key và nội dung cần giải mã!");
                return;
            }

            txtPlayfairOutput.Text = ProcessPlayfair(txtPlayfairInput.Text, txtPlayfairKey.Text, false);
        }
    }
}
