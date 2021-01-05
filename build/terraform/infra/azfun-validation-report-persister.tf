module "azfun_validationreportpersister" {
  source                                    = "../modules/function-app"
  name                                      = "azfun-validationreportpersister-${var.organisation}-${var.environment}"
  resource_group_name                       = data.azurerm_resource_group.greenenergyhub.name
  location                                  = data.azurerm_resource_group.greenenergyhub.location
  storage_account_access_key                = module.azfun_validationreportpersister_stor.primary_access_key
  storage_account_name                      = module.azfun_validationreportpersister_stor.name
  app_service_plan_id                       = module.azfun_validationreportpersister_plan.id
  application_insights_instrumentation_key  = module.appi_shared.instrumentation_key
  tags                                      = data.azurerm_resource_group.greenenergyhub.tags
  app_settings                              = {
    # Region: Default Values
    WEBSITE_ENABLE_SYNC_UPDATE_SITE     = true
    WEBSITE_RUN_FROM_PACKAGE            = 1
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = true
    FUNCTIONS_WORKER_RUNTIME            = "dotnet"
    # Endregion: Default Values
    VALIDATION_REPORTS_STORAGE_ACCOUNT  = module.stor_validationreportsstorage.primary_connection_string
    VALIDATION_REPORTS_QUEUE            = module.evhar_validationreport_receiver.primary_connection_string
  }
  dependencies                              = [
    module.appi_shared.dependent_on,
    module.azfun_validationreportpersister_plan.dependent_on,
    module.azfun_validationreportpersister_stor.dependent_on,
    module.stor_validationreportsstorage.dependent_on,
    module.evhar_validationreport_receiver.dependent_on,
  ]
}

module "azfun_validationreportpersister_plan" {
  source              = "../modules/app-service-plan"
  name                = "asp-validationreportpersister-${var.organisation}-${var.environment}"
  resource_group_name = data.azurerm_resource_group.greenenergyhub.name
  location            = data.azurerm_resource_group.greenenergyhub.location
  kind                = "FunctionApp"
  sku                 = {
    tier  = "Basic"
    size  = "B1"
  }
  tags                = data.azurerm_resource_group.greenenergyhub.tags
}

module "azfun_validationreportpersister_stor" {
  source                    = "../modules/storage-account"
  name                      = "stor${random_string.validationreportpersister.result}${var.organisation}${lower(var.environment)}"
  resource_group_name       = data.azurerm_resource_group.greenenergyhub.name
  location                  = data.azurerm_resource_group.greenenergyhub.location
  account_replication_type  = "LRS"
  access_tier               = "Cool"
  account_tier              = "Standard"
  tags                      = data.azurerm_resource_group.greenenergyhub.tags
}

# Since all functions need a storage connected we just generate a random name
resource "random_string" "validationreportpersister" {
  length  = 5
  special = false
  upper   = false
}