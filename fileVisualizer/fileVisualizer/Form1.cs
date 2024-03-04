using fileVisualizer.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Container = fileVisualizer.classes.Container;
using Label = System.Windows.Forms.Label;

namespace fileVisualizer
{
    public partial class Form1 : Form
    {
        public string SelectedFolderPath { get; set; } = "C:\\Users\\Hussain\\Desktop\\HW2 SWE 316\\test folder";
        public double ZoomLevel { get; set; } = 1;
        public string currentTree { get; set; } = "V";
        public Form1()
        {
            InitializeComponent();
            this.AutoScroll = true;
        }

        
        public void drawVerticalTree(Folder folder)
        {
            int xPos = 0;
            drawRecursiveVert((Container)folder, 0, ref xPos);
            currentTree = "V";

        }
        public void drawRecursiveVert(Container continer, int indent , ref int xPos) {
            Boolean isFolder;
            if (continer is Folder)
                isFolder = true;
            else
                isFolder = false;



            Point currentPoint = new Point(50 + xPos , 50 + 150 * continer.getLevel());
            Label label = CreateLabel(continer.getName(), continer.getSize(), currentPoint, isFolder);
            this.Controls.Add(label);
            this.Invalidate();
            xPos += (label.Right - label.Left) + 40;
            Console.WriteLine(label.Right - label.Left);

            if (continer.getLevel() != 0)
            {

                Panel verticalSpine = CreateVerticalSpine(new Point((label.Right + label.Left)/2, label.Top -125 ) ,  125);
                this.Controls.Add(verticalSpine);
            }
            if (isFolder)
            {
                Folder currFold = (Folder)continer;
                List<Container> children = ((Folder)continer).getChildren();
                if (children.Count != 0)
                {
                    Panel horizantalLine = CreateHorizantalLine(new Point(label.Right, (label.Top + label.Bottom) / 2), 240 * currFold.getALLChildsCount() + 20);
                    this.Controls.Add(horizantalLine);
                }


                for (int i = 0; i < children.Count; i++)
                {
                    drawRecursiveVert(children[i], indent + 70, ref xPos);
                    //drawSpine(new Point(), children[i].getLevel(), children.Count);

                }
            }

        }



        public void drawHorizantalTree(Folder folder) {
            int yPos = 0;
            drawRecursiveHoriz((Container)folder, 0 , ref yPos );
            currentTree = "H";
        }

        public void drawRecursiveHoriz(Container continer , int indent , ref int yPos) {

            Boolean isFolder;
            if (continer is Folder)
                 isFolder = true;
            else
                isFolder = false;

            Point currentPoint = new Point(50 + 150 * continer.getLevel(), 50 + yPos);
            Label label = CreateLabel(continer.getName(), continer.getSize(), currentPoint, isFolder);
            //TextRenderer.DrawText(e.Graphics, label.Text, label.Font, label.Location, label.ForeColor);

            this.Controls.Add(label);
            this.Invalidate();
            yPos += label.Height + 40;


            if (continer.getLevel() != 0) {
                Panel horizantalLine = CreateHorizantalLine(new Point(label.Left, (label.Top + label.Bottom) / 2), 200);
                this.Controls.Add(horizantalLine);
            }

            if (isFolder) {
                Folder currFold = (Folder)continer;
                List<Container> children = ((Folder)continer).getChildren();
                if (children.Count != 0) {
                    Panel verticalSpine = CreateVerticalSpine(new Point((label.Right + label.Left) / 2, label.Bottom ), 85 * currFold.getALLChildsCount());
                    this.Controls.Add(verticalSpine);
                }


                for (int i = 0; i < children.Count; i++)
                {
                    drawRecursiveHoriz(children[i], indent + 70 ,ref yPos);
                    //drawSpine(new Point(), children[i].getLevel(), children.Count);

                }
            }
            


        }

        public Label CreateLabel(string text, long size,  Point location, Boolean isFolder)
        {
            // Create label
            Label label = new Label();

            // Set properties
            label.Text = text+ "\n" + size ;
            Console.WriteLine("text is "+text);

            label.Font = new Font("Arial", 12);

            label.Location = location;
            label.Width =(int)( 200 * ZoomLevel);
            label.Height = (int)(50 * ZoomLevel);
            // Customize further
            if (isFolder)
                label.BackColor = Color.LightBlue;
            else {
                label.BackColor = Color.AntiqueWhite;
            }

            // Add event handler 
          

            return label;
        }
        public Panel CreateVerticalSpine(Point location, int height)
        {
            Panel spine = new Panel
            {
                BackColor = Color.Black,
                Location = location,
                Size = new Size(2, (int)(height * ZoomLevel))
            };

            return spine;
        }
        public Panel CreateHorizantalLine(Point location , int width) {
            location.Offset(-50, 0);
            Panel spine = new Panel
            {

                BackColor = Color.Black,
                Location = location,
                Size = new Size((int) (ZoomLevel * width), 2)
            };

            return spine;

        }





        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;  // Enable key events for the form
            this.KeyDown += Form1_KeyDown;  // Handle the KeyDown event
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemMinus)  // Zoom out when '-' key is pressed
            {
                ZoomLevel -= 0.1;
                RedrawTree();   
            }
            else if (e.KeyCode == Keys.Oemplus)  // Zoom in when '=' key is pressed
            {
                ZoomLevel += 0.1;
                RedrawTree();
            }
        }
        private void RedrawTree()
        {
            clearAllDraws();
            this.Invalidate();

            DirectoryInfo selectedFolderInfo = new DirectoryInfo(SelectedFolderPath);

            Folder mainFolder = new Folder(selectedFolderInfo.Name, SelectedFolderPath, 0);

            if (currentTree.Equals("H"))
            {
                drawHorizantalTree(mainFolder);
            }
            else {
                drawVerticalTree(mainFolder);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("draw vertica");
            clearAllDraws();
            this.Invalidate();
           
            DirectoryInfo selectedFolderInfo = new DirectoryInfo(SelectedFolderPath);

            Folder mainFolder = new Folder(selectedFolderInfo.Name, SelectedFolderPath, 0);
            drawVerticalTree(mainFolder);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("draw horizanta");
            clearAllDraws();
            this.Invalidate();
           
            DirectoryInfo selectedFolderInfo = new DirectoryInfo(SelectedFolderPath);

            Folder mainFolder = new Folder(selectedFolderInfo.Name, SelectedFolderPath, 0);
            drawHorizantalTree(mainFolder);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            DialogResult result = folderDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string folderPath = folderDialog.SelectedPath;
                SelectedFolderPath = folderDialog.SelectedPath;
                Console.WriteLine("Folder selected: " + folderPath);

                label1.Text = "Folder Selected " + folderPath;
                // Use selected folder path
            }
        }

        private void clearAllDraws() {

            List<Control> exclude = new List<Control> { button1, button2, button3, label1 };

            this.Controls.Clear();

            foreach (Control c in exclude)
            {
                this.Controls.Add(c);
            }



        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
