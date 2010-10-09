using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TonyHeupel.HyperCore;

namespace XmlSimple
{
    /// <summary>
    /// XmlSimple provides methods for parsing XML into dynamic objects
    /// whose members are the attributes and sub-elements of the document.
    /// XmlSimple is based off of the ruby gem XmlSimple: http://xml-simple.rubyforge.org/
    /// (gem install xml-simple), which was based off of Perl's XML::Simple.
    /// </summary>
    public class XmlSimple : HyperDynamo
    {
        #region Public static members
        public static dynamic XmlIn(string input)
        {
            return XmlIn(input, null);
        }

        public static dynamic XmlIn(string input, Options options)
        {
            if (String.IsNullOrWhiteSpace(input)) throw new ArgumentException("Empty and null strings are not allowed (this isn't Ruby's XmlSimple)");

            // If it's an XML String, use a StringReader to load the document, 
            // otherwise it's a uri to a doc
            return XmlIn(IsXmlString(input) ? XDocument.Load(new StringReader(input)) : XDocument.Load(input), options);
        }

        public static dynamic XmlIn(System.IO.Stream stream)
        {
            return XmlIn(stream, null);
        }


        public static dynamic XmlIn(System.IO.Stream stream, Options options)
        {
            if (stream == null || stream.Length <= 0) throw new ArgumentException("Must pass an non-empty stream");

            return XmlIn(XDocument.Load(stream), options);
        }


        public static dynamic XmlIn(XDocument document)
        {
            return XmlIn(document, null);
        }


        public static dynamic XmlIn(XDocument document, Options options)
        {
            if (document == null || document.Root == null) throw new ArgumentException("Must pass a non-empty XDocument");

            if (options == null) options = new Options();

            return ParseElement(document.Root, options);
        }
        #endregion


        #region Protected static members
        protected static bool IsXmlString(string input)
        {
            // Implementation taken from Ruby's XmlSimple's documented rules
            return (input.Contains('<') && input.Contains('>'));
        }


        protected static dynamic ParseElement(XElement element, Options options)
        {
            if (element.HasAttributes || element.HasElements)
            {
                dynamic current = new XmlSimple();

                if (!options.NoAttr)
                {
                    ParseAttributes(element, ref current, options.AttrPrefix);
                }

                ParseElements(element, ref current, options);

                return current;
            }
            else
            {
                return element.Value;
            }
        }


        protected static void ParseAttributes(XElement element, ref dynamic elementObject, bool withPrefix)
        {
            foreach (XAttribute attribute in element.Attributes())
            {
                elementObject[GetAttributeName(attribute, withPrefix)] = attribute.Value;
            }
        }


        protected static void ParseElements(XElement element, ref dynamic elementObject, Options options)
        {
            foreach (XElement current in element.Elements())
            {
                string name = GetElementName(current);


                //  If there is already a property of this name, then
                //  we have a collection of these items.  
                //  Yes, it stinks that Library->Books->Book, Book, Book
                //  will be accessed via library.Books.Book[2] notation, 
                //  but since we can't assume that the Books element
                //  is ONLY an array object, we need to stick with it
                //  this way for now. (This is how XMLSimple does it).
                if (elementObject.ContainsKey(name))
                {
                    if (!(elementObject[name] is IEnumerable<object>))
                    {
                        List<object> value = new List<object>();
                        value.Add(elementObject[name]);
                        elementObject[name] = value;
                    }

                    elementObject[name].Add(ParseElement(current, options));
                }
                else
                {
                    elementObject[name] = ParseElement(current, options);
                }
            }
        }


        #region Member Name Formatting
        protected static readonly string AttributePrefix = "@";


        protected static string GetAttributeName(XAttribute attribute, bool withPrefix)
        {
            return GetAttributeName(attribute.Name, withPrefix);
        }


        protected static string GetAttributeName(XName name, bool withPrefix)
        {
            string n = name.LocalName;

            if (withPrefix) return GetName(AttributePrefix, n);

            return n;
        }


        protected static string GetElementName(XElement element)
        {
            return GetElementName(element.Name);
        }


        protected static string GetElementName(XName name)
        {
            return name.LocalName;
        }


        protected static string GetName(string prefix, string fullName)
        {
            return String.Format("{0}{1}", prefix, fullName);
        }
        #endregion


        #endregion

        #region Overrides
        public override object this[string name]
        {
            get
            {
                dynamic result;
                if (GetNameHeuristic(name, out result)) return result;

                return base[name];
            }
            set
            {
                base[name] = value;
            }
        }
        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            return GetNameHeuristic(binder.Name, out result);
        }


        protected bool GetNameHeuristic(string name, out dynamic result)
        {
            if (TryGetName(name, out result)) return true;

            if (TryGetName(GetAttributeName(name, false), out result)) return true;

            if (TryGetName(GetAttributeName(name, true), out result)) return true;

            if (TryGetName(GetElementName(name), out result)) return true;

            result = null;
            return false;
        }

        protected bool TryGetName(string name, out dynamic result)
        {
            if (this.ContainsKey(name))
            {
                return this.TryGetValue(name, out result);
            }

            string foundKey = this.Keys.FirstOrDefault(key => 0 == String.Compare(key, name, true));
            if (!String.IsNullOrWhiteSpace(foundKey))
            {
                result = this[foundKey];
                return true;
            }

            result = null;
            return false;
        }
        #endregion
    }
}

