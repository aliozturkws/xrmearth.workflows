using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace XrmEarth.Core.Activity
{
    public class CodeActivityHelper
    {
        #region | Fields |

        IOrganizationService _orgService = null;
        IWorkflowContext _context = null;
        ITracingService _tracingService = null;
        IOrganizationServiceFactory _serviceFactory = null;

        #endregion

        #region | Constructors |
        public CodeActivityHelper(CodeActivityContext activityHelper)
        {
            CodeActivityContext = activityHelper;
            _serviceFactory = ServiceFactory;
        }
        #endregion

        #region | Enums |
        public enum TraceSeverity
        {
            Info,
            Warning,
            Exception,
            DebugInfo
        }
        #endregion | Enums |

        #region | Properties |

        public IOrganizationServiceFactory ServiceFactory
        {
            get
            {
                return GetServiceFactory();
            }
        }

        public CodeActivityContext CodeActivityContext { get; }

        public IWorkflowContext Context
        {
            get
            {
                return GetContext();
            }
        }

        public IOrganizationService OrganizationService
        {
            get
            {
                return GetUserOrganizationService();
            }
        }

        public ITracingService TracingService
        {
            get
            {
                return GetTracingService();
            }
        }
        #endregion

        #region | Public Methods |

        public IOrganizationService GetOrganizationService(Guid userId)
        {
            return CreateOrganizationService(userId);
        }

        public void Trace(string message)
        {
            TracingService.Trace(message);
        }

        public void Trace(TraceSeverity severity, string message)
        {
            TracingService.Trace(string.Format("{0} : {1}", severity.ToString(), message));
        }
        #endregion

        #region | Private Methods |

        private IOrganizationServiceFactory GetServiceFactory()
        {
            if (_serviceFactory == null)
            {
                _serviceFactory = CodeActivityContext.GetExtension<IOrganizationServiceFactory>();
            }

            return _serviceFactory;
        }

        private IWorkflowContext GetContext()
        {
            if (_context == null)
            {
                _context = CodeActivityContext.GetExtension<IWorkflowContext>();
            }

            return _context;
        }

        private ITracingService GetTracingService()
        {
            if (_tracingService == null)
            {
                _tracingService = CodeActivityContext.GetExtension<ITracingService>();
            }

            return _tracingService;
        }

        private IOrganizationService GetUserOrganizationService()
        {
            if (_orgService == null)
            {
                _orgService = GetOrganizationService();
            }

            return _orgService;
        }

        private IOrganizationService GetOrganizationService()
        {
            return CreateOrganizationService(this.Context.UserId);
        }

        private IOrganizationService CreateOrganizationService(Guid userId)
        {
            return _serviceFactory.CreateOrganizationService(userId);
        }
        #endregion
    }
}