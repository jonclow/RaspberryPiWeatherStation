namespace System.Runtime.CompilerServices {
    internal class __BlockReflectionAttribute : Attribute { }
}

namespace Microsoft.Xml.Serialization.GeneratedAssembly {


    [System.Runtime.CompilerServices.__BlockReflection]
    public class XmlSerializationWriter1 : System.Xml.Serialization.XmlSerializationWriter {

        public void Write4_WeatherData(object o, string parentRuntimeNs = null, string parentCompileTimeNs = null) {
            string defaultNamespace = parentRuntimeNs ?? @"";
            WriteStartDocument();
            if (o == null) {
                WriteNullTagLiteral(@"WeatherData", defaultNamespace);
                return;
            }
            TopLevelElement();
            string namespace1 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            Write3_WeatherData(@"WeatherData", namespace1, ((global::WeatherStationHeadless.WeatherData)o), true, false, namespace1, @"");
        }

        void Write3_WeatherData(string n, string ns, global::WeatherStationHeadless.WeatherData o, bool isNullable, bool needType, string parentRuntimeNs = null, string parentCompileTimeNs = null) {
            string defaultNamespace = parentRuntimeNs;
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::WeatherStationHeadless.WeatherData)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"WeatherData", defaultNamespace);
            string namespace2 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            WriteElementStringRaw(@"Altitude", namespace2, System.Xml.XmlConvert.ToString((global::System.Single)((global::System.Single)o.@Altitude)));
            string namespace3 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            WriteElementStringRaw(@"BarometricPressure", namespace3, System.Xml.XmlConvert.ToString((global::System.Single)((global::System.Single)o.@BarometricPressure)));
            string namespace4 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            WriteElementStringRaw(@"CelsiusTemperature", namespace4, System.Xml.XmlConvert.ToString((global::System.Single)((global::System.Single)o.@CelsiusTemperature)));
            string namespace5 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            WriteElementStringRaw(@"FahrenheitTemperature", namespace5, System.Xml.XmlConvert.ToString((global::System.Single)((global::System.Single)o.@FahrenheitTemperature)));
            string namespace6 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            WriteElementStringRaw(@"Humidity", namespace6, System.Xml.XmlConvert.ToString((global::System.Single)((global::System.Single)o.@Humidity)));
            string namespace7 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            Write2_DateTimeOffset(@"TimeStamp", namespace7, ((global::System.DateTimeOffset)o.@TimeStamp), false, namespace7, @"");
            string namespace8 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            WriteElementStringRaw(@"LightSensorVoltage", namespace8, System.Xml.XmlConvert.ToString((global::System.Single)((global::System.Single)o.@LightSensorVoltage)));
            string namespace9 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            WriteElementStringRaw(@"WindDirection", namespace9, System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@WindDirection)));
            string namespace10 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            WriteElementStringRaw(@"WindSpeed", namespace10, System.Xml.XmlConvert.ToString((global::System.Double)((global::System.Double)o.@WindSpeed)));
            string namespace11 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            WriteElementStringRaw(@"PeakWindSpeed", namespace11, System.Xml.XmlConvert.ToString((global::System.Double)((global::System.Double)o.@PeakWindSpeed)));
            string namespace12 = ( parentCompileTimeNs == @"" && parentRuntimeNs != null ) ? parentRuntimeNs : @"";
            WriteElementStringRaw(@"RainFall", namespace12, System.Xml.XmlConvert.ToString((global::System.Double)((global::System.Double)o.@RainFall)));
            WriteEndElement(o);
        }

        void Write2_DateTimeOffset(string n, string ns, global::System.DateTimeOffset o, bool needType, string parentRuntimeNs = null, string parentCompileTimeNs = null) {
            string defaultNamespace = parentRuntimeNs;
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::System.DateTimeOffset)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"DateTimeOffset", defaultNamespace);
            WriteEndElement(o);
        }

        protected override void InitCallbacks() {
        }
    }

    [System.Runtime.CompilerServices.__BlockReflection]
    public class XmlSerializationReader1 : System.Xml.Serialization.XmlSerializationReader {

        public object Read4_WeatherData(string defaultNamespace = null) {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_WeatherData && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                    o = Read3_WeatherData(true, true, defaultNamespace);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, defaultNamespace ?? @":WeatherData");
            }
            return (object)o;
        }

        global::WeatherStationHeadless.WeatherData Read3_WeatherData(bool isNullable, bool checkType, string defaultNamespace = null) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id1_WeatherData && string.Equals( ((System.Xml.XmlQualifiedName)xsiType).Namespace, defaultNamespace ?? id2_Item))) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::WeatherStationHeadless.WeatherData o;
            o = new global::WeatherStationHeadless.WeatherData();
            bool[] paramsRead = new bool[11];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (!paramsRead[0] && ((object) Reader.LocalName == (object)id3_Altitude && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        {
                            o.@Altitude = System.Xml.XmlConvert.ToSingle(Reader.ReadElementContentAsString());
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id4_BarometricPressure && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        {
                            o.@BarometricPressure = System.Xml.XmlConvert.ToSingle(Reader.ReadElementContentAsString());
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id5_CelsiusTemperature && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        {
                            o.@CelsiusTemperature = System.Xml.XmlConvert.ToSingle(Reader.ReadElementContentAsString());
                        }
                        paramsRead[2] = true;
                    }
                    else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id6_FahrenheitTemperature && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        {
                            o.@FahrenheitTemperature = System.Xml.XmlConvert.ToSingle(Reader.ReadElementContentAsString());
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id7_Humidity && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        {
                            o.@Humidity = System.Xml.XmlConvert.ToSingle(Reader.ReadElementContentAsString());
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id8_TimeStamp && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        o.@TimeStamp = Read2_DateTimeOffset(true, defaultNamespace);
                        paramsRead[5] = true;
                    }
                    else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id9_LightSensorVoltage && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        {
                            o.@LightSensorVoltage = System.Xml.XmlConvert.ToSingle(Reader.ReadElementContentAsString());
                        }
                        paramsRead[6] = true;
                    }
                    else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id10_WindDirection && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        {
                            o.@WindDirection = System.Xml.XmlConvert.ToInt32(Reader.ReadElementContentAsString());
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id11_WindSpeed && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        {
                            o.@WindSpeed = System.Xml.XmlConvert.ToDouble(Reader.ReadElementContentAsString());
                        }
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id12_PeakWindSpeed && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        {
                            o.@PeakWindSpeed = System.Xml.XmlConvert.ToDouble(Reader.ReadElementContentAsString());
                        }
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id13_RainFall && string.Equals(Reader.NamespaceURI, defaultNamespace ?? id2_Item))) {
                        {
                            o.@RainFall = System.Xml.XmlConvert.ToDouble(Reader.ReadElementContentAsString());
                        }
                        paramsRead[10] = true;
                    }
                    else {
                        UnknownNode((object)o, @":Altitude, :BarometricPressure, :CelsiusTemperature, :FahrenheitTemperature, :Humidity, :TimeStamp, :LightSensorVoltage, :WindDirection, :WindSpeed, :PeakWindSpeed, :RainFall");
                    }
                }
                else {
                    UnknownNode((object)o, @":Altitude, :BarometricPressure, :CelsiusTemperature, :FahrenheitTemperature, :Humidity, :TimeStamp, :LightSensorVoltage, :WindDirection, :WindSpeed, :PeakWindSpeed, :RainFall");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            ReadEndElement();
            return o;
        }

        global::System.DateTimeOffset Read2_DateTimeOffset(bool checkType, string defaultNamespace = null) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id14_DateTimeOffset && string.Equals( ((System.Xml.XmlQualifiedName)xsiType).Namespace, defaultNamespace ?? id2_Item))) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            global::System.DateTimeOffset o;
            try {
                o = (global::System.DateTimeOffset)ActivatorHelper.CreateInstance(typeof(global::System.DateTimeOffset));
            }
            catch (System.MissingMemberException) {
                throw CreateInaccessibleConstructorException(@"global::System.DateTimeOffset");
            }
            catch (System.Security.SecurityException) {
                throw CreateCtorHasSecurityException(@"global::System.DateTimeOffset");
            }
            bool[] paramsRead = new bool[0];
            while (Reader.MoveToNextAttribute()) {
                if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations1 = 0;
            int readerCount1 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations1, ref readerCount1);
            }
            ReadEndElement();
            return o;
        }

        protected override void InitCallbacks() {
        }

        string id2_Item;
        string id14_DateTimeOffset;
        string id9_LightSensorVoltage;
        string id7_Humidity;
        string id11_WindSpeed;
        string id8_TimeStamp;
        string id6_FahrenheitTemperature;
        string id4_BarometricPressure;
        string id5_CelsiusTemperature;
        string id3_Altitude;
        string id10_WindDirection;
        string id12_PeakWindSpeed;
        string id1_WeatherData;
        string id13_RainFall;

        protected override void InitIDs() {
            id2_Item = Reader.NameTable.Add(@"");
            id14_DateTimeOffset = Reader.NameTable.Add(@"DateTimeOffset");
            id9_LightSensorVoltage = Reader.NameTable.Add(@"LightSensorVoltage");
            id7_Humidity = Reader.NameTable.Add(@"Humidity");
            id11_WindSpeed = Reader.NameTable.Add(@"WindSpeed");
            id8_TimeStamp = Reader.NameTable.Add(@"TimeStamp");
            id6_FahrenheitTemperature = Reader.NameTable.Add(@"FahrenheitTemperature");
            id4_BarometricPressure = Reader.NameTable.Add(@"BarometricPressure");
            id5_CelsiusTemperature = Reader.NameTable.Add(@"CelsiusTemperature");
            id3_Altitude = Reader.NameTable.Add(@"Altitude");
            id10_WindDirection = Reader.NameTable.Add(@"WindDirection");
            id12_PeakWindSpeed = Reader.NameTable.Add(@"PeakWindSpeed");
            id1_WeatherData = Reader.NameTable.Add(@"WeatherData");
            id13_RainFall = Reader.NameTable.Add(@"RainFall");
        }
    }

    [System.Runtime.CompilerServices.__BlockReflection]
    public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
            return new XmlSerializationReader1();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
            return new XmlSerializationWriter1();
        }
    }

    [System.Runtime.CompilerServices.__BlockReflection]
    public sealed class WeatherDataSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"WeatherData", this.DefaultNamespace ?? @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWriter1)writer).Write4_WeatherData(objectToSerialize, this.DefaultNamespace, @"");
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReader1)reader).Read4_WeatherData(this.DefaultNamespace);
        }
    }

    [System.Runtime.CompilerServices.__BlockReflection]
    public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
        public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReader1(); } }
        public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriter1(); } }
        System.Collections.IDictionary readMethods = null;
        public override System.Collections.IDictionary ReadMethods {
            get {
                if (readMethods == null) {
                    System.Collections.IDictionary _tmp = new System.Collections.Generic.Dictionary<string, string>();
                    _tmp[@"WeatherStationHeadless.WeatherData::"] = @"Read4_WeatherData";
                    if (readMethods == null) readMethods = _tmp;
                }
                return readMethods;
            }
        }
        System.Collections.IDictionary writeMethods = null;
        public override System.Collections.IDictionary WriteMethods {
            get {
                if (writeMethods == null) {
                    System.Collections.IDictionary _tmp = new System.Collections.Generic.Dictionary<string, string>();
                    _tmp[@"WeatherStationHeadless.WeatherData::"] = @"Write4_WeatherData";
                    if (writeMethods == null) writeMethods = _tmp;
                }
                return writeMethods;
            }
        }
        System.Collections.IDictionary typedSerializers = null;
        public override System.Collections.IDictionary TypedSerializers {
            get {
                if (typedSerializers == null) {
                    System.Collections.IDictionary _tmp = new System.Collections.Generic.Dictionary<string, System.Xml.Serialization.XmlSerializer>();
                    _tmp.Add(@"WeatherStationHeadless.WeatherData::", new WeatherDataSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::WeatherStationHeadless.WeatherData)) return true;
            if (type == typeof(global::System.Reflection.TypeInfo)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::WeatherStationHeadless.WeatherData)) return new WeatherDataSerializer();
            return null;
        }
        public static global::System.Xml.Serialization.XmlSerializerImplementation GetXmlSerializerContract() { return new XmlSerializerContract(); }
    }

    [System.Runtime.CompilerServices.__BlockReflection]
    public static class ActivatorHelper {
        public static object CreateInstance(System.Type type) {
            System.Reflection.TypeInfo ti = System.Reflection.IntrospectionExtensions.GetTypeInfo(type);
            foreach (System.Reflection.ConstructorInfo ci in ti.DeclaredConstructors) {
                if (!ci.IsStatic && ci.GetParameters().Length == 0) {
                    return ci.Invoke(null);
                }
            }
            return System.Activator.CreateInstance(type);
        }
    }
}
