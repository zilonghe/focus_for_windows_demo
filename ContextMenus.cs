using System;
using System.Diagnostics;
using System.Windows.Forms;
using SystemTrayApp.Properties;
using System.Drawing;
using System.IO;
using System.Text;

namespace SystemTrayApp
{
	/// <summary>
	/// 
	/// </summary>
	class ContextMenus
	{
        string BACKUP_PATH = @"C:\Windows\System32\drivers\etc\hostsbackup";
        string BLOCK_PY_FILE_PATH = @"..\..\scripts\block.py";
        string UNBLOCK_PY_FILE_PATH = @"..\..\scripts\unblock.py";
        string BLOCK_LIST_FILE_PATH = @"..\..\static\block_list.txt";

        /// create a global menu instance.
        ContextMenuStrip menu = new ContextMenuStrip();

        /// <summary>
        /// Is the About box displayed?
        /// </summary>
        bool isAboutLoaded = false;

        /// block or unblock
        bool isBlock = false;

        bool isFileExists(string path)
        {
            if(File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns>ContextMenuStrip</returns>
		public ContextMenuStrip Create()
		{
            isBlock = isFileExists(BACKUP_PATH);

            // Add the default menu options.
            ToolStripMenuItem item;
			ToolStripSeparator sep;

            //// Edit block list file.
            //item = new ToolStripMenuItem();
            //item.Text = "Block list";
            //ToolStripMenuItem subitem;
            //// add first subitem.
            //subitem = new ToolStripMenuItem();
            //subitem.Text = "Edit file";y
            //subitem.Click += new EventHandler(Open_file_in_Explorer);
            //item.DropDownItems.Add(subitem);
            //// add second subitem.
            //subitem = new ToolStripMenuItem();
            //subitem.Text = "Reload file";
            //item.DropDownItems.Add(subitem);
            //menu.Items.Insert(0, item);

            // Edit block list file.
            item = new ToolStripMenuItem();
            item.Text = "Block list";
            item.Click += new EventHandler(Block_file_watcher);
            menu.Items.Insert(0, item);

            // Start to block websites.
            item = new ToolStripMenuItem();
            item.Text = "Block";
            item.Checked = isBlock;
            item.Click += new EventHandler(Run_block_py);
            menu.Items.Insert(1, item);

            // Unblock websites.
            item = new ToolStripMenuItem();
            item.Text = "Unblock";
            item.Checked = !isBlock;
            item.Click += new EventHandler(Run_unblock_py);
            menu.Items.Insert(2, item);

            //// Windows Explorer.
            //item = new ToolStripMenuItem();
            //item.Text = "Explorer";
            //item.Click += new EventHandler(Explorer_Click);
            /////item.Image = Resources.Explorer;
            //menu.Items.Add(item);

            //// About.
            //item = new ToolStripMenuItem();
            //item.Text = "About";
            //item.Click += new EventHandler(About_Click);
            /////item.Image = Resources.About;
            //menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
			menu.Items.Add(sep);

			// Exit.
			item = new ToolStripMenuItem();
			item.Text = "Exit";
			item.Click += new System.EventHandler(Exit_Click);
			///item.Image = Resources.Exit;
			menu.Items.Add(item);

			return menu;
		}

        /// Run block python script to block websites.
        void Run_block_py(object sender, EventArgs e)
        {
            if (!isBlock)
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(@"E:\python\python 2.7.9\Python.exe", BLOCK_PY_FILE_PATH)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                Console.WriteLine(output);
                Console.ReadLine();

                ((ToolStripMenuItem)menu.Items[1]).Checked = true;
                ((ToolStripMenuItem)menu.Items[2]).Checked = false;
                isBlock = true;
            }
        }

        /// Run unblock python script to block websites.
        void Run_unblock_py(object sender, EventArgs e)
        {
            if (isBlock)
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(@"E:\python\python 2.7.9\Python.exe", UNBLOCK_PY_FILE_PATH)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                Console.WriteLine(output);
                Console.ReadLine();

                ((ToolStripMenuItem)menu.Items[1]).Checked = false;
                ((ToolStripMenuItem)menu.Items[2]).Checked = true;
                isBlock = false;
            }
        }

        /// Run the watcher script if is blocking and file modified or just simply open the file.
        void Block_file_watcher(object sender, EventArgs e)
        {
            if(isBlock)
            {
                ///rerun the Block.py
            }
            ///simply open file in Explorer.
            Open_file_in_Explorer();
        }

        /// Open_file_in_Explorer if exists or create and open it.
        void Open_file_in_Explorer()
        {
            if (!isFileExists(BLOCK_LIST_FILE_PATH))
            {
                try
                {
                    using (FileStream fs = File.Create(BLOCK_LIST_FILE_PATH))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes("#Example: #www.zhihu.com");
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            Process.Start("explorer", BLOCK_LIST_FILE_PATH);
        }

        /// <summary>
        /// Handles the Click event of the Explorer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Explorer_Click(object sender, EventArgs e)
		{
			Process.Start("explorer", null);
		}

		/// <summary>
		/// Handles the Click event of the About control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void About_Click(object sender, EventArgs e)
		{
			if (!isAboutLoaded)
			{
				isAboutLoaded = true;
				new AboutBox().ShowDialog();
				isAboutLoaded = false;
			}
		}

		/// <summary>
		/// Processes a menu item.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Exit_Click(object sender, EventArgs e)
		{
			// Quit without further ado.
			Application.Exit();
		}
	}
}