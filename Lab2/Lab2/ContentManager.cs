using Lab2;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ToA
{
    public class ContentManager
    {
        ContentManager content;
        List<Sprite> spriteList;
        XDocument xDoc;
        public ContentManager(String filename, ContentManager content)
        {
            xDoc = XDocument.Load(filename);
            this.content = content;
        }
        
    }
}
