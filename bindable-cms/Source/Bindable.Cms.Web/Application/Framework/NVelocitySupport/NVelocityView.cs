using System.IO;
using System.Web.Mvc;
using NVelocity;
using NVelocity.App;

namespace Bindable.Cms.Web.Application.Framework.NVelocitySupport
{
    /// <summary>
    /// Represents a view for NVelocity.
    /// </summary>
    public class NVelocityView : IView, IViewDataContainer
    {
        private readonly VelocityContext _context;
        private readonly VelocityEngine _engine;
        private readonly Template _template;

        /// <summary>
        /// Initializes a new instance of the <see cref="NVelocityView"/> class.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="viewData">The view data.</param>
        /// <param name="engine">The engine.</param>
        public NVelocityView(Template template, ViewDataDictionary viewData, VelocityEngine engine)
        {
            _context = new VelocityContext();
            _template = template;
            _engine = engine;
            ViewData = viewData;
        }

        /// <summary>
        /// Gets the NVelocity engine so that child views can render themselves.
        /// </summary>
        public VelocityEngine Engine
        {
            get { return _engine; }
        }

        /// <summary>
        /// Gets the context with all information that is passed to NHibernate.
        /// </summary>
        /// <value>The context.</value>
        public VelocityContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Gets or sets the view data.
        /// </summary>
        /// <value>The view data.</value>
        public ViewDataDictionary ViewData { get; set; }

        /// <summary>
        /// Renders the specified view context.
        /// </summary>
        /// <param name="viewContext">The view context.</param>
        /// <param name="writer">The writer.</param>
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            _template.Merge(Context, writer);
        }
    }
}