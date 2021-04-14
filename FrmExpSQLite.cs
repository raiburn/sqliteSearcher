using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;


namespace ExploradorDeSqlite
{
    
    public partial class FrmExpSQLite : Form
    {
        String[] basesd;
        private string directorio;
        private string bd;
        private SQLiteConnection con;
        private SQLiteCommand cmd;
        private SQLiteDataAdapter DA;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private SQLiteDataReader SEL;

        public FrmExpSQLite()
        {
            InitializeComponent();
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            try
            {
                folder.ShowDialog();
                label1.Text = "Current Folder " + folder.SelectedPath;
                basesd = Directory.GetFiles(folder.SelectedPath,"*.db");
                comboBox1.Items.Add("Select the database");
                comboBox1.Text = comboBox1.Items[0].ToString();
                directorio = folder.SelectedPath;
                foreach (var item in basesd)
                {
                    comboBox1.Items.Add(Path.GetFileName(item));
                }
            }
            catch (Exception)
            {

                //MessageBox.Show("Route Failed ");
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Show Database");
            comboBox1.Text = comboBox1.Items[0].ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label3.Text = "Database Tables " + comboBox1.Text.ToString();
            bd = comboBox1.Text.ToString();
            comboMostrar.Items.Clear();

            try
            {
                con = new SQLiteConnection("Data Source=" + directorio+ "\\"+ bd + ";");
                con.Open();
                cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type = \"table\";", con);
                SEL = cmd.ExecuteReader();

                if (SEL.HasRows)
                {
                    comboMostrar.Enabled = true;
                    while (SEL.Read())
                    {
                        comboMostrar.Items.Add(SEL.GetString(0));
                       
                    }
                    comboMostrar.Text = comboMostrar.Items[0].ToString();


                }
                con.Close();

            }
            catch (Exception error)
            {

                //MessageBox.Show("Failed to select tables \n"+error);
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {
      
            label3.Text = "Current Database  " + folder.SelectedPath;
            basesd = Directory.GetFiles(folder.SelectedPath, "*.db");
            directorio = folder.SelectedPath;
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            String Sqlquery = "SELECT * FROM " + comboMostrar.Text;
            try
            {
                con = new SQLiteConnection("Data Source=" + directorio + "\\" + bd + ";");
                con.Open();
                cmd = new SQLiteCommand(Sqlquery , con);
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter tablaqsl = new SQLiteDataAdapter(cmd);
                DataTable tablaMostrar = new DataTable();
                tablaqsl.Fill(tablaMostrar);
                dgvMostrar.DataSource = tablaMostrar;
                con.Close();

            }
            catch (Exception error)
            {

                //MessageBox.Show("Failed to select table data " + error);
            }

        }
    }
}
