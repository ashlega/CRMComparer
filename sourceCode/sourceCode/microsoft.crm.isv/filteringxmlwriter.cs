using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Microsoft.Crm.Isv
{
    class FilteringXmlWriter: XmlWriter
    {
        XmlWriter _writer;
        int _filterDepth;

        public XmlFilter Filter { get; set; }
        
        public FilteringXmlWriter(XmlWriter writer)
        {
            _writer = writer;
        }

        public override void Close()
        {
            _writer.Close();
        }

        public override void Flush()
        {
            _writer.Flush();
        }

        public override string LookupPrefix(string ns)
        {
            return _writer.LookupPrefix(ns);
        }

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            if (_filterDepth > 0) return;
            _writer.WriteBase64(buffer, index, count);
        }

        public override void WriteCData(string text)
        {
            if (_filterDepth > 0) return;
            _writer.WriteCData(text);
        }

        public override void WriteCharEntity(char ch)
        {
            if (_filterDepth > 0) return;
            _writer.WriteCharEntity(ch);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            if (_filterDepth > 0) return;
            _writer.WriteChars(buffer, index, count);
        }

        public override void WriteComment(string text)
        {
            if (_filterDepth > 0) return;
            _writer.WriteComment(text);
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            if (_filterDepth > 0) return;
            _writer.WriteDocType(name, pubid, sysid, subset);
        }

        public override void WriteEndAttribute()
        {
            if (_filterDepth > 1) return;
            _writer.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            if (_filterDepth > 0) return;
            _writer.WriteEndDocument();
        }

        public override void WriteEndElement()
        {
            if (_filterDepth == 1)
            {
                _writer.WriteString("...");
            }

            if (_filterDepth > 0) _filterDepth--;
            if (_filterDepth > 0) return;

            _writer.WriteEndElement();
        }

        public override void WriteEntityRef(string name)
        {
            if (_filterDepth > 0) return;
            _writer.WriteEntityRef(name);
        }

        public override void WriteFullEndElement()
        {
            if (_filterDepth == 1)
            {
                _writer.WriteString("...");
            }
            if (_filterDepth > 0) _filterDepth--;
            if (_filterDepth > 0) return;
            _writer.WriteFullEndElement();
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            if (_filterDepth > 0) return;
            _writer.WriteProcessingInstruction(name, text);
        }

        public override void WriteRaw(string data)
        {
            if (_filterDepth > 0) return;
            _writer.WriteRaw(data);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            if (_filterDepth > 0) return;
            _writer.WriteRaw(buffer, index, count);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            if (_filterDepth > 1) return;
            _writer.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteStartDocument(bool standalone)
        {
            if (_filterDepth > 0) return;
            _writer.WriteStartDocument(standalone);
        }

        public override void WriteStartDocument()
        {
            if (_filterDepth > 0) return;
            _writer.WriteStartDocument();
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            if (_filterDepth > 0) { _filterDepth++; return; }

            if (this.Filter != null && this.Filter.Elements.Contains(localName))
            {
                _filterDepth = 1;
            }
            _writer.WriteStartElement(prefix, localName, ns);
        }

        public override WriteState WriteState
        {
            get { return _writer.WriteState; }
        }

        public override void WriteString(string text)
        {
            if (_filterDepth > 1) return;
            _writer.WriteString(text);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            if (_filterDepth > 0) return;
            _writer.WriteSurrogateCharEntity(lowChar, highChar);
        }

        public override void WriteWhitespace(string ws)
        {
            if (_filterDepth > 0) return;
            _writer.WriteWhitespace(ws);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                IDisposable disposable = _writer as IDisposable;
                if (disposable != null) disposable.Dispose();
            }

            _writer = null;
        }
    }
}
