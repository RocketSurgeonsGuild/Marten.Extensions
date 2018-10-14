using System;
using System.IO;
using Baseline;
using Marten;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Rocket.Surgery.Binding;

namespace Rocket.Surgery.Core.Marten
{
    /// <summary>
    /// A custon serializer instance
    /// </summary>
    public class CustomJsonNetSerializer : ISerializer
    {
        private readonly JsonSerializer _clean = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.None,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Include,
            ContractResolver = new PrivateSetterContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
        }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

        private readonly JsonSerializer _serializer = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.None,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
            NullValueHandling = NullValueHandling.Include,
            ContractResolver = new PrivateSetterContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },

        }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

        private EnumStorage _enumStorage;
        private Casing _casing;

        public CustomJsonNetSerializer()
        {
            EnumStorage = EnumStorage.AsString;
            Casing = Casing.CamelCase;
        }

        public EnumStorage EnumStorage
        {
            get => _enumStorage;
            set
            {
                _enumStorage = value;
                if (value == EnumStorage.AsString)
                {
                    var stringEnumConverter = new StringEnumConverter();
                    _serializer.Converters.Add(stringEnumConverter);
                    _clean.Converters.Add(stringEnumConverter);
                }
                else
                {
                    _serializer.Converters.RemoveAll(x => x is StringEnumConverter);
                    _clean.Converters.RemoveAll(x => x is StringEnumConverter);
                }
            }
        }

        public Casing Casing
        {
            get => _casing;
            set
            {
                _casing = value;
                switch (value)
                {
                    case Casing.Default:
                        ((PrivateSetterContractResolver) _serializer.ContractResolver).NamingStrategy =
                            ((PrivateSetterContractResolver) _clean.ContractResolver).NamingStrategy =
                            new DefaultNamingStrategy();
                        break;
                    case Casing.CamelCase:
                        ((PrivateSetterContractResolver) _serializer.ContractResolver).NamingStrategy =
                            ((PrivateSetterContractResolver) _clean.ContractResolver).NamingStrategy =
                            new CamelCaseNamingStrategy() { ProcessDictionaryKeys = true, OverrideSpecifiedNames = true };
                        break;
                    case Casing.SnakeCase:
                        ((PrivateSetterContractResolver) _serializer.ContractResolver).NamingStrategy =
                            ((PrivateSetterContractResolver) _clean.ContractResolver).NamingStrategy =
                            new SnakeCaseNamingStrategy() { ProcessDictionaryKeys = true, OverrideSpecifiedNames = true };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        public void Customize(Action<JsonSerializer> configure)
        {
            configure(_clean);
            configure(_serializer);
            _clean.TypeNameHandling = TypeNameHandling.None;
        }

        public void ToJson(object document, TextWriter writer)
        {
            _serializer.Serialize(writer, document);
        }

        public string ToJson(object document)
        {
            var stringWriter = new StringWriter();
            _serializer.Serialize(stringWriter, document);
            return stringWriter.ToString();
        }

        public T FromJson<T>(Stream stream)
        {
            return _serializer.Deserialize<T>(new JsonTextReader(new StreamReader(stream)));
        }

        public T FromJson<T>(TextReader reader)
        {
            return _serializer.Deserialize<T>(new JsonTextReader(reader));
        }

        public object FromJson(Type type, TextReader reader)
        {
            return _serializer.Deserialize(new JsonTextReader(reader), type);
        }

        public string ToCleanJson(object document)
        {
            var stringWriter = new StringWriter();
            _clean.Serialize(stringWriter, document);
            return stringWriter.ToString();
        }
    }
}
