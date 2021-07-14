using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml_editor
{
    class Compressor
    {
        public string compress_result;
        public Compressor() { compress_result = ""; }
        public string compress(string uncompressed)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

            string w = string.Empty;
            List<int> compressed = new List<int>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    // write w to output
                    compressed.Add(dictionary[w]);
                    // wc is a new sequence; add it to the dictionary
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);
            var result = string.Join(" ", compressed);
            string[] values = result.Split(' ');
            result = " ";
            for (int i = 0; i < compressed.Count; i++)
            {
                result += Convert.ToString(Convert.ToString(compressed[i], 2))+" ";
            }
            return result;
        }
        public string Decompress(string compressed_text)
        {
            if (compressed_text == "")
                return "";
            string[] values = compressed_text.Split(' ');
            compressed_text = "";
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == "") continue;
               if(i!=values.Length-2) compressed_text += Convert.ToInt32(values[i], 2).ToString() + " ";
               else compressed_text += Convert.ToInt32(values[i], 2).ToString();
            }
            List<int> compressed = compressed_text.Split(' ').Select(Int32.Parse).ToList();
            // build the dictionary
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
                for (int i = 0; i < 256; i++)
                    dictionary.Add(i, ((char)i).ToString());

                string w = dictionary[compressed[0]];
                compressed.RemoveAt(0);
                StringBuilder decompressed = new StringBuilder(w);

                foreach (int k in compressed)
                {
                    string entry = null;
                    if (dictionary.ContainsKey(k))
                        entry = dictionary[k];
                    else if (k == dictionary.Count)
                        entry = w + w[0];

                    decompressed.Append(entry);

                    // new sequence; add it to the dictionary
                    dictionary.Add(dictionary.Count, w + entry[0]);

                    w = entry;

                }
            
                return decompressed.ToString();
            
        }
        public void print_xml(Node root)
            {
            if (root.children.Count > 0)
                compress_result += "<" + root.tag_name + ">" + Environment.NewLine;
            else
                compress_result += "<" + root.tag_name + ">";
               compress_result += root.tag_value;
                for (int i = 0; i < root.children.Count; i++)
                {
                    if (root.children[i] != null) print_xml(root.children[i]);
                }
                compress_result += "</" + root.tag_name + ">" + Environment.NewLine;
            
        }
    }
}

