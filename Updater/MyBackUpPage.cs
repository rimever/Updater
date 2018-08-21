using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LiveForever;

namespace Updater
{
    public class MyBackUpPage
    {
        /// <summary>
        /// ファイルを区切る区切り文字
        /// </summary>
        private const String PAUSE_FILE = "|";
        /// <summary>
        /// 内容を区切る文字
        /// </summary>
        private const String PAUSE_CONTENTS = "?";
        /// <summary>
        /// 
        /// </summary>
        enum Contents
        {
            Title,
            From,
            To,
            SIZE
        }
        /// <summary>
        /// タイトル
        /// </summary>
        public String Title;
        /// <summary>
        /// バックアップ元
        /// </summary>
        public List<MyBackUpFile> BackUpFromList = new List<MyBackUpFile>();
        /// <summary>
        /// バックアップ先
        /// </summary>
        public List<MyBackUpFile> BackUpToList = new List<MyBackUpFile>();
        /// <summary>
        /// 保存内容
        /// </summary>
        public String SaveContents
        {
            get
            {
                String ret_text = PAUSE_CONTENTS;

                for (int i = 0; i < (int)Contents.SIZE; i++)
                {
                    switch ((Contents)i)
                    {
                        case Contents.From:
                            foreach (MyBackUpFile file in BackUpFromList)
                            {
                                ret_text += PAUSE_FILE;
                                ret_text += file.SaveContents;
                            }
                            ret_text += PAUSE_FILE;
                            break;
                        case Contents.Title:
                            ret_text += Title;
                            break;
                        case Contents.To:
                            foreach (MyBackUpFile file in BackUpToList)
                            {
                                ret_text += PAUSE_FILE;
                                ret_text += file.SaveContents;
                            }
                            ret_text += PAUSE_FILE;
                            break;
                        default:
                            break;
                    }
                    ret_text += PAUSE_CONTENTS;
                }
                //
                return ret_text;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public MyBackUpPage()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prm_code"></param>
        public MyBackUpPage(String prm_code)
        {
            String[] str_array = MyString.Tokenizer(prm_code, PAUSE_CONTENTS);
            // 
            for (int i = 0; i < str_array.GetLength(0); i++)
            {
                switch ((Contents)i)
                {
                    case Contents.From:
                        AddListBackUpFile(BackUpFromList, str_array[i]);
                        break;
                    case Contents.Title:
                        Title = str_array[i];
                        break;
                    case Contents.To:
                        AddListBackUpFile(BackUpToList, str_array[i]);
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prm_list"></param>
        /// <param name="prm_code"></param>
        void AddListBackUpFile(List<MyBackUpFile> prm_list, String prm_code)
        {
           String[] str_array = MyString.Tokenizer(prm_code, PAUSE_FILE);

           foreach (String str in str_array)
           {
               prm_list.Add(new MyBackUpFile(str));
           }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MyBackUpPage Copy()
        {
            MyBackUpPage target = new MyBackUpPage(this.SaveContents);
            String local = this.SaveContents;
            return target;
        }


    }
}
