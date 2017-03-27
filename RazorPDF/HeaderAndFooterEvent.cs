using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace RazorPDF
{
    public class HeaderAndFooterEvent : PdfPageEventHelper, IPdfPageEvent
    {

        private iTextSharp.text.Font _font;
        private string _fontDic;
        public string FontDic
        {
            get
            {
                return _fontDic;
            }
            set
            {
                _fontDic = value;
                var baseFont = BaseFont.CreateFont(System.IO.Path.Combine(FontDic, "meiryo.ttc")+",0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                _font = new Font(baseFont, 9, Font.NORMAL,Color.BLACK);
            }
        }
        public String Ttitle { get; set; }
        public String OfficeName { get; set; }
        public bool ShowHeader { get; set; }
        public bool ShowFooter { get; set; }
        public PdfTemplate PageCountTemplate { get; private set; }


        public HeaderAndFooterEvent()
        {
                //font = BaseFontAndSize("黑体", 10, Font.NORMAL, Color.BLACK);
        }


        public void EnsurePageTemplate(PdfWriter writer)
        {
            if (PageCountTemplate == null)
            {
                PageCountTemplate = writer.DirectContent.CreateTemplate(100, 100);
            }

        }


        /// <summary>
        /// OnEndPage イベント
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            EnsurePageTemplate(writer);

            if (ShowHeader)
            {
                //Phrase header1 = new Phrase(Ttitle, _font);
                Phrase header2 = new Phrase(OfficeName, _font);

                PdfContentByte cb = writer.DirectContent;

                //页眉显示的位置
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, header1,
                //       document.LeftMargin, document.Top + 10, 0);

                ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT, header2,
                       document.Right, document.Top + 10, 0);

            }

            if (ShowFooter)
            {
                Phrase footer = new Phrase(string.Format("{0}/  ", writer.PageNumber-1), _font);
                PdfContentByte cb = writer.DirectContent;
                //模版 显示总共页数
                cb.AddTemplate(PageCountTemplate, document.Right / 2, document.Bottom - 20);//调节模版显示的位置

                //页脚显示的位置
                ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT, footer,
                       document.Right / 2, document.Bottom - 20, 0);

            }
        }
        /// <summary>
        /// 新ページを開くイベント
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            EnsurePageTemplate(writer);
            if (ShowHeader)
            {
                writer.PageCount = writer.PageNumber - 1;
            }
        }
        /// <summary>
        /// PDF閉じるイベント
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            EnsurePageTemplate(writer);

            PageCountTemplate.BeginText();
            PageCountTemplate.SetFontAndSize(_font.BaseFont,_font.Size);
            PageCountTemplate.ShowText((writer.PageNumber - 2).ToString());
            PageCountTemplate.EndText();
            PageCountTemplate.ClosePath();
        }
        
        /*
        /// <summary>
        /// フォントをセット
        /// </summary>
        /// <param name="font_name"></param>
        /// <param name="size"></param>
        /// <param name="style"></param>
        /// <param name="baseColor"></param>
        /// <returns></returns>
        public static Font BaseFontAndSize(string font_name, int size, int style, Color baseColor)
        {
            BaseFont baseFont;
            Font font = null;
            string file_name = "";
            int fontStyle;
            switch (font_name)
            {
                case "メイリオ":
                    file_name = "SIMHEI.TTF";
                    break;
                case "华文中宋":
                    file_name = "STZHONGS.TTF";
                    break;
                case "宋体":
                    file_name = "SIMYOU.TTF";
                    break;
                default:
                    file_name = "SIMYOU.TTF";
                    break;
            }            
            baseFont = BaseFont.CreateFont(@"c:\windows\fonts\meiryob.ttc,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            //baseFont = baseFontNormal.BaseFont;
            if (style < -1)
            {
                fontStyle = Font.NORMAL;
            }
            else
            {
                fontStyle = style;
            }
            font = new Font(baseFont, size, fontStyle, baseColor);
            return font;
        }

        */

        /*
        /// <summary>
        /// 出力テキストを定義する
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Paragraph InsertTitleContent(string text)
        {

            iTextSharp.text.Font font = BaseFontAndSize("メイリオ", 12, Font.BOLD, Color.BLACK);
            Paragraph paragraph = new Paragraph(text, font);

            paragraph.Alignment = Element.ALIGN_CENTER;

            paragraph.SpacingBefore = 2;
            paragraph.SpacingAfter = 2;

            //paragraph.SetLeading(1, 2);//毎行のmargin

            return paragraph;
        }
        */

    }
}