using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WindowsFormsApplication3
{
    class DynamicObj
    {
        //private Map values = new LinkedHashMap();
        //private Hashtable values = new Hashtable();
        //private SortedList values = new SortedList();
        private NoSortHashTable values = new NoSortHashTable();

        String vipDivide = "\r\n";
        String propertyDivide = OperateString.getAsciiString(22);

        String partitionCard = OperateString.getAsciiString(23);
        public DynamicObj()
        {

        }

        public void addNewAttribute(String name, String value)
        {
            values.Add(name, value);

        }
        public string addSpecialCharactere()
        {
            //for (Iterator iter = values.keySet().iterator(); iter.hasNext(); )
            //{
            //    Object key = iter.next();
            //    String val = (String)values.get(key);
            //    str += val + propertyDivide;
            //}
            //String str = "";
            //foreach (DictionaryEntry de in values)
            //{
            //    //Console.WriteLine("Key = {0}, Value = {1}", values.Key, values.Value);
            //    str += de.Value + getAsciiString(25);
            //}
            String str = "";
            ArrayList list = new ArrayList(values.Keys);
    
            foreach (string sfor in list)
            {
                str += values[sfor] + getAsciiString(25);
            }
            return str;
        }
        public string addSpecialCharactere(string s)
        {
            s = s.Replace("[", "");
            s = s.Replace("]", "");
            s = s.Replace(",", getAsciiString(25));
            return s;
        }

        private static string getAsciiString(int i)
        {
            char[] a = {
        (char) i};
            return new String(a);
        }
    }
}
