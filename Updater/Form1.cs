using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//
using LiveForever;

namespace Updater
{
    public partial class Form1 : Form
    {
        FormName frmName;
        /// <summary>
        /// 
        /// </summary>
        private String SaveFileName
        {
            get
            {
                return MyFile.ExecutableDirectory + "\\" + "Updater.upd";
            }
        }
        /// <summary>
        /// キーコード
        /// </summary>
        private const String KEYWORD = "pDAu";
        /// <summary>
        /// タイトル
        /// </summary>
        private const String TITLE = "Updater";
        /// <summary>
        /// バックアップページ
        /// </summary>
        private List<MyBackUpPage> BackUpPage = new List<MyBackUpPage>();
        /// <summary>
        /// FormNameで入力した文字
        /// </summary>
        public String NameResult;
        /// <summary>
        /// コピーしたページ情報
        /// </summary>
        public MyBackUpPage ClipBackUpPage;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.Text = TITLE;
            MyFile local_file = new MyFile();
            // 読み込み
            if (local_file.LoadCode(SaveFileName, KEYWORD))
            {
                String local_str;
                for (int i = 0; ; i++)
                {
                    if ((local_str = local_file.Reader()) == null) { break; }
                    BackUpPage.Add(new MyBackUpPage(local_str));
                    if (i == 0)
                    {
                        tabControl1.TabPages[0].Text = BackUpPage[0].Title;
                    }
                    else
                    {
                        tabControl1.TabPages.Add(BackUpPage[BackUpPage.Count - 1].Title);
                    }
                }
            }
            if (BackUpPage.Count == 0)
            {
                BackUpPage.Add(new MyBackUpPage());
                BackUpPage[0].Title = tabControl1.TabPages[0].Text = "Title1";
            }
            buttonSave.Enabled = false;
            RenewEnabledComponent();
            tabControl1.TabIndex = 0;
            RenewBackUpPage();
        }
        /// <summary>
        /// アップデート元のファイルを追加
        /// </summary>
        private void AddUpdateFrom()
        {            
            if (openFileDialogFrom.ShowDialog(this) == DialogResult.OK)
            {
                //openFileDialogFrom.FileNames
                foreach (String item_file in openFileDialogFrom.FileNames)
                {
                    if (ExistUpdateFrom(item_file)) { continue; }
                    if (checkedListBoxFrom.SelectedIndex != -1)
                    {
                        checkedListBoxFrom.Items.Insert(checkedListBoxFrom.SelectedIndex, item_file);
                        int local_index = checkedListBoxFrom.SelectedIndex - 1;
                        checkedListBoxFrom.SetItemChecked(local_index, true);
                        SelectedBackUpPage.BackUpFromList.Insert(checkedListBoxFrom.SelectedIndex, new MyBackUpFile(item_file, true));
                    }
                    else
                    {
                        checkedListBoxFrom.Items.Add(item_file, true);
                        SelectedBackUpPage.BackUpFromList.Add(new MyBackUpFile(item_file, true));
                    }
                    buttonSave.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private MyBackUpPage SelectedBackUpPage
        {
            get
            {
                return BackUpPage[tabControl1.SelectedIndex];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            RenewBackUpPage();
            // コントロールを渡す
            tabControl1.SelectedTab.Controls.Add(panel1);
            panel1.Location = new Point(0, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prm_text"></param>
        /// <param name="prm_button"></param>
        /// <returns></returns>
        private DialogResult ShowNameDialog(String prm_text, String prm_button, String prm_title)
        {
            if (frmName == null || !frmName.Visible)
            {
                frmName = new FormName(this, prm_text, prm_button, prm_title);
                return frmName.ShowDialog();
            }
            return DialogResult.Cancel;
        }
        /// <summary>
        /// 保存ボタンの使用可能が切り替えないためのフラグ
        /// </summary>
        private bool StopChangeSaveEnable = false;
        /// <summary>
        /// 
        /// </summary>
        private void RenewBackUpPage()
        {
            StopChangeSaveEnable = true;
            //
            checkedListBoxFrom.Items.Clear();
            foreach (MyBackUpFile item in SelectedBackUpPage.BackUpFromList)
            {
                checkedListBoxFrom.Items.Add(item.FileName, item.Check);
            }
            checkedListBoxTo.Items.Clear();
            foreach (MyBackUpFile item in SelectedBackUpPage.BackUpToList)
            {
                checkedListBoxTo.Items.Add(item.FileName, item.Check);
            }
            //
            StopChangeSaveEnable = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ExistUpdateFrom(String prm_file)
        {
            for (int i = 0; i < checkedListBoxFrom.Items.Count; i++)
            {
                if (checkedListBoxFrom.Items[i].ToString() == prm_file) { return true; }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prm_path"></param>
        /// <returns></returns>
        private bool ExistUpdateTo(String prm_path)
        {
            for (int i = 0; i < checkedListBoxTo.Items.Count; i++)
            {
                if (checkedListBoxTo.Items[i].ToString() == prm_path) { return true; }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        private void AddFolderFrom()
        {
            if (folderBrowserDialogFrom.ShowDialog(this) == DialogResult.OK)
            {
                if (ExistUpdateFrom(folderBrowserDialogFrom.SelectedPath) == false)
                {
                    if (checkedListBoxFrom.SelectedIndex != -1)
                    {
                        checkedListBoxFrom.Items.Insert(checkedListBoxFrom.SelectedIndex, folderBrowserDialogFrom.SelectedPath);
                        SelectedBackUpPage.BackUpFromList.Insert(checkedListBoxFrom.SelectedIndex, new MyBackUpFile(folderBrowserDialogFrom.SelectedPath, true));
                    }
                    else
                    {
                        checkedListBoxFrom.Items.Add(folderBrowserDialogFrom.SelectedPath, true);
                        SelectedBackUpPage.BackUpFromList.Add(new MyBackUpFile(folderBrowserDialogFrom.SelectedPath, true));
                    }
                    buttonSave.Enabled = true;
                }
            }
        }
        /// <summary>
        /// アップデート先のフォルダを追加
        /// </summary>
        private void AddUpdateTo()
        {
            if (folderBrowserDialogTo.ShowDialog(this) == DialogResult.OK)
            {
                if (ExistUpdateTo(folderBrowserDialogTo.SelectedPath) == false)
                {
                    if (checkedListBoxTo.SelectedIndex != -1)
                    {
                        checkedListBoxTo.Items.Insert(checkedListBoxTo.SelectedIndex, folderBrowserDialogTo.SelectedPath);
                        SelectedBackUpPage.BackUpToList.Insert(checkedListBoxTo.SelectedIndex, new MyBackUpFile(folderBrowserDialogTo.SelectedPath, true));
                    }
                    else
                    {
                        checkedListBoxTo.Items.Add(folderBrowserDialogTo.SelectedPath, true);
                        SelectedBackUpPage.BackUpToList.Add(new MyBackUpFile(folderBrowserDialogTo.SelectedPath, true));
                    }
                    buttonSave.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void アップデート元の追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddUpdateFrom();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void アップデート元の削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteUpdateFrom();
        }
        /// <summary>
        /// アップデート元の削除
        /// </summary>
        void DeleteUpdateFrom()
        {
            if (checkedListBoxFrom.SelectedIndex == -1) { return; }
            SelectedBackUpPage.BackUpFromList.RemoveAt(checkedListBoxFrom.SelectedIndex);
            checkedListBoxFrom.Items.Remove(checkedListBoxFrom.SelectedItem);
            buttonSave.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        private void RenewEnabledComponent()
        {
            buttonDeleteTo.Enabled = アップデート先の削除ToolStripMenuItem.Enabled
                = buttonEditTo.Enabled = アップデート先の編集ToolStripMenuItem.Enabled
                = (checkedListBoxTo.SelectedIndex != -1);
            buttonDeleteFrom.Enabled = アップデート元の削除ToolStripMenuItem.Enabled
                = buttonEditFrom.Enabled = アップデート元の編集ToolStripMenuItem.Enabled
                = (checkedListBoxFrom.SelectedIndex != -1);

            buttonDeletePage.Enabled = (tabControl1.TabCount >= 2);

            buttonPastePage.Enabled = (ClipBackUpPage != null);
        }
        /// <summary>
        /// アップデート先のインデックスの変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkedListBoxTo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            RenewEnabledComponent();
        }
        /// <summary>
        /// アップデート元のインデックスの変更 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkedListBoxFrom_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            RenewEnabledComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void アップデート先の追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddUpdateTo();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void アップデート先の削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteUpdateTo();
        }
        /// <summary>
        /// アップデート先の削除
        /// </summary>
        private void DeleteUpdateTo()
        {
            if (checkedListBoxTo.SelectedIndex == -1) { return; }
            SelectedBackUpPage.BackUpToList.RemoveAt(checkedListBoxTo.SelectedIndex);
            checkedListBoxTo.Items.Remove(checkedListBoxTo.SelectedItem);
            buttonSave.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkedListBoxTo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    DeleteUpdateTo();
                    break;
                case Keys.Enter:
                    AddUpdateTo();
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkedListBoxFrom_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    DeleteUpdateFrom();
                    break;
                case Keys.Enter:
                    AddUpdateFrom();
                    break;
            }
        }
        FormResult frmResult;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (false
                || frmResult == null
                || frmResult.Visible == false)
            {
                frmResult = new FormResult();
                frmResult.Show();
            }
            frmResult.GetListBoxResult.Items.Clear();
            //
            FileInfo local_file = null;
            DirectoryInfo local_dir = null;
            //
            for (int i = 0; i < checkedListBoxFrom.CheckedItems.Count; i++)
            {
                local_file = null;
                local_dir = null;
                if (Directory.Exists(checkedListBoxFrom.CheckedItems[i].ToString()))
                {
                    local_dir = new DirectoryInfo(checkedListBoxFrom.CheckedItems[i].ToString());
                }
                else
                {
                    // ファイルは存在しない
                    if (File.Exists(checkedListBoxFrom.CheckedItems[i].ToString()) == false)
                    {
                        MessageBox.Show("[" + checkedListBoxFrom.CheckedItems[i].ToString() + "] は、存在しません。"
                            , "アップデートするファイルが存在しません"
                            , MessageBoxButtons.OK
                            , MessageBoxIcon.Error);
                        continue;
                    }
                    local_file = new FileInfo(checkedListBoxFrom.CheckedItems[i].ToString());
                }
                for (int j = 0; j < checkedListBoxTo.CheckedItems.Count; j++)
                {
                    String local_result = "";
                    if (local_dir != null)
                    {
                        local_result = local_dir.FullName + System.Environment.NewLine
                            + "  ->" + checkedListBoxTo.CheckedItems[j].ToString() + "\\" + local_dir.Name;
                        try
                        {
                            //MessageBox.Show("[" + local_info.FullName + "] から [" + checkedListBoxTo.CheckedItems[j].ToString() + "\\" + local_info.Name + "]");
                            MyFile.CopyDirectory(local_dir.FullName, checkedListBoxTo.CheckedItems[j].ToString() + "\\" + local_dir.Name);
                            local_result = "[OK]" + local_result;
                        }
                        catch (Exception)
                        {
                            local_result = "[NG]" + local_result;
                        }
                    }
                    else
                    {
                        local_result = local_file.FullName + System.Environment.NewLine
                            + "  ->" + checkedListBoxTo.CheckedItems[j].ToString() + "\\" + local_file.Name;
                        try
                        {
                            //MessageBox.Show("[" + local_info.FullName + "] から [" + checkedListBoxTo.CheckedItems[j].ToString() + "\\" + local_info.Name + "]");
                            local_file.CopyTo(checkedListBoxTo.CheckedItems[j].ToString() + "\\" + local_file.Name, true);
                            local_result = "[OK]" + local_result;
                        }
                        catch (Exception)
                        {
                            local_result = "[NG]" + local_result;
                        }
                    }
                    if (frmResult != null)
                    {
                        frmResult.GetListBoxResult.Items.Add(local_result);
                    }
                }
            }
            MessageBox.Show("アップデートが終了しました。", "終了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonAddFrom_Click(object sender, EventArgs e)
        {
            AddUpdateFrom();
        }

        private void buttonDeleteFrom_Click(object sender, EventArgs e)
        {
            DeleteUpdateFrom();
        }

        private void buttonAddTo_Click(object sender, EventArgs e)
        {
            AddUpdateTo();
        }

        private void buttonDeleteTo_Click(object sender, EventArgs e)
        {
            DeleteUpdateTo();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveUpdateInfo();
            //
            buttonSave.Enabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        private void SaveUpdateInfo()
        {
            // アップデート元
            String local_str = "";
            for (int i = 0; i < BackUpPage.Count; i++)
            {
                if (i > 0)
                {
                    local_str += System.Environment.NewLine;
                } local_str += BackUpPage[i].SaveContents;

            }
            //
            MyFile.SaveCode(SaveFileName, local_str, KEYWORD);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void アップデート元の編集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditUpdateFrom();
        }
        /// <summary>
        /// アップデート元の編集
        /// </summary>
        private void EditUpdateFrom()
        {
            int local_index = checkedListBoxFrom.SelectedIndex;
                // ディレクトリ
            if (Directory.Exists(checkedListBoxFrom.SelectedItem.ToString()))
            {
                folderBrowserDialogFrom.SelectedPath = checkedListBoxFrom.SelectedItem.ToString();
                if (folderBrowserDialogFrom.ShowDialog() == DialogResult.OK)
                {
                    checkedListBoxFrom.Items[checkedListBoxFrom.SelectedIndex] = SelectedBackUpPage.BackUpFromList[checkedListBoxFrom.SelectedIndex].FileName = folderBrowserDialogFrom.SelectedPath;
                    checkedListBoxFrom.SelectedIndex = local_index;
                    buttonSave.Enabled = true;
                }
            }
            else
            {
                // ファイル
                if (File.Exists(checkedListBoxFrom.SelectedItem.ToString()))
                {
                    openFileDialogFrom.FileName = checkedListBoxFrom.SelectedItem.ToString();
                    if (openFileDialogFrom.ShowDialog() == DialogResult.OK)
                    {
                        checkedListBoxFrom.Items[checkedListBoxFrom.SelectedIndex] = SelectedBackUpPage.BackUpFromList[checkedListBoxFrom.SelectedIndex].FileName = openFileDialogFrom.FileName;
                        checkedListBoxFrom.SelectedIndex = local_index;
                        buttonSave.Enabled = true;
                    }
                }
            }
        }
        /// <summary>
        /// アップデート先の編集
        /// </summary>
        private void EditUpdateTo()
        {
            int local_index = checkedListBoxTo.SelectedIndex;
            folderBrowserDialogTo.SelectedPath = checkedListBoxTo.SelectedItem.ToString();
            if (folderBrowserDialogTo.ShowDialog() == DialogResult.OK)
            {
                checkedListBoxTo.Items[checkedListBoxTo.SelectedIndex] = SelectedBackUpPage.BackUpToList[checkedListBoxTo.SelectedIndex].FileName = folderBrowserDialogTo.SelectedPath;
                checkedListBoxTo.SelectedIndex = local_index;
                buttonSave.Enabled = true;
            }
        }
        void checkedListBoxFrom_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (!StopChangeSaveEnable)
            {
                buttonSave.Enabled = true;
            }
        }

        void checkedListBoxTo_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (!StopChangeSaveEnable)
            {
                buttonSave.Enabled = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void アップデート先の編集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditUpdateTo();
        }

        private void buttonEditFrom_Click(object sender, EventArgs e)
        {
            EditUpdateFrom();
        }

        private void buttonEditTo_Click(object sender, EventArgs e)
        {
            EditUpdateTo();
        }
        /// <summary>
        /// ウインドウを閉じるときの処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (buttonSave.Enabled)
            {
                switch (MessageBox.Show("アップデート情報は変更されています。保存しますか？", TITLE
                    , MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        SaveUpdateInfo();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }
            }
            base.OnFormClosing(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPageName_Click(object sender, EventArgs e)
        {
            if (ShowNameDialog(SelectedBackUpPage.Title, "名前変更", "[" + SelectedBackUpPage.Title + "]の名前変更") == DialogResult.OK)
            {
                SelectedBackUpPage.Title = tabControl1.SelectedTab.Text = NameResult;
                buttonSave.Enabled = true;
            }
        }

        private void buttonAddPage_Click(object sender, EventArgs e)
        {
            if (ShowNameDialog("Title" + (tabControl1.SelectedIndex + 2), "新規作成", "ページの作成") == DialogResult.OK)
            {
                BackUpPage.Insert(tabControl1.SelectedIndex + 1, new MyBackUpPage());
                tabControl1.TabPages.Insert(tabControl1.SelectedIndex + 1, NameResult);
                tabControl1.SelectedIndex++;
                SelectedBackUpPage.Title = NameResult;
                RenewEnabledComponent();
                buttonSave.Enabled = true;
            }
        }

        private void buttonCopyPage_Click(object sender, EventArgs e)
        {
            ClipBackUpPage = SelectedBackUpPage.Copy();
            RenewEnabledComponent();
        }

        private void buttonPageClear_Click(object sender, EventArgs e)
        {
            SelectedBackUpPage.BackUpFromList.Clear();
            SelectedBackUpPage.BackUpToList.Clear();
            RenewBackUpPage();
            buttonSave.Enabled = true;
        }

        private void buttonDeletePage_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount <= 1) { return; }
            int local_index = tabControl1.SelectedIndex;
            tabControl1.TabPages.RemoveAt(local_index);
            BackUpPage.RemoveAt(local_index);
            tabControl1.SelectedIndex = local_index;
        }

        private void buttonPastePage_Click(object sender, EventArgs e)
        {
            if (ClipBackUpPage == null) { return; }
            BackUpPage.Insert(tabControl1.SelectedIndex + 1, ClipBackUpPage.Copy());
            tabControl1.TabPages.Insert(tabControl1.SelectedIndex + 1, ClipBackUpPage.Title);
            tabControl1.SelectedIndex++;
            buttonSave.Enabled = true;
            RenewEnabledComponent();            
        }

        private void buttonAddFolderFrom_Click(object sender, EventArgs e)
        {
            AddFolderFrom();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            /*
            System.Collections.Specialized.StringCollection file_path = new System.Collections.Specialized.StringCollection();

            IDataObject iData = new DataObject(
            Clipboard.SetFileDropList(file_path);
            System.Collections.Specialized.StringCollection str =  Clipboard.GetFileDropList();
            int a = 1;
             */
            /*
            //コピーするファイルのパス
            string[] fileNames = { "C:\\1.txt", "C:\\2.txt", "C:\\3.txt" };
            //ファイルドロップ形式のDataObjectを作成する
            IDataObject iData = new DataObject(DataFormats.FileDrop, fileNames);
            //クリップボードにコピーする
            Clipboard.SetDataObject(iData);
             */
            //コピーするファイルのパス
            System.Collections.Specialized.StringCollection files =
                new System.Collections.Specialized.StringCollection();
            foreach (MyBackUpFile item in SelectedBackUpPage.BackUpFromList)
            {
                if (item.Check == false) { continue; }
                files.Add(item.FileName);
            }            //クリップボードにコピーする
            Clipboard.SetFileDropList(files);


        }
    }
}
