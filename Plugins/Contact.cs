using System;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace Plugins
{
    public class Contact : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            #region Context and Services
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            #endregion 

            try
            {
                if (context.Depth > 1)
                    return;

                if (context.MessageName.Equals("Retrieve") &&
                    context.Mode.Equals(0) && // sincrono 
                    context.Stage.Equals(40) && // post-operation
                    context.Depth == 1)
                {
                    if (context.OutputParameters.Contains("BusinessEntity") &&
                        context.OutputParameters["BusinessEntity"] is Entity)
                    {
                        Entity entity = (Entity)context.OutputParameters["BusinessEntity"];

                        if (entity.LogicalName != "contact")
                            return;

                        tracingService.Trace("Contact Retrieve");

                        // consulta dos dados do contato 
                        Entity contact = service.Retrieve("contact", entity.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet("fullname", "new_identificador"));
                        var identificador = contact.Contains("new_identificador") ? contact["new_identificador"].ToString() : string.Empty;
                        var fullname = contact.Contains("fullname") ? contact["fullname"].ToString() : string.Empty;
                        var nomeRegistro = $"{identificador} - {fullname}";

                        // lógica do plugin
                        if (entity.Attributes.Contains("new_nomedoregistro"))
                        {
                            tracingService.Trace("new_nomedoregistro antes: " + entity["new_nomedoregistro"].ToString());
                            entity["new_nomedoregistro"] = nomeRegistro;
                        }
                        else
                        {
                            tracingService.Trace("new_nomedoregistro not found");
                            entity.Attributes.Add("new_nomedoregistro", nomeRegistro);
                        }

                        tracingService.Trace("new_nomedoregistro depois: " + entity["new_nomedoregistro"].ToString());
                    }
                }
                else if (context.MessageName.Equals("RetrieveMultiple") &&
                    context.Mode.Equals(0) && // sincrono 
                    context.Stage.Equals(40) && // post-operation
                    context.Depth == 1)
                {
                    if (context.OutputParameters.Contains("BusinessEntityCollection") &&
                        context.OutputParameters["BusinessEntityCollection"] is EntityCollection)
                    {
                        EntityCollection entityCollection = (EntityCollection)context.OutputParameters["BusinessEntityCollection"];

                        if (entityCollection.EntityName != "contact")
                            return;

                        tracingService.Trace("Contact RetrieveMultiple");

                        foreach (var entity in entityCollection.Entities)
                        {
                            // consulta dos dados do contato 
                            Entity contact = service.Retrieve("contact", entity.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet("fullname", "new_identificador"));
                            var identificador = contact.Contains("new_identificador") ? contact["new_identificador"].ToString() : string.Empty;
                            var fullname = contact.Contains("fullname") ? contact["fullname"].ToString() : string.Empty;
                            var nomeRegistro = $"{identificador} - {fullname}";

                            // lógica do plugin
                            if (entity.Attributes.Contains("new_nomedoregistro"))
                            {
                                tracingService.Trace("new_nomedoregistro antes: " + entity["new_nomedoregistro"].ToString());
                                entity["new_nomedoregistro"] = nomeRegistro;
                            }
                            else
                            {
                                tracingService.Trace("new_nomedoregistro not found");
                                entity.Attributes.Add("new_nomedoregistro", nomeRegistro);
                            }

                            tracingService.Trace("new_nomedoregistro depois: " + entity["new_nomedoregistro"].ToString());
                        }
                    }
                }          
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in the plug-in.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("Plugin: {0}", ex.ToString());
                throw;
            }
        }
    }
}
