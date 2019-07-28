using System;

namespace Jeedom.API.Network
{
    internal class Address
    {
        private const string HttpHeader = "http://";
        private const string HttpsHeader = "https://";
        private string _access;
        private string _header;
        private bool _ignoreCertificateError = false;
        private Protocol _type;
        private UriBuilder _uri;

        public enum Protocol { Http, Https, SelfSigned };

        public string Access { get { return _access; } }
        public string Header { get { return _header; } }

        public bool IgnoreCertificateError { get { return _ignoreCertificateError; } }

        public string Link
        {
            get
            {
                return _header + _access;
            }

            set
            {
                var link = value.ToLower();
                if (link.Contains(HttpHeader))
                {
                    ProtocolType = Protocol.Http;
                    _access = link.Remove(0, 7);
                }
                else
                {
                    ProtocolType = Protocol.Https; //Default to https
                    if (link.Contains(HttpsHeader))
                    {
                        _access = link.Remove(0, 8);
                    }
                    else
                        _access = link;
                }
            }
        }

        public Protocol ProtocolType
        {
            get { return _type; }
            set
            {
                _type = value;
                switch (_type)
                {
                    case Protocol.Http:
                        _header = HttpHeader;
                        _ignoreCertificateError = false;
                        break;

                    case Protocol.Https:
                        _header = HttpsHeader;
                        _ignoreCertificateError = false;
                        break;

                    case Protocol.SelfSigned:
                        _header = HttpsHeader;
                        _ignoreCertificateError = true;
                        break;
                }
            }
        }

        public Uri Uri
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Link))
                    return null;
                _uri = new UriBuilder(Link);

                // Termine l'URL par un "/" pour Docker par exemple dont le chemin est de la forme "http://xxx.xxx.xxx.xxx:xxxx/jeedom"
                if (!_uri.Path.EndsWith("/"))
                    _uri.Path += "/";

                return _uri.Uri;
            }
        }
    }
}