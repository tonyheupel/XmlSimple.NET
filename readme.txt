XmlSimple.NET - C# 4 dynamic implementation of XmlSimple
(Ruby Gem XmlSimple (xml-simple), Perl's XML::Simple, or PHP's SimpleXML)

Projects initiated by Tony Heupel (pronounced "High-pull")
===========================
Copyright 2010 (C) Tony Heupel

Contents:
  XmlSimple.NET:
    Class library that parses XML documents into a Dictionary, similar to
    XmlSimple's hashes BUT ALSO ALLOWS ACCESS USING DYNAMIC OBJECT MEMBERS!

    Example:   <Library Name="Covington">
                 <Books>
                   <Book Title="Soon Will Come the Light" Author="Tom McKean"></Book>
                   <Book Title="Fall of Giants" Author="Ken Follett" />
                 </Books>
               </Library>

      ----> Inside of a Unit Test <----               
      xml = @"<Library Name=""Covington""><Books><Book Title=""Soon Will Come the Light"" Author=""Tom McKean""></Book><Book Title=""Fall of Giants"" Author=""Ken Follett"" /></Books></Library>";
      dynamic library = XmlSimple.XmlIn(xml);

      Assert.IsInstanceOfType(library, typeof(XmlSimple));
      Assert.IsTrue(library.ContainsKey("Books"));
      Assert.AreSame(library["Books"]["Book"][1], library.Books.Book[1]);
      Assert.AreEqual("Soon Will Come the Light", library.Books.Book[0].Title);


    Takes dependencies: 
      * TonyHeupel.HyperCore (http://github.com/tonyheupel/hypercore)
      * Microsoft.CSharp
      * System.Linq
      * System.Xml.Linq
      * System.XML
      * System.Core 

  XmlSimple.NET.UnitTest:  
    Tests to ensure the library is working as expected.
