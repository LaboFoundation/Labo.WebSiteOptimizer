using Labo.WebSiteOptimizer.ResourceManagement.Minify;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.Minify
{
    [TestFixture]
    public class YahooCssMinifierTestFixture
    {
        [Test]
        public void Minify()
        {
            const string css = @"/* Sticky footer styles -------------------------------------------------- */
html,
body {
    height: 100%;
    /* The html and body elements 
       cannot have any padding or margin. */
}   
 
/* Wrapper for page content to push down footer */
#wrap {
    min-height: 100%;
    height: auto !important;
    height: 100%;
    /* Negative indent footer by it's height */
    margin: 0 auto -115px;
}

/* Set the fixed height of the footer here */
#push,
#footer {
    height: 100px;
}  
 
/* Set the fixed height of the footer here */
#footer {
    background-color: #f5f5f5;
    text-align: center;
    padding-top: 15px;
}

/* Lastly, apply responsive CSS fixes as necessary */
@media (max-width: 767px) {
#footer {
    margin-left: -20px;
    margin-right: -20px;
    padding-left: 20px;
    padding-right: 20px;
}
}

.footer-links {
  margin: 10px 0;
}
.footer-links li {
  display: inline;
  padding: 0 2px;
}
.footer-links li:first-child {
  padding-left: 0;
}";
            const string expectedMinifiedCss = "html,body{height:100%}#wrap{min-height:100%;height:auto!important;height:100%;margin:0 auto -115px}#push,#footer{height:100px}#footer{background-color:#f5f5f5;text-align:center;padding-top:15px}@media(max-width:767px){#footer{margin-left:-20px;margin-right:-20px;padding-left:20px;padding-right:20px}}.footer-links{margin:10px 0}.footer-links li{display:inline;padding:0 2px}.footer-links li:first-child{padding-left:0}";
            YahooCssMinifier yahooCssMinifier = new YahooCssMinifier();
            Assert.AreEqual(expectedMinifiedCss, yahooCssMinifier.Minify(css));
        }
    }
}
