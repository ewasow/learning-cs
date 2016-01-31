using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XmlParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = XDocument.Load("Config.xml");   
            var output = new XDocument(input.Declaration);
            var outputRoot = new XElement(input.Root);
            outputRoot.RemoveNodes();
            output.Add(outputRoot);
            foreach (var node in input.Descendants().Where(n => !n.Elements().Any()))
            {
                var newNode = new XElement(node.Name);
                var p = node.Parent;
                int level = 0;
                while (p != input.Root)
                {
                    string attrName;
                    if (level == 0)
                    {
                        attrName = "parent";
                    }
                    else
                    {
                        attrName = string.Join("", Enumerable.Repeat("grand",level));
                    }
                    newNode.SetAttributeValue(attrName, p.Name);
                    level++;
                    p = p.Parent;
                }
                newNode.Add(node.Attributes());
                outputRoot.Add(newNode);
            }
            Console.WriteLine(output);
            output.Save("output.xml");
            Console.Read();
        }
    }
}
