using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LiveForever;

namespace Updater
{
    public class MyBackUpFile
    {
        const String PAUSE_FILE = "*";
        /// <summary>
        /// チェックしているか
        /// </summary>
        public bool Check = true;
        public String FileName = "";
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="prm_file"></param>
        public MyBackUpFile(String prm_file,bool prm_check)
        {
            FileName = prm_file;
        }

        public MyBackUpFile(String prm_code)
        {
            String[] str_array = MyString.Tokenizer(prm_code, PAUSE_FILE);
            // 
            for (int i = 0; i < str_array.GetLength(0); i++)
            {
                switch ((Contents)i)
                {
                    case Contents.Check:
                        Check = (Convert.ToInt16(str_array[i]) > 0);
                        break;
                    case Contents.FileName:
                        FileName = str_array[i];
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        enum Contents
        {
            Check,
            FileName,
            SIZE
        }

        public String SaveContents
        {
            get
            {
                String str_ret = PAUSE_FILE;
                for (int i = 0; i < (int)Contents.SIZE; i++)
                {
                    switch ((Contents)i)
                    {
                        case Contents.Check:
                            str_ret += "" + (Check ? 1 : 0);
                            break;
                        case Contents.FileName:
                            str_ret += FileName;
                            break;
                        default:
                            break;
                    }
                    str_ret += PAUSE_FILE;
                }
                return str_ret;
            }
        }
            
    }
}
