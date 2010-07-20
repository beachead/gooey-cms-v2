using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;

namespace Gooeycms.Business.Web
{
    public class CmsUrl
    {
        public const string Amp = "&";
        private static string[] querySplitter = new string[] { "&amp;", Amp };

        private string scheme;
        private string authority;
        private string path;
        private string query;
        private string fragment;

        public CmsUrl()
        {
        }

        public CmsUrl(CmsUrl other)
        {
            scheme = other.scheme;
            authority = other.authority;
            path = other.path;
            query = other.query;
            fragment = other.fragment;
        }

        public CmsUrl(string scheme, string authority, string path, string query, string fragment)
        {
            this.scheme = scheme;
            this.authority = authority;
            this.path = path;
            this.query = query;
            this.fragment = fragment;
        }

        public CmsUrl(string url)
        {
            if (url != null)
            {
                int queryIndex = QueryIndex(url);
                int hashIndex = url.IndexOf('#', queryIndex > 0 ? queryIndex : 0);
                int authorityIndex = url.IndexOf("://");

                if (hashIndex >= 0)
                    fragment = url.Substring(hashIndex + 1);
                else
                    fragment = null;

                if (hashIndex >= 0 && queryIndex >= 0)
                    query = url.Substring(queryIndex + 1, hashIndex - queryIndex - 1);
                else if (queryIndex >= 0)
                    query = url.Substring(queryIndex + 1);
                else
                    query = null;

                if (authorityIndex >= 0)
                {
                    scheme = url.Substring(0, authorityIndex);
                    int slashIndex = url.IndexOf('/', authorityIndex + 3);
                    if (slashIndex > 0)
                    {
                        authority = url.Substring(authorityIndex + 3, slashIndex - authorityIndex - 3);
                        if (queryIndex >= slashIndex)
                            path = url.Substring(slashIndex, queryIndex - slashIndex);
                        else if (hashIndex >= 0)
                            path = url.Substring(slashIndex, hashIndex - slashIndex);
                        else
                            path = url.Substring(slashIndex);
                    }
                    else
                    {
                        // is this case tolerated?
                        authority = url.Substring(authorityIndex + 3);
                        path = "/";
                    }
                }
                else
                {
                    scheme = null;
                    authority = null;
                    if (queryIndex >= 0)
                        path = url.Substring(0, queryIndex);
                    else if (hashIndex >= 0)
                        path = url.Substring(0, hashIndex);
                    else if (url.Length > 0)
                        path = url;
                    else
                        path = "/";
                }
            }
            else
            {
                scheme = null;
                authority = null;
                path = "/";
                query = null;
                fragment = null;
            }
        }

        /// <summary>E.g. http</summary>
        public string Scheme
        {
            get { return scheme; }
        }

        /// <summary>The domain name and port information.</summary>
        public string Authority
        {
            get { return authority; }
        }

        /// <summary>The path after domain name and before query string, e.g. /path/to/a/page.aspx.</summary>
        public string Path
        {
            get { return path; }
        }

        public string ApplicationRelativePath
        {
            get
            {
                string appPath = ApplicationPath;
                if (appPath.Equals("/"))
                    return "~" + Path;
                else if (Path.StartsWith(appPath, StringComparison.InvariantCultureIgnoreCase))
                    return Path.Substring(appPath.Length);
                else
                    return Path;
            }
        }

        public string RelativePath
        {
            get
            {
                String appPath = this.ApplicationRelativePath;
                return appPath.Replace("~", "");
            }
        }

        /// <summary>The query string, e.g. key=value.</summary>
        public string Query
        {
            get { return query; }
        }

        public string Extension
        {
            get
            {
                int dotIndex = path.LastIndexOf(".");
                if (dotIndex >= 0)
                    return path.Substring(dotIndex);
                else
                    return null;
            }
        }

        public string PathWithoutExtension
        {
            get
            {
                int dotIndex = path.LastIndexOf(".");
                if (dotIndex >= 0)
                    return path.Substring(0, dotIndex);
                else
                    return path;
            }
        }

        /// <summary>The combination of the path and the query string, e.g. /path.aspx?key=value.</summary>
        public string PathAndQuery
        {
            get { return string.IsNullOrEmpty(Query) ? Path : Path + "?" + Query; }
        }

        public string ApplicationRelativePathAndQuery
        {
            get { return string.IsNullOrEmpty(Query) ? ApplicationRelativePath : ApplicationRelativePath + "?" + Query; }
        }

        /// <summary>The bookmark.</summary>
        public string Fragment
        {
            get { return fragment; }
        }

        public override string ToString()
        {
            string url;
            if (authority != null)
                url = scheme + "://" + authority + path;
            else
                url = path;
            if (query != null)
                url += "?" + query;
            if (fragment != null)
                url += "#" + fragment;
            return url;
        }

