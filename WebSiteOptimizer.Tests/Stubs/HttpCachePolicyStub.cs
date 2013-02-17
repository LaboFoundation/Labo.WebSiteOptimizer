using System;
using System.Web;

namespace Labo.WebSiteOptimizer.Tests.Stubs
{
    public sealed class HttpCachePolicyStub : HttpCachePolicyBase
    {
        private DateTime m_ExpireDate;
        private DateTime m_LastModifiedDate;
        private string m_VaryByCustom;
        private TimeSpan m_MaxAge;
        private bool m_ValidUntilExpires;
        private HttpCacheability m_Cacheability;

        public DateTime ExpireDate
        {
            get { return m_ExpireDate; }
        }

        public DateTime LastModifiedDate
        {
            get { return m_LastModifiedDate; }
        }

        public string VaryByCustom
        {
            get { return m_VaryByCustom; }
        }

        public TimeSpan MaxAge
        {
            get { return m_MaxAge; }
        }

        public bool ValidUntilExpires
        {
            get { return m_ValidUntilExpires; }
        }

        public HttpCacheability Cacheability
        {
            get { return m_Cacheability; }
        }

        public override void SetCacheability(HttpCacheability cacheability)
        {
            m_Cacheability = cacheability;
        }

        public override void SetValidUntilExpires(bool validUntilExpires)
        {
            m_ValidUntilExpires = validUntilExpires;
        }

        public override void SetExpires(DateTime date)
        {
            m_ExpireDate = date;
        }

        public override void SetLastModified(DateTime date)
        {
            m_LastModifiedDate = date;
        }

        public override void SetVaryByCustom(string custom)
        {
            m_VaryByCustom = custom;
        }

        public override void SetMaxAge(TimeSpan delta)
        {
            m_MaxAge = delta;
        }
    }
}