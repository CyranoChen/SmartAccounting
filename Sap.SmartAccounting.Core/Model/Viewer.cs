using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sap.SmartAccounting.Core.Dapper;
using Sap.SmartAccounting.Core.Extension;

namespace Sap.SmartAccounting.Core
{
    public abstract class Viewer : IViewer
    {

        protected virtual string GenerateKey()
        {
            return KeyGenerator.Generate();
        }

        public override string ToString()
        {
            return Key;
        }

        private static class KeyGenerator
        {
            public static string Generate()
            {
                return Generate(Guid.NewGuid().ToString("D").Substring(24));
            }

            private static string Generate(string input)
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(input));
                return HttpUtility.UrlEncode(input.Replace(" ", "_").Replace("-", "_").Replace("&", "and"));
            }
        }

        public virtual void Many<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao, new()
        {
            // Get the property which matches IEnumerable<T>
            var property = GetType().GetProperties()
                .FirstOrDefault(
                    x => (Nullable.GetUnderlyingType(x.PropertyType) ?? x.PropertyType) == typeof(IEnumerable<T>));

            //var propertyName = string.Format("List{0}", typeof(T).Name);
            //var property = this.GetType().GetProperty(propertyName, typeof(IEnumerable<T>));

            if (property != null)
            {
                IRepository repo = new Repository();

                var list = repo.Query(whereBy);

                if (list != null && list.Count > 0)
                {
                    property.SetValue(this, list, null);
                }
            }
        }

        #region Members and Properties

        [Unique, StringLength(50)]
        public virtual string Key
        {
            get { return _key = _key ?? GenerateKey(); }
            protected set { _key = value; }
        }

        private string _key;

        #endregion
    }

    public interface IViewer : IMany
    {
        /// <summary>
        ///     The entity's unique (and URL-safe) public identifier
        /// </summary>
        /// <remarks>
        ///     This is the identifier that should be exposed via the web, etc.
        /// </remarks>
        string Key { get; }

        void Many<T>(Expression<Func<T, bool>> whereBy) where T : class, IDao, new();
    }
}
