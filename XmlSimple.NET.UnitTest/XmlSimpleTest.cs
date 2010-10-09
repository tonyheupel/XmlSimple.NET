using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XmlSimple.NET.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class XmlSimpleTest
    {
        [TestMethod]
        public void TryXmlSimple()
        {
            string xml = @"<Library Name=""Covington""><Books><Book Title=""Soon Will Come the Light"" Author=""Tom McKean""></Book><Book Title=""Fall of Giants"" Author=""Ken Follett"" /></Books></Library>";

            dynamic library = XmlSimple.XmlIn(xml);

            Assert.IsInstanceOfType(library, typeof(XmlSimple));
            Assert.IsTrue(library.ContainsKey("Books"));
            Assert.AreSame(library["Books"]["Book"][1], library.Books.Book[1]);
            Assert.AreEqual("Soon Will Come the Light", library.Books.Book[0].Title);
        }

        [Ignore]
        [TestMethod]
        public void TryXmlSimpleWithYahoo()
        {
            /* THE RUBY VERSION
            require 'net/http'
            require 'rubygems'
            require 'xmlsimple'

            url = 'http://api.search.yahoo.com/WebSearchService/V1/webSearch?appid=YahooDemo&query=madonna&results=2'
            xml_data = Net::HTTP.get_response(URI.parse(url)).body

            data = XmlSimple.xml_in(xml_data)

            data['Result'].each do |item|
               item.sort.each do |k, v|
                  if ["Title", "Url"].include? k
                     print "#{v[0]}" if k=="Title"
                     print " => #{v[0]}\n" if k=="Url"
                  end
               end
            end
            */

            // The XmlSimple.NET version
            var url = "http://api.search.yahoo.com/WebSearchService/V1/webSearch?appid=YahooDemo&query=madonna&results=2";
            //var xmlData = new System.Net.WebClient().DownloadString(url);

            dynamic data = XmlSimple.XmlIn(url);

            Dictionary<XmlSimple.Options, object> options = new Dictionary<XmlSimple.Options, object>();
            options[XmlSimple.Options.AttrPrefix] = true;
            dynamic dataAttrPrefix = XmlSimple.XmlIn(url, options);

            // The next two lines are equivalent - notice the use of mixed case
            data.Result.ForEach(new Action<dynamic>(result => Console.WriteLine("{0} => {1}", result.Title, result.URl)));
            data["result"].ForEach(new Action<dynamic>(result => Console.WriteLine("{0} => {1}", result["TiTle"], result["URL"])));
        }
    }
}
