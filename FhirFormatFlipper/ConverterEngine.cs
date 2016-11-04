using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Model;
using Newtonsoft.Json.Converters;

namespace FhirFormatFlipper
{
  public static class ConverterEngine
  {
    public enum FhirFormatType { none, xml, json };
    public static string FormatConvert(string InboundResource)
    {
      FhirFormatType TargetType = ConverterEngine.IsFormat(InboundResource);
      if (TargetType == FhirFormatType.xml)
      {
        return ConverterEngine.XmlToJson(InboundResource);
      }
      else if (TargetType == FhirFormatType.json)
      {
        return ConverterEngine.JsonToXml(InboundResource);
      }
      else
      {
        return InboundResource;
      }
    }

    public static string XmlToJson(string xml)
    {
      FhirXmlParser FhirXmlParser = new FhirXmlParser();
      try
      {
        Resource resource = FhirXmlParser.Parse<Resource>(xml);
        string json = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToJson(resource);
        return Newtonsoft.Json.Linq.JValue.Parse(json).ToString(Newtonsoft.Json.Formatting.Indented);        
      }
      catch (Exception Exec)
      {
        return Exec.Message;
      }      
    }

    public static string JsonToXml(string json)
    {
      FhirJsonParser FhirJsonParser = new FhirJsonParser();
      try
      {
        Resource resource = FhirJsonParser.Parse<Resource>(json);
        string xml = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToXml(resource);
        System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Parse(xml);
        return xDoc.ToString();         
      }
      catch (Exception Exec)
      {
        return Exec.Message;
      }      
    }

    public static FhirFormatType IsFormat(string InputString)
    {
      if (InputString.StartsWith("<") && InputString.EndsWith(">"))
        return FhirFormatType.xml;
      if (InputString.StartsWith("{") && InputString.EndsWith("}"))
        return FhirFormatType.json;
      else
        return FhirFormatType.none;
    }
  }
}
