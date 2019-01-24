using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CommandLine;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;


namespace ExtractFileProperties
{
    class Program
    {
        public class Options
        {
            [Option('a', "all", Required = false, HelpText = "Read all property values.")]
            public bool All { get; set; }

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }

            [Value(0, Min = 1, HelpText = "Files from which to read properties")]
            public IEnumerable<string> Files { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       foreach (var f in o.Files)
                       {
                           ReadProperties(f, o);
                       }
                   });

#if DEBUG
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
#endif
        }

        static void ReadProperties(string file, Options o)
        {
            if (!File.Exists(file))
                return;

            var data = new Data();

            if (o.All)
            {
                foreach (var prop in new ShellPropertyCollection(file))
                {
                    data.Add(prop);
                }
            }
            else
            {
                var props = ShellObject.FromParsingName(file).Properties;
                data.Add(props.System.Keywords);
                data.Add(props.System.Comment);
            }

            var xmlFile = file + ".props.xml";

            using (TextWriter writer = new StreamWriter(xmlFile))
            {
                data.Serializer.Serialize(writer, data);
            }
         
            if (o.Verbose)
                Console.WriteLine($"Wrote properties of {file} to {xmlFile}");
        }
    }

    [Serializable]
    public class Data
    {
        public List<Property> Properties { get; private set; } = new List<Property>();

        public void Add(IShellProperty p)
        {
            if (p.CanonicalName != null)
                Properties.Add(new Property
                {
                    Name = p.CanonicalName,
                    Value = p.FormatForDisplay(PropertyDescriptionFormatOptions.None)
                });
        }

        private static XmlSerializer serializer = null;
        [XmlIgnore]
        public XmlSerializer Serializer
        {
            get
            {
                if (serializer == null)
                    serializer = new XmlSerializer(typeof(Data));
                return serializer;
            }
        }

        public class Property
        {
            [XmlAttribute]
            public string Name;
            public string Value;
        }
    }
}
