using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml_editor
{

    class Node
    {
        public string tag_name;
        public string tag_value;
        public List<Node> children;
        public Node(string name, string value) { tag_name = name; tag_value = value; children = new List<Node>(); }

    }
    class Tree
    {
        public Node root;

        public void add_node(Node n, Node parent) { parent.children.Add(n); }
        public string json_result;
        public List<Node> leaf_nodes;
        public Tree() { root = null; json_result = ""; leaf_nodes = new List<Node>(); }
        public void Tree_leaf_nodes(Node n)
        {
            if (n.children.Count == 0) leaf_nodes.Add(n);
            for (int i = 0; i < n.children.Count; i++)
            {
                if (n.children[i] != null) { Tree_leaf_nodes(n.children[i]); }
            }
        }
        public int get_leaf_nodes_num(Node n)
        {
            int sum = 0;

            for (int i = 0; i < n.children.Count; i++)
            {
                //if (n.children[i] != null) { get_leaf_nodes_num(n.children[i]); }
                if (n.children[i].children.Count == 0) sum++;
            }
            return sum;
        }
        public List<Node> node_leaf_nodes_(Node n)
        {
            List<Node> leaves = new List<Node>();
            for (int i = 0; i < n.children.Count; i++)
            {
                //if (n.children[i] != null) { get_leaf_nodes_num(n.children[i]); }
                if (n.children[i].children.Count == 0) leaves.Add(n.children[i]);
            }
            return leaves;
        }
        public void print_json(Node r, int tab, int repeat, int max_repreated)
        //public void print_json(Node r)
        {
            if (r.children.Count > 0)
            {
                for (int i = 0; i < tab; i++)
                {
                    json_result += " ";
                }
                if (repeat > 0)
                {
                    json_result += ",{" + Environment.NewLine;
                }
                else if (repeat == 0)
                {
                    json_result += "\"" + r.tag_name + "\"" + ":" + "[" + Environment.NewLine;
                    tab++;
                    for (int i = 0; i < tab; i++)
                    {
                        json_result += " ";
                    }
                    json_result += "{" + Environment.NewLine;
                    tab++;
                }
                else if (repeat == -1)
                {
                    json_result += "\"" + r.tag_name + "\"" + ":" + "{" + Environment.NewLine;
                    tab++;
                }
                int sooo = tab;
                List<Node> children = r.children;
                methods m1 = new methods(); List<int> repeated = m1.sort_children(children);
                //children = r.children;
                int j = 0;
                for (int k = 0; k < repeated.Count; k++)
                {
                    if (repeated[k] > 1)
                    {
                        for (int i = 0; i < repeated[k]; i++)
                        {
                            print_json(r.children[j], tab, i, repeated[k]);
                            j++;
                        }
                        for (int i = 0; i < tab; i++)
                        {
                            json_result += " ";
                        }
                        json_result += "]";
                    }
                    else
                    {
                        print_json(r.children[j], tab, -1, 1);
                        tab--; j++;
                    }
                    //if (k != repeated.Count - 1) { json_result += "," + Environment.NewLine; }
                    //  else json_result += Environment.NewLine;
                    int atr_num = get_leaf_nodes_num(r);
                    if (atr_num == 0)
                    {
                        json_result += "}" + Environment.NewLine;
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                if (get_leaf_nodes_num(r) > 0 || r.tag_value.Length > 0)
                {
                    if (r.children.Count == 0)
                    {
                        for (int i = 0; i < tab; i++)
                        {
                            json_result += " ";
                        }
                        if (repeat == 0)
                        {
                            json_result += "\"" + r.tag_name + "\"" + ":" + "[" + Environment.NewLine; ////
                            tab++;
                        }
                        else if (repeat == -1)
                        {
                            json_result += "\"" + r.tag_name + "\"" + ":";
                        }
                    }
                    if (get_leaf_nodes_num(r) > 0)
                    {
                        //get_leaf_nodes(r);
                        List<Node> attributes = node_leaf_nodes_(r);

                        for (int i = 0; i < attributes.Count; i++)
                        {
                            /*if (i!=0)
                                for (int k = 0; k < tab; k++)
                                {
                                    json_result += " ";
                                }
                            

                            else
                            {
                                if (r.children.Count == 0) json_result += "{" + Environment.NewLine ;
                            }*/
                            for (int k = 0; k < sooo; k++)
                            {
                                json_result += " ";
                            }


                            json_result += "\"" + attributes[i].tag_name + "\"" + " : " + "\"" + attributes[i].tag_value + "\"";
                            if (i != attributes.Count - 1) json_result += "," + Environment.NewLine;
                        }
                        if (r.tag_value.Length > 0)
                            json_result += "," + Environment.NewLine;
                        else
                            json_result += Environment.NewLine;
                        if (r.tag_value.Length == 0)
                        {
                            for (int i = 0; i < sooo; i++)
                            {
                                json_result += " ";
                            }
                            json_result += "}" + Environment.NewLine;
                        }
                    }
                    if (r.tag_value.Length > 0)
                    {
                        if (repeat == 0 || get_leaf_nodes_num(r) > 0)
                            for (int i = 0; i < sooo; i++)
                            {
                                json_result += " ";
                            }
                        /* if (get_leaf_nodes_num(r) >= 0)
                         {
                             json_result += "\"text\":" + "\"" + node_leaf_nodes_(r).tag_value + "\"";
                              json_result += "}" + Environment.NewLine;
                            // json_result += "\"" + r.tag_name + "\"";
                         }
                         else
                         {
                             json_result += "\"" + r.tag_name + "\"";
                         } */

                        if (repeat >= 0 && repeat != max_repreated - 1)
                        {
                            json_result += "," + Environment.NewLine;

                        }
                        else if (repeat == max_repreated - 1)
                        {
                            json_result += Environment.NewLine;
                        }

                    }



                }






                /*
                char c = Convert.ToChar(34); string parenthess = Convert.ToString(c); // include " " in text
                //if(if (r.children.Distinct().Skip(1).Any()))
                if (r.children.Count > 0)
                    json_result += parenthess + r.tag_name + parenthess + ":" + "{" + r.tag_value + Environment.NewLine;
                else if(r.children.Count==0)
                    json_result += parenthess + r.tag_name + parenthess + ":" + parenthess + r.tag_value + parenthess + ","+ Environment.NewLine;
                for (int i=0;i<r.children.Count;i++)
                {
                    if(r.children[i]!=null)
                    { print_json(r.children[i]); }
                }
                if (r.children.Count > 1)
                    json_result += "}" + Environment.NewLine;
                else if(r.children.Count==0)
                    json_result += Environment.NewLine;*/

            }
        }
        class GFG : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {

                // CompareTo() method
                return x.tag_name.CompareTo(y.tag_name);

            }
        }
        class methods
        {
            /* private int compare (string s1,string s2)
             {
                 return s1.CompareTo(s2);
             }*/
            public List<int> sort_children(List<Node> child_list)
            {
                GFG gg = new GFG();
                List<int> repeat = new List<int>();
                string temp = "";
                List<Node> children = child_list;
                children.Sort(gg);
                if (children.Count > 0)
                    temp = children[0].tag_name;
                repeat.Add(1);
                int j = 0;
                for (int i = 1; i < children.Count; i++)
                {
                    if ((children[i].tag_name) == temp)
                    {
                        repeat[j]++;
                    }
                    else
                    {
                        repeat.Add(1);
                        j++;
                        temp = children[i].tag_name;
                    }
                }
                return repeat;
            }
        }
    }
}
