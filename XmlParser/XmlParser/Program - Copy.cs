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
            var outputRoot = copyWithoutChildren(input.Root);
            output.Add(outputRoot);
            foreach (var node in getAllLeaves(input))
            {
                var newNode = createOutputNode(node, input);
                outputRoot.Add(newNode);
            }
            Console.WriteLine(output);
            output.Save("output.xml");
            Console.Read();
        }

        private static XElement createOutputNode(XElement inputNode, XDocument input)
        {
            var outputNode = new XElement(inputNode.Name);
            setAttributesWithParentsNames(inputNode, input.Root, outputNode);
            outputNode.Add(inputNode.Attributes());
            return outputNode;
        }

        private static void setAttributesWithParentsNames(XElement inputNode, XElement inputRoot, XElement outputNode)
        {
            var parent = inputNode.Parent;
            int level = 0;
            while (parent != inputRoot)
            {
                var attrName = getParentRelatedAttributeName(level);
                outputNode.SetAttributeValue(attrName, parent.Name);
                level++;
                parent = parent.Parent;
            }
        }

        private static string getParentRelatedAttributeName(int level)
        {
            if (level == 0)
            {
                return "parent";
            }
            return string.Join("", Enumerable.Repeat("grand", level));
        }

        private static XElement copyWithoutChildren(XElement element)
        {
            var newElement = new XElement(element);
            newElement.RemoveNodes();
            return newElement;
        }

        private static IEnumerable<XElement> getAllLeaves(XDocument document)
        {
            return document.Descendants().Where(n => !n.Elements().Any());
        } 
    }
}