        public static implicit operator string(CmsUrl u)
        {
            if (u == null)
                return null;
            return u.ToString();
        }

        public static implicit operator CmsUrl(string url)
        {
            return Parse(url);
        }

        /// <summary>Retrieves the path part of an url, e.g. /path/to/page.aspx.</summary>
        public static string PathPart(string url)
        {
            url = RemoveHash(url);

            int queryIndex = QueryIndex(url);
            if (queryIndex >= 0)
                url = url.Substring(0, queryIndex);

            return url;
        }

        /// <summary>Retrieves the query part of an url, e.g. page=12&value=something.</summary>
        public static string QueryPart(string url)
        {
            url = RemoveHash(url);

            int queryIndex = QueryIndex(url);
            if (queryIndex >= 0)
                return url.Substring(queryIndex + 1);
            return string.Empty;
        }

        private static int QueryIndex(string url)
        {
            return url.IndexOf('?');
        }

        static string defaultExtension = ".aspx";
        /// <summary>The extension used for url's to content items.</summary>
        public static string DefaultExtension
        {
            get { return defaultExtension; }
            set { defaultExtension = value; }
        }

        /// <summary>Removes the hash (#...) from an url.</summary>
        /// <param name="url">An url that might hav a hash in it.</param>
        /// <returns>An url without the hash part.</returns>
        public static string RemoveHash(string url)
        {
            int hashIndex = url.IndexOf('#');
            if (hashIndex >= 0)
                url = url.Substring(0, hashIndex);
            return url;
        }

        public static CmsUrl Parse(string url)
        {
            if (url == null)
                return null;
            else if (url.StartsWith("~"))
                url = ToAbsolute(url);

            return new CmsUrl(url);
        }

        public string GetQuery(string key)
        {
            IDictionary<string, string> queries = GetQueries();
            if (queries.ContainsKey(key))
                return queries[key];
            return null;
        }

        public IDictionary<string, string> GetQueries()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (query == null)
                return dictionary;

            string[] queries = query.Split(querySplitter, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < queries.Length; i++)
            {
                string q = queries[i];
                int eqIndex = q.IndexOf("=");
                if (eqIndex >= 0)
                    dictionary[q.Substring(0, eqIndex)] = q.Substring(eqIndex + 1);
            }
            return dictionary;
        }

        public CmsUrl AppendQuery(string key, string value)
        {
            return AppendQuery(key + "=" + HttpUtility.UrlEncode(value));
        }

        public CmsUrl AppendQuery(string key, int value)
        {
            return AppendQuery(key + "=" + value);
        }

        public CmsUrl AppendQuery(string key, object value)
        {
            if (value == null)
                return this;
            else
                return AppendQuery(key + "=" + value.ToString());
        }

        public CmsUrl AppendQuery(string keyValue)
        {
            CmsUrl clone = new CmsUrl(this);
            if (string.IsNullOrEmpty(query))
                clone.query = keyValue;
            else if (!string.IsNullOrEmpty(keyValue))
                clone.query += Amp + keyValue;
            return clone;
        }

