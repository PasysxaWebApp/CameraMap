using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CameraMap.Models.Notice
{
    public class NoticeModel
    {
        /// <summary>
        /// お知らせID
        /// </summary>
        [Display(Name = "お知らせID")]
        public long NoticeID { get; set; }
        /// <summary>
        /// 事業所
        /// </summary>
        [Display(Name = "事業所")]
        public string OrganizationID { get; set; }
        /// <summary>
        /// タイトル
        /// </summary>
        [Display(Name = "タイトル")]
        [Required(ErrorMessage = "{0}を入力してください。")]
        public string Title { get; set; }

        /// <summary>
        /// お知らせ日
        /// </summary>
        [Display(Name = "お知らせ日")]
        [DataType(DataType.Date)]
        public DateTime NoticeDateTime{get;set;}

        public string AttachmentFileFileName1 { get; set; }

        /// <summary>
        /// 添付ファイル1
        /// </summary>
        [Display(Name = "添付ファイル")]
        public long AttachmentFileID1 { get; set; }

        /// <summary>
        /// 内容Type
        /// 0 text;1 UEditor
        /// </summary>
        public int ContentType { get; set; }
        /// <summary>
        /// 内容Description
        /// </summary>
        public string ContentDesc { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        public string ContentTxt { get; set; }

        public string ContextTxtForList
        {
            get
            {
                var contentTxt = ContentDesc;
                if (string.IsNullOrEmpty(contentTxt))
                {
                    contentTxt = ContentTxt;
                }
                if (string.IsNullOrEmpty(contentTxt))
                {
                    return "";
                }
                string r = "";
                using (System.IO.StringReader sr = new System.IO.StringReader(contentTxt))
                {
                    r = sr.ReadLine();
                    sr.Close();
                }
                if (r.Length > 50)
                {
                    return r.Substring(0, 49) + "...";
                }
                else
                {
                    if (contentTxt.Length > 50)
                    {
                        return r + "...";
                    }
                    else
                    {
                        return r;
                    }
                }
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        
        [Display(Name = "作成者ID")]
        public long CreateUserID { get; set; }

        [Display(Name = "作成者")]
        public string CreateUserName { get; set; }

        [Display(Name = "作成日時")]
        public DateTime CreateDateTime { get; set; }
        
        [Display(Name = "更新者ID")]
        public long LastUserID { get; set; }

        [Display(Name = "更新者")]
        public string LastUserName { get; set; }

        [Display(Name = "最終更新時間")]
        public DateTime LastUpdatetime { get; set; }
        
        //public string DeletedFiles { get; set; }


        [Display(Name = "スティッキー")] //OnTop
        public bool Sticky { get; set; }

        public string TitleForGrid
        {
            get
            {
                if (Sticky)
                {
                    return string.Format("{0}[スティッキー]", Title);
                }
                else
                {
                    return string.Format("{0}", Title);
                }
            }
        }


        [Display(Name = "表示順")]
        public int DisplayNo { get; set; }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }


    }

    public class NoticeModelListViewModel
    {
        public List<NoticeModel> Items { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int RowTotal { get; set; }
        public int PageCount
        {
            get
            {
                var noticeCount = RowTotal;
                var pageCount = noticeCount / PageSize;
                if (noticeCount % PageSize != 0)
                {
                    pageCount++;
                }
                return pageCount;

            }
        }

    }
    
}
