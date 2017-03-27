// Copyright 2012 Al Nyveldt - http://nyveldt.com
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;

namespace RazorPDF
{
    public class PdfView : IView, IViewEngine
    {
        private readonly ViewEngineResult _result;
        public String Title{get;set;}
        public String OfficeName { get; set; }

        public PdfView(ViewEngineResult result)
        {
            _result = result;
        }

        public void Render(ViewContext viewContext, TextWriter textWriter)
        {
            var sb = new System.Text.StringBuilder();
            TextWriter tw = new System.IO.StringWriter(sb);
            _result.View.Render(viewContext, tw);
            var resultCache = sb.ToString();

            XmlParser parser;
            using (var reader = GetXmlReader(resultCache))
            {
                while (reader.Read() && reader.NodeType != XmlNodeType.Element)
                {
                    // no-op
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "itext")
                    parser = new XmlParser();
                else
                    parser = new HtmlParser();
            }

            var document = new Document(iTextSharp.text.PageSize.A4, 25, 25, 50, 50);
            var pdfWriter = PdfWriter.GetInstance(document, viewContext.HttpContext.Response.OutputStream);
            
            document.Open();
            var fontDic = viewContext.HttpContext.Server.MapPath("~/Contents/fonts");

            var pageEvent = new HeaderAndFooterEvent();
            pageEvent.FontDic = fontDic;
            pageEvent.ShowHeader = true;
            pageEvent.ShowFooter = true;
            pageEvent.Ttitle = this.Title;
            pageEvent.OfficeName = this.OfficeName;
            pdfWriter.PageEvent = pageEvent;

            viewContext.HttpContext.Response.ContentType = "application/pdf";           

            using (var reader = GetXmlReader(resultCache))
            {                 
                parser.Go(document, reader);
            }
            pdfWriter.Close();
        }

        private static XmlTextReader GetXmlReader(string source)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(source);
            MemoryStream stream = new MemoryStream(byteArray);

            var xtr = new XmlTextReader(stream);
            xtr.WhitespaceHandling = WhitespaceHandling.None; // Helps iTextSharp parse 
            return xtr;
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            throw new System.NotImplementedException();
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            throw new System.NotImplementedException();
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            _result.ViewEngine.ReleaseView(controllerContext, _result.View);
        }

        /*
        private void PdfHeader(Document doc, PdfWriter writer)
        {
            string totalStar = string.Empty;
            writer.PageEvent = new HeaderAndFooterEvent();
            doc.Add(HeaderAndFooterEvent.InsertTitleContent(Title));

            HeaderAndFooterEvent.PageCountTemplate = writer.DirectContent.CreateTemplate(100, 100);
            HeaderAndFooterEvent.Ttitle = Title;
            HeaderAndFooterEvent.OfficeName = OfficeName;
            PdfContentByte cb = writer.DirectContent;
            cb.AddTemplate(HeaderAndFooterEvent.PageCountTemplate, 266, 714);
        }
        */

        /*
        private void First(Document doc, PdfWriter writer)
        {
            //string tmp = "分析報告qqqqqqqqq";
            //doc.Add(HeaderAndFooterEvent.InsertTitleContent(tmp));

            //tmp = "正文qqqqqqq";
            //doc.Add(HeaderAndFooterEvent.InsertTitleContent(tmp));

            //HeaderAndFooterEvent.tpl = writer.DirectContent.CreateTemplate(100, 100); 
            //PdfContentByte cb = writer.DirectContent;
            //cb.AddTemplate(HeaderAndFooterEvent.tpl, 266, 714);
        }
        */

        //private static bool _isRegisterFont = false;
        //private static object lockObj;
        //static PdfView()
        //{
        //    lockObj = new object();
        //    RegisterFont();
        //}
        //public static void RegisterFont()
        //{
        //    if (!_isRegisterFont)
        //    {
        //        lock (lockObj)
        //        {
        //            if (!_isRegisterFont)
        //            {
        //                //BaseFont.AddToResourceSearch("iTextAsian.dll");
        //                //BaseFont.AddToResourceSearch("iTextAsianCmaps.dll");
        //                FontFactory.Register(@"C:\Windows\Fonts\meiryo.ttc");
        //                //FontFactory.Register(Environment.GetFolderPath(Environment.SpecialFolder.System) +
        //                //                     @"\..\Fonts\simhei.ttf");
        //                //FontFactory.Register(Environment.GetFolderPath(Environment.SpecialFolder.System) +
        //                //                     @"\..\Fonts\simsun.ttc");
        //                _isRegisterFont = true;
        //            }
        //        }
        //    }

        //}

    }
}
