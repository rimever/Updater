using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//
using LiveForever;

namespace Updater
{
    public partial class FormResult : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormResult()
        {
            InitializeComponent();
            // this.Text
            this.Text = "結果";
        }
        /// <summary>
        /// リストボックスを取得
        /// </summary>
        public ListBox GetListBoxResult
        {
            get
            {
                return listBoxResult;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxResult_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
        {
            ListBox local_list = sender as ListBox;
            e.ItemHeight = local_list.ItemHeight
                * (MyString.SearchNum(local_list.Items[e.Index].ToString(), System.Environment.NewLine) + 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listBoxResult_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            ListBox list_box = sender as ListBox;
            if (e.Index == -1) { return; }
            //
            //e.DrawBackground();
            e.Graphics.FillRectangle(new SolidBrush(e.Index == list_box.SelectedIndex ? System.Drawing.SystemColors.Highlight : list_box.BackColor), e.Bounds);
            //
            Color local_color = list_box.ForeColor;
            if (list_box.Items[e.Index].ToString().IndexOf("[NG]", 0) == 0)
            {
                local_color = Color.Red;
            }
            if (e.Index == list_box.SelectedIndex)
            {
                local_color = System.Drawing.SystemColors.HighlightText;
            }

            e.Graphics.DrawString(list_box.Items[e.Index].ToString()
                , e.Font
                , new SolidBrush(local_color)
                , e.Bounds
                );
            // フォーカス描画
            e.DrawFocusRectangle();
        }

        private void listBoxResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            // こうしないと描画がおかしくなる
            listBoxResult.BeginUpdate();
            listBoxResult.EndUpdate();
        }
        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