        public CmsUrl SetQueryParameter(string key, string value)
        {
            if (query == null)
                return AppendQuery(key, value);

            CmsUrl clone = new CmsUrl(this);
            string[] queries = query.Split(querySplitter, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < queries.Length; i++)
            {
                if (queries[i].StartsWith(key + "=", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (value != null)
                    {
                        queries[i] = key + "=" + HttpUtility.UrlEncode(value);
                        clone.query = string.Join(Amp, queries);
                        return clone;
                    }
                    else
                    {
                        if (queries.Length == 1)
                            clone.query = null;
                        else if (query.Length == 2)
                            clone.query = queries[i == 0 ? 1 : 0];
                        else if (i == 0)
                            clone.query = string.Join(Amp, queries, 1, queries.Length - 1);
                        else if (i == queries.Length - 1)
                            clone.query = string.Join(Amp, queries, 0, queries.Length - 1);
                        else
                            clone.query = string.Join(Amp, queries, 0, i) + Amp + string.Join(Amp, queries, i + 1, queries.Length - i - 1);
                        return clone;
                    }
                }
            }
            return AppendQuery(key, value);
        }

        public CmsUrl SetQueryParameter(string keyValue)
        {
            if (query == null)
                return AppendQuery(keyValue);

            int eqIndex = keyValue.IndexOf('=');
            if (eqIndex >= 0)
                return SetQueryParameter(keyValue.Substring(0, eqIndex), keyValue.Substring(eqIndex + 1));
            else
                return SetQueryParameter(keyValue, string.Empty);
        }

        public CmsUrl SetScheme(string scheme)
        {
            return new CmsUrl(scheme, this.authority, this.path, this.query, this.fragment);
        }

        public CmsUrl SetAuthority(string authority)
        {
            return new CmsUrl(this.scheme ?? "http", authority, this.path, this.query, this.fragment);
        }

        public CmsUrl SetPath(string path)
        {
            if (path.StartsWith("~"))
                path = ToAbsolute(path);
            int queryIndex = QueryIndex(path);
            return new CmsUrl(this.scheme, this.authority, queryIndex < 0 ? path : path.Substring(0, queryIndex), this.query, this.fragment);
        }

        public CmsUrl SetQuery(string query)
        {
            return new CmsUrl(this.scheme, this.authority, this.path, query, this.fragment);
        }

        public CmsUrl SetFragment(string fragment)
        {
            return new CmsUrl(this.scheme, this.authority, this.path, this.query, fragment);
        }

        public CmsUrl AppendSegment(string segment, string extension)
        {
            string newPath;
            if (string.IsNullOrEmpty(path) || path == "/")
                newPath = "/" + segment + extension;
            else if (!string.IsNullOrEmpty(extension))
            {
                int extensionIndex = path.LastIndexOf(extension);
                if (extensionIndex >= 0)
                    newPath = path.Insert(extensionIndex, "/" + segment);
                else if (path.EndsWith("/"))
                    newPath = path + segment + extension;
                else
                    newPath = path + "/" + segment + extension;
            }
            else if (path.EndsWith("/"))
                newPath = path + segment;
            else
                newPath = path + "/" + segment;

            return new CmsUrl(scheme, authority, newPath, query, fragment);
        }

        public CmsUrl AppendSegment(string segment)
        {
            if (string.IsNullOrEmpty(Path) || Path == "/")
                return AppendSegment(segment, DefaultExtension);
            else
                return AppendSegment(segment, Extension);
        }

        public CmsUrl AppendSegment(string segment, bool useDefaultExtension)
        {
            return AppendSegment(segment, useDefaultExtension ? DefaultExtension : Extension);
        }

        public CmsUrl PrependSegment(string segment, string extension)
        {
            string newPath;
            if (string.IsNullOrEmpty(path) || path == "/")
                newPath = "/" + segment + extension;
            else if (extension != Extension)
            {
                newPath = "/" + segment + PathWithoutExtension + extension;
            }
            else
            {
                newPath = "/" + segment + path;
            }

            return new CmsUrl(scheme, authority, newPath, query, fragment);
        }

        public CmsUrl PrependSegment(string segment)
        {
            if (string.IsNullOrEmpty(Path) || Path == "/")
                return PrependSegment(segment, DefaultExtension);
            else
                return PrependSegment(segment, Extension);
        }

        public CmsUrl AppendQuery(NameValueCollection queryString)
        {
            CmsUrl u = new CmsUrl(scheme, authority, path, query, fragment);
            foreach (string key in queryString.AllKeys)
                u = u.SetQueryParameter(key, queryString[key]);
            return u;
        }

        /// <summary>Converts a possibly relative to an absolute url.</summary>
        /// <param name="url">The url to convert.</param>
        /// <returns>The absolute url.</returns>
        public static string ToAbsolute(string path)
        {
            if (!string.IsNullOrEmpty(path) && path[0] == '~' && path.Length > 1)
                return ApplicationPath + path.Substring(2);
            else if (path == "~")
                return ApplicationPath;
            return path;
        }

        /// <summary>Converts a virtual path to a relative path, e.g. /myapp/path/to/a/page.aspx -> ~/path/to/a/page.aspx</summary>
        /// <param name="path">The virtual path.</param>
        /// <returns>A relative path</returns>
        public static string ToRelative(string path)
        {
            if (!string.IsNullOrEmpty(path) && path.StartsWith(ApplicationPath))
                return "~/" + path.Substring(ApplicationPath.Length);
            return path;
        }

        private static string applicationPath = null;
        /// <summary>Gets the root path of the web application. e.g. "/" if the application doesn't run in a virtual directory.</summary>
        public static string ApplicationPath
        {
            get
            {
                if (applicationPath == null)
                {
                    try
                    {
                        applicationPath = VirtualPathUtility.ToAbsolute("~/");
                    }
                    catch
                    {
                        return "/";
                    }
                }
                return applicationPath;
            }
            set { applicationPath = value; }
        }

        private static string serverUrl = null;
        /// <summary>The address to the server where the site is running.</summary>
        public static string ServerUrl
        {
            get { return serverUrl; }
            set { serverUrl = value; }
        }

        /// <summary>
        /// Resolves an application relative url (e.g. ~/path) to its physical url
        /// </summary>
        /// <param name="url">The url to resolve</param>
        /// <returns>The physical url</returns>
        public static String ResolveUrl(String url)
        {
            Control resolver = new Control();
            return resolver.ResolveUrl(url);
        }
    }
}
