//Todo: Auto add braces
//	if semi-colon is forgotton, add semicolon
//	add preferences tab
//	add font tab
//	code source highlighting tab
//	auto indent
//	compile button
//	fix exit button dialog box

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;

using System.Windows.Forms;

/// <summary>
/// A text editor for code.
/// </summary>
namespace TabTextEditor
{
    public partial class Form1 : Form
    {
	private TabPage tp;			//variable for each tab
	private int tabNumber = 1;  //counts tab count; potentially unncessary
	private string path = null, fileName;	//variable "path" stores path of opened file, variable "fileName" stores the files name(whether new file or opened file)
	private RichTextBox r;		//for each RichTextBox created when new tab/file is created
	private SaveFileDialog saveFile1 = new SaveFileDialog();	//For saving files; unnecessary as declared and intitialized later and does not need to be gloabl

	/// <summary>
	/// Initializes a new instance of the <see cref="TabTextEditor.Form1"/> class and sets starting states of application.
	/// </summary>
        public Form1()
        {
            InitializeComponent();
			newFileToolStripMenuItem_Click(null, null);	//create new tab at start
			this.FormClosing += new FormClosingEventHandler(messageBox_FormClosing);	  
        }

	/// <summary>
	/// Creates a new tab(file) when user clicks "New File" in form.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
		tabControl1.Visible = true;	//unncessary

		tp = new TabPage();	//initialize new tab
		SaveFileDialog saveFile1 = new SaveFileDialog();	//for saving files

		tp.Text = "Untitled " + tabNumber;	//sets text of each new tab to "Untitled" plus unique number
            
	        r = new RichTextBox()	//initialize RichTextBox(RTB)
             	{
			Width = tp.Size.Width,	//set RTB width same as tab's width
			Height = tp.Size.Height,	//set RTB height same as tab's height
        		 //Location = new Point(0, 0),
			Visible = true,	//sets RTB to be visible; potentially unncessary
			Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left)	//allow RTB to grow/shrink proportionally as form window grows/shrinks
        	};
            
		tp.Controls.Add(r);		//add RTB to tab
		tabControl1.SelectedTab = tp;	//have tab be selected
		tabControl1.TabPages.Add(tp);	//add tab to tabControl
  
		tabNumber++;	//increment number of tabs
        }

	/// <summary>
	/// Allow user to open a specified file into the text editor.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
		OpenFileDialog openFileDialog1 = new OpenFileDialog();	//For choosing files
		openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";	//allow .txt and All files to be opened (being changed)
		openFileDialog1.Title = "Select a Text File";	//Title of open file dialog

		tp = new TabPage();	//creates new tab for file

		r = new RichTextBox()	//initialize RTB
                {
			Width = tp.Size.Width,	//set RTB width same as tab's width
			Height = tp.Size.Height,	//set RTB width same as tab's height
			//Location = new Point(0, 0),	
			Visible = true,	//sets RTB to be visible; potentially unncessary
			Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left)	//allow RTB to grow/shrink proportionally as form window grows/shrinks   
                };

		if (openFileDialog1.ShowDialog() == DialogResult.OK)	//If user clicks "OK," open file and read into RTb
            	{
                	System.IO.StreamReader sr = new
				    System.IO.StreamReader(openFileDialog1.FileName); //open files and read characters
			r.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);	//load text from file into RTB
			sr.Close();	//close file
            	}
            
		tp.Controls.Add(r);	//add RTB to tab
		tabControl1.Controls.Add(tp);	//add tab to tabControl
		tabControl1.SelectedTab = tp;	//have tab be selected

		path = openFileDialog1.FileName;	//set variable "path" to path received from open file dialog
		fileName = Path.GetFileName(path);	//set variable "fileName" to extracted file name of the path

		tabControl1.Visible = true;	//sets tabControl to visibile; potentially unncessary
           	// tp = new TabPage();
           	
		if (fileName.Length < 15)	//if file name contains less than 15 characters
            	{
			tp.Text = fileName;	//set tab name to the file name
            	}
        	else
        	{
			string sub1 = fileName.Substring(0, 10) + "...";	//get first ten characters and add "..." and assign to variable "sub1"
			int l = fileName.Length - 7;	//set variable "l" to length - 7 characters
			int k = fileName.Length-2;		//unncessary
			string sub2 = fileName.Substring(l,7);	//get characters from variable "l" to end and assign to variable "sub2"
			tp.Text = sub1 + sub2;	//set tab name variables "sub1" and "sub2"
        	}
            	//tabControl1.TabPages.Add(tp);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Initialize the SaveFileDialog to specify the RTF extension for the file.
            saveFile1.DefaultExt = "*.*";
            saveFile1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            // Determine if the user selected a file name from the saveFileDialog. 
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {
                // Save the contents of the RichTextBox into the file.
                r.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
            }
        }

	/// <summary>
	/// Saves file when clicked.  Saves as new file if path does not exist, saves current file if path exists.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
		if (path == null)	//if path does not exist
            	{
			saveAsToolStripMenuItem_Click(null, null);	//call method that "Save As..."
		}	
		else   //if path does exist (if file is opened)
            	{
			r.SaveFile(path, RichTextBoxStreamType.PlainText);	//rewrite file with current text in RTB
            	}
        }

	/// <summary>
	/// Closes form when clicked.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
		messageBox_FormClosing(null, null);	//checks if changes were made to file and not saved

		Application.Exit();	//closes form
        }

	/// <summary>
	/// Checks if changes were made since last save.  If so, open dialog box and prompt user to save, close, or close dialog.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="f">F.</param>
        private void messageBox_FormClosing(object sender, FormClosingEventArgs f)
        {
		if (path != null)	//if path exists (if file is opened)
            	{
        	 	//RichTextBox temp = new RichTextBox();
			string t = System.IO.File.ReadAllText(path);	//store in variable "t" all text from file of the path

			if (r.Text != t)	//if current text in editor is different than text in variable "t"
                	{
				DialogResult dialogResult = MessageBox.Show("Would you like to save your changes to " + fileName + "?", "#pad", MessageBoxButtons.YesNoCancel);	//prompt user to save, close, or cancel
				if (dialogResult == DialogResult.Yes)	//if user clicks "Yes"
                		{
					saveToolStripMenuItem_Click(null, null);	//save file
					Application.Exit();	//close form
                		}
				else if (dialogResult == DialogResult.No)	//if user clicks "No"
                    		{
					Application.Exit();	//close form
                    		}
				else if (dialogResult == DialogResult.Cancel)	//if user clicks "Cancel"
                		{
					this.DialogResult = DialogResult.Cancel;	//close dialog box
                    		}
        		}
            	}
		else if (r.Text != "")	//if file was not opened, and current text in editor is not empty
            	{
			DialogResult dialogResult = MessageBox.Show("Would you like to save your changes to " + tp.Text + "?", "#pad", MessageBoxButtons.YesNoCancel);	//prompt user
			if (dialogResult == DialogResult.Yes)	//if user clicks "Yes"
                	{
				saveToolStripMenuItem_Click(null, null);	//save file
				Application.Exit();	//close form
                	}
			else if (dialogResult == DialogResult.No)	//if user clicks "No"
                	{
				Application.Exit();	//close form
                	}
			else if (dialogResult == DialogResult.Cancel)	//if user clicks "Cancel"
                	{
				this.DialogResult = DialogResult.Cancel;	//close dialog box
                	}

                	//return;
            	} 
        }
    }
}
